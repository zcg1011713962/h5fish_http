using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

public class PageGen
{
    // 每页多少条记录
    protected int m_rowEachPage = 50;

    // 当前页
    protected int m_curPage = 1;

    public PageGen()
    {
    }

    public PageGen(int row_each_page)
    {
        m_rowEachPage = row_each_page;
    }

    public int rowEachPage
    {
        get { return m_rowEachPage; }
        set { m_rowEachPage = value; }
    }

    public int curPage
    {
        get { return m_curPage; }
        set { m_curPage = value; }
    }

    public virtual bool parse(HttpRequest Request)
    {
        bool res = false;
        string page = Request.QueryString["page"];
        if (page != null)
        {
            m_curPage = Convert.ToInt32(page);
            res = true;
        }
        return res;
    }

    // 生成分页
    public virtual void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        long total_page = 0;
        PageBrowseGenerator p = new PageBrowseGenerator();
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//玩家炮倍破产详情
public class PagePlayerOpenRateBankrupt : PageGen
{
    public string m_time = "";
    public string m_playerId;
    public string m_turret;
    public PagePlayerOpenRateBankrupt(int row_each_page)
        : base(row_each_page)
    {
    }
    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["playerId"];
        if (str != null)
        {
            m_playerId = str;
            res = true;
        }

