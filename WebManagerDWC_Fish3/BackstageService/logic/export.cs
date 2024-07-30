using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Text.RegularExpressions;
using System.IO;

// 导出Excel
public abstract class ExportExcelBase
{
    // 将数据导出到文件filename内
    public OpRes exportExcel(ExportParam param, string exportDir)
    {
        int id = DBMgr.getInstance().getDbId(param.m_dbServerIP);
        if (id == -1)
        {
            LogMgr.error("exportExcel，无法找到IP:{0}", param.m_dbServerIP);
            return OpRes.op_res_failed;
        }

        string dir = Path.Combine(exportDir, param.m_account);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string tmpDir = Path.Combine(dir, "tmp");
        if (!Directory.Exists(tmpDir))
        {
            Directory.CreateDirectory(tmpDir);
        }

        OpRes res = OpRes.opres_success;
        StreamWriter sw = null;
        string srcFileName = "";
        string dstFileName = "";

        try
        {
            string file = initExport(param, id);
            dstFileName = Path.Combine(dir, file);
            srcFileName = Path.Combine(tmpDir, file);
            sw = new StreamWriter(srcFileName, false, Encoding.Default);
            exportData(sw, param, id);
        }
        catch (System.Exception ex)
        {
            if (sw != null)
            {
                sw.Close();
                res = OpRes.op_res_failed;
            }
            LogMgr.error(ex.ToString());
        }
        finally
        {
            if (sw != null)
            {
                sw.Close();

                if (srcFileName != "" && dstFileName != "")
                {
                    try
                    {
                        File.Copy(srcFileName, dstFileName, true);
                        File.Delete(srcFileName);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }
        }
        return res;
    }

    //返回一条数据
    public static Dictionary<string, object> oneData(IMongoQuery imq, string table,
        int serverid, int dbid, string[] fields = null)
    {
        Dictionary<string, object> data =
                    DBMgr.getInstance().getTableData(table, serverid, dbid, imq, fields);
        return data;
    }

    // 返回下一条数据
    public static List<Dictionary<string, object>> nextData(ref int skip, int count, IMongoQuery imq, string table,
        int serverid, int dbid, string[] fields = null, string sort = "", bool asc = true)
    {
        List<Dictionary<string, object>> data =
            DBMgr.getInstance().executeQuery(table, serverid, dbid, imq, skip, count, fields, sort, asc);
        if (data == null || data.Count == 0)
            return null;
        skip += count;
        return data;
    }

    // 返回下一条数据
    public static List<Dictionary<string, object>> nextData2(ref int skip, int count, IMongoQuery imq, string table,
        int serverid, int dbid, string[] fields = null, SortByBuilder sort = null)
    {
        List<Dictionary<string, object>> data =
            DBMgr.getInstance().executeQuery2(table, serverid, dbid, imq, skip, count, fields, sort);
        if (data == null || data.Count == 0)
            return null;
        skip += count;
        return data;
    }

    // 通过guid查询EtPlayer表中指定的属性
    public static Dictionary<string, object> getPlayerProperty(int playerId, int serverId, string[] fields)
    {
        Dictionary<string, object> ret =
             DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "player_id", playerId, fields, serverId, DbName.DB_PLAYER);
        return ret;
    }

    // 通过账号返回玩家属性
    public static Dictionary<string, object> getPlayerPropertyByAcc(string acc, int serverId, string[] fields)
    {
        Dictionary<string, object> ret =
                        DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, "account", acc, fields, serverId, DbName.DB_PLAYER);
        return ret;
    }

    public abstract OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId);

    // 初始化导出，返回存储的文件名
    public abstract string initExport(ExportParam param, int dbServerId);

    public static string convertToCsvStr(string str)
    {
        if (str.IndexOf(',') > 0)
            return "\"" + str + "\"";
        return str;
    }

    public static string getPayName(int payId)
    {
        RechargeCFGData data = RechargeCFG.getInstance().getValue(payId);
        if (data != null)
        {
            return data.m_name;
        }
        return "";
    }

    public static string GetChannelName(string channelNum)
    {
        string c = channelNum.PadLeft(6, '0');
        TdChannelInfo tinfo = TdChannel.getInstance().getValue(c);
        if (tinfo != null)
        {
            return tinfo.m_channelName;
        }
        else
        {
            return channelNum;
        }
    }
}
///////////////////////////////////////////////////////////////////////////////////
//玩家炮数成长分布
public class ExportPlayerActivityTurret : ExportExcelBase 
{
    private IMongoQuery m_imq = null;
    static string[] s_head = new string[] { "日期","注册时间","渠道","玩家ID","当日炮倍","上线天数","当日游戏时间","当日金币","总充值金额","次留","三留","七留"};
    public override string initExport(ExportParam param, int dbServerId)
    {
        string fileName = "";
        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null) 
        {
            DbServerInfo info = ResMgr.getInstance().getDbInfo(param.m_dbServerIP);
            if (info != null)
            {
                string dbName = info.m_serverName;
                fileName += "-" + dbName;
            }

            if(cond.ContainsKey("time")) //时间
            {
                string time = Convert.ToString(cond["time"]);
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                Tool.splitTimeStr(time, ref mint, ref maxt);
                IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
                queryList.Add(Query.And(imq1, imq2));
                time = mint.ToString("yyyy年MM月dd日 HH时mm分ss秒") + "~" + maxt.ToString("yyyy年MM月dd日 HH时mm分ss秒");

                fileName += "-" + time;
            }

            if (cond.ContainsKey("day"))
            {
                IMongoQuery imq = null;
                int day = Convert.ToInt32(cond["day"]);
                if (day < 0)
                {
                    imq = Query.LTE("day", 6);
                    fileName += "-" + "全部7天";
                }
                else
                {
                    imq = Query.EQ("day", day - 1);
                    fileName += "-" + "第" + day + "天";
                }
                queryList.Add(imq);
            }

            //炮倍等级 默认全部
            if (cond.ContainsKey("turret_level"))   //  1-38
            {
                //炮倍
                string turret_lv = Convert.ToString(cond["turret_level"]);
                string[] turretLv = turret_lv.Split('-');
                int lv1 = Convert.ToInt32(turretLv[0]);
                int lv2 = Convert.ToInt32(turretLv[1]);
                IMongoQuery imq = Query.And(Query.GTE("turret_level", lv1), Query.LTE("turret_level", lv2));
                queryList.Add(imq);
            }
            //炮倍
            if (cond.ContainsKey("turret_range"))   //  1~10000
            {
                //炮倍
                string turret_range = Convert.ToString(cond["turret_range"]);
                if (turret_range == "1-10000")
                {
                    fileName += "全部炮倍";
                }
                else {
                    string[] turretRange = turret_range.Split('-');
                    int range1 = Convert.ToInt32(turretRange[0]);
                    int range2 = Convert.ToInt32(turretRange[1]);
                    fileName += "炮倍["+range1 +'~'+range2+"]";
                }
            }

            m_imq = queryList.Count > 0 ? Query.And(queryList) : null;
        }

        return string.Format("玩家炮数成长分布-{0}.csv", fileName);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        List<Dictionary<string, object>> dataList = null;

        sheet.WriteLine(string.Join(",", s_head));
        while (true)
        {
            SortByBuilder sort = new SortByBuilder();
            sort = sort.Descending("time").Descending("createTime").Ascending("day").Ascending("turret_level");

            dataList = nextData2(ref skip, 1000, m_imq, TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, dbServerId, DbName.DB_PUMP, null, sort);
            if (dataList == null)
                break;

            for (int i = 0; i < dataList.Count; i++)
            {
                try
                {
                    Dictionary<string, object> data = dataList[i];

                    sheet.Write(Convert.ToDateTime(data["time"]).ToLocalTime().ToString());
                    sheet.Write(",");

                    if (data.ContainsKey("createTime"))
                    {
                        sheet.Write(Convert.ToDateTime(data["createTime"]).ToLocalTime().ToString());
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("channel"))
                    {
                        string channel = Convert.ToString(data["channel"]).PadLeft(6, '0');
                        TdChannelInfo tinfo = TdChannel.getInstance().getValue(channel);
                        if (tinfo != null)
                            channel = tinfo.m_channelName;
                        sheet.Write(channel);
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("playerId"))
                    {
                        sheet.Write(Convert.ToString(data["playerId"]));
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("turret_level"))
                    {
                        int turret_lv = Convert.ToInt32(data["turret_level"]);
                        var turretLv = Fish_TurretLevelCFG.getInstance().getValue(turret_lv);
                        if (turretLv != null)
                           sheet.Write(turretLv.m_openRate.ToString());
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("loginDay"))
                    {
                        sheet.Write(Convert.ToString(data["loginDay"]));
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("todayPlayTime"))
                    {
                        sheet.Write(Convert.ToString(data["todayPlayTime"]));
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("gold"))
                    {
                        sheet.Write(Convert.ToString(data["gold"]));
                    }
                    sheet.Write(",");

                    //次留 三留 七留 126
                    if (data.ContainsKey("playerId"))
                    {
                        int playerId = Convert.ToInt32(data["playerId"]);

                        //总充值金额
                        IMongoQuery q4 = Query.EQ("player_id", playerId);
                        Dictionary<string, object> playerRecharge =
                            DBMgr.getInstance().getTableData(TableName.PLAYER_INFO, dbServerId, DbName.DB_PLAYER, q4, new string[] { "player_id", "recharged" });
                        if (playerRecharge != null && playerRecharge.ContainsKey("recharged"))
                            sheet.Write(Convert.ToString(playerRecharge["recharged"]));
                        sheet.Write(",");

                        IMongoQuery q1 = Query.And(Query.EQ("playerId", playerId), Query.EQ("day", 1));
                        long count1 = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, q1, dbServerId, DbName.DB_PUMP);
                        if (count1 > 0)
                            sheet.Write("1");
                        sheet.Write(",");

                        IMongoQuery q2 = Query.And(Query.EQ("playerId", playerId), Query.EQ("day", 2));
                        long count2 = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, q2, dbServerId, DbName.DB_PUMP);
                        if (count2 > 0)
                            sheet.Write("1");
                        sheet.Write(",");

                        IMongoQuery q6 = Query.And(Query.EQ("playerId", playerId), Query.EQ("day", 6));
                        long count6 = DBMgr.getInstance().getRecordCount(TableName.STAT_PUMP_PLAYER_ACTIVITY_TURRET, q6, dbServerId, DbName.DB_PUMP);
                        if (count6 > 0)
                            sheet.Write("1");
                    }
                    else 
                    {
                        sheet.Write(",");
                        sheet.Write(",");
                        sheet.Write(",");
                    }

                    sheet.WriteLine();
                }
                catch (System.Exception ex)
                {
                    LogMgr.error("玩家炮数成长分布导出出错:{0}", ex.ToString());
                }
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////////////
//客服补单/大户随访/换包福利历史操作查询 导出
public class ExportRepairOrder : ExportExcelBase
{
    private IMongoQuery m_imq = null;
    static string[] s_head = new string[] { "操作时间", "操作原因", "操作ID", "补单项目", "补单补贴/客服回访福利", "操作账号", "备注" };

