using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Text.RegularExpressions;

// 充值记录
public class RechargeItem
{
    // 充值时间
    public string m_time = "";
    // 玩家ID
    public int m_playerId;

    public DbServerInfo m_serverInfo = null;

    // 账号
    public string m_account = "";
    // 客户端类型
    public string m_clientType = "";
    // 订单ID
    public string m_orderId = "";
    public string m_payCode = "";
    // 充值金额
    public int m_totalPrice = 0;
    // 是否作了处理
    public bool m_process = false;
    public string m_processTime = "";

    public string m_channelId = "";

    public string getPayName()
    {
        int payId = 0;
        if (int.TryParse(m_payCode, out payId))
        {
            return ResultRechargePointStat.getPayName(payId);
        }
        return m_payCode;
    }

    public string getChannelName()
    {
        TdChannelInfo info = TdChannel.getInstance().getValue(m_channelId.PadLeft(6, '0'));
        if (info != null)
        {
            return info.m_channelName;
        }
        return m_channelId;
    }
}

public class ParamQueryRecharge : ParamQuery
{
    // 平台索引(平台关键字)
    public string m_platKey = "";

    public int m_result = -1;

    public string m_range = "";

    // 游戏服务器索引
    public int m_gameServerIndex;

    // 游戏服务器ID
    public int m_serverId;

    // 渠道id
    public string m_channelNo;

    // 付费点
    public string m_rechargePoint = "";
}

// 平台特有信息
public class PlatInfo
{
    // 平台名称
    public string m_platName = "";

    // 充值数据所在表名
    public string m_tableName;
}

public class QueryRecharge : QueryBase
{
    static string[] s_field = { "platform" };
    protected List<RechargeItem> m_result = new List<RechargeItem>();
    // 充值表
    private Dictionary<string, RechargeBase> m_items = new Dictionary<string, RechargeBase>();
    private PlatInfo m_platInfo = new PlatInfo();
    private RechargeBase m_rbase = null;
    private QueryCondition m_cond = new QueryCondition();

    public QueryRecharge()
    {
        m_items.Add("default", new RechargeDefault());
        m_items.Add("anysdk", new RechargeAnySdk());
        m_items.Add("ysdk", new RechargeQbao());
        m_items.Add("wechat", new RechargeWChat());
        m_items.Add("xunlei", new RechargeXunLei());
    }

    public PlatInfo getPlatInfo() { return m_platInfo; }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        m_cond.startQuery();
        OpRes res = makeQuery(param, user, m_cond);
        if (res != OpRes.opres_success)
            return res;

