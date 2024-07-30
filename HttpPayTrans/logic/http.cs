using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;

public class HttpService
{
    Thread m_thread;
    bool m_isRun = true;
    WorkThreadPay m_state;
    HttpListener m_listerner;

    public void init(WorkThreadPay state)
    {
        m_state = state;
        m_thread = new Thread(new ThreadStart(run));
        m_thread.Start();
    }

    public void exit()
    {
        try
        {
            m_isRun = false;
            m_listerner.Abort();
        }
        catch (System.Exception ex)
        {
        }
    }

    private void run()
    {
        try
        {
            m_listerner = new HttpListener();
            string ip = ResMgr.getInstance().getString(JsonCfg.JSON_CONFIG, "addr");
            m_listerner.Prefixes.Add(ip);
            m_listerner.Start();

            while (m_isRun)
            {
                try
                {
                    HttpListenerContext ctx = m_listerner.GetContext();
                    process(ctx);
                    returnResult(ctx);
                }
                catch (System.Exception ex)
                {
                    LogMgr.error(ex.ToString());
                }
            }
        }
        catch (System.Exception ex)
        {
            LogMgr.error(ex.ToString());
        }
    }

    private void process(HttpListenerContext ctx)
    {
        if (ctx == null) return;

        string playerId = ctx.Request.QueryString["playerId"];
        if (string.IsNullOrEmpty(playerId))
        {
            LogMgr.error("HttpService.process, player null");
            return;
        }

        string orderId = ctx.Request.QueryString["orderId"];
        if (string.IsNullOrEmpty(orderId))
        {
            LogMgr.error("HttpService.process, orderId null");
            return;
        }

        string payType = ctx.Request.QueryString["payType"];
        if (string.IsNullOrEmpty(payType))
        {
            LogMgr.error("HttpService.process, payType null");
            return;
        }
        
        OrderInfo info = new OrderInfo();
        info.m_playerId = playerId;
        info.m_orderId = orderId;
        if (payType.IndexOf("_pay") >= 0)
        {
            payType = payType.Substring(0, payType.Length - 4);
        }
        
        info.m_payType = payType;
        m_state.addOrder(info);

        confirm(payType, orderId);
    }

    void returnResult(HttpListenerContext ctx)
    {
        //使用Writer输出http响应代码  
        using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream))
        {
            writer.Write("ok");
            writer.Close();
            ctx.Response.Close();
        }
    }

    void confirm(string payType, string key)
    {
        if (payType == DEF.NEED_CONFIRM_QQ)
        {
            WorkThreadConfirm obj = ServiceMgr.getInstance().getSys<WorkThreadConfirm>((int)SType.ServerType_Confirm);
            if (obj != null)
            {
                obj.addNewConfirmData(payType, key);
            }
        }
    }
}