    public override string initExport(ExportParam param, int dbServerId)
    {
        string fileName = "";
        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            DbServerInfo info = ResMgr.getInstance().getDbInfo(param.m_dbServerIP);
            if (info != null)
            {
                string dbName = info.m_serverName;
                fileName += "-" + dbName;
            }

            if (cond.ContainsKey("time")) // 时间
            {
                string time = Convert.ToString(cond["time"]);
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                Tool.splitTimeStr(time, ref mint, ref maxt);
                IMongoQuery imq1 = Query.LT("time", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("time", BsonValue.Create(mint));
                queryList.Add(Query.And(imq1, imq2));
                time = mint.ToString("yyyy年MM月dd日 HH时mm分ss秒") + "~" + maxt.ToString("yyyy年MM月dd日 HH时mm分ss秒");

                fileName += "-" + time;
            }
        }
        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;
        return string.Format("客服补单-大户随访-换包福利-系统操作历史-{0}.csv", fileName);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        List<Dictionary<string, object>> dataList = null;

        sheet.WriteLine(string.Join(",", s_head));
        while (true)
        {
            dataList = nextData(ref skip, 1000, m_imq, TableName.PUMP_REPAIR_ORDER, dbServerId, DbName.DB_PUMP);
            if (dataList == null)
                break;

            for (int i = 0; i < dataList.Count; i++)
            {
                try
                {
                    Dictionary<string, object> data = dataList[i];
                    sheet.Write(Convert.ToDateTime(data["time"]).ToLocalTime().ToString());
                    sheet.Write(",");

                    if (data.ContainsKey("opReason"))
                    {
                        string opName = "";
                        int m_op = Convert.ToInt32(data["opReason"]);
                        switch (m_op)
                        {
                            case 0: opName = "补单"; break;
                            case 1: opName = "换包福利"; break;
                            case 2: opName = "访问客服福利"; break;
                            case 3: opName = "大户回流引导"; break;
                        }
                        sheet.Write(opName);
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("playerIdList"))
                    {
                        sheet.Write(Convert.ToString(data["playerIdList"]));
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("repairOrder"))
                    {
                        string orderName = "";
                        int m_itemId = Convert.ToInt32(data["repairOrder"]);
                        if (m_itemId == -1)
                        {
                            sheet.Write("false");
                        }
                        else
                        {
                            var orderItem = RechargeCFG.getInstance().getValue(m_itemId);
                            if (orderItem != null)
                                orderName = orderItem.m_name;
                            sheet.Write(orderName);
                        }
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("repairBonus"))
                    {
                        string bonusName = "";
                        int m_bonusId = Convert.ToInt32(data["repairBonus"]);
                        if (m_bonusId == -1)
                        {
                            sheet.Write("false");
                        }
                        else
                        {
                            var bonusItem = RepairOrderItem.getInstance().getValue(m_bonusId);
                            if (bonusItem != null)
                                bonusName = bonusItem.m_itemName;
                            sheet.Write(bonusName);
                        }
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("operator")) 
                    {
                        sheet.Write(Convert.ToString(data["operator"]));
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("comments"))
                    {
                        sheet.Write(Convert.ToString(data["comments"]));
                    }
                    sheet.WriteLine();
                }
                catch (System.Exception ex)
                {
                    LogMgr.error("导出客服补单/大户随访/换包福利-历史操作记录出错:{0}", ex.ToString());
                }
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
//vip玩家信息导出
public class ExportVipPlayerInfo : ExportExcelBase 
{
    public const string TABLE_NAME = TableName.PLAYER_INFO+"_vip";
    public const string TABLE_NAME_FISHLORD = TableName.FISHLORD_PLAYER;

    private string[] m_playerFields = { "player_id", "VipLevel", "VipExp", "bindPhone", "nickname", "ChannelID", "create_time", "logout_time", "gold", "recharged" };
    private string[] m_fishlordFields = { "player_id", "ContributionValue", "ContributeHistory" };
    private IMongoQuery m_imq = null;

    public override string initExport(ExportParam param, int dbServerId)
    {
        string dbName = "";
        int vipLv = 0;
        string bindPhone = "";
        string sortType = "";

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            DbServerInfo info = ResMgr.getInstance().getDbInfo(param.m_dbServerIP);
            if (info != null)
                dbName = info.m_serverName;

            if (cond.ContainsKey("bindPhone"))
            {
                int isBindPhone = Convert.ToInt32(cond["bindPhone"]);
                if (isBindPhone == 0) //不绑定
                {
                    queryList.Add(Query.EQ("bindPhone", ""));
                    bindPhone = "未绑定手机";
                }
                else
                {
                    queryList.Add(Query.NE("bindPhone", ""));
                    bindPhone = "绑定手机";
                }
            }

            if (cond.ContainsKey("vipLevel"))
            {
                int vipLevel = Convert.ToInt32(cond["vipLevel"]);
                queryList.Add(Query.GTE("VipLevel", vipLevel));
                vipLv = vipLevel;
            }
            if (cond.ContainsKey("sortType"))
            {
                int sort = Convert.ToInt32(cond["sortType"]);
                if (sort == 0)
                {
                    sortType = "最后上线时间排序";
                }
                else
                {
                    sortType = "VIP等级排序";
                }
            }
        }
        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return string.Format("VIP玩家信息-{0}-vip等级[大于等于{1}]-{2}-{3}.csv", dbName,vipLv, bindPhone, sortType);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        string table = TableName.PLAYER_INFO;
        int skip = 0;
        string sort = "";
        if (Convert.ToInt32(param.m_condition["sortType"]) == 0)
        {
            sort = "logout_time";
        }
        else
        {
            sort = "VipLevel";
        }
        sheet.WriteLine("玩家ID,用户名,手机号,渠道,Vip经验值,Vip等级,注册时间,最后上线时间,剩余金币,当前贡献值,历史贡献值,总充值额");

        List<Dictionary<string, object>> data = null;
        while (true)
        {
            data = nextData(ref skip, 1000, m_imq, table, dbServerId, DbName.DB_PLAYER, m_playerFields, sort,false);
            if (data == null)
                break;

            try
            {
                for (int i = 0; i < data.Count; i++)
                {
                    //////
                    int playerId = 0;
                    if (data[i].ContainsKey("player_id"))
                        playerId = Convert.ToInt32(data[i]["player_id"]);
                    sheet.Write(playerId.ToString());
                    sheet.Write(",");
                    /////
                    string nickName = "";
                    if (data[i].ContainsKey("nickname"))
                        nickName = Convert.ToString(data[i]["nickname"]);
                    sheet.Write(nickName);
                    sheet.Write(",");
                    /////
                    string bindPhone = "";
                    if (data[i].ContainsKey("bindPhone"))
                        bindPhone = Convert.ToString(data[i]["bindPhone"]);
                    sheet.Write(bindPhone);
                    sheet.Write(",");
                    ////
                    string channel = "";
                    if (data[i].ContainsKey("ChannelID"))
                    {
                        string channelNo = Convert.ToString(data[i]["ChannelID"]).PadLeft(6, '0');
                        TdChannelInfo cd = TdChannel.getInstance().getValue(channelNo);
                        if (cd != null)
                        {
                            channel = cd.m_channelName;
                        }
                        else
                        {
                            channel = channelNo;
                        }
                    }
                    sheet.Write(channel);
                    sheet.Write(",");
                    /////
                    int vipExp = 0;
                    if (data[i].ContainsKey("VipExp"))
                        vipExp = Convert.ToInt32(data[i]["VipExp"]);
                    sheet.Write(vipExp);
                    sheet.Write(",");
                    //////
                    int viplevel = 0;
                    if (data[i].ContainsKey("VipLevel"))
                        viplevel = Convert.ToInt32(data[i]["VipLevel"]);
                    sheet.Write(viplevel);
                    sheet.Write(",");
                    //////
                    string create_time = "";
                    if (data[i].ContainsKey("create_time"))
                        create_time = Convert.ToDateTime(data[i]["create_time"]).ToLocalTime().ToString();
                    sheet.Write(create_time);
                    sheet.Write(",");
                    /////
                    string logout_time = "";
                    if (data[i].ContainsKey("logout_time"))
                        logout_time = Convert.ToDateTime(data[i]["logout_time"]).ToLocalTime().ToString();
                    sheet.Write(logout_time);
                    sheet.Write(",");
                    /////
                    long gold = 0;
                    if (data[i].ContainsKey("gold"))
                        gold = Convert.ToInt64(data[i]["gold"]);
                    sheet.Write(gold);
                    sheet.Write(",");
                    /////
                    //贡献值
                    IMongoQuery imq = Query.EQ("player_id", playerId);
                    Dictionary<string, object> data1 = oneData(imq, TABLE_NAME_FISHLORD, dbServerId, DbName.DB_GAME, m_fishlordFields);
                    if (data1 != null)
                    {
                        double val = 0, valH = 0;
                        if (data1.ContainsKey("ContributionValue"))
                            val = Convert.ToDouble(data1["ContributionValue"]);
                        sheet.Write(val);
                        sheet.Write(",");

                        if (data1.ContainsKey("ContributeHistory"))
                            valH = Convert.ToDouble(data1["ContributeHistory"]);
                        sheet.Write(valH);
                        sheet.Write(",");
                    }
                    else
                    {
                        sheet.Write("");
                        sheet.Write(",");
                        sheet.Write("");
                        sheet.Write(",");
                    }
                    ////
                    int recharged = 0;
                    if (data[i].ContainsKey("recharged"))
                        recharged = Convert.ToInt32(data[i]["recharged"]);
                    sheet.Write(recharged);
                    
                    sheet.WriteLine();
                }
            }
            catch (System.Exception ex)
            {
                LogMgr.error("导出vip玩家信息出现异常：{0}", ex.ToString());
            }
        }
        return OpRes.opres_success;
    }
}
/////////////////////////////////////////////////////////////////////////
//玩家道具获取详情
public class ExportPlayerItemRecord : ExportExcelBase 
{
    public const string TABLE_NAME_PLAYER_ITEM = TableName.PUMP_PLAYER_ITEM;
    private IMongoQuery m_imq = null;
    XmlConfig xml = ResMgr.getInstance().getRes("money_reason.xml");

    public override string initExport(ExportParam param, int dbServerId)
    {
        //导出 
        string dbName = "";
        string time = "";
        int playerId = 0;
        string reasonStr = "";
        string itemStr = "";
        string syncKey = "";
       
        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            DbServerInfo info = ResMgr.getInstance().getDbInfo(param.m_dbServerIP);
            if (info != null)
                dbName = info.m_serverName;

            if (cond.ContainsKey("time")) // 时间
            {
                time = Convert.ToString(cond["time"]);
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                Tool.splitTimeStr(time, ref mint, ref maxt);
                IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
                queryList.Add(Query.And(imq1, imq2));

                time = mint.ToString("yyyy年MM月dd日 HH时mm分ss秒") + "~" + maxt.ToString("yyyy年MM月dd日 HH时mm分ss秒");
            }

            if (cond.ContainsKey("playerId"))
            {
                playerId = Convert.ToInt32(cond["playerId"]);
                if (playerId != 0)
                    queryList.Add(Query.EQ("playerId", playerId));
            }

            if (cond.ContainsKey("reason"))
            {
                int reasonId = Convert.ToInt32(cond["reason"]);
                if (reasonId != 0)
                {
                    queryList.Add(Query.EQ("reason", reasonId));
                    reasonStr = xml.getString(reasonId.ToString(), "");
                }
                else
                {
                    reasonStr = "全部";
                }
            }

            if (cond.ContainsKey("itemId"))
            {
                int itemId = Convert.ToInt32(cond["itemId"]);
                if (itemId != 0)
                {
                    queryList.Add(Query.EQ("itemId", itemId));
                    itemStr = getItemName(itemId);
                }
            }

            if (cond.ContainsKey("syncKey"))
            {
                syncKey = Convert.ToString(cond["syncKey"]);
                if (!string.IsNullOrEmpty(syncKey))
                    queryList.Add(Query.EQ("syncKey", syncKey));
            }
        }

        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;
        
        if (playerId == 0)
                 return string.Format("玩家道具获取详情-{0}-{1}-{2}-{3}-{4}.csv", dbName,time, reasonStr, itemStr, syncKey);

        return string.Format("玩家道具获取详情-{0}-{1}-{2}-{3}-{4}-{5}.csv", dbName,time, playerId, reasonStr, itemStr, syncKey);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        sheet.WriteLine("日期,玩家ID,变化原因,道具ID,道具,起始值,结束值,差额,同步字段");

        List<Dictionary<string, object>> data = null;
        while (true)
        {
            data = nextData(ref skip, 1000, m_imq, TABLE_NAME_PLAYER_ITEM, dbServerId, DbName.DB_PUMP, null, "genTime", false);
            if (data == null)
                break;
            try
            {
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.Write(Convert.ToDateTime(data[i]["genTime"]).ToLocalTime().ToString());
                    sheet.Write(",");
                    /////
                    sheet.Write(Convert.ToInt32(data[i]["playerId"]));
                    sheet.Write(",");
                    /////
                    string reasonStr = "";
                    if (data[i].ContainsKey("reason"))
                    {
                        int reason = Convert.ToInt32(data[i]["reason"]);
                        reasonStr = xml.getString(reason.ToString(), "");
                    }
                    sheet.Write(reasonStr);
                    sheet.Write(",");
                    //////
                    string strItem = "";
                    int itemId = 0;
                    if (data[i].ContainsKey("itemId")) {
                        itemId = Convert.ToInt32(data[i]["itemId"]);
                        strItem = getItemName(itemId);
                    }
                    sheet.Write(itemId);
                    sheet.Write(",");
                    sheet.Write(strItem);
                    sheet.Write(",");
                    ///////
                    int oldCount = 0, newCount = 0, addCount=0;
                    if (data[i].ContainsKey("oldCount"))
                        oldCount = Convert.ToInt32(data[i]["oldCount"]);
                    sheet.Write(oldCount);
                    sheet.Write(",");
                    //////
                    if (data[i].ContainsKey("newCount"))
                        newCount = Convert.ToInt32(data[i]["newCount"]);
                    sheet.Write(newCount);
                    sheet.Write(",");
                    /////
                    if (data[i].ContainsKey("addCount"))
                        addCount = Convert.ToInt32(data[i]["addCount"]);
                    sheet.Write(addCount);
                    sheet.Write(",");
                    ////////
                    string syncKey = "";
                    if (data[i].ContainsKey("syncKey"))
                        syncKey = Convert.ToString(data[i]["syncKey"]);
                    sheet.Write(syncKey);

                    sheet.WriteLine();
                }
            }
            catch (System.Exception ex)
            {
                LogMgr.error("导出玩家道具获取详情出现异常：{0}",ex.ToString());
            }
        }
        return OpRes.opres_success;
    }

    string getItemName(int itemId)
    {
        string itemStr = itemId.ToString();
        ItemCFGData pItem = ItemCFG.getInstance().getValue(itemId);
        if (pItem != null)
            itemStr = pItem.m_itemName;

        return itemStr;
    }
}
/////////////////////////////////////////////////////////////////////////
// 充值记录导出
public class ExportRecharge : ExportExcelBase
{
    public const string TABLE_NAME = "recharge";
    private string[] m_playerFields = { "account", "player_id" };
    private IMongoQuery m_imq = null;
    // 在哪个表中查询
    private string m_searchTable = "";
    private string m_plat = "";

