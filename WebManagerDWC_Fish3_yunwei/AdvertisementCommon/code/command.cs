using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Web.Configuration;
using System.Text;

//////////////////////////////////////////////////////////////////////////
public class CommandBase
{
    protected int m_opRes;

    public virtual object getResult(object param)
    {
        return null;
    }

    public virtual string execute(CMemoryBuffer buf)
    {
        return "error";
    }

    public int getOpRes()
    {
        return m_opRes;
    }

    public void setOpRes(int res)
    {
        m_opRes = res;
    }

    public static CMemoryBuffer createBuf()
    {
        CMemoryBuffer buf = new CMemoryBuffer();
        buf.begin();
        return buf;
    }
}

////////////////////////////极光效果通//////////////////////////////////////////////
public class CommandAdvertisement : CommandBase 
{
    Dictionary<string, object> m_ret = new Dictionary<string, object>();
    public override object getResult(object param)
    {
        return null;
    }
    public override string execute(CMemoryBuffer buf)
    {
        m_ret.Clear();
        buf.begin();
        string muid = buf.Reader.ReadString();
        string active_cb = buf.Reader.ReadString();
        string reg_cb = buf.Reader.ReadString();
        try
        {
            AdvertisementXgt adata = new AdvertisementXgt();
            int ret_num = adata.Load(muid, active_cb, reg_cb);
            m_ret.Add("result", ret_num);
        }
        catch(System.Exception ex)
        {
        }
        return JsonHelper.genJson(m_ret);
    }
}