        IMongoQuery imq = m_cond.getImq();
        ParamQueryRecharge p = (ParamQueryRecharge)param;
        return query(p, imq, m_rbase, user);
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition queryCond) 
    {
        ParamQueryRecharge p = (ParamQueryRecharge)param;

        int condCount = 0;
        PlatformInfo pinfo = null;

        if (!string.IsNullOrEmpty(p.m_param))
        {
            switch (p.m_way)
            {
                case QueryWay.by_way0: //  通过玩家id查询
                    {
                        int val = -1;
                        if (!int.TryParse(p.m_param, out val))
                        {
                            return OpRes.op_res_param_not_valid;
                        }
                        Dictionary<string, object> ret = QueryBase.getPlayerProperty(val, user, s_field);
                        if (ret == null)
                        {
                            return OpRes.op_res_not_found_data;
                        }
                        if (!ret.ContainsKey("platform"))
                        {
                            return OpRes.op_res_failed;
                        }

                        // 取玩家ID所在平台
                        string platName = Convert.ToString(ret["platform"]);

                        if (p.m_platKey == "wechat")
                        {
                            platName = p.m_platKey;
                        }

                        queryCond.addQueryCond("PlayerId", val);

                        pinfo = ResMgr.getInstance().getPlatformInfoByName(platName);

                        // 获取服务器ID
                        /*DbServerInfo dbInfo = ResMgr.getInstance().getDbInfo(user.m_dbIP);
                        if (dbInfo != null)
                        {
                            queryCond.addQueryCond("ServerId", dbInfo.m_serverId);
                        }*/
                    }
                    break;
                case QueryWay.by_way1: //  通过账号查询
                    {
                        Dictionary<string, object> ret = QueryBase.getPlayerPropertyByAcc(p.m_param, user, s_field);
                        if (ret == null)
                        {
                            return OpRes.op_res_not_found_data;
                        }
                        if (!ret.ContainsKey("platform"))
                        {
                            return OpRes.op_res_failed;
                        }
                       
                        // 取玩家账号所在平台，然后从相应的平台去查
                        string platName = Convert.ToString(ret["platform"]);
                        queryCond.addQueryCond("Account", p.m_param);

                        if (p.m_platKey == "wechat")
                        {
                            platName = p.m_platKey;
                        }

                        pinfo = ResMgr.getInstance().getPlatformInfoByName(platName);

                        // 获取服务器ID
                       /* DbServerInfo dbInfo = ResMgr.getInstance().getDbInfo(user.m_dbIP);
                        if (dbInfo != null)
                        {
                            queryCond.addQueryCond("ServerId", dbInfo.m_serverId);
                        }*/
                    }
                    break;
                case QueryWay.by_way2: //  通过订单号查询
                    {
                        pinfo = ResMgr.getInstance().getPlatformInfoByName(p.m_platKey);
                        queryCond.addQueryCond("OrderID", p.m_param);
                    }
                    break;
                default:
                    {
                        return OpRes.op_res_failed;
                    }
            }
            condCount++;
        }
        else
        {
            pinfo = ResMgr.getInstance().getPlatformInfoByName(p.m_platKey);

            // 获取服务器ID
            /*DbServerInfo dbInfo = ResMgr.getInstance().getDbInfo(user.m_dbIP);
            if (dbInfo != null)
            {
                queryCond.addQueryCond("ServerId", dbInfo.m_serverId);
            }*/
        }

        if (pinfo == null)
            return OpRes.op_res_need_sel_platform;

        if (!m_items.ContainsKey(pinfo.m_engName))
            return OpRes.op_res_not_found_data;

        m_rbase = m_items[pinfo.m_engName];
        
        m_platInfo.m_tableName = pinfo.m_tableName;
        m_platInfo.m_platName = pinfo.m_engName;

        if (queryCond.isExport())
        {
            queryCond.addCond("table", m_platInfo.m_tableName);
            queryCond.addCond("plat", m_platInfo.m_platName);
        }

        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            condCount++;
            if (queryCond.isExport())
            {
                queryCond.addCond("time", p.m_time);
            }
            else
            {
                IMongoQuery imq1 = Query.LT("PayTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("PayTime", BsonValue.Create(mint));
                queryCond.addImq(Query.And(imq1, imq2));
            }
        }

        if (p.m_result > 0)
        {
            queryCond.addQueryCond("Process", p.m_result == 1 ? true : false);
        }
        if (!string.IsNullOrEmpty(p.m_range))
        {
            if (!Tool.isTwoNumValid(p.m_range))
                return OpRes.op_res_param_not_valid;

            if (queryCond.isExport())
            {
                queryCond.addCond("range", p.m_range);
            }
            else
            {
                List<int> range = new List<int>();
                Tool.parseNumList(p.m_range, range);
                IMongoQuery timq1 = Query.LTE("RMB", BsonValue.Create(range[1]));
                IMongoQuery timq2 = Query.GTE("RMB", BsonValue.Create(range[0]));
                IMongoQuery tmpImq = Query.And(timq1, timq2);
                queryCond.addImq(tmpImq);
            }
        }

        if (pinfo.m_engName == "anysdk" && p.m_channelNo != "")
        {
            if (queryCond.isExport())
            {
                queryCond.addCond("channel", p.m_channelNo);
            }
            else
            {
                queryCond.addImq(Query.EQ("channel_number", p.m_channelNo));
            }
        }
        if (!string.IsNullOrEmpty(p.m_rechargePoint))
        {
            queryCond.addQueryCond("PayCode", p.m_rechargePoint);
        }

        if (condCount == 0)
            return OpRes.op_res_need_at_least_one_cond;

        return OpRes.opres_success; 
    }

    private OpRes query(ParamQueryRecharge param, IMongoQuery imq, RechargeBase r, GMUser user)
    {
        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_PAYMENT);
        if (serverId == -1)
            return OpRes.op_res_failed;

        user.totalRecord = DBMgr.getInstance().getRecordCount(m_platInfo.m_tableName, imq, serverId, DbName.DB_PAYMENT);

        List<Dictionary<string, object>> data =
             DBMgr.getInstance().executeQuery(m_platInfo.m_tableName, serverId, DbName.DB_PAYMENT, imq,
                                              (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage,
                                              null, "PayTime", false);

        if (data == null || data.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        r.fillResultList(m_result, data, user);
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////

public class RechargeBase
{
    public void fillResultList(List<RechargeItem> result, List<Dictionary<string, object>> data, GMUser user)
    {
        string[] fieldChannel = { "ChannelID" };

        for (int i = 0; i < data.Count; i++)
        {
            RechargeItem tmp = new RechargeItem();
            result.Add(tmp);
            tmp.m_time = Convert.ToDateTime(data[i]["PayTime"]).ToLocalTime().ToString();
            tmp.m_account = Convert.ToString(data[i]["Account"]);
            tmp.m_orderId = Convert.ToString(data[i]["OrderID"]);
            tmp.m_payCode = Convert.ToString(data[i]["PayCode"]);
            tmp.m_process = Convert.ToBoolean(data[i]["Process"]);
            if (data[i].ContainsKey("RMB"))
            {
                tmp.m_totalPrice = Convert.ToInt32(data[i]["RMB"]);
            }
            if (data[i].ContainsKey("PlayerId"))
            {
                tmp.m_playerId = Convert.ToInt32(data[i]["PlayerId"]);

                Dictionary<string, object> pd = QueryBase.getPlayerProperty(tmp.m_playerId, user, fieldChannel);
                if (pd != null)
                {
                    if (pd.ContainsKey("ChannelID"))
                    {
                        tmp.m_channelId = Convert.ToString(pd["ChannelID"]).PadLeft(6, '0');
                    }
                }
            }
           /* if (data[i].ContainsKey("ServerId"))
            {
                int id = Convert.ToInt32(data[i]["ServerId"]);
                tmp.m_serverInfo = ResMgr.getInstance().getDbInfoById(id);
            }*/
            fillResult(tmp, data[i]);
        }
    }

    public virtual void fillResult(RechargeItem tmp, Dictionary<string, object> data) { }
}

//////////////////////////////////////////////////////////////////////////

public class RechargeDefault : RechargeBase
{
    public override void fillResult(RechargeItem tmp, Dictionary<string, object> data)
    {
        tmp.m_clientType = "default";
    }
}

//////////////////////////////////////////////////////////////////////////

public class RechargeAnySdk : RechargeBase
{
    public override void fillResult(RechargeItem tmp, Dictionary<string, object> data)
    {
        tmp.m_clientType = "anysdk";
    }
}

//////////////////////////////////////////////////////////////////////////

public class RechargeQbao : RechargeBase
{
    public override void fillResult(RechargeItem tmp, Dictionary<string, object> data)
    {
        tmp.m_clientType = "ysdk";
    }
}

//////////////////////////////////////////////////////////////////////////

public class RechargeBaidu : RechargeBase
{
    public override void fillResult(RechargeItem tmp, Dictionary<string, object> data)
    {
        tmp.m_clientType = "baidu";
    }
}

//////////////////////////////////////////////////////////////////////////
public class RechargeWChat : RechargeBase
{
    public override void fillResult(RechargeItem tmp, Dictionary<string, object> data)
    {
        tmp.m_clientType = "wechat";
    }
}

//////////////////////////////////////////////////////////////////////////
public class RechargeXunLei : RechargeBase
{
    public override void fillResult(RechargeItem tmp, Dictionary<string, object> data)
    {
        tmp.m_clientType = "xunlei";
    }
}

/////////////////////////////////////////////////////////////////////////

//收支错误查询
public class IncomeExpensesError 
{
    public string m_createTime;
    public int m_playerId;
    public string m_goldError;
    public string m_gemError;
    public string m_dbError;
    public string m_chipError;
}
public class QueryIncomeExpensesError : QueryBase
{
    protected List<IncomeExpensesError> m_result=new List<IncomeExpensesError>();
    private QueryCondition m_cond = new QueryCondition();
    public override OpRes doQuery(object param,GMUser user) 
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        m_cond.startQuery();
        OpRes res = makeQuery(param,user,m_cond);
        if(res!=OpRes.opres_success)
            return res;
        IMongoQuery imq=m_cond.getImq();
        return query(p,imq,user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition queryCond)
    {
        ParamQuery p = (ParamQuery)param;
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        switch(Convert.ToInt32(p.m_showWay))
        {
            case 1://金币
                queryCond.addImq(Query.EQ("goldError", true));
                break;
            case 2://钻石
                queryCond.addImq(Query.EQ("gemError", true));
                break;
            case 3://龙珠
                queryCond.addImq(Query.EQ("dbError", true));
                break;
            case 4://彩券
                queryCond.addImq(Query.EQ("chipError", true));
                break;
        }

        if (queryCond.isExport())
        {
            queryCond.addCond("time", p.m_time);
        }
        else
        {
           IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
           IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
           queryCond.addImq(Query.And(imq1, imq2));
        }
        return OpRes.opres_success;
    }

    public OpRes query(ParamQuery param, IMongoQuery imq, GMUser user) 
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.STAT_INCOME_EXPENSES_ERROR, imq, dip);

        List<Dictionary<string, object>> dataList =
             DBMgr.getInstance().executeQuery(TableName.STAT_INCOME_EXPENSES_ERROR, dip, imq,
                                              (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage,
                                              null, "genTime", false);

        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            IncomeExpensesError tmp = new IncomeExpensesError();
            m_result.Add(tmp);

            tmp.m_createTime = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            tmp.m_playerId=Convert.ToInt32(data["playerId"]);
            if (data.ContainsKey("goldError"))
            {
                tmp.m_goldError = Convert.ToString(data["goldError"]);
            }
            if (data.ContainsKey("gemError"))
            {
                tmp.m_gemError = Convert.ToString(data["gemError"]);
            }
            if (data.ContainsKey("dbError"))
            {
                tmp.m_dbError = Convert.ToString(data["dbError"]);
            }
            if (data.ContainsKey("chipError"))
            {
                tmp.m_chipError = Convert.ToString(data["chipError"]);
            }
        }
        return OpRes.opres_success;
    }

}