    public override string initExport(ExportParam param, int dbServerId)
    {
        string account = "";
        int playerId = -1;
        string dbName = "";
        int userOpDbId = -1;
        string time = "";

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            m_searchTable = Convert.ToString(cond["table"]);
            m_plat = Convert.ToString(cond["plat"]);
            DbServerInfo info = ResMgr.getInstance().getDbInfo(Convert.ToString(cond["userOpDbIp"]));
            if (info != null)
            {
                dbName = info.m_serverName;
                userOpDbId = DBMgr.getInstance().getDbId(info.m_serverIp);
            }

            if (cond.ContainsKey("PlayerId"))
            {
                playerId = Convert.ToInt32(cond["PlayerId"]);
                queryList.Add(Query.EQ("PlayerId", BsonValue.Create(playerId)));
                
                if (userOpDbId >= 0)
                {
                    Dictionary<string, object> ret = getPlayerProperty(playerId, userOpDbId, m_playerFields);
                    if (ret != null)
                    {
                        account = Convert.ToString(ret["account"]);
                    }
                }
            }
            /*if (cond.ContainsKey("ServerId"))
            {
                int serverId = Convert.ToInt32(cond["ServerId"]);
                queryList.Add(Query.EQ("ServerId", BsonValue.Create(serverId)));
            }*/
            if (cond.ContainsKey("Account"))
            {
                account = Convert.ToString(cond["Account"]);
                queryList.Add(Query.EQ("Account", BsonValue.Create(account)));

                if (userOpDbId >= 0)
                {
                    Dictionary<string, object> ret = getPlayerPropertyByAcc(account, userOpDbId, m_playerFields);
                    if (ret != null)
                    {
                        playerId = Convert.ToInt32(ret["player_id"]);
                    }
                }
            }
            if (cond.ContainsKey("time")) // 时间
            {
                time = Convert.ToString(cond["time"]);
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                Tool.splitTimeStr(time, ref mint, ref maxt);
                IMongoQuery imq1 = Query.LT("PayTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("PayTime", BsonValue.Create(mint));
                queryList.Add(Query.And(imq1, imq2));
                
                time = mint.ToString("yyyy年MM月dd日 HH时mm分ss秒") + "~" + maxt.ToString("yyyy年MM月dd日 HH时mm分ss秒");
            }
            if (cond.ContainsKey("Process"))
            {
                bool result = Convert.ToBoolean(cond["Process"]);
                queryList.Add(Query.EQ("Process", BsonValue.Create(result)));
            }
            if (cond.ContainsKey("range")) // 范围
            {
                string rangeStr = Convert.ToString(cond["range"]);

                List<int> range = new List<int>();
                Tool.parseNumList(rangeStr, range);
                IMongoQuery timq1 = Query.LTE("RMB", BsonValue.Create(range[1]));
                IMongoQuery timq2 = Query.GTE("RMB", BsonValue.Create(range[0]));
                IMongoQuery tmpImq = Query.And(timq1, timq2);
                queryList.Add(tmpImq);
            }
        }

        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;

        if (playerId == -1)
        {
            return string.Format("充值记录-{0}-{1}-{2}.csv", account, dbName, time);
        }
        return string.Format("充值记录-{0}-{1}-{2}-{3}.csv", account, playerId, dbName, time);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        List<Dictionary<string, object>> data = null;

        sheet.WriteLine("充值时间,玩家ID,玩家账号,客户端类型,订单ID,PayCode,充值金额(元),process");

        int t = 0;

        while (true)
        {
            data = nextData(ref skip, 1000, m_imq, m_searchTable, dbServerId, DbName.DB_PAYMENT);
            if (data == null)
                break;

            for (int i = 0; i < data.Count; i++)
            {
                try
                {
                    sheet.Write(Convert.ToDateTime(data[i]["PayTime"]).ToLocalTime().ToString());
                    sheet.Write(",");

                    if (data[i].ContainsKey("PlayerId"))
                    {
                        t = Convert.ToInt32(data[i]["PlayerId"]);
                        sheet.Write(t);
                    }
                    sheet.Write(",");

                    sheet.Write(Convert.ToString(data[i]["Account"]));
                    sheet.Write(",");

                    /* if (data[i].ContainsKey("ServerId"))
                     {
                         t = Convert.ToInt32(data[i]["ServerId"]);
                         DbServerInfo info = ResMgr.getInstance().getDbInfoById(t);
                         if (info != null)
                         {
                             sheet.Write(info.m_serverName);
                         }
                     }
                     sheet.Write(",");*/

                    sheet.Write(m_plat);
                    sheet.Write(",");

                    sheet.Write(" " + Convert.ToString(data[i]["OrderID"] + " "));
                    sheet.Write(",");

                    sheet.Write(" " + Convert.ToString(data[i]["PayCode"]) + " ");
                    sheet.Write(",");

                    if (data[i].ContainsKey("RMB"))
                    {
                        t = Convert.ToInt32(data[i]["RMB"]);
                        sheet.Write(t);
                    }
                    sheet.Write(",");

                    sheet.Write(Convert.ToBoolean(data[i]["Process"]));
                    sheet.WriteLine();
                }
                catch (System.Exception ex)
                {
                    LogMgr.error("导出充值记录出现异常:{0}", ex.ToString());
                }
            }
        }

        return OpRes.opres_success;
    }
}
// 充值记录导出(新)
public class ExportRechargeNew : ExportExcelBase
{
    private string[] m_playerFields = { "account", "player_id" };
    private IMongoQuery m_imq = null;
    static string[] s_head = new string[] { "创建时间", "充值时间", "玩家ID", "玩家账号", "订单ID", "付费点", "充值金额(元)", "订单状态", "渠道" };