        str = Request.QueryString["turret"];
        if (str != null)
        {
            m_turret = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["playerId"] = dparam.m_playerId;
        urlParam["turret"] = dparam.m_param;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//玩家道具详情
public class PagePlayerItem : PageGen
{
    public string m_time = "";
    public string m_playerId;
    public string m_itemId;
    public string m_syncKey = "";
    public int m_op;
    public PagePlayerItem(int row_each_page)
        : base(row_each_page)
    {
    }
    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["playerId"];
        if (str != null)
        {
            m_playerId = str;
            res = true;
        }

        str = Request.QueryString["itemId"];
        if (str != null)
        {
            m_itemId = str;
            res = true;
        }

        str=Request.QueryString["syncKey"];
        if(str!=null)
        {
            m_syncKey = str;
            res = true;
        }

        str = Request.QueryString["op"];
        if (str != null) 
        {
            m_op = Convert.ToInt32(str);
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["playerId"] = dparam.m_playerId;
        urlParam["itemId"] = dparam.m_param;
        urlParam["syncKey"] = dparam.m_channelNo;
        urlParam["op"] = Convert.ToInt32(dparam.m_op);
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//高级场奖池统计详情
public class PageGenFishlordAdvancedRoom : PageGen
{
    public string m_time = "";

    public PageGenFishlordAdvancedRoom(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;

        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//七日连充购买详情
public class PageGenStatNp7DayRechargeDetail : PageGen
{
    public string m_time = "";
    public string m_channel = "";
    public PageGenStatNp7DayRechargeDetail(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }

        str = Request.QueryString["channel"];
        if (str != null)
        {
            m_channel = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["channel"] = dparam.m_param;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//南海寻宝玩家详情
public class PageTreasureHuntPlayer : PageGen
{
    public int m_roomId;
    public string m_time = "";
    public string m_playerId;
    public PageTreasureHuntPlayer(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);
        string str = Request.QueryString["roomId"];
        if (str != null)
        {
            m_roomId = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["playerId"];
        if (str != null)
        {
            m_playerId = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["roomId"] = (int)dparam.m_op;
        url_param["time"] = dparam.m_time;
        url_param["playerId"] = dparam.m_param;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}

//////////////////////////////////////////////////////////////////////////
//鱼统计
public class PageStatFish : PageGen
{
    public string m_time = "";
    public int m_roomid;

    public PageStatFish(int row_each_page)
        : base(row_each_page)
    {
    }
    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["roomid"];
        if (str != null)
        {
            m_roomid = Convert.ToInt32(str);
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["roomid"] = dparam.m_op;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//活动时间查询
public class PageStatActCFG : PageGen
{
    public string m_time = "";
    public int m_op;

    public PageStatActCFG(int row_each_page)
        : base(row_each_page)
    {
    }
    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["op"];
        if (str != null)
        {
            m_op = Convert.ToInt32(str);
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["op"] = (int)dparam.m_op;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//捕鱼盛宴活动
public class PageFishlordFeast : PageGen
{
    public string m_time = "";
    public int m_op;
    public PageFishlordFeast(int row_each_page)
        : base(row_each_page)
    {
    }
    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["way"];
        if (str != null)
        {
            m_op = Convert.ToInt32(str);
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["way"] = (int)dparam.m_op;

        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//玩家聊天记录查询
public class PagePlayerChat : PageGen
{
    public string m_time = "";
    public string m_playerId;
    public PagePlayerChat(int row_each_page)
        : base(row_each_page)
    {
    }
    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["playerId"];
        if (str != null)
        {
            m_playerId = str;
            res = true;
        }
        return res;
    }
    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        if(!string.IsNullOrEmpty(dparam.m_time))
        {
            urlParam["time"] = dparam.m_time;
        }
        if(!string.IsNullOrEmpty(dparam.m_playerId))
        {
            urlParam["playerId"] = dparam.m_playerId;
        }
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//黑红分页
public class PageShcdCards : PageGen
{
    public string m_time = "";
    public string m_param;
    public string m_playerId;
    public int m_op;
    public PageShcdCards(int row_each_page)
        : base(row_each_page)
    {
    }
    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["param"];
        if (str != null)
        {
            m_param = str;
            res = true;
        }
        str = Request.QueryString["op"];
        if (str != null)
        {
            m_op = Convert.ToInt32(str);
            res = true;
        }
        return res;
    }
    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["param"] = dparam.m_param;
        urlParam["op"] = (int)dparam.m_op;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//牛牛分页
public class PageCowsCard : PageGen
{
    public string m_time = "";
    public string m_param;
    public string m_playerId;
    public int m_op;
    public PageCowsCard(int row_each_page)
        : base(row_each_page)
    {
    }
    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["param"];
        if(str!=null)
        {
            m_param = str;
            res = true;
        }
        str=Request.QueryString["playerId"];
        if(str!=null)
        {
            m_playerId = str;
            res = true;
        }
        str=Request.QueryString["op"];
        if(str!=null)
        {
            m_op = Convert.ToInt32(str);
            res = true;
        }
        return res;
    }
    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["param"] = dparam.m_param;
        urlParam["playerId"] = dparam.m_playerId;
        urlParam["op"] = (int)dparam.m_op;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//爆金比赛场
public class PageFishBaojin : PageGen 
{
    public string m_time = "";
    public int m_param;
    public int m_way;
    public int m_showWay;
    public int m_op;
    public PageFishBaojin(int row_each_page)
        : base(row_each_page)
    {
    }
    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str=Request.QueryString["param"];
        if(str!=null)
        {
            m_param = Convert.ToInt32(str);
            res = true;
        }
        str=Request.QueryString["way"];
        if(str!=null)
        {
            m_way = Convert.ToInt32(str);
            res = true;
        }
        str=Request.QueryString["showWay"];
        if(str!=null)
        {
            m_showWay = Convert.ToInt32(str);
            res = true;
        }
        str=Request.QueryString["op"];
        if(str!=null)
        {
            m_op = Convert.ToInt32(str);
            res = true;
        }
        return res;
    }
    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["way"] = (int)dparam.m_way;
        urlParam["time"] = dparam.m_time;
        urlParam["op"]=(int)dparam.m_op;
        if(Convert.ToInt32(dparam.m_op)!=1)
        {
            urlParam["param"] = Convert.ToInt32(dparam.m_param);
        }
        
        if (Convert.ToInt32(dparam.m_way) == 1) 
        {
            urlParam["param"] = Convert.ToInt32(dparam.m_param);
            urlParam["showWay"] = Convert.ToInt32(dparam.m_showWay);
        }
        
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//集玩偶
public class PageCollectPuppet : PageGen 
{
    public string m_time = "";
    public int m_way;
    public int m_type;
    public PageCollectPuppet(int row_each_page)
        : base(row_each_page)
    {
    }
    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str=Request.QueryString["way"];
        if(str!=null)
        {
            m_way = Convert.ToInt32(str);
            res = true;
        }
        str=Request.QueryString["type"];
        if(str!=null)
        {
            m_type = Convert.ToInt32(str);
            res = true;
        }
        return res;
    }
    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["way"] = (int)dparam.m_way;
        urlParam["type"] = Convert.ToInt32(dparam.m_showWay);
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//拉霸抽奖
public class PageLabaLottery : PageGen 
{
    public string m_time = "";
    public string m_playerId;
    public int m_way;
    public PageLabaLottery(int row_each_page)
        : base(row_each_page)
    {
    }
    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["way"];
        if (str != null)
        {
            m_way = Convert.ToInt32(str);
            res = true;
        }
        str=Request.QueryString["playerId"];
        if(str!=null)
        {
            m_playerId = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["playerId"] = dparam.m_param;
        urlParam["way"] = (int)dparam.m_way;

        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//话费鱼统计
public class PagePumpChipFish : PageGen 
{
     public string m_time = "";
    public string m_param = "";
    public PagePumpChipFish(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str=Request.QueryString["fishId"];
        if(str!=null)
        {
            m_param = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["fishId"]=dparam.m_param;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//星星抽奖
public class PageStarLottery : PageGen 
{
    public string m_time = "";
    public string m_param = "";
    public PageStarLottery(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str=Request.QueryString["level"];
        if(str!=null)
        {
            m_param = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["level"]=dparam.m_param;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
/////////////////////////////////////////////////////////////////////////
//转盘抽奖
public class PageDialLottery : PageGen
{
    public string m_time = "";
    public PageDialLottery(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;

        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
/////////////////////////////////////////////////////////////////////////
//比武场数据统计
public class PagePlayerPumpBw : PageGen
{
    public string m_time = "";
    public PagePlayerPumpBw(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
/////////////////////////////////////////////////////////////////////////
//水浒传单个玩家盈利率参考
public class PageShuihzSingleEarning : PageGen
{
    public string m_time = "";
    public string m_param = "";
    public PageShuihzSingleEarning(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);
        string str = Request.QueryString["param"];
        if (str != null)
        {
            m_param = str;
            res = true;
        }
        str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["param"] = dparam.m_param;
        urlParam["time"] = dparam.m_time;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
/// /////////////////////////////////////////////////////////////////////
// 系统空投分页
public class PageGenAirdropSys : PageGen 
{
    public int m_state;
    public string m_playerId;
    public int m_type;
    public string m_time;

    public int m_airDropType;

    public PageGenAirdropSys(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["state"];
        if (str != null)
        {
            m_state = Convert.ToInt32(str);
            res = true;
        }

        str = Request.QueryString["playerId"];
        if (str != null)
        {
            m_playerId = str;
            res = true;
        }

        str = Request.QueryString["type"];
        if (str != null)
        {
            m_type = Convert.ToInt32(str);
            res = true;
        }

        str = Request.QueryString["airPlayer"];
        if (str != null)
        {
            m_airDropType = Convert.ToInt32(str);
            res = true;
        }

        str = Request.QueryString["time"];
        if (!string.IsNullOrEmpty(str))
        {
            m_time = Convert.ToString(str);
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["state"] = dparam.m_op;
        urlParam["playerId"] = dparam.m_playerId;
        urlParam["type"] = dparam.m_type;
        urlParam["airPlayer"] = dparam.m_param;
        urlParam["time"] = dparam.m_time;


        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }

}
////////////////////////////////////////////////////////////////////////
public class PageGenMoney : PageGen
{
    public string m_time = "";
    public string m_param = "";
    public int m_way;
    public int m_filter;
    public int m_property;
    public string m_range = "";
    public int m_gameId;

    public PageGenMoney(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["param"];
        if (str != null)
        {
            m_param = str;
            res = true;
        }
        str = Request.QueryString["way"];
        if (str != null)
        {
            m_way = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["filter"];
        if (str != null)
        {
            m_filter = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["property"];
        if (str != null)
        {
            m_property = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["range"];
        if (str != null)
        {
            m_range = str;
            res = true;
        }
        str = Request.QueryString["gameId"];
        if (str != null)
        {
            m_gameId = Convert.ToInt32(str);
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamMoneyQuery dparam = (ParamMoneyQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["param"] = dparam.m_param;
        urlParam["time"] = dparam.m_time;
        urlParam["way"] = (int)dparam.m_way;
        urlParam["filter"] = dparam.m_filter;
        urlParam["property"] = dparam.m_property;
        urlParam["range"] = dparam.m_range;
        urlParam["gameId"] = dparam.m_gameId;

        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}

//////////////////////////////////////////////////////////////////////////
//添加同步道具失败
public class PageGenItemError : PageGen
{
    public string m_time = "";
    public string m_playerId;
    public string m_syncKey = "";
    public PageGenItemError(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);
        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["playerId"];
        if (str != null)
        {
            m_playerId = str;
            res = true;
        } 
        str = Request.QueryString["syncKey"];
        if (str != null)
        {
            m_syncKey = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["time"] = dparam.m_time;
        url_param["playerId"] = dparam.m_playerId;
        url_param["syncKey"] = dparam.m_channelNo;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
public class PageGenDailyTask : PageGen
{
    public string m_time = "";
    public string m_param = "";
    public int m_way;
    public string m_channelNo = "";

    public PageGenDailyTask(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["channelNo"];
        if (str != null)
        {
            m_channelNo = str;
            res = true;
        }
        str = Request.QueryString["param"];
        if (str != null)
        {
            m_param = str;
            res = true;
        }
        str = Request.QueryString["way"];
        if (str != null)
        {
            m_way = Convert.ToInt32(str);
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["param"] = dparam.m_param;
        url_param["time"] = dparam.m_time;
        url_param["way"] = (int)dparam.m_way;
        url_param["channelNo"] = dparam.m_channelNo;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}
////////////////////////////////////////////////////////////////////////////
//邮件查询分页
public class PageGenMail : PageGen
{
    public string m_time = "";
    public string m_param = "";
    public int m_way;
    public int m_isDel;

    public PageGenMail(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str=Request.QueryString["way"];
        if(str!=null)
        {
            m_way = Convert.ToInt32(str);
            res = true;
        }
        str=Request.QueryString["param"];
        if(str!=null)
        {
            m_param = str;
            res = true;
        }
        str=Request.QueryString["isDel"];
        if(str!=null)
        {
            m_isDel = Convert.ToInt32(str);
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["param"] = dparam.m_param;
        url_param["time"] = dparam.m_time;
        url_param["way"] = (int)dparam.m_way;
        url_param["isDel"] = (int)dparam.m_op;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}

//////////////////////////////////////////////////////////////////////////

// 抽奖
public class PageGenLottery : PageGen
{
    public string m_time = "";
    public string m_playerId = "";
    public string m_boxId = "";
    public string m_param = "";

    public PageGenLottery(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["playerId"];
        if (str != null)
        {
            m_playerId = str;
            res = true;
        }
        str = Request.QueryString["boxId"];
        if (str != null)
        {
            m_boxId = str;
            res = true;
        }
        str = Request.QueryString["param"];
        if (str != null)
        {
            m_param = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamLottery dparam = (ParamLottery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["playerId"] = dparam.m_playerId;
        urlParam["boxId"] = dparam.m_boxId;
        urlParam["param"] = dparam.m_param;
        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}

//////////////////////////////////////////////////////////////////////////
//收支错误的分页查询
public class PageGenIncomeExpensesError : PageGen 
{
    public string m_time = "";
    public string m_showWay;

    public PageGenIncomeExpensesError(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["showWay"];
        if (str != null)
        {
            m_showWay = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase queryParam, string url, ref string pageLink, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)queryParam;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> urlParam = new Dictionary<string, object>();
        urlParam["time"] = dparam.m_time;
        urlParam["showWay"] = dparam.m_showWay;

        pageLink = p.genPageFoot(queryParam.m_curPage, m_rowEachPage, url, ref total_page, user, urlParam);
        if (total_page != 0)
        {
            foot = queryParam.m_curPage + "/" + total_page;
        }
    }
}

///////////////////////////////////////////////////////////////////////////////////////
//内部库
public class PageInnerPlayer : PageGen 
{
    public string m_time;
    public PageInnerPlayer(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);
        string str = Request.QueryString["time"];
        if (str != null) 
        {
            m_time = str;
            res = true;
        }
        return res;
    }
    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["time"] = dparam.m_time;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}
///////////////////////////////////////////////////////////////////////////////////////
//活动幸运宝箱详情
public class PagePanicBoxPlayer : PageGen 
{
    public int m_boxId;
    public string m_time = "";
    public PagePanicBoxPlayer(int rowEachPage)
        : base(rowEachPage) 
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);
        string str = Request.QueryString["boxId"];
        if(str!=null)
        {
            m_boxId = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["time"];
        if(str!=null)
        {
            m_time = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["boxId"] = (int)dparam.m_op;
        url_param["time"] = dparam.m_time;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}

//金币玩法每局下注玩家列表
public class PageGoldBetPlayer : PageGen
{
    public long m_turnId;
    public PageGoldBetPlayer(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);
        string str = Request.QueryString["turnId"];
        if (str != null)
        {
            m_turnId = Convert.ToInt64(str);
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["turnId"] = Convert.ToInt64(dparam.m_param);
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}

//破产统计详情
public class PagePlayerBankruptDetail : PageGen
{
    public int m_lotteryId;
    public string m_time = "";
    public string m_param = "";
    public PagePlayerBankruptDetail(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);
        string str = Request.QueryString["lotteryId"];
        if (str != null)
        {
            m_lotteryId = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["channelType"];
        if (str != null)
        {
            m_param = str;
            res = true;
        }

        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["lotteryId"] = (int)dparam.m_op;
        url_param["time"] = dparam.m_time;
        url_param["channelType"] = dparam.m_param;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}

//国庆中秋快乐抽奖详情
public class PageJiuQiuNationalActPlayer : PageGen
{
    public int m_lotteryId;
    public string m_time = "";
    public PageJiuQiuNationalActPlayer(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);
        string str = Request.QueryString["lotteryId"];
        if (str != null)
        {
            m_lotteryId = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["lotteryId"] = (int)dparam.m_op;
        url_param["time"] = dparam.m_time;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}

//新进玩家付费监控
public class PageTdNewPlayerRechargeMonitor : PageGen 
{
    public string m_time = "";

    public string m_channel = "";

    public PageTdNewPlayerRechargeMonitor(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);
        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }

        str = Request.QueryString["channel"];
        if (str != null)
        {
            m_channel = str;
            res = true;
        }

        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["time"] = dparam.m_time;
        url_param["channel"] = dparam.m_channelNo;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}

//开服抽奖详情
public class PageStatSdLotteryActDetail : PageGen
{
    public int m_lotteryId;
    public string m_time = "";

    public string m_channel = "";

    public PageStatSdLotteryActDetail(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);
        string str = Request.QueryString["lotteryId"];
        if (str != null)
        {
            m_lotteryId = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }

        str = Request.QueryString["channel"];
        if (str != null)
        {
            m_channel = str;
            res = true;
        }

        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["lotteryId"] = (int)dparam.m_op;
        url_param["time"] = dparam.m_time;
        url_param["channel"] = dparam.m_channelNo;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}

//在线奖励
public class PageGenOnlineReward : PageGen 
{
    public string m_channelID = "";
    public string m_time = "";
    public PageGenOnlineReward(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);
        string str = Request.QueryString["channelID"];
        if (str != null)
        {
            m_channelID = str;
            res = true;
        }
        str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["channelID"] = dparam.m_channelNo;
        url_param["time"] = dparam.m_time;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}

//客服补单/大户随访/换包福利-系统
public class PageGenRepairOrder : PageGen 
{
    public string m_time = "";

    public PageGenRepairOrder(int row_each_page) 
        : base(row_each_page) { }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);
        string str = Request.QueryString["time"];
        if(str!=null){
            m_time = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["time"] = dparam.m_time;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}

//流失大户筛选
public class PageSelectLostPlayers : PageGen 
{
    public string m_vipLevel = "";
    public string m_days = "";
    public string m_time = "";
    public int m_isBindPhone;
    public int m_sort;

    public PageSelectLostPlayers(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["vip"];
        if (str != null)
        {
            m_vipLevel = str;
            res = true;
        }
        str = Request.QueryString["days"];
        if (str != null)
        {
            m_days = Convert.ToString(str);
            res = true;
        }
        str=Request.QueryString["time"];
        if(str!=null)
        {
            m_time = Convert.ToString(str);
            res = true;
        }
        str = Request.QueryString["isBindPhone"];
        if (str != null)
        {
            m_isBindPhone = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["sort"];
        if (str != null)
        {
            m_sort = Convert.ToInt32(str);
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamSelectLostPlayer dparam = (ParamSelectLostPlayer)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["vip"] = dparam.m_vipLevel;
        url_param["days"] = dparam.m_days;
        url_param["isBindPhone"] = (int)dparam.m_isBindPhone;
        url_param["sort"] = dparam.m_op;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}

// 充值记录的分页查询
public class PageGenRecharge : PageGen
{
    public string m_platKey = "";
    public int m_result;
    public string m_range = "";
    public int m_way;
    public string m_param = "";
    public string m_time = "";
    public int m_serverIndex = 0;
    public string m_channelNo = "";
    public string m_rechargePoint = "";
    public int m_op;

    public PageGenRecharge(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["plat"];
        if (str != null)
        {
            m_platKey = str;
            res = true;
        }
        str = Request.QueryString["result"];
        if (str != null)
        {
            m_result = Convert.ToInt32(str);
            res = true;
        }
        str=Request.QueryString["op"];
        if(str!=null)
        {
            m_op = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["range"];
        if (str != null)
        {
            m_range = str;
            res = true;
        }
        str = Request.QueryString["way"];
        if (str != null)
        {
            m_way = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["param"];
        if (str != null)
        {
            m_param = str;
            res = true;
        }
        str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["server"];
        if (str != null)
        {
            m_serverIndex = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["channelNo"];
        if (str != null)
        {
            m_channelNo = str;
            res = true;
        }
        str =Request.QueryString["rechargePoint"];
        if(str!=null)
        {
            m_rechargePoint = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQueryRecharge dparam = (ParamQueryRecharge)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["param"] = dparam.m_param;
        url_param["way"] = (int)dparam.m_way;
        url_param["time"] = dparam.m_time;
        url_param["plat"] = dparam.m_platKey;
        url_param["result"] = dparam.m_result;
        url_param["range"] = dparam.m_range;
        url_param["server"] = dparam.m_gameServerIndex;
        url_param["channelNo"] = dparam.m_channelNo;
        url_param["rechargePoint"] = dparam.m_rechargePoint;
        url_param["op"] = (int)dparam.m_op;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
// 充值记录的分页查询
public class PageGenPlayerTurretAct : PageGen
{
    public string m_time = "";
    public int m_days;
    public string m_param = "";

    public PageGenPlayerTurretAct(int row_each_page)
        : base(row_each_page)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        str = Request.QueryString["days"];
        if (str != null)
        {
            m_days = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["param"];
        if (str != null)
        {
            m_param = str;
            res = true;
        }
        
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQuery dparam = (ParamQuery)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["param"] = dparam.m_param;
        url_param["days"] = dparam.m_op;
        url_param["time"] = dparam.m_time;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
//彩券兑换
public class PageChip : PageGen
{
    // 状态
    public string m_time;
    public string m_playerId = "";

    public PageChip(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }

        str = Request.QueryString["playerId"];
        if (str != null)
        {
            m_playerId = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQueryGift dparam = (ParamQueryGift)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["time"] = dparam.m_time;
        url_param["playerId"] = dparam.m_param;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////////////////////////////////////
// 礼券分页
public class PageGift : PageGen
{
    // 状态
    public int m_state = 0;
    public int m_type=0;
    public string m_playerId = "";

    public PageGift(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["state"];
        if (str != null)
        {
            m_state = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["type"];
        if (str != null)
        {
            m_type = Convert.ToInt32(str);
            res = true;
        }

        str = Request.QueryString["playerId"];
        if (str != null)
        {
            m_playerId = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQueryGift dparam = (ParamQueryGift)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["state"] = dparam.m_state;
        url_param["type"] = dparam.m_type;
        url_param["playerId"] = dparam.m_param;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}

////////////////////////////////
//兑换管理分页
public class PageExchange : PageGen
{
    // 状态
    public int m_state = 0;
    public int m_type = 0;
    public string m_playerId = "";

    public string m_channel;
    public string m_bindPhone;

    public PageExchange(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["state"];
        if (str != null)
        {
            m_state = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["type"];
        if (str != null)
        {
            m_type = Convert.ToInt32(str);
            res = true;
        }

        str = Request.QueryString["playerId"];
        if (str != null)
        {
            m_playerId = str;
            res = true;
        }

        str = Request.QueryString["channel"];
        if(str != null)
        {
            m_channel = str;
            res = true;
        }

        str = Request.QueryString["bindPhone"];
        if (str != null)
        {
            m_bindPhone = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQueryGift dparam = (ParamQueryGift)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["state"] = dparam.m_state;
        url_param["type"] = dparam.m_type;
        url_param["playerId"] = dparam.m_playerId;
        url_param["channel"] = dparam.m_channelNo;
        url_param["bindPhone"] = dparam.m_param;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}
//////////////////////////////////////////

public class PageViewLog : PageGen
{
    public int m_opType = 0;
    public string m_time = "";

    public PageViewLog(int rowEachPage)
        : base(rowEachPage)
    {
    }

    public override bool parse(HttpRequest Request)
    {
        bool res = base.parse(Request);

        string str = Request.QueryString["opType"];
        if (str != null)
        {
            m_opType = Convert.ToInt32(str);
            res = true;
        }
        str = Request.QueryString["time"];
        if (str != null)
        {
            m_time = str;
            res = true;
        }
        return res;
    }

    public override void genPage(ParamQueryBase query_param, string url, ref string page_link, ref string foot, GMUser user)
    {
        ParamQueryOpLog dparam = (ParamQueryOpLog)query_param;
        PageBrowseGenerator p = new PageBrowseGenerator();
        long total_page = 0;
        Dictionary<string, object> url_param = new Dictionary<string, object>();
        url_param["opType"] = dparam.m_logType;
        url_param["time"] = dparam.m_time;
        page_link = p.genPageFoot(query_param.m_curPage, m_rowEachPage, url, ref total_page, user, url_param);
        if (total_page != 0)
        {
            foot = query_param.m_curPage + "/" + total_page;
        }
    }
}





