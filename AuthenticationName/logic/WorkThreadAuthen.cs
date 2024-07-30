using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Net;

public class WorkThreadAuthen : ServiceBase
{
    private bool m_run = true;
    Queue<AuthInfoReq> m_taskQue = new Queue<AuthInfoReq>();
    object m_syn = new object();
    HttpService m_hservice;
    protected AutoResetEvent m_event = new AutoResetEvent(false);

    public WorkThreadAuthen()
    {
        m_sysType = (int)SType.ServerType_Authen;
    }

    public override void initService()
    {
        Thread t = new Thread(new ThreadStart(run));
        t.Start();

        m_hservice = new HttpService();
        m_hservice.EventInputData = onInputData;
        m_hservice.init(this);
    }

    public override void exitService()
    {
        m_run = false;
        m_hservice.exit();
        m_event.Set();
    }

    public override bool isBusy()
    {
        return false;
    }

    public void addAuthReq(AuthInfoReq info)
    {
        if (info == null) return;

        lock (m_syn)
        {
            m_taskQue.Enqueue(info);
            m_event.Set();
        }
    }

    private void run()
    {
        while (m_run)
        {
            m_event.WaitOne();

            AuthInfoReq data = null;
            while (m_taskQue.Count > 0)
            {
                lock (m_syn)
                {
                    data = m_taskQue.Dequeue();
                }

                if (!m_run)
                {
                    break;
                }

                if (data != null)
                {
                    CheckBase p = CheckBase.create(data);
                    if (p != null)
                    {
                        p.check(data);
                    }
                }
            }
        }
    }

    // 收到传过来的数据
    void onInputData(HttpListenerContext ctx, HttpReturn httpRes)
    {
        string opcode = ctx.Request.QueryString["opCode"];
        if (string.IsNullOrEmpty(opcode))
        {
            httpRes.m_retStr = "opcode empty";
            LogMgr.error("onInputData, opcode null");
            return;
        }

        int code = -1;
        if(int.TryParse(opcode, out code))
        {
            AuthInfoReq req = null;
            if (code == ConstDef.AUTH_CHECK)
            {
                req = parseCheck(code, ctx, httpRes);
            }
            else if(code == ConstDef.AUTH_QUEYR)
            {
                req = parseQuery(code, ctx, httpRes);
            }
            else if (code == ConstDef.AUTH_UPDATA)
            {
                req = parseUpdata(code, ctx, httpRes);
            }

            addAuthReq(req);
        }
        else
        {
            httpRes.m_retStr = "opcode not a num";
            LogMgr.error("onInputData, opcode not a num");
        }
    }

    /* 实名认证
        http://192.168.1.235:4676/?opCode=1&id=&idNum=&name=
        id    玩家id   
        idNum 身份证号  
        name  真实姓名

        返回
        cmd=8&status=&pi=&playerId=&opCode=1
        status 0：认证成功  1：认证中  2：认证失败
        pi  已通过实名认证用户的唯一标识    
    */
    AuthInfoReq parseCheck(int code, HttpListenerContext ctx, HttpReturn httpRes)
    {
        AuthInfoVerify req = new AuthInfoVerify();
        req.m_opCode = code;
        req.m_id = ctx.Request.QueryString["id"];
        if (string.IsNullOrEmpty(req.m_id))
        {
            httpRes.m_retStr = "id empty";
            LogMgr.error("parseCheck, id null");
            return null;
        }
        req.m_idNum = ctx.Request.QueryString["idNum"];
        if (string.IsNullOrEmpty(req.m_idNum))
        {
            httpRes.m_retStr = "idNum empty";
            LogMgr.error("parseCheck, idNum null");
            return null;
        }

        string queryData = Path.GetFileName(ctx.Request.RawUrl);
        var cd = System.Web.HttpUtility.ParseQueryString(queryData);
        req.m_name = cd.Get("name");
        if (string.IsNullOrEmpty(req.m_name))
        {
            httpRes.m_retStr = "name empty";
            LogMgr.error("parseCheck, name null");
            return null;
        }

        return req;
    }

    /* 数据上报
        http://192.168.1.235:4676/?opCode=3&session=&action=&userType=&deviceId=&pi=
        session  临时会话,上线临时生成
        action  0-下线 1-上线
        userType   0：已认证通过用户   2：游客用户
        deviceId    设备id
        pi    通过认证后的唯一标识符,是由实证认证接口返回的. 
    */
    AuthInfoReq parseUpdata(int code, HttpListenerContext ctx, HttpReturn httpRes)
    {
        AuthInfoUpdata req = new AuthInfoUpdata();
        req.m_opCode = code;
        string queryData = Path.GetFileName(ctx.Request.RawUrl);
        var qd = System.Web.HttpUtility.ParseQueryString(queryData);
        req.m_session = qd.Get("session");
        if (string.IsNullOrEmpty(req.m_session))
        {
            httpRes.m_retStr = "session empty";
            LogMgr.error("parseUpdata, session null");
            return null;
        }

        string action = qd.Get("action");
        if (string.IsNullOrEmpty(action))
        {
            httpRes.m_retStr = "action empty";
            LogMgr.error("parseUpdata, action null");
            return null;
        }
        req.m_action = Convert.ToInt32(action);

        string userType = qd.Get("userType");
        if (string.IsNullOrEmpty(userType))
        {
            httpRes.m_retStr = "userType empty";
            LogMgr.error("parseUpdata, userType null");
            return null;
        }
        req.m_userType = Convert.ToInt32(userType);

        req.m_deviceId = qd.Get("deviceId");
        if (req.m_deviceId == null)
        {
            req.m_deviceId = "";
        }

        req.m_pi = qd.Get("pi");
        if (req.m_pi == null)
        {
            req.m_pi = "";
        }

        return req;
    }

    /* 实名认证结果查询
       http://192.168.1.235:4676/?opCode=2&playerId=
       playerId  玩家id

       返回
        cmd=8&status=&pi=&playerId=&opCode=2

        status 0：认证成功  1：认证中  2：认证失败
        pi  已通过实名认证用户的唯一标识    
   */
    AuthInfoReq parseQuery(int code, HttpListenerContext ctx, HttpReturn httpRes)
    {
        AuthInfoQuery req = new AuthInfoQuery();
        req.m_opCode = code;
        string queryData = Path.GetFileName(ctx.Request.RawUrl);
        var qd = System.Web.HttpUtility.ParseQueryString(queryData);
        req.m_playerId = qd.Get("playerId");
        if (string.IsNullOrEmpty(req.m_playerId))
        {
            httpRes.m_retStr = "playerId empty";
            LogMgr.error("parseQuery, playerId null");
            return null;
        }
        return req;
    }
}