    public override string initExport(ExportParam param, int dbServerId)
    {
        string fileName = "";
        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            DbServerInfo info = ResMgr.getInstance().getDbInfo(Convert.ToString(cond["userOpDbIp"]));
            if (info != null)
            {
                string dbName = info.m_serverName;
                fileName += "-" + dbName;
            }

            if (cond.ContainsKey("PlayerId"))
            {
                int playerId = Convert.ToInt32(cond["PlayerId"]);
                queryList.Add(Query.EQ("PlayerId", BsonValue.Create(playerId)));

                fileName += "-" + playerId.ToString();
            }
            if (cond.ContainsKey("Account"))
            {
                string account = Convert.ToString(cond["Account"]);
                queryList.Add(Query.EQ("Account", BsonValue.Create(account)));
                fileName += "-" + account;
            }
            if (cond.ContainsKey("time")) // 时间
            {
                string time = Convert.ToString(cond["time"]);
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                Tool.splitTimeStr(time, ref mint, ref maxt);
                IMongoQuery imq1 = Query.LT("CreateTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("CreateTime", BsonValue.Create(mint));
                queryList.Add(Query.And(imq1, imq2));
                time = mint.ToString("yyyy年MM月dd日 HH时mm分ss秒") + "~" + maxt.ToString("yyyy年MM月dd日 HH时mm分ss秒");

                fileName += "-" + time;
            }
            if (cond.ContainsKey("status"))
            {
                int status1 = Convert.ToInt32(cond["status"]);
                queryList.Add(Query.EQ("status", status1));
                fileName += "-" + StrName.s_rechargeState[status1];
            }

            if (cond.ContainsKey("range")) // 范围
            {
                string rangeStr = Convert.ToString(cond["range"]);
                List<int> range = new List<int>();
                Tool.parseNumList(rangeStr, range);
                IMongoQuery timq1 = Query.LTE("RMB", BsonValue.Create(range[1]));
                IMongoQuery timq2 = Query.GTE("RMB", BsonValue.Create(range[0]));
                IMongoQuery tmpImq = Query.And(timq1, timq2);
                queryList.Add(tmpImq);

                fileName += "-" + rangeStr;
            }
            if (cond.ContainsKey("channel")) //渠道
            {
                string channel = Convert.ToString(cond["channel"]);
                queryList.Add(Query.EQ("channel_number", channel));
                TdChannelInfo tinfo = TdChannel.getInstance().getValue(channel);
                if (tinfo != null)
                {
                    channel=tinfo.m_channelName;
                }
                fileName += "-" + channel;
            }
            if (cond.ContainsKey("PayCode")) //付费点
            {
                queryList.Add(Query.EQ("PayCode", Convert.ToInt32(cond["PayCode"])));
                fileName += "-" + Convert.ToString(cond["PayCode"]);
            }
        }

        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return string.Format("充值记录-{0}.csv", fileName);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        List<Dictionary<string, object>> dataList = null;

        sheet.WriteLine(string.Join(",", s_head));

        int t = 0;

        while (true)
        {
            dataList = nextData(ref skip, 1000, m_imq, TableName.RECHARGE_TOTAL, dbServerId, DbName.DB_PAYMENT);
            if (dataList == null)
                break;

            for (int i = 0; i < dataList.Count; i++)
            {
                try
                {
                    Dictionary<string, object> data = dataList[i];

                    sheet.Write(Convert.ToDateTime(data["CreateTime"]).ToLocalTime().ToString());
                    sheet.Write(",");

                    if(data.ContainsKey("PayTime"))
                    {
                        sheet.Write(Convert.ToDateTime(data["PayTime"]).ToLocalTime().ToString());
                    }
                    sheet.Write(",");

                    t = Convert.ToInt32(data["PlayerId"]);
                    sheet.Write(t);
                    sheet.Write(",");

                    sheet.Write(Convert.ToString(data["Account"]));
                    sheet.Write(",");

                    sheet.Write(Convert.ToString(data["OrderID"]) + "\t");
                    sheet.Write(",");

                    t = Convert.ToInt32(data["PayCode"]);
                    RechargeCFGData rd = RechargeCFG.getInstance().getValue(t);
                    if (rd != null)
                    {
                        sheet.Write(rd.m_name);
                    }
                    else
                    {
                        sheet.Write(Convert.ToString(data["PayCode"]));
                    }
                    sheet.Write(",");

                    t = Convert.ToInt32(data["RMB"]);
                    sheet.Write(t);
                    sheet.Write(",");

                    t = Convert.ToInt32(data["status"]);
                    sheet.Write(StrName.s_rechargeState[t]);
                    sheet.Write(",");

                    string c = Convert.ToString(data["channel_number"]).PadLeft(6, '0');
                    TdChannelInfo tinfo = TdChannel.getInstance().getValue(c);
                    if (tinfo != null)
                    {
                        sheet.Write(tinfo.m_channelName);
                    }
                    else
                    {
                        sheet.Write(c);
                    }
                    sheet.WriteLine();
                }
                catch (System.Exception ex)
                {
                    LogMgr.error("导出充值记录出现异常:{0}", ex.ToString());
                }
            }
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
// 金币，礼券变化导出
public class ExportMoney : ExportExcelBase
{
    private string[] m_playerFields = { "account", "player_id" };
    private IMongoQuery m_imq = null;
    private string[] m_playerFields1 = { "nickname" };

    public override string initExport(ExportParam param, int dbServerId)
    {
        string account = "";
        int playerId = -1;
        string dbName = "";
        string time = "";
        string gameId = "";
        string reasonStr = "";

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            DbServerInfo info = ResMgr.getInstance().getDbInfo(param.m_dbServerIP);
            if (info != null)
            {
                dbName = info.m_serverName;
            }

            if (cond.ContainsKey("playerId"))
            {
                playerId = Convert.ToInt32(cond["playerId"]);
                queryList.Add(Query.EQ("playerId", BsonValue.Create(playerId)));

                Dictionary<string, object> ret = getPlayerProperty(playerId, dbServerId, m_playerFields);
                if (ret != null)
                {
                    account = Convert.ToString(ret["account"]);
                }                
            }
            if (cond.ContainsKey("reason"))
            {
                int reason = Convert.ToInt32(cond["reason"]);
                queryList.Add(Query.EQ("reason", BsonValue.Create(reason)));

                XmlConfig xml = ResMgr.getInstance().getRes("money_reason.xml");
                reasonStr = xml.getString(reason.ToString(), "");
            }
            if (cond.ContainsKey("itemId"))
            {
                int item = Convert.ToInt32(cond["itemId"]);
                queryList.Add(Query.EQ("itemId", BsonValue.Create(item)));
            }
            if (cond.ContainsKey("time"))
            {
                time = Convert.ToString(cond["time"]);
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                Tool.splitTimeStr(time, ref mint, ref maxt);
                IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
                queryList.Add(Query.And(imq1, imq2));

                time = mint.ToString("yyyy年MM月dd日 HH时mm分ss秒") + "~" + maxt.ToString("yyyy年MM月dd日 HH时mm分ss秒");
            }
            if (cond.ContainsKey("range"))
            {
                List<int> range = new List<int>();
                string rangeStr = Convert.ToString(cond["range"]);
                Tool.parseNumList(rangeStr, range);
                IMongoQuery imq1 = Query.LTE("addValue", BsonValue.Create(range[1]));
                IMongoQuery imq2 = Query.GTE("addValue", BsonValue.Create(range[0]));
                queryList.Add(Query.And(imq1, imq2));
            }
            if (cond.ContainsKey("gameId"))
            {
                int id = Convert.ToInt32(cond["gameId"]);
                queryList.Add(Query.EQ("gameId", id));
                gameId = StrName.getGameName3(id);
            }
        }

        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;

        if (playerId == -1)
        {
            return string.Format("金币钻石变化-{0}-{1}-{2}-{3}-{4}.csv", account, dbName, time, gameId, reasonStr);
        }
        return string.Format("金币钻石变化-{0}-{1}-{2}-{3}-{4}-{5}.csv", account, playerId, dbName, time, gameId, reasonStr);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        List<Dictionary<string, object>> data = null;

        sheet.WriteLine("时间,玩家ID,玩家昵称,属性,变化原因,初始值,结束值,差值(结束值-初始值),投注,返还,盈利,获胜盈利,所在游戏,备注");

        int t = 0;
        XmlConfig xml = ResMgr.getInstance().getRes("money_reason.xml");

        while (true)
        {
            data = nextData(ref skip, 1000, m_imq, TableName.PUMP_PLAYER_MONEY, dbServerId, DbName.DB_PUMP);
            if (data == null)
                break;

            try
            {
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.Write(Convert.ToDateTime(data[i]["genTime"]).ToLocalTime().ToString());
                    sheet.Write(",");

                    t = Convert.ToInt32(data[i]["playerId"]);
                    sheet.Write(t);
                    sheet.Write(",");

                    Dictionary<string, object> ret = getPlayerProperty(t, dbServerId, m_playerFields1);
                    if (ret != null && ret.ContainsKey("nickname"))
                    {
                        sheet.Write(Convert.ToString(ret["nickname"]));
                    }
                    sheet.Write(",");

                    t = Convert.ToInt32(data[i]["itemId"]);
                    sheet.Write(getPropertyName(t));
                    sheet.Write(",");

                    t = Convert.ToInt32(data[i]["reason"]);
                    if (xml != null)
                    {
                        sheet.Write(xml.getString(t.ToString(), ""));
                    }
                    sheet.Write(",");

                    int t1 = Convert.ToInt32(data[i]["oldValue"]);
                    sheet.Write(t1);
                    sheet.Write(",");

                    int t2 = Convert.ToInt32(data[i]["newValue"]);
                    sheet.Write(t2);
                    sheet.Write(",");

                    t = t2 - t1;//Convert.ToInt32(data[i]["addValue"]);
                    sheet.Write(t);
                    sheet.Write(",");

                    t1 = 0;
                    if (data[i].ContainsKey("playerOutlay"))
                    {
                        t1 = Convert.ToInt32(data[i]["playerOutlay"]);
                    }
                    sheet.Write(t1);
                    sheet.Write(",");

                    t2 = 0;
                    if (data[i].ContainsKey("playerIncome"))
                    {
                        t2 = Convert.ToInt32(data[i]["playerIncome"]);
                    }
                    sheet.Write(t2);
                    sheet.Write(",");

                    sheet.Write(t2 - t1);
                    sheet.Write(",");

                    t = 0;
                    if (data[i].ContainsKey("playerWinBet"))
                    {
                        t = Convert.ToInt32(data[i]["playerWinBet"]);
                    }
                    sheet.Write(t);
                    sheet.Write(",");

                    t = Convert.ToInt32(data[i]["gameId"]);
                    sheet.Write(StrName.getGameName3(t));
                    sheet.Write(",");

                    if (data[i].ContainsKey("param"))
                    {
                        string str = ExportExcelBase.convertToCsvStr(Convert.ToString(data[i]["param"]));
                        sheet.Write(str);
                    }

                    sheet.WriteLine();
                }
            }
            catch (System.Exception ex)
            {
                LogMgr.error(ex.ToString());
            }
        }

        return OpRes.opres_success;
    }

    public string getPropertyName(int propertyType)
    {
        if (propertyType == (int)PropertyType.property_type_gold)
            return "金币";

        if (propertyType == (int)PropertyType.property_type_diamond)
            return "钻石";

        if (propertyType == (int)PropertyType.property_type_ticket)
            return "彩券";

        if (propertyType == (int)PropertyType.property_type_dragon_ball)
            return "龙珠币";

        if (propertyType == (int)PropertyType.property_type_moshi)
            return "魔石";

        if (propertyType == (int)PropertyType.property_type_chip)
            return "碎片";

        return "";
    }
}
//////////////////////////////////////////////////////////////////////////
// 任务导出基类
public class ExportTaskBase : ExportExcelBase
{
    private string[] m_playerFields = { "account", "player_id" };
    private IMongoQuery m_imq = null;
    private string[] m_playerFields1 = { "nickname" };
    protected string m_tableName = "";

