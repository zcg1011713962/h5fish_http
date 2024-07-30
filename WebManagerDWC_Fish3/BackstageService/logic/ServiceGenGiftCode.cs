using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

// 礼包码生成服务
public class ServiceGenGiftCode : ServiceBase
{
    private ThreadState m_curState;
    private bool m_run = true;
    private object m_lockObj = new object();
    
    // 待生成队列
    private Queue<ParamGenGiftCode> m_taskQue = new Queue<ParamGenGiftCode>();
    private Thread m_thread = null;
    private Random m_rand = new Random();

    char[] m_alphaLower = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    char[] m_alphaUpper = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    char[] m_digit = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    public ServiceGenGiftCode()
    {
        m_sysType = ServiceType.serviceTypeGiftCode;
    }

    public override void initService()
    {
        m_curState = ThreadState.state_idle;
        m_thread = new Thread(new ThreadStart(run));
        m_thread.Start();
    }

    public override void exitService()
    {
        m_run = false;
        m_thread = null;
    }

    public override bool isBusy()
    {
        if (m_curState == ThreadState.state_busy)
            return true;

        if (m_taskQue.Count > 0)
            return true;

        return false;
    }

    public override int doService(ServiceParam param)
    {
        if (param == null || m_thread == null)
            return (int)RetCode.ret_error;

        ParamGenGiftCode p = (ParamGenGiftCode)param;
        lock (m_lockObj)
        {
            m_taskQue.Enqueue(p);
            m_curState = ThreadState.state_busy;
        }
        return (int)RetCode.ret_success;
    }

    private void run()
    {
        while (m_run)
        {
            switch (m_curState)
            {
                case ThreadState.state_idle:
                    {
                        Thread.Sleep(5000);
                    }
                    break;
                case ThreadState.state_busy:
                    {
                        ParamGenGiftCode data = null;
                        DateTime now = DateTime.Now;
                        while (m_taskQue.Count > 0)
                        {
                            lock (m_lockObj)
                            {
                                data = m_taskQue.Dequeue();
                            }

                            if (!m_run)
                            {
                                break;
                            }

                            if (data != null)
                            {
                                genGiftCode(data, now);
                            }
                        }
                        m_curState = ThreadState.state_idle;
                    }
                    break;
            }
        }
    }

    public void genGiftCode(ParamGenGiftCode codeParam, DateTime now)
    {
        int i = 0;
        List<Dictionary<string, object>> docList = new List<Dictionary<string, object>>();

        XmlConfig xml = ResMgr.getInstance().getRes("dbserver.xml");
        string accServer = xml.getString("account", "");

        int serverId = DBMgr.getInstance().getDbId(accServer);
        if (serverId == -1)
        {
            LogMgr.error("genGiftCode, cannot find accdb, {0}", accServer);
            return;
        }

        string cdkey = "";

        for (i = 0; i < codeParam.m_count; i++)
        {
            while (true)
            {
                cdkey = getRandStringAndEncrypt(8, codeParam.m_pici);

                bool exist = DBMgr.getInstance().keyExists(TableName.GIFT_CODE, "cdkey", cdkey, serverId, DbName.DB_ACCOUNT);
                if (!exist)
                    break;
            }

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("genTime", now.Date);
            data.Add("cdkey", cdkey);
            data.Add("pici", codeParam.m_pici);
            data.Add("playerId", -1);
            docList.Add(data);
        }

        if (docList.Count > 0)
        {
            DBMgr.getInstance().insertData(TableName.GIFT_CODE, docList, serverId, DbName.DB_ACCOUNT);
        }
    }

    private Dictionary<string, object> createOneCode(long giftId, string plat, DateTime now)
    {
        Dictionary<string, object> tmp = new Dictionary<string, object>();
        tmp.Add("genTime", now);
        // 礼包码
        tmp.Add("giftCode", generateStringID());
        // 对应的礼包ID
        tmp.Add("giftId", giftId);
        // 为哪个平台生成的
        tmp.Add("plat", plat); 

        // 玩家使用礼包码时所在服务器id
        tmp.Add("playerServerId", -1);
        // 玩家平台
        tmp.Add("playerPlat", "");
        tmp.Add("playerId", -1);
        tmp.Add("playerAcc", "");
        // 使用时间
        tmp.Add("useTime", DateTime.MinValue);
        return tmp;
    }

    private string generateStringID()
    {
        long i = 1;
        byte[] arr = Guid.NewGuid().ToByteArray();
        foreach (byte b in arr)
        {
            i *= ((int)b + 1);
        }
        return string.Format("{0:x}", i - DateTime.Now.Ticks);
    }

    public string getRandStringAndEncrypt(int bitCount, int pici)
    {
        string str = getRandString(bitCount, pici);
        return AESHelper.AESEncrypt(str, ConstDef.AES_FOR_CDKEY);
    }

    public string getRandString(int bitCount, int pici)
    {
        char[] chs = new char[bitCount];
        Stack<char> st = calPiciChar(pici);
        int i = bitCount - 1;
        while (st.Count > 0)
        {
            char t = st.Pop();
            chs[i] = t;
            i--;
        }
        chs[i] = getRandChar(m_alphaLower);
        i--;

        while (i >= 0)
        {
            chs[i] = getRandChar();
            i--;
        }
        return new String(chs);
    }

    char getRandChar()
    {
        char[] buf = null;
        int r = m_rand.Next(3);
        switch (r)
        {
            case 0:
                {
                    buf = m_alphaLower;
                }
                break;
            case 1:
                {
                    buf = m_alphaUpper;
                }
                break;
            case 2:
                {
                    buf = m_digit;
                }
                break;
        }

        return getRandChar(buf);
    }

    char getRandChar(char[] buf)
    {
        int index = m_rand.Next(buf.Length);
        return buf[index];
    }

    public Stack<int> calcPiciIndex(int pici)
    {
        Stack<int> st = new Stack<int>();

        int n = pici;
        while (n > 0)
        {
            int tmp = n % 36;
            n = n / 36;
            st.Push(tmp);
        }

        return st;
    }

    Stack<char> calPiciChar(int pici)
    {
        Stack<char> result = new Stack<char>();
        Stack<int> st = calcPiciIndex(pici);
        while (st.Count > 0)
        {
            int t = st.Pop();
            result.Push(getPiciChar(t));
        }

        return result;
    }

    char getPiciChar(int index)
    {
        if (index < m_digit.Length)
        {
            return m_digit[index];
        }

        return m_alphaUpper[index - m_digit.Length];
    }
}



