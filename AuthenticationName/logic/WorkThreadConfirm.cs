using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Collections.Concurrent;

public class WorkThreadConfirm : ServiceBase
{
    private bool m_run = true;

    protected ConcurrentQueue<AuthInfoRep> m_que = new ConcurrentQueue<AuthInfoRep>();

    protected AutoResetEvent m_event = new AutoResetEvent(false);

    public WorkThreadConfirm()
    {
        m_sysType = (int)SType.ServerType_Confirm;
    }

    public override void initService()
    {
        startWork();
    }

    public override void exitService()
    {
        m_run = false;
        m_event.Set();
    }

    public void startWork()
    {
        Thread t = new Thread(new ThreadStart(run));
        t.Start();
    }

    public override bool isBusy()
    {
        return false;
    }

    public void addResult(AuthInfoRep rep)
    {
        if(rep != null)
        {
            m_que.Enqueue(rep);
            m_event.Set();
        }
    }

    private void run()
    {
        while (m_run)
        {
            m_event.WaitOne();

            AuthInfoRep info = null;
            while (!m_que.IsEmpty)
            {
                bool res = m_que.TryDequeue(out info);
                if (res)
                {
                    retMsg(info);
                }
            }
        }
    }

    void retMsg(AuthInfoRep info)
    {
        if(info.m_opCode == ConstDef.AUTH_CHECK)
        {
            retCheckMsg((AuthInfoCheckRep)info.m_param);
        }
        else if (info.m_opCode == ConstDef.AUTH_QUEYR)
        {
            retQuerykMsg((AuthInfoQueryRep)info.m_param);
        }
        else if (info.m_opCode == ConstDef.AUTH_UPDATA)
        {

        }
    }

    void retCheckMsg(AuthInfoCheckRep rep)
    {
        string fmt = string.Format("cmd={0}&status={1}&pi={2}&playerId={3}&opCode={4}",
            ConstDef.FMT_CODE,
            rep.status,
            rep.pi,
            rep.m_playerId,
            ConstDef.AUTH_CHECK);

        retMessageToGameServer(fmt);
    }

    void retQuerykMsg(AuthInfoQueryRep rep)
    {
        string fmt = string.Format("cmd={0}&status={1}&pi={2}&playerId={3}&opCode={4}",
            ConstDef.FMT_CODE,
            rep.m_status,
            rep.m_pi,
            rep.m_playerId,
            ConstDef.AUTH_QUEYR);

        retMessageToGameServer(fmt);
    }

    // 向服务器返回消息
    void retMessageToGameServer(string fmt)
    {
        try
        {
            string notifyUrl = ResMgr.getInstance().getString(JsonCfg.JSON_CONFIG, "notifyUrl");
            string url = notifyUrl + fmt;
            var ret = HttpPost.Get(new Uri(url));
            if (ret != null)
            {
                string retStr = Encoding.UTF8.GetString(ret);
                if (retStr != "ok")
                {
                    LogMgr.error("WorkThreadConfirm.send " + retStr);
                }
            }
        }
        catch (System.Exception ex)
        {
            LogMgr.error(ex.ToString());
        }
    }
}