    public override string initExport(ExportParam param, int dbServerId)
    {
        string account = "";
        int playerId = -1;
        string dbName = "";
        string time = "";

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            DbServerInfo info = ResMgr.getInstance().getDbInfo(param.m_dbServerIP);
            if (info != null)
            {
                dbName = info.m_serverName;
            }

            if (cond.ContainsKey("playerId"))
            {
                playerId = Convert.ToInt32(cond["playerId"]);
                queryList.Add(Query.EQ("playerId", BsonValue.Create(playerId)));

                Dictionary<string, object> ret = getPlayerProperty(playerId, dbServerId, m_playerFields);
                if (ret != null)
                {
                    account = Convert.ToString(ret["account"]);
                }
            }
            if (cond.ContainsKey("taskId"))
            {
                int taskId = Convert.ToInt32(cond["taskId"]);
                queryList.Add(Query.EQ("taskId", BsonValue.Create(taskId)));
            }
            if (cond.ContainsKey("time"))
            {
                time = Convert.ToString(cond["time"]);
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                Tool.splitTimeStr(time, ref mint, ref maxt);
                IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
                queryList.Add(Query.And(imq1, imq2));

                time = mint.ToString("yyyy年MM月dd日 HH时mm分ss秒") + "~" + maxt.ToString("yyyy年MM月dd日 HH时mm分ss秒");
            }
        }

        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return getExportFileName(playerId, account, dbName, time);
    }

    protected virtual string getExportFileName(int playerId, string account, string dbName, string time) { return ""; }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        List<Dictionary<string, object>> data = null;

        sheet.WriteLine("时间,玩家ID,任务ID,任务名称,道具id,道具名称,数量,道具id,道具名称,数量,道具id,道具名称,数量,道具id,道具名称,数量,道具id,道具名称,数量");

        int t = 0;
        XmlConfig xml = ResMgr.getInstance().getRes("money_reason.xml");

        List<ParamItem> rewardList = new List<ParamItem>();

        while (true)
        {
            data = nextData(ref skip, 1000, m_imq, m_tableName, dbServerId, DbName.DB_PUMP);
            if (data == null)
                break;

            for (int i = 0; i < data.Count; i++)
            {
                sheet.Write(Convert.ToDateTime(data[i]["genTime"]).ToLocalTime().ToString());
                sheet.Write(",");

                t = Convert.ToInt32(data[i]["playerId"]);
                sheet.Write(t);
                sheet.Write(",");

                t = Convert.ToInt32(data[i]["taskId"]);
                sheet.Write(t);
                sheet.Write(",");

                QusetCFGData qd = QuestCFG.getInstance().getValue(t);
                if (qd != null)
                {
                    sheet.Write(qd.m_questName);
                }
                sheet.Write(",");

                rewardList.Clear();
                Tool.parseItemFromDic(data[i]["items"] as Dictionary<string, object>, rewardList);

                for (int j = 0; j < rewardList.Count; j++)
                {
                    sheet.Write(rewardList[j].m_itemId);
                    sheet.Write(",");
                    
                    ItemCFGData pItem = ItemCFG.getInstance().getValue(rewardList[j].m_itemId);
                    if (pItem != null)
                    {
                        sheet.Write(pItem.m_itemName);
                    }
                    sheet.Write(",");

                    sheet.Write(rewardList[j].m_itemCount);

                    if (j != rewardList.Count - 1)
                    {
                        sheet.Write(",");
                    }
                }
                sheet.WriteLine();
            }
        }

        return OpRes.opres_success;
    }
}
///////////////////////////////////////////////////////////////////////////////
// 每日任务导出
public class ExportDailyTask : ExportTaskBase
{
    public ExportDailyTask()
    {
        m_tableName = TableName.PUMP_DAILY_TASK;
    }

    protected override string getExportFileName(int playerId, string account, string dbName, string time)
    {
        if (playerId == -1)
        {
            return string.Format("每日任务-{0}-{1}-{2}.csv", account, dbName, time);
        }
        return string.Format("每日任务-{0}-{1}-{2}-{3}.csv", account, playerId, dbName, time);
    }
}
//////////////////////////////////////////////////////////////////////////////
// 成就导出
public class ExportAchievement : ExportTaskBase
{
    public ExportAchievement()
    {
        m_tableName = TableName.PUMP_TASK;
    }

    protected override string getExportFileName(int playerId, string account, string dbName, string time)
    {
        if (playerId == -1)
        {
            return string.Format("成就-{0}-{1}-{2}.csv", account, dbName, time);
        }
        return string.Format("成就-{0}-{1}-{2}-{3}.csv", account, playerId, dbName, time);
    }
}
//////////////////////////////////////////////////////////////////////////
// 金币预警导出
public class ExportMoneyWarn : ExportExcelBase
{
    static string[] s_fieldGold = new string[] { "player_id", "gold", "safeBoxGold" };
    static string[] s_fieldTicket = new string[] { "player_id", "ticket" };
    static string[] s_field = new string[] { "nickname" };
    private int m_sel;
    private int m_count;

    public override string initExport(ExportParam param, int dbServerId)
    {
        string dbName = "";

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            DbServerInfo info = ResMgr.getInstance().getDbInfo(param.m_dbServerIP);
            if (info != null)
            {
                dbName = info.m_serverName;
            }

            if (cond.ContainsKey("sel"))
            {
                m_sel = Convert.ToInt32(cond["sel"]);
            }
            if (cond.ContainsKey("count"))
            {
                m_count = Convert.ToInt32(cond["count"]);
            }
        }

        string time = DateTime.Now.ToString("yyyy年MM月dd日");
        return string.Format("{0}预警-{1}-{2}.csv", m_sel == 0 ? "金币" : "钻石", time, dbName);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        List<Dictionary<string, object>> data = null;

        if (m_sel == 0)
        {
            sheet.WriteLine("玩家ID,玩家昵称,金币,保险箱");
        }
        else
        {
            sheet.WriteLine("玩家ID,玩家昵称,钻石");
        }

        //while (true)
        {
            string[] retField = null;
            if(m_sel == 0)
            {
                retField = s_fieldGold;
            }
            else
            {
                retField = s_fieldTicket;
            }
            IMongoQuery imq = Query.EQ("is_robot", false);
            data = nextData(ref skip, m_count, imq, TableName.PLAYER_INFO, dbServerId, DbName.DB_PLAYER, retField, retField[1], false);
            int t = 0;
            for (int i = 0; i < data.Count; i++)
            {
                t = Convert.ToInt32(data[i]["player_id"]);
                sheet.Write(t);
                sheet.Write(",");

                Dictionary<string, object> ret = getPlayerProperty(t, dbServerId, s_field);
                if (ret != null && ret.ContainsKey("nickname"))
                {
                    sheet.Write(Convert.ToString(ret["nickname"]));
                }
                sheet.Write(",");

                if (m_sel == 0)
                {
                    long gold = Convert.ToInt64(data[i]["gold"]);
                    sheet.Write(gold);
                    if (data[i].ContainsKey("safeBoxGold"))
                    {
                        sheet.Write(",");
                        t = Convert.ToInt32(data[i]["safeBoxGold"]);
                        sheet.Write(t);
                    }
                }
                else
                {
                    t = Convert.ToInt32(data[i]["ticket"]);
                    sheet.Write(t);
                }
                sheet.WriteLine();
            }
        }

        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
public class RechargePlayerItem : ResultRPlayerItem
{
    public string m_channelId;

