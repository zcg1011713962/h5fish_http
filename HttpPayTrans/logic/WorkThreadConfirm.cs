using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Collections.Concurrent;

public enum ConfirmType
{
    Confirm_QQGame,
}

public class ConfirmInfo
{
    public IConfirm m_obj;
    public string m_key;
}

public class WorkThreadConfirm : ServiceBase
{
    private ThreadState m_curState;
    private bool m_run = true;

    protected Dictionary<ConfirmType, IConfirm> m_confirm = new Dictionary<ConfirmType, IConfirm>();

    protected ConcurrentQueue<ConfirmInfo> m_que = new ConcurrentQueue<ConfirmInfo>();

    public WorkThreadConfirm()
    {
        m_sysType = (int)SType.ServerType_Confirm;
    }

    public override void initService()
    {
        m_curState = ThreadState.state_null; 
    }

    public override void exitService()
    {
        m_run = false;
    }

    public void addConfirmObj(ConfirmType t, IConfirm c)
    {
        if (c == null)
        {
            LogMgr.error("addConfirmObj, obj null");
            return;
        }
        if (!m_confirm.ContainsKey(t))
        {
            m_confirm.Add(t, c);
        }
    }

    public void startWork()
    {
        m_curState = ThreadState.state_init;
        Thread t = new Thread(new ThreadStart(run));
        t.Start();
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

    public static ConfirmType getConfirmType(string payType)
    {
        return ConfirmType.Confirm_QQGame;
    }

    public void addNewConfirmData(string payType, string key)
    {
        ConfirmType t = getConfirmType(payType);
        if (m_confirm.ContainsKey(t))
        {
            var d = new ConfirmInfo();
            d.m_key = key;
            d.m_obj = m_confirm[t];
            m_que.Enqueue(d);
        }
    }

    private void run()
    {
        while (m_run)
        {
            switch (m_curState)
            {
                case ThreadState.state_init:
                    {
                        try
                        {
                            foreach (var d in m_confirm)
                            {
                                if (d.Value != null)
                                {
                                    d.Value.loadFromDb();
                                }
                            }
                            m_curState = ThreadState.state_busy;
                        }
                        catch(Exception ex)
                        {
                            LogMgr.error(ex.ToString());
                        }
                    }
                    break;
                case ThreadState.state_busy:
                    {
                        try
                        {
                            foreach (var d in m_confirm)
                            {
                                if (d.Value != null)
                                {
                                    d.Value.update();
                                }
                            }

                            ConfirmInfo info = null;
                            while (!m_que.IsEmpty)
                            {
                                bool res = m_que.TryDequeue(out info);
                                if (res)
                                {
                                    info.m_obj.notifyLoad(info.m_key);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogMgr.error(ex.ToString());
                        }
                           
                        Thread.Sleep(1);
                    }
                    break;
            }
        }
    }
}