//////////////////////////////////////////////////////////////////////////////////////////////
public class RechargeItemNew : RechargeItem
{
    public string m_createTime; // 订单创建时间
    public int m_status;        // 订单状态
}

// 新充值查询
public class QueryRechargeNew : QueryBase
{
    static string[] s_field = { "platform" };
    protected List<RechargeItemNew> m_result = new List<RechargeItemNew>();
    // 充值表
    private Dictionary<string, RechargeBase> m_items = new Dictionary<string, RechargeBase>();
    private PlatInfo m_platInfo = new PlatInfo();
    private RechargeBase m_rbase = null;
    private QueryCondition m_cond = new QueryCondition();

    public QueryRechargeNew()
    {
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        m_cond.startQuery();
        OpRes res = makeQuery(param, user, m_cond);
        if (res != OpRes.opres_success)
            return res;
        IMongoQuery imq = m_cond.getImq();
        ParamQueryRecharge p = (ParamQueryRecharge)param;

        int serverId = DBMgr.getInstance().getSpecialServerId(DbName.DB_PAYMENT);
        if (serverId == -1)
            return OpRes.op_res_failed;
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
        if (p.m_op == 1) //通过爱贝支付
        {
            dataList=query(p, imq, user,serverId, TableName.RECHARGE_TOTAL_AIBEI,"PayTime");
            if(dataList==null||dataList.Count==0)
                return OpRes.op_res_not_found_data;

            for (int i = 0; i < dataList.Count; i++)
            {
                Dictionary<string, object> data = dataList[i];
                RechargeItemNew tmp = new RechargeItemNew();
                m_result.Add(tmp);
                tmp.m_time = Convert.ToDateTime(data["PayTime"]).ToLocalTime().ToString();
                tmp.m_status = Convert.ToInt32(data["result"]);
                tmp.m_channelId = Convert.ToString(data["channel_number"]);
                tmp.m_totalPrice = Convert.ToInt32(data["RMB"]);
                tmp.m_payCode = Convert.ToString(data["PayCode"]);
                tmp.m_account = Convert.ToString(data["Account"]);
                tmp.m_orderId = Convert.ToString(data["OrderID"]);
                tmp.m_playerId = Convert.ToInt32(data["PlayerId"]);
                tmp.m_process = Convert.ToBoolean(data["Process"]);
            }
            return OpRes.opres_success;
        }
        else 
        {
            dataList = query(p, imq, user,serverId,TableName.RECHARGE_TOTAL,"CreateTime");
            if (dataList == null || dataList.Count == 0)
                return OpRes.op_res_not_found_data;

            for (int i = 0; i < dataList.Count; i++)
            {
                Dictionary<string, object> data = dataList[i];
                RechargeItemNew tmp = new RechargeItemNew();
                m_result.Add(tmp);
                tmp.m_createTime = Convert.ToDateTime(data["CreateTime"]).ToLocalTime().ToString();
                if (data.ContainsKey("PayTime"))
                {
                    tmp.m_time = Convert.ToDateTime(data["PayTime"]).ToLocalTime().ToString();
                }
                tmp.m_status = Convert.ToInt32(data["status"]);
                tmp.m_channelId = Convert.ToString(data["channel_number"]);
                tmp.m_totalPrice = Convert.ToInt32(data["RMB"]);
                tmp.m_payCode = Convert.ToString(data["PayCode"]);
                tmp.m_account = Convert.ToString(data["Account"]);
                tmp.m_orderId = Convert.ToString(data["OrderID"]);
                tmp.m_playerId = Convert.ToInt32(data["PlayerId"]);
            }
            return OpRes.opres_success;
        }
    }

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes makeQuery(object param, GMUser user, QueryCondition queryCond)
    {
        ParamQueryRecharge p = (ParamQueryRecharge)param;

        int condCount = 0;

        if (!string.IsNullOrEmpty(p.m_param))
        {
            switch (p.m_way)
            {
                case QueryWay.by_way0: //  通过玩家id查询
                    {
                        int val = -1;
                        if (!int.TryParse(p.m_param, out val))
                        {
                            return OpRes.op_res_param_not_valid;
                        }
                        queryCond.addQueryCond("PlayerId", val);
                    }
                    break;
                case QueryWay.by_way1: //  通过账号查询
                    {
                        queryCond.addQueryCond("Account", p.m_param);
                    }
                    break;
                case QueryWay.by_way2: //  通过订单号查询
                    {
                        queryCond.addQueryCond("OrderID", p.m_param);
                    }
                    break;
                default:
                    {
                        return OpRes.op_res_failed;
                    }
            }
            condCount++;
        }
        
        if (queryCond.isExport())
        {
           // queryCond.addCond("table", m_platInfo.m_tableName);
           // queryCond.addCond("plat", m_platInfo.m_platName);
        }

        string createTime = "";
        string status = "";
        if (p.m_op == 1)
        {
            createTime = "PayTime";
            status = "result";
        }
        else 
        {
            createTime = "CreateTime";
            status = "status";
        }

        if (p.m_time != "")
        {
            DateTime mint = DateTime.Now, maxt = DateTime.Now;
            bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
            if (!res)
                return OpRes.op_res_time_format_error;

            condCount++;
            if (queryCond.isExport())
            {
                queryCond.addCond("time", p.m_time);
            }
            else
            {
                IMongoQuery imq1 = Query.LT(createTime, BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE(createTime, BsonValue.Create(mint));
                queryCond.addImq(Query.And(imq1, imq2));
            }
        }

        if (p.m_result >= 0) // 初始值为-1
        {
            queryCond.addQueryCond(status, p.m_result);
        }
        if (!string.IsNullOrEmpty(p.m_range))
        {
            if (!Tool.isTwoNumValid(p.m_range))
                return OpRes.op_res_param_not_valid;

            if (queryCond.isExport())
            {
                queryCond.addCond("range", p.m_range);
            }
            else
            {
                List<int> range = new List<int>();
                Tool.parseNumList(p.m_range, range);
                IMongoQuery timq1 = Query.LTE("RMB", BsonValue.Create(range[1]));
                IMongoQuery timq2 = Query.GTE("RMB", BsonValue.Create(range[0]));
                IMongoQuery tmpImq = Query.And(timq1, timq2);
                queryCond.addImq(tmpImq);
            }
        }

        if (p.m_channelNo != "")
        {
            if (queryCond.isExport())
            {
                queryCond.addCond("channel", p.m_channelNo);
            }
            else
            {
                queryCond.addImq(Query.EQ("channel_number", p.m_channelNo));
            }
        }
        if (!string.IsNullOrEmpty(p.m_rechargePoint))
        {
            queryCond.addQueryCond("PayCode", Convert.ToInt32(p.m_rechargePoint));
        }

        if (condCount == 0)
            return OpRes.op_res_need_at_least_one_cond;

        return OpRes.opres_success;
    }

    private List<Dictionary<string,object>> query(ParamQueryRecharge param, IMongoQuery imq, GMUser user,int serverId ,string tableName,string sortName)
    {
        user.totalRecord = DBMgr.getInstance().getRecordCount(tableName, imq, serverId, DbName.DB_PAYMENT);

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(tableName, serverId, DbName.DB_PAYMENT, imq,
             (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, sortName, false);
        return dataList;
    }
}


