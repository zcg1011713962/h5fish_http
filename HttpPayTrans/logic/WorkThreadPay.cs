using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;

// 线程当前状态
public enum ThreadState
{
    state_null,
    state_idle,         // 空闲中
    state_busy,         // 忙碌中
    state_init,
}

public class OrderInfo
{
    public string m_orderId;
    public string m_payType;
    public string m_playerId;
}

public class WorkThreadPay : ServiceBase
{
    private ThreadState m_curState;
    private bool m_run = true;
    Queue<OrderInfo> m_taskQue = new Queue<OrderInfo>();
    object m_syn = new object();
    HttpService m_hservice;

    public WorkThreadPay()
    {
        m_sysType = (int)SType.ServerType_Order;
    }

    public override void initService()
    {
        m_curState = ThreadState.state_idle;
        Thread t = new Thread(new ThreadStart(run));
        t.Start();

        m_hservice = new HttpService();
        m_hservice.init(this);
    }

    public override void exitService()
    {
        m_run = false;
        m_hservice.exit();
    }

    public override bool isBusy()
    {
//         if (m_curState == ThreadState.state_busy)
//             return true;
// 
//         if (m_taskQue.Count > 0)
//             return true;

        return false;
    }

    public void addOrder(OrderInfo info)
    {
        lock (m_syn)
        {
            m_taskQue.Enqueue(info);
        }
    }

    private void run()
    {
        while (m_run)
        {
            switch (m_curState)
            {
                case ThreadState.state_idle:
                    {
                        if (m_taskQue.Count > 0)
                        {
                            m_curState = ThreadState.state_busy;
                        }
                        Thread.Sleep(1);
                    }
                    break;
                case ThreadState.state_busy:
                    {
                        OrderInfo data = null;
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
                                send(data);
                            }
                        }
                        m_curState = ThreadState.state_idle;
                    }
                    break;
            }
        }
    }

    void send(OrderInfo data)
    {
        try
        {
            string fmt = string.Format("cmd=7&playerId={0}&payType={1}&orderId={2}", data.m_playerId, data.m_payType, data.m_orderId);
            LogMgr.info(fmt);
            bool isNotify = ResMgr.getInstance().getBool(JsonCfg.JSON_CONFIG, "isNotify");
            if (!isNotify) return;

            string notifyUrl = ResMgr.getInstance().getString(JsonCfg.JSON_CONFIG, "notifyUrl");
            string url = notifyUrl + fmt;
            var ret = HttpPost.Get(new Uri(url));
            if (ret != null)
            {
                string retStr = Encoding.UTF8.GetString(ret);
                if (retStr != "ok")
                {
                    LogMgr.error("WorkThreadPay.send " + retStr);
                }
            }
        }
        catch (System.Exception ex)
        {
            LogMgr.error(ex.ToString());
        }
    }
}