    public string getChannelName()
    {
        TdChannelInfo info = TdChannel.getInstance().getValue(m_channelId);
        if (info != null)
        {
            return info.m_channelName;
        }
        return m_channelId;
    }
}
// 充值用户统计
public class ExportRechargePlayer : ExportExcelBase
{
    private static string[] s_head = new string[] { "玩家id","绑定手机号码","渠道", "充值次数", "充值金额", 
        "注册时间", "上线次数", "剩余金币","最后上线时间", "曾经最大金币",
        StrName.getGameName3((int)GameId.fishlord)};
    static string MapTable = TableName.PUMP_RECHARGE_PLAYER_NEW + "_map";
    static string[] s_fields = { "create_time", "gold", "maxGold", "logout_time", "ChannelID", "bindPhone" };

    private IMongoQuery m_imq = null;

    public override string initExport(ExportParam param, int dbServerId)
    {
        string dbName = "";
        string time = "";

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            DbServerInfo info = ResMgr.getInstance().getDbInfo(param.m_dbServerIP);
            if (info != null)
                dbName = info.m_serverName;

            if (cond.ContainsKey("time"))
            {
                time = Convert.ToString(cond["time"]);
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                Tool.splitTimeStr(time, ref mint, ref maxt);
                IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

                IMongoQuery imq3 = Query.GT("rechargeCount", 0);
                queryList.Add(Query.And(imq1, imq2, imq3));

                time = mint.ToString("yyyy年MM月dd日 HH时mm分ss秒") + "~" + maxt.ToString("yyyy年MM月dd日 HH时mm分ss秒");
            }
        }

        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;
        return string.Format("充值用户统计-{0}-{1}.csv", dbName, time);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        for (int i = 0; i < s_head.Length; i++)
        {
            sheet.Write(s_head[i]);
            sheet.Write(',');
        }
        sheet.WriteLine();

        MapReduceResult map_result = DBMgr.getInstance().executeMapReduce(TableName.PUMP_RECHARGE_PLAYER_NEW,
                                                                            dbServerId,
                                                                            DbName.DB_PUMP,
                                                                            m_imq,
                                                                            MapReduceTable.getMap("rechargePlayer"),
                                                                            MapReduceTable.getReduce("rechargePlayer"),
                                                                            MapTable);
        if (map_result != null)
        {
            IEnumerable<BsonDocument> bson = map_result.GetResults();
            foreach (BsonDocument d in bson)
            {
                try
                {
                    RechargePlayerItem tmp = new RechargePlayerItem();

                    tmp.m_playerId = Convert.ToInt32(d["_id"]);
                    BsonValue resValue = d["value"];
                    tmp.m_rechargeCount = resValue["rechargeCount"].ToInt32();
                    tmp.m_rechargeMoney = resValue["rechargeMoney"].ToInt32();
                    tmp.m_loginCount = resValue["loginCount"].ToInt32();

                    var arr = resValue["enterGame"].AsBsonArray;
                    for (int i = 0; i < arr.Count; i++)
                    {
                        tmp.addEnterCount(i + 1, arr[i].ToInt32());
                    }

                    Dictionary<string, object> pd = ExportExcelBase.getPlayerProperty(tmp.m_playerId, dbServerId, s_fields);
                    if (pd != null)
                    {
                        tmp.m_mostGold = Convert.ToInt64(pd["maxGold"]);
                        tmp.m_remainGold = Convert.ToInt64(pd["gold"]);
                        tmp.m_regTime = Convert.ToDateTime(pd["create_time"]).ToLocalTime();
                        tmp.m_lastLoginTime = Convert.ToDateTime(pd["logout_time"]).ToLocalTime();
                        if (pd.ContainsKey("bindPhone"))
                        {
                            tmp.m_bindPhone = Convert.ToString(pd["bindPhone"]);
                        }
                        if (pd.ContainsKey("ChannelID"))
                        {
                            tmp.m_channelId = Convert.ToString(pd["ChannelID"]).PadLeft(6, '0');
                        }
                    }

                    sheet.Write(tmp.m_playerId); sheet.Write(",");
                    sheet.Write(tmp.m_bindPhone); sheet.Write(",");
                    sheet.Write(tmp.m_channelId); sheet.Write(",");
                    sheet.Write(tmp.m_rechargeCount); sheet.Write(",");
                    sheet.Write(tmp.m_rechargeMoney); sheet.Write(",");

                    sheet.Write(tmp.m_regTime.ToString()); sheet.Write(",");
                    sheet.Write(tmp.m_loginCount); sheet.Write(",");
                    sheet.Write(tmp.m_remainGold); sheet.Write(",");
                    sheet.Write(tmp.m_lastLoginTime.ToString()); sheet.Write(",");
                    sheet.Write(tmp.m_mostGold); sheet.Write(",");

                    sheet.Write(tmp.getEnterCount((int)GameId.fishlord)); 
                    
                    sheet.WriteLine();
                }
                catch (System.Exception ex)
                {
                    LogMgr.error("导出充值用户统计异常情况：{0}", ex.ToString());
                }
            }
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////
// 充值玩家监控
public class ExportRechargePlayerMonitor : ExportExcelBase
{
    private static string[] s_head = new string[] { "玩家ID", "当前捕鱼等级", 
       "累计付费",  "累计游戏时间", 
       "初次付费日期", "初次付费游戏时间","初次付费项目","初次付费时拥有金币","初次付费捕鱼等级",
       "再次付费日期", "再次付费游戏时间","再次付费项目","再次付费时拥有金币","再次付费捕鱼等级",
       "注册时间", "龙珠结余", "龙珠累计获得", "龙珠累计转出"
        };
    static string[] FIELD_FISH_LEVEL = { "Level" };
    static string[] FIELD_PLAYER = { "recharged", "dragonBall", "GainDragonBallCount", "SendDragonBallCount", "create_time" };

    private IMongoQuery m_imq = null;

    public override string initExport(ExportParam param, int dbServerId)
    {
        string fileName = "";
        string dbName = "";
        string time = "";

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            DbServerInfo info = ResMgr.getInstance().getDbInfo(param.m_dbServerIP);
            if (info != null)
            {
                dbName = info.m_serverName;
            }

            if (cond.ContainsKey("channel")) //渠道
            {
                string channel = Convert.ToString(cond["channel"]);
                queryList.Add(Query.EQ("channel", channel));
                TdChannelInfo tinfo = TdChannel.getInstance().getValue(channel);
                if (tinfo != null)
                {
                    channel = tinfo.m_channelName;
                }
                fileName += "-" + channel;
            }

            if (cond.ContainsKey("time"))
            {
                time = Convert.ToString(cond["time"]);
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                Tool.splitTimeStr(time, ref mint, ref maxt);
                IMongoQuery imq1 = Query.LT("regTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("regTime", BsonValue.Create(mint));
                queryList.Add(Query.And(imq1, imq2));

                fileName +='-'+ mint.ToString("yyyy年MM月dd日 HH时mm分ss秒") + "~" + maxt.ToString("yyyy年MM月dd日 HH时mm分ss秒");
            }
        }

        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return string.Format("充值玩家监控-{0}-{1}.csv", dbName, fileName);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        List<Dictionary<string, object>> dataList = null;

        string head = string.Join(",", s_head);
        sheet.WriteLine(head);

        int t = 0;
        while (true)
        {
            dataList = nextData(ref skip, 1000, m_imq, TableName.PUMP_RECHARGE_FIRST, dbServerId, DbName.DB_PUMP,
                null, "firstRechargeTime", false);
            if (dataList == null)
                break;

            for (int i = 0; i < dataList.Count; i++)
            {
                Dictionary<string, object> data = dataList[i];
                int playerId = Convert.ToInt32(data["playerId"]);
                sheet.Write(playerId); sheet.Write(",");

                RechargePlayerMonitorItemBase item = new RechargePlayerMonitorItemBase();
                setOhterInfo(playerId, dbServerId, item);

                t = getFishLevel(playerId, dbServerId);
                sheet.Write(t); sheet.Write(",");

                sheet.Write(item.m_totalRecharge); sheet.Write(",");

                t = getTotalGameTime(playerId, dbServerId);
                sheet.Write(item.getGameTime(t)); sheet.Write(",");

                if (data.ContainsKey("firstRechargeTime"))
                {
                    DateTime time = Convert.ToDateTime(data["firstRechargeTime"]).ToLocalTime();
                    sheet.Write(time.ToString());
                }
                sheet.Write(",");

                if (data.ContainsKey("firstRechargeGameTime"))
                {
                    t = Convert.ToInt32(data["firstRechargeGameTime"]);
                    sheet.Write(item.getGameTime(t));
                }
                sheet.Write(",");

                if (data.ContainsKey("firstRechargePoint"))
                {
                    t = Convert.ToInt32(data["firstRechargePoint"]);
                    sheet.Write(getPayName(t));
                }
                sheet.Write(",");
                if (data.ContainsKey("firstRechargeGold"))
                {
                    t = Convert.ToInt32(data["firstRechargeGold"]);
                    sheet.Write(t);
                }
                sheet.Write(",");
                if (data.ContainsKey("firstRechargeFishLevel"))
                {
                    t = Convert.ToInt32(data["firstRechargeFishLevel"]);
                    sheet.Write(t);
                }
                sheet.Write(",");
                if (data.ContainsKey("secondRechargeTime"))
                {
                    DateTime time = Convert.ToDateTime(data["secondRechargeTime"]).ToLocalTime();
                    sheet.Write(time.ToString());
                }
                sheet.Write(",");

                if (data.ContainsKey("secondRechargeGameTime"))
                {
                    t = Convert.ToInt32(data["secondRechargeGameTime"]);
                    sheet.Write(item.getGameTime(t));
                }
                sheet.Write(",");
                if (data.ContainsKey("secondRechargePoint"))
                {
                    t = Convert.ToInt32(data["secondRechargePoint"]);
                    sheet.Write(getPayName(t));
                }
                sheet.Write(",");
                if (data.ContainsKey("secondRechargeGold"))
                {
                   t = Convert.ToInt32(data["secondRechargeGold"]);
                   sheet.Write(t);
                }
                sheet.Write(",");
                if (data.ContainsKey("secondRechargeFishLevel"))
                {
                    t = Convert.ToInt32(data["secondRechargeFishLevel"]);
                    sheet.Write(t);
                }
                sheet.Write(",");

                sheet.Write(item.m_regTime.ToString()); sheet.Write(",");
                sheet.Write(item.m_remainDragon); sheet.Write(",");
                sheet.Write(item.m_gainDragon); sheet.Write(",");
                sheet.Write(item.m_sendDragon);

                sheet.WriteLine();
            }
        }

        return OpRes.opres_success;
    }

    int getFishLevel(int playerId, int serverId)
    {
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.FISHLORD_PLAYER,
            "player_id", playerId,
            FIELD_FISH_LEVEL,
            serverId,
            DbName.DB_GAME);

        if (data != null)
        {
            return Convert.ToInt32(data["Level"]);
        }

        return 0;
    }

    int getTotalGameTime(int playerId, int serverId)
    {
        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.STAT_PLAYER_GAME_TIME,
            "playerId", playerId,
            null,
            serverId,
            DbName.DB_PLAYER);

        if (data != null)
        {
            return Convert.ToInt32(data["totalGameTime"]);
        }

        return 0;
    }

