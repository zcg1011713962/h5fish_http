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
    WorkThreadAuthen m_state;
    HttpListener m_listerner;

    public Action<HttpListenerContext, HttpReturn> EventInputData;

    HttpReturn m_httpRes = new HttpReturn();

    public void init(WorkThreadAuthen state)
    {
        m_httpRes.reset();
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

    void parseIPList(string str, HttpListener listener )
    {
        string[] arr = str.Split(';');
        for(int i = 0; i < arr.Length;i++)
        {
            listener.Prefixes.Add(arr[i]);
        }
    }

    private void run()
    {
        try
        {
            m_listerner = new HttpListener();
            string ip = ResMgr.getInstance().getString(JsonCfg.JSON_CONFIG, "addr");
            parseIPList(ip, m_listerner);
            //m_listerner.Prefixes.Add(ip);
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

        if(EventInputData != null)
        {
            m_httpRes.reset();
            EventInputData(ctx, m_httpRes);
        }
        else
        {
            LogMgr.error("HttpService.process event null");
        }
    }

    void returnResult(HttpListenerContext ctx)
    {
        //使用Writer输出http响应代码  
        using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream))
        {
            writer.Write(m_httpRes.m_retStr);
            writer.Close();
            ctx.Response.Close();
        }
    }
}