    void setOhterInfo(int playerId, int serverId, RechargePlayerMonitorItemBase item)
    {
        Dictionary<string, object> data = ExportExcelBase.getPlayerProperty(playerId, serverId, FIELD_PLAYER);
        if (data != null)
        {
            if (data.ContainsKey("recharged"))
            {
                item.m_totalRecharge = Convert.ToInt32(data["recharged"]);
            }
            if (data.ContainsKey("dragonBall"))
            {
                item.m_remainDragon = Convert.ToInt32(data["dragonBall"]);
            }
            if (data.ContainsKey("GainDragonBallCount"))
            {
                item.m_gainDragon = Convert.ToInt64(data["GainDragonBallCount"]);
            }
            if (data.ContainsKey("SendDragonBallCount"))
            {
                item.m_sendDragon = Convert.ToInt64(data["SendDragonBallCount"]);
            }
            if (data.ContainsKey("create_time"))
            {
                item.m_regTime = Convert.ToDateTime(data["create_time"]).ToLocalTime();
            }
        }
    }
}
//////////////////////////////////////////////////////////////////////////
// 每日龙珠
public class ExportDragonBallDaily : ExportExcelBase
{
    private static string[] s_head = new string[] { "日期", "每日充值", 
       "龙珠产出",  "龙珠消耗", "龙珠结余", "盈利折算RMB"
        };
    private double m_discount;
    private double m_eachValue;

    private IMongoQuery m_imq = null;

    public override string initExport(ExportParam param, int dbServerId)
    {
        string dbName = "";
        string time = "";

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            DbServerInfo info = ResMgr.getInstance().getDbInfo(param.m_dbServerIP);
            if (info != null)
            {
                dbName = info.m_serverName;
            }
            if (cond.ContainsKey("time"))
            {
                time = Convert.ToString(cond["time"]);
                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                Tool.splitTimeStr(time, ref mint, ref maxt);
                IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
                queryList.Add(Query.And(imq1, imq2));

                time = mint.ToString("yyyy年MM月dd日") + "~" + maxt.ToString("yyyy年MM月dd日");
            }
            if (cond.ContainsKey("discount"))
            {
                m_discount = Convert.ToDouble(cond["discount"]);
            }
            else
            {
                m_discount = 1;
            }
            if (cond.ContainsKey("eachValue"))
            {
                m_eachValue = Convert.ToDouble(cond["eachValue"]);
            }
            else
            {
                m_eachValue = 1;
            }
        }

        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return string.Format("每日龙珠-{0}-{1}-每龙珠价值[{2}]-渠道折价[{3}].csv", dbName, time, m_eachValue, m_discount);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        List<Dictionary<string, object>> dataList = null;

        string head = string.Join(",", s_head);
        sheet.WriteLine(head);

        long tmp = 0;
        while (true)
        {
            dataList = nextData(ref skip, 1000, m_imq, TableName.STAT_DRAGON_DAILY, dbServerId, DbName.DB_PUMP);
            if (dataList == null)
                break;

            for (int i = 0; i < dataList.Count; i++)
            {
                Dictionary<string, object> data = dataList[i];

                DateTime time = Convert.ToDateTime(data["genTime"]).ToLocalTime();
                sheet.Write(time.ToString()); sheet.Write(",");

                int todayR = Convert.ToInt32(data["todayRecharge"]);
                sheet.Write(todayR); sheet.Write(",");

                long dgen = Convert.ToInt64(data["dragonBallGen"]);
                sheet.Write(dgen); sheet.Write(",");

                long dbc = Convert.ToInt64(data["dragonBallConsume"]);
                sheet.Write(dbc); sheet.Write(",");

                tmp = Convert.ToInt64(data["dragonBallRemain"]);
                sheet.Write(tmp); sheet.Write(",");

                double rmb = todayR * m_discount - (dgen - dbc) * m_eachValue;
                sheet.Write(rmb);

                sheet.WriteLine();
            }
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class StatPlayerDragonBallItem : StatDragonItem
{
    public int m_rechargeFromReg;
    public DateTime m_regTime;
}

// 玩家龙珠监控
public class ExportPlayerDragonBallMonitor : ExportExcelBase
{
    private static string[] s_head = new string[] { "玩家ID", "注册时间", 
       "打到龙珠",  "送出龙珠", "收取龙珠", "龙珠兑换","初始龙珠", "龙珠结余",
       "金币充值获得", "金币其余获得", "金币消耗","初始金币", "结余金币",
       "钻石充值获得", "钻石其余获得", "钻石消耗", "初始钻石", "结余钻石",
       "总充值", "时间段内充值"};

    static string[] DB_SE = { "dbStart", "dbRemain" };
    static string[] GOLD_SE = { "goldStart", "goldRemain" };
    static string[] GEM_SE = { "gemStart", "gemRemain" };
    static string[] PLAYER_FIELDS = { "recharged", "create_time" };

    static string MapTable = TableName.STAT_PLAYER_DRAGON + "_mapexport";

    private string m_lastSearchTime = "";
    private int m_dbServerId = -1;
    private bool m_isSame = false;

    private IMongoQuery m_imq = null;

    public override string initExport(ExportParam param, int dbServerId)
    {
        string dbName = "";
        string time = "";
        m_isSame = false;

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            DbServerInfo info = ResMgr.getInstance().getDbInfo(param.m_dbServerIP);
            if (info != null)
            {
                dbName = info.m_serverName;
            }
            if (cond.ContainsKey("time"))
            {
                time = Convert.ToString(cond["time"]);
                if (time == m_lastSearchTime && m_dbServerId == dbServerId)
                {
                    m_isSame = true;
                }
                m_lastSearchTime = time;
                m_dbServerId = dbServerId;

                DateTime mint = DateTime.Now, maxt = DateTime.Now;
                Tool.splitTimeStr(time, ref mint, ref maxt);
                IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
                IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
                queryList.Add(Query.And(imq1, imq2));

                time = mint.ToString("yyyy年MM月dd日") + "~" + maxt.ToString("yyyy年MM月dd日");
            }
        }

        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return string.Format("玩家龙珠监控-{0}-{1}.csv", dbName, time);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        List<BsonDocument> dataList = null;
        string head = string.Join(",", s_head);
        sheet.WriteLine(head);

        if (!m_isSame)
        {
            MapReduceResult map_result = DBMgr.getInstance().executeMapReduce(TableName.STAT_PLAYER_DRAGON,
                                                                    dbServerId,
                                                                    DbName.DB_PUMP,
                                                                    m_imq,
                                                                    MapReduceTable.getMap("playerDragonBall"),
                                                                    MapReduceTable.getReduce("playerDragonBall"),
                                                                    MapTable);
        }

        while (true)
        {
            dataList = DBMgr.getInstance().executeQueryBsonDoc(MapTable, dbServerId, DbName.DB_PUMP, null,
                                        skip, 1000,
                                        null, "value.dbgain", false);

            if (dataList == null || dataList.Count == 0)
                break;

            skip += 1000;

            for (int i = 0; i < dataList.Count; i++)
            {
                BsonDocument data = dataList[i];

                addResult(data, sheet, dbServerId);

                sheet.WriteLine();
            }
        }

        return OpRes.opres_success;
    }

    void addResult(BsonDocument d, StreamWriter sheet, int dbServerId)
    {
        try
        {
            StatPlayerDragonBallItem tmp = new StatPlayerDragonBallItem();

            tmp.m_playerId = Convert.ToInt32(d["_id"]);
            BsonValue resValue = d["value"];
            tmp.m_dbgain = resValue["dbgain"].ToInt64();
            tmp.m_dbsend = resValue["dbsend"].ToInt64();
            tmp.m_dbaccept = resValue["dbaccept"].ToInt64();
            tmp.m_dbexchange = resValue["dbexchange"].ToInt64();
            getSEValue(DB_SE, tmp.m_playerId, dbServerId, ref tmp.m_dbStart, ref tmp.m_dbRemain);

            tmp.m_goldByRecharge = resValue["goldByRecharge"].ToInt64();
            tmp.m_goldByOther = resValue["goldByOther"].ToInt64();
            tmp.m_goldConsume = resValue["goldConsume"].ToInt64();
            getSEValue(GOLD_SE, tmp.m_playerId, dbServerId, ref tmp.m_goldStart, ref tmp.m_goldRemain);

            tmp.m_gemByRecharge = resValue["gemByRecharge"].ToInt64();
            tmp.m_gemByOther = resValue["gemByOther"].ToInt64();
            tmp.m_gemConsume = resValue["gemConsume"].ToInt64();
            getSEValue(GEM_SE, tmp.m_playerId, dbServerId, ref tmp.m_gemStart, ref tmp.m_gemRemain);

            tmp.m_todayRecharge = resValue["totalRecharge"].ToInt32();

            Dictionary<string, object> ret = getPlayerProperty(tmp.m_playerId, dbServerId, PLAYER_FIELDS);
            if (ret != null)
            {
                tmp.m_rechargeFromReg = Convert.ToInt32(ret["recharged"]);
                tmp.m_regTime = Convert.ToDateTime(ret["create_time"]).ToLocalTime();
            }

            sheet.Write(tmp.m_playerId); sheet.Write(",");
            sheet.Write(tmp.m_regTime.ToString()); sheet.Write(",");
            sheet.Write(tmp.m_dbgain); sheet.Write(",");
            sheet.Write(tmp.m_dbsend); sheet.Write(",");
            sheet.Write(tmp.m_dbaccept); sheet.Write(",");
            sheet.Write(tmp.m_dbexchange); sheet.Write(",");
            sheet.Write(tmp.m_dbStart); sheet.Write(",");
            sheet.Write(tmp.m_dbRemain); sheet.Write(",");

            sheet.Write(tmp.m_goldByRecharge); sheet.Write(",");
            sheet.Write(tmp.m_goldByOther); sheet.Write(",");
            sheet.Write(tmp.m_goldConsume); sheet.Write(",");
            sheet.Write(tmp.m_goldStart); sheet.Write(",");
            sheet.Write(tmp.m_goldRemain); sheet.Write(",");

            sheet.Write(tmp.m_gemByRecharge); sheet.Write(",");
            sheet.Write(tmp.m_gemByOther); sheet.Write(",");
            sheet.Write(tmp.m_gemConsume); sheet.Write(",");
            sheet.Write(tmp.m_gemStart); sheet.Write(",");
            sheet.Write(tmp.m_gemRemain); sheet.Write(",");

            sheet.Write(tmp.m_rechargeFromReg); sheet.Write(",");
            sheet.Write(tmp.m_todayRecharge); sheet.Write(",");
        }
        catch (System.Exception ex)
        {
        }
    }

    void getSEValue(string[] fields, int playerId, int dbServerId, ref long svalue, ref long evalue)
    {
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        Tool.splitTimeStr(m_lastSearchTime, ref mint, ref maxt);

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        IMongoQuery imq = Query.And(imq1, imq2, Query.EQ("playerId", BsonValue.Create(playerId)));

        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.STAT_PLAYER_DRAGON,
              dbServerId,
              DbName.DB_PUMP,
             imq, 0, 1, fields, "genTime", true);
        if (dataList.Count > 0)
        {
            svalue = Convert.ToInt32(dataList[0][fields[0]]);
        }

        dataList = DBMgr.getInstance().executeQuery(TableName.STAT_PLAYER_DRAGON,
              dbServerId,
              DbName.DB_PUMP,
             imq, 0, 1, fields, "genTime", false);
        if (dataList.Count > 0)
        {
            evalue = Convert.ToInt32(dataList[0][fields[1]]);
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// cdkey导出
public class ExportCDKEY : ExportExcelBase
{
    private IMongoQuery m_imq = null;

    public override string initExport(ExportParam param, int dbServerId)
    {
        int pici = 1;

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        Dictionary<string, object> cond = param.m_condition;
        if (cond != null)
        {
            if (cond.ContainsKey("pici"))
            {
                pici = Convert.ToInt32(cond["pici"]);
                queryList.Add(Query.EQ("pici", BsonValue.Create(pici)));
            }
        }

        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return string.Format("cdkey-批次-{0}.csv", pici);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        List<Dictionary<string, object>> dataList = null;

        sheet.WriteLine("生成时间,cdkey,批次,使用玩家ID,使用日期");

        while (true)
        {
            dataList = nextData(ref skip, 1000, m_imq, TableName.GIFT_CODE, dbServerId, DbName.DB_ACCOUNT);
            if (dataList == null)
                break;

            for (int i = 0; i < dataList.Count; i++)
            {
                try
                {
                    Dictionary<string, object> data = dataList[i];
                    sheet.Write(Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString());
                    sheet.Write(",");

                    string src = Convert.ToString(data["cdkey"]);
                    sheet.Write(AESHelper.AESDecrypt(src, ConstDef.AES_FOR_CDKEY));
                    sheet.Write(",");

                    sheet.Write(Convert.ToString(data["pici"]));
                    sheet.Write(",");

                    int t = Convert.ToInt32(data["playerId"]);
                    if (t > 0)
                    {
                        sheet.Write(t);
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("useTime"))
                    {
                        sheet.Write(Convert.ToDateTime(data["useTime"]).ToLocalTime().ToShortDateString());
                    }

                    sheet.WriteLine();
                }
                catch (System.Exception ex)
                {
                    LogMgr.error("导出cdkey记录出现异常:{0}", ex.ToString());
                }
            }
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
// 兑换导出
public class ExportExchange : ExportExcelBase
{
    private IMongoQuery m_imq = null;

    static string[] s_field = { "bindPhone", "recharged" };

    public override string initExport(ExportParam param, int dbServerId)
    {
        //int pici = 1;

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        queryList.Add(Query.EQ("status", BsonValue.Create(4)));

        List<int> chgIdList = new List<int>();
        getChangIdList(1, ref chgIdList); //1实物
        if (chgIdList.Count != 0)
        {
            IMongoQuery[] queries = new IMongoQuery[chgIdList.Count];
            int k = 0;
            foreach (var chgId in chgIdList)
            {
                queries[k] = Query.EQ("chgId", chgId);
                k++;
            }
            queryList.Add(Query.Or(queries));
        }

        /* Dictionary<string, object> cond = param.m_condition;
         if (cond != null)
         {
             if (cond.ContainsKey("pici"))
             {
                 pici = Convert.ToInt32(cond["pici"]);
                 queryList.Add(Query.EQ("pici", BsonValue.Create(pici)));
             }
         }*/

        m_imq = queryList.Count > 0 ? Query.And(queryList) : null;

        return string.Format("exchange-state-{0}.csv", 4);
    }

    public override OpRes exportData(StreamWriter sheet, ExportParam param, int dbServerId)
    {
        int skip = 0;
        List<Dictionary<string, object>> dataList = null;

        sheet.WriteLine("玩家ID,渠道,手机号,绑定手机号,历史充值金额,历史发放金额,兑换ID,道具名称,数量,领取时间,成功时间,兑换状态");

        while (true)
        {
            dataList = nextData(ref skip, 1000, m_imq, TableName.EXCHANGE, dbServerId, DbName.DB_PLAYER);
            if (dataList == null)
                break;

            for (int i = 0; i < dataList.Count; i++)
            {
                try
                {
                    Dictionary<string, object> data = dataList[i];
                    int playerId = 0;
                    if(data.ContainsKey("playerId"))
                    {
                        sheet.Write(Convert.ToString(data["playerId"]));
                        playerId = Convert.ToInt32(data["playerId"]);
                    }
                    sheet.Write(",");
                    if(data.ContainsKey("channel"))
                    {
                        string name = GetChannelName(Convert.ToString(data["channel"]));
                        sheet.Write(name);
                    }
                    sheet.Write(",");

                    if(data.ContainsKey("phone"))
                    {
                        sheet.Write(Convert.ToString(data["phone"]));
                    }
                    sheet.Write(",");

                    Dictionary<string, object> daInfo = oneData(Query.EQ("player_id", playerId),
                        TableName.PLAYER_INFO, dbServerId, DbName.DB_PLAYER, s_field);
                    if (daInfo != null)
                    {
                        if (daInfo.ContainsKey("bindPhone"))
                        {
                            sheet.Write(Convert.ToString(daInfo["bindPhone"]));
                        }
                        sheet.Write(",");

                        if (daInfo.ContainsKey("recharged"))
                        {
                            sheet.Write(Convert.ToString(daInfo["recharged"]));
                        }
                        sheet.Write(",");
                    }
                    else
                    {
                        sheet.Write(",");
                        sheet.Write(",");
                    }

                    Dictionary<string, object> historyInfo = oneData(Query.EQ("playerId", playerId),
                        TableName.STAT_PLAYER_EXCHANGE, dbServerId, DbName.DB_PLAYER);
                    if (historyInfo != null)
                    {
                        if (historyInfo.ContainsKey("historyMoney"))
                        {
                            sheet.Write(Convert.ToString(historyInfo["historyMoney"]));
                        }
                        sheet.Write(",");
                    }
                    else
                    {
                        sheet.Write(",");
                    }

                    int chgId = 0;
                    if (data.ContainsKey("chgId"))
                    {
                        sheet.Write(Convert.ToString(data["chgId"]));
                        chgId = Convert.ToInt32(data["chgId"]);
                    }
                    sheet.Write(",");

                    string[] items = getChgItem(chgId);
                    sheet.Write(items[0]);
                    sheet.Write(",");  // 道具名称
                    sheet.Write(items[1]);
                    sheet.Write(",");  // 数量

                    if (data.ContainsKey("genTime")) // 领取时间
                    {
                        sheet.Write(Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString());
                    }
                    sheet.Write(",");
                    if (data.ContainsKey("giveOutTime")) // 成功时间
                    {
                        sheet.Write(Convert.ToDateTime(data["giveOutTime"]).ToLocalTime().ToShortDateString());
                    }
                    sheet.Write(",");

                    if (data.ContainsKey("status"))
                    {
                        sheet.Write("已兑换"/*Convert.ToString(data["status"])*/);
                    }
                    
                    sheet.WriteLine();
                }
                catch (System.Exception ex)
                {
                    LogMgr.error("导出Exchange记录出现异常:{0}", ex.ToString());
                }
            }
        }

        return OpRes.opres_success;
    }


    //获取对应id 0 不显示  1实物 2道具
    public List<int> getChangIdList(int type, ref List<int> chgIdList)
    {
        Dictionary<int, ExchangeData> chgItemList = ExchangeCfg.getInstance().getAllData();
        if (chgItemList.Count != 0 && chgItemList != null)
        {
            foreach (ExchangeData chgItem in chgItemList.Values)
            {
                if (chgItem.m_type == type)
                    chgIdList.Add(chgItem.m_id);
            }
        }
        return chgIdList;
    }

    protected string[] getChgItem(int chgId)
    {
        string[] item = new string[2] { "", "" };

        ExchangeData chgItem = ExchangeCfg.getInstance().getValue(chgId);
        if (chgItem != null)
        {
            item[0] = chgItem.m_managementName;
            item[1] = "1"; //chgItem.m_itemCount.ToString();
        }

        return item;
    }
}

