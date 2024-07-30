using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Linq;

// 牛牛的参数查询结果
public class ResultParamCows : ResultFishlordExpRate
{
    public long m_burstZhuang;   // 爆庄支出

    public long m_serviceCharge; // 手续费

    // 总盈利率
    public string getTotalRate()
    {
        long totalIncome = m_totalIncome + m_serviceCharge;
        long totalOutlay = m_totalOutlay + m_burstZhuang;

        if (totalIncome == 0 && totalOutlay == 0)
            return "0";
        if (totalIncome == 0)
            return "-∞";

        double factGain = (double)(totalIncome - totalOutlay) / totalIncome;
        return Math.Round(factGain, 3).ToString();
    }
}

// 牛牛参数查询
public class QueryCowsParam : QueryBase
{
    private Dictionary<int, ResultParamCows> m_result = new Dictionary<int, ResultParamCows>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.COWS_ROOM, dip);
        if (dataList == null)
            return OpRes.op_res_not_found_data;
 
        for (int i = 0; i < dataList.Count; i++)
        {
            ResultParamCows info = new ResultParamCows();
            info.m_roomId = Convert.ToInt32(dataList[i]["room_id"]);
            if (dataList[i].ContainsKey("ExpectEarnRate"))
            {
                info.m_expRate = Convert.ToDouble(dataList[i]["ExpectEarnRate"]);
            }
            else
            {
                info.m_expRate = 0.05;
            }

            if (dataList[i].ContainsKey("room_income")) // 总收入
            {
                info.m_totalIncome = Convert.ToInt64(dataList[i]["room_income"]);
            }
            if (dataList[i].ContainsKey("room_outcome"))  // 总支出
            {
                info.m_totalOutlay = Convert.ToInt64(dataList[i]["room_outcome"]);
            }
            if (dataList[i].ContainsKey("BankerAddGold")) // 上庄手续费
            {
                info.m_serviceCharge = Convert.ToInt64(dataList[i]["BankerAddGold"]);
            }
            if (dataList[i].ContainsKey("BankerSubGold")) // 爆庄支出
            {
                info.m_burstZhuang = Convert.ToInt64(dataList[i]["BankerSubGold"]);
            }
            if (dataList[i].ContainsKey("TotalRobotWinGold")) // 机器人收入
            {
                info.m_robotIncome = Convert.ToInt64(dataList[i]["TotalRobotWinGold"]);
            }
            if (dataList[i].ContainsKey("TotalRobotLoseGold")) // 机器人支出
            {
                info.m_robotOutlay = Convert.ToInt64(dataList[i]["TotalRobotLoseGold"]);
            }
            if (dataList[i].ContainsKey("player_count"))
            {
                info.m_curPlayerCount = Convert.ToInt32(dataList[i]["player_count"]);
            }
            if(dataList[i].ContainsKey("TotalPlayerPump"))
            {
                info.m_totalPlayerPump = Convert.ToInt64(dataList[i]["TotalPlayerPump"]);
            }
            m_result.Add(info.m_roomId, info);
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
//牛牛牌局查询
public class CowsCardsItem 
{
    public string m_time;
    public long m_cardId;
    public long m_playerWinLose0;
    public long m_playerWinLose1;
    public long m_playerWinLose2;
    public long m_playerWinLose3;
    public long m_pumpTotal;
    public long m_pumpBankerTotal;
    public long m_sysOutlay;
    public long m_sysIncome;
    public long m_playerOutlay;
    public long m_playerIncome;
    public long m_bankerWinLose;
    public string m_bankerId;
    public int m_roomId;
    public string getExParam(int index)
    {
        if (m_cardId > 0)
        {
            URLParam uParam = new URLParam();
            uParam.m_text = "详情";
            uParam.m_key = "cardId";
            uParam.m_value = m_cardId.ToString();
            uParam.m_url = DefCC.ASPX_GAME_DETAIL_COWS_CARDS;
            uParam.m_target = "_blank";
            return Tool.genHyperlink(uParam);
        }
        return "";
    }
    public string getExParam1(int index)
    {
        if (m_cardId > 0)
        {
            URLParam uParam = new URLParam();
            uParam.m_text = "详情";
            uParam.m_key = "cardId";
            uParam.m_value = m_cardId.ToString();
            uParam.m_url = DefCC.ASPX_GAME_DETAIL_COWS_CARDS_PLAYER_LIST;
            uParam.m_target = "_blank";
            uParam.addExParam("roomId", m_roomId);
            return Tool.genHyperlink(uParam);
        }
        return "";
    }
}

public class QueryCowsCardsQuery : QueryBase 
{
    private List<CowsCardsItem> m_result = new List<CowsCardsItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p=(ParamQuery)param;

        QueryCondition queryCond = new QueryCondition();

        long cardId = 0;
        if(p.m_param!="")
        {
            if (!long.TryParse(p.m_param, out cardId))
            {
                return OpRes.op_res_param_not_valid;
            }
            queryCond.addImq(Query.EQ("id",Convert.ToInt64(p.m_param)));
        }

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);

        if (!res && p.m_param == "")
        {
            return OpRes.op_res_time_format_error;
        }
        else if (res) 
        {
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            IMongoQuery imq3 = Query.And(imq1, imq2);
            queryCond.addImq(imq3);
        }

        int bankerId = 0;
        if (p.m_playerId != "")
        {
            if (!int.TryParse(p.m_playerId, out bankerId))
            {
                return OpRes.op_res_param_not_valid;
            }
            queryCond.addImq(Query.EQ("bankerId", Convert.ToInt32(p.m_playerId)));
        }

        queryCond.addImq(Query.EQ("roomId",Convert.ToInt32(p.m_op))); //房间ID
        

        IMongoQuery imq = queryCond.getImq();
        return query(p,imq,user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param,IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.LOG_COWS_CARD_BOARD, imq, dip);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.LOG_COWS_CARD_BOARD,
            dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);
        if (dataList == null||dataList.Count==0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            CowsCardsItem info = new CowsCardsItem();
            info.m_time = Convert.ToDateTime(dataList[i]["genTime"]).ToLocalTime().ToString();
            info.m_cardId = Convert.ToInt64(dataList[i]["id"]);

            info.m_roomId = Convert.ToInt32(dataList[i]["roomId"]);

            Dictionary<string,object> area0=(Dictionary<string,object>)dataList[i]["area0"];
            info.m_playerWinLose0=Convert.ToInt64(area0["winLose"]);

            Dictionary<string, object> area1 = (Dictionary<string, object>)dataList[i]["area1"];
            info.m_playerWinLose1 = Convert.ToInt64(area1["winLose"]);

            Dictionary<string, object> area2 = (Dictionary<string, object>)dataList[i]["area2"];
            info.m_playerWinLose2 = Convert.ToInt64(area2["winLose"]);

            Dictionary<string, object> area3 = (Dictionary<string, object>)dataList[i]["area3"];
            info.m_playerWinLose3 = Convert.ToInt64(area3["winLose"]);
            
            info.m_pumpTotal=Convert.ToInt64(dataList[i]["pumpTotal"]);
            info.m_pumpBankerTotal = Convert.ToInt64(dataList[i]["pumpBankerTotal"]);
            info.m_sysOutlay=Convert.ToInt64(dataList[i]["sysOutlay"]);
            info.m_sysIncome=Convert.ToInt64(dataList[i]["sysIncome"]);
            info.m_playerOutlay=Convert.ToInt64(dataList[i]["playerOutlay"]);
            info.m_playerIncome=Convert.ToInt64(dataList[i]["playerIncome"]);
            info.m_bankerWinLose = Convert.ToInt64(dataList[i]["bankerWinLose"]);

            if (dataList[i].ContainsKey("bankerId"))
            {
                if (Convert.ToInt32(dataList[i]["bankerId"]) != 0)
                {
                    if (Convert.ToBoolean(dataList[i]["isBankerRobot"]))//是机器人
                    {
                        info.m_bankerId = Convert.ToString(dataList[i]["bankerId"]) + "(R)";
                    }
                    else
                    {
                        info.m_bankerId = Convert.ToString(dataList[i]["bankerId"]);
                    }
                }
                else
                {
                    info.m_bankerId = "系统";
                }
            }
            m_result.Add(info);
        }
        return OpRes.opres_success;
    }
}

//牛牛下注玩家列表
public class CowsPlayerItem 
{
    public int m_playerId;
    public int m_pumpMoney;
    public int m_betMoney0;
    public int m_winGold0;
    public int m_betMoney1;
    public int m_winGold1;
    public int m_betMoney2;
    public int m_winGold2;
    public int m_betMoney3;
    public int m_winGold3;
}
public class QueryCowsCardsPlayerList : QueryBase 
{
    private List<CowsPlayerItem> m_result = new List<CowsPlayerItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        long cardId = Convert.ToInt64(p.m_param);

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.EQ("cardBoardId", cardId);
        queryCond.addImq(imq1);
        IMongoQuery imq2 = Query.EQ("gameId", 4);
        queryCond.addImq(imq2);

        IMongoQuery imq3 = Query.EQ("roomId", Convert.ToInt32(p.m_op));
        queryCond.addImq(imq3);

        IMongoQuery imq = queryCond.getImq();
        return query(user,imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user,IMongoQuery imq)
    {
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_PLAYER_MONEY,
            user.getDbServerID(), DbName.DB_PUMP, imq, 0, 0, null, "playerOutlay", false);
        if (dataList == null ||dataList.Count==0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data= dataList[i];
            CowsPlayerItem info = new CowsPlayerItem();
            info.m_playerId = Convert.ToInt32(data["playerId"]);
            if(data.ContainsKey("pumpMoney"))
            {
                info.m_pumpMoney = Convert.ToInt32(data["pumpMoney"]);
            }
            
            if(data.ContainsKey("exInfo"))
            {
                string exInfo=Convert.ToString(data["exInfo"]);
                string[] str=Tool.split(exInfo,',');

                string[] bet0 = Tool.split(str[0],':');
                string[] gold0 = Tool.split(str[1],':');
                string[] bet1 = Tool.split(str[2], ':');
                string[] gold1 = Tool.split(str[3],':');
                string[] bet2 = Tool.split(str[4], ':');
                string[] gold2 = Tool.split(str[5],':');
                string[] bet3 = Tool.split(str[6], ':');
                string[] gold3 = Tool.split(str[7],':');

                info.m_betMoney0 = Convert.ToInt32(bet0[1]);
                info.m_winGold0 = Convert.ToInt32(gold0[1]);
                info.m_betMoney1 = Convert.ToInt32(bet1[1]);
                info.m_winGold1 = Convert.ToInt32(gold1[1]);
                info.m_betMoney2 = Convert.ToInt32(bet2[1]);
                info.m_winGold2 = Convert.ToInt32(gold2[1]);
                info.m_betMoney3 = Convert.ToInt32(bet3[1]);
                info.m_winGold3 = Convert.ToInt32(Tool.split(gold3[1],'}')[0]);
            }
            m_result.Add(info);
        }

        return OpRes.opres_success;
    }
}
//牛牛牌局详情
public class QueryCowsCardsDetail : QueryBase
{
    private List<InfoCows> m_result = new List<InfoCows>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        long cardId = Convert.ToInt64(p.m_param);
        return query(user, cardId);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    public DbCowsCard splitStr(string cardStr)
    {
        DbCowsCard c = new DbCowsCard();
        string[] arr = Tool.split(cardStr, ',');
        string[] cardType = Tool.split(arr[0], ':');
        c.cardsType = Convert.ToInt32(cardType[1]);

        string[] cardsValue = Tool.split(arr[1], ':');
        c.cardsValue = Convert.ToInt32(cardsValue[1]);

        string[] cards0 = Tool.split(arr[2], ':');
        c.cards0 = Convert.ToInt32(cards0[1]);

        string[] cards1 = Tool.split(arr[3], ':');
        c.cards1 = Convert.ToInt32(cards1[1]);

        string[] cards2 = Tool.split(arr[4], ':');
        c.cards2 = Convert.ToInt32(cards2[1]);

        string[] cards3 = Tool.split(arr[5], ':');
        c.cards3 = Convert.ToInt32(cards3[1]);

        string[] cards4 = Tool.split(arr[6], ':');
        c.cards4 = Convert.ToInt32(Tool.split(cards4[1], '}')[0]);
        return c;
    }

    private OpRes query(GMUser user, long cardId)
    {
        string [] m_fields={"isBankerRobot","bankerId","bankerWinLose","pumpBankerTotal","bankerCards","area0","area1","area2","area3"};

        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.LOG_COWS_CARD_BOARD, "id", cardId, m_fields,
            user.getDbServerID(), DbName.DB_PUMP);

        if (data != null)
        {
            InfoCows info = new InfoCows();

            if(data.ContainsKey("bankerId"))
            {
                info.m_banker_id = Convert.ToInt32(data["bankerId"]);
                if (Convert.ToInt32(data["bankerId"]) != 0)
                {
                    if (Convert.ToBoolean(data["isBankerRobot"])) //机器人
                    {
                        info.m_bankerId = Convert.ToString(data["bankerId"]) + "(R)";
                    }
                    else
                    {
                        info.m_bankerId = Convert.ToString(data["bankerId"]);
                    }
                }
                else 
                {
                    info.m_bankerId = "系统";
                }
            }

            if (data.ContainsKey("pumpBankerTotal"))
            {
                info.m_pumpBankerTotal = Convert.ToInt32(data["pumpBankerTotal"]);
            }

            if (data.ContainsKey("bankerCards")) 
            {
                string cardStr = Convert.ToString(data["bankerCards"]);

                DbCowsCard c=splitStr(cardStr);
                if (data.ContainsKey("bankerWinLose"))
                {
                    c.cardsWinLose = Convert.ToInt64(data["bankerWinLose"]);
                }
                info.createBankerCard(c);
            }

            if(data.ContainsKey("area0"))
            {
                Dictionary<string,object> m_area0=(Dictionary<String,object>)data["area0"];
                string cardStr=Convert.ToString(m_area0["cardStr"]);

                DbCowsCard c = splitStr(cardStr);
                c.cardsWinLose = Convert.ToInt64(m_area0["winLose"]);
                info.createEastCard(c);
            }
            
            if(data.ContainsKey("area1"))
            {
                Dictionary<string, object> m_area1 = (Dictionary<String, object>)data["area1"];
                string cardStr = Convert.ToString(m_area1["cardStr"]);

                DbCowsCard c = splitStr(cardStr);
                c.cardsWinLose = Convert.ToInt64(m_area1["winLose"]);
                info.createSouthCard(c);
            }

            if (data.ContainsKey("area2"))
            {
                Dictionary<string, object> m_area2 = (Dictionary<String, object>)data["area2"];
                string cardStr = Convert.ToString(m_area2["cardStr"]);

                DbCowsCard c = splitStr(cardStr);
                c.cardsWinLose = Convert.ToInt64(m_area2["winLose"]);
                info.createWestCard(c);
            }

            if (data.ContainsKey("area3"))
            {
                Dictionary<string, object> m_area3 = (Dictionary<String, object>)data["area3"];
                string cardStr = Convert.ToString(m_area3["cardStr"]);

                DbCowsCard c = splitStr(cardStr);
                c.cardsWinLose = Convert.ToInt64(m_area3["winLose"]);
                info.createNorthCard(c);
            }

            m_result.Add(info);
        }
        return OpRes.opres_success;
    }
}
//////////////////////////////////////////////////////////////////////////

public class ResultCowsCard : ParamAddCowsCard
{
    public string m_key = "";
    public string m_time = "";
}

// 牛牛牌型查询
public class QueryCowsCardsType : QueryBase
{
    private List<ResultCowsCard> m_result = new List<ResultCowsCard>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.COWS_CARDS,
            dip, null, 0, 0, null, "insert_time", false);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultCowsCard info = new ResultCowsCard();
            info.m_key = Convert.ToString(dataList[i]["key"]);
            info.m_time = Convert.ToDateTime(dataList[i]["insert_time"]).ToLocalTime().ToString();
            info.m_bankerType = Convert.ToInt32(dataList[i]["banker_cards"]);
            info.m_other1Type = Convert.ToInt32(dataList[i]["other_cards1"]);
            info.m_other2Type = Convert.ToInt32(dataList[i]["other_cards2"]);
            info.m_other3Type = Convert.ToInt32(dataList[i]["other_cards3"]);
            info.m_other4Type = Convert.ToInt32(dataList[i]["other_cards4"]);
            m_result.Add(info);
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
// 游戏结果控制
public class ParamGameResultControl
{
    public GameId m_gameId;
}

// 查询游戏控制结果
public class QueryGameResultControl : QueryBase
{
    private Dictionary<GameId, QueryBase> m_games = new Dictionary<GameId, QueryBase>();

    public QueryGameResultControl()
    {
        m_games.Add(GameId.shcd, new QueryGameResultShcd());
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamGameResultControl p = (ParamGameResultControl)param;
        if (m_games.ContainsKey(p.m_gameId))
        {
            return m_games[p.m_gameId].doQuery(p, user);
        }
        return OpRes.op_res_failed;
    }

    public override object getQueryResult(object param, GMUser user)
    {
        ParamGameResultControl p = (ParamGameResultControl)param;
        if (m_games.ContainsKey(p.m_gameId))
        {
            return m_games[p.m_gameId].getQueryResult();
        }
        return null;
    }
}

public class GameResultShcd
{
    public string m_insertTime;
    public int m_result;
    public int m_roomId = 1; // 默认是金币场
}

public class QueryGameResultShcd : QueryBase
{
    List<GameResultShcd> m_result = new List<GameResultShcd>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.SHCD_RESULT, dip, null, 0, 0, null, "insert_time", false);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            GameResultShcd info = new GameResultShcd();
            info.m_insertTime = Convert.ToDateTime(data["insert_time"]).ToLocalTime().ToString();
            info.m_result = Convert.ToInt32(data["next_card_type"]);
            if (data.ContainsKey("room_id"))
            {
                info.m_roomId = Convert.ToInt32(data["room_id"]);
            }
            m_result.Add(info);
        }

        return OpRes.opres_success;
    }

    public override object getQueryResult()
    {
        return m_result;
    }
}

//////////////////////////////////////////////////////////////////////////
//水浒传
public class ResultShuihzEarningItem : ResultExpRateParam
{
    public long m_gameTotal=0;
    public string m_time;
    public int m_playerId;

    public long m_playCount;
    public long m_bonusTurnCount;
    public long m_rewardGold;
    public int m_lowerLimit;
    public int m_upperLimit;
}

//总盈利率
public class QueryShuihzTotalEarning : QueryBase 
{
    List<ResultShuihzEarningItem> m_result = new List<ResultShuihzEarningItem>();

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.SHUIHZ_ROOM, dip);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            ResultShuihzEarningItem info = new ResultShuihzEarningItem();
            m_result.Add(info);

            info.m_totalIncome = Convert.ToInt64(data["room_income"]);
            info.m_totalOutlay = Convert.ToInt64(data["room_outcome"]);
            if (data.ContainsKey("buffRecharge"))
            {
                info.m_gameTotal = Convert.ToInt64(data["buffRecharge"]);
            }
        }
        return OpRes.opres_success;
    }
}

//每日盈利率
public class QueryShuihzDailyEarning : QueryBase
{
    List<ResultShuihzEarningItem> m_result = new List<ResultShuihzEarningItem>();

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("Date", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("Date", BsonValue.Create(mint));

        IMongoQuery imq = Query.And(imq1, imq2);

        return query(p, user, imq);
    }

    private OpRes query(ParamQuery param, GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.SHUIHZ_DAILY, dip, imq, 0, 0, null, "Date", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            ResultShuihzEarningItem info = new ResultShuihzEarningItem();
            m_result.Add(info);

            info.m_time = Convert.ToDateTime(data["Date"]).ToLocalTime().ToShortDateString();
            info.m_totalIncome = Convert.ToInt64(data["TodayIncome"]);
            info.m_totalOutlay = Convert.ToInt64(data["TodayOutlay"]);
           
            info.m_gameTotal = Convert.ToInt64(data["room1BufRecharge"]);

        }

        return OpRes.opres_success;
    }
}

//单个玩家盈利率
public class QueryShuihzSingleEarning : QueryBase
{
    List<ResultShuihzEarningItem> m_result = new List<ResultShuihzEarningItem>();

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;
        IMongoQuery imq;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
        imq = Query.And(imq1, imq2);

        //如果有输入玩家ID则条件加入ID，若只输入时间，则查询所有玩家
        int playerId = 0;
        if (!string.IsNullOrEmpty(p.m_param))
        {
            if (!int.TryParse(p.m_param, out playerId))
                return OpRes.op_res_param_not_valid;
            imq = Query.And(imq,Query.EQ("playerId", playerId));
        }

        return query(p, user, imq);
    }

    private OpRes query(ParamQuery param, GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.SHUIHZ_PLAYER_EVERY_DAY, imq, dip);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.SHUIHZ_PLAYER_EVERY_DAY,
              dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            ResultShuihzEarningItem info = new ResultShuihzEarningItem();
            m_result.Add(info);

            info.m_playerId = Convert.ToInt32(data["playerId"]);
            info.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            if (data.ContainsKey("sysIncomeGold"))
            {
                info.m_totalIncome = Convert.ToInt64(data["sysIncomeGold"]);
            }

            if (data.ContainsKey("sysOutlayGold"))
            {
                info.m_totalOutlay = Convert.ToInt64(data["sysOutlayGold"]);
            }
            if (data.ContainsKey("buffRecharge"))
            {
                info.m_gameTotal = Convert.ToInt64(data["buffRecharge"]);
            }
        }
        return OpRes.opres_success;
    }
}

//水浒传每日游戏情况查看
public class QueryShuihzDailyState : QueryBase
{
    List<ResultShuihzEarningItem> m_result = new List<ResultShuihzEarningItem>();

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

        IMongoQuery imq = Query.And(imq1, imq2);

        return query(p, user, imq);
    }

    private OpRes query(ParamQuery param, GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.SHUIHZ_DAILY_STATE,
              dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            ResultShuihzEarningItem info = new ResultShuihzEarningItem();
            m_result.Add(info);

            info.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            info.m_playCount = Convert.ToInt64(data["playCount"]);
            info.m_bonusTurnCount = Convert.ToInt64(data["bonusTurnCount"]);
            info.m_rewardGold = Convert.ToInt64(data["rewardGold"]);

        }

        return OpRes.opres_success;
    }
}

//水浒传每日达上下限人数统计
public class QueryShuihzReachLimit : QueryBase
{
    List<ResultShuihzEarningItem> m_result = new List<ResultShuihzEarningItem>();

    // 返回查询结果
    public override object getQueryResult()
    {
        return m_result;
    }

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        ParamQuery p = (ParamQuery)param;

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

        IMongoQuery imq = Query.And(imq1, imq2);

        return query(p, user, imq);
    }

    private OpRes query(ParamQuery param, GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.SHUIHZ_REACH_LIMIT,
              dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
        {
            return OpRes.op_res_not_found_data;
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            ResultShuihzEarningItem info = new ResultShuihzEarningItem();
            m_result.Add(info);

            info.m_time = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();
            if(dataList[i].ContainsKey("lowerLimit"))
            {
                info.m_lowerLimit = Convert.ToInt32(data["lowerLimit"]);
            }
            if(dataList[i].ContainsKey("upperLimit"))
            {
                info.m_upperLimit = Convert.ToInt32(data["upperLimit"]);
            }
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ResultExpRateParam
{
    public long m_totalIncome;
    public long m_totalOutlay;
    public double m_expRate;
    public double m_expMaxRate;

    // 返回实际盈利率
    public double getFactExpRate()
    {
        if (m_totalIncome == 0)
            return 0;

        double factGain = (double)(m_totalIncome - m_totalOutlay) / m_totalIncome;
        return Math.Round(factGain, 3);
    }

    // 总盈亏
    public long getDelta()
    {
        return m_totalIncome - m_totalOutlay;
    }
}

public class ResultDragonParam : ResultExpRateParam
{
    public int m_roomId;
    public long m_doubleIncome;
    public long m_doubleOutcome;

    // 玩家个数
    public int m_curPlayerCount = 0;
}

// 五龙参数查询
public class QueryDragonParam : QueryBase
{
    private List<ResultDragonParam> m_result = new List<ResultDragonParam>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.DRAGON_ROOM, dip);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultDragonParam info = new ResultDragonParam();
            m_result.Add(info);

            info.m_roomId = Convert.ToInt32(dataList[i]["room_id"]);
            if (dataList[i].ContainsKey("expect_earn_rate"))
            {
                info.m_expRate = Convert.ToDouble(dataList[i]["expect_earn_rate"]);
            }
            else
            {
                info.m_expRate = 0.05;
            }

            if (dataList[i].ContainsKey("room_income")) // 总收入
            {
                info.m_totalIncome = Convert.ToInt64(dataList[i]["room_income"]);
            }
            if (dataList[i].ContainsKey("room_outcome"))  // 总支出
            {
                info.m_totalOutlay = Convert.ToInt64(dataList[i]["room_outcome"]);
            }
            if (dataList[i].ContainsKey("double_income"))  // 翻倍收入
            {
                info.m_doubleIncome = Convert.ToInt64(dataList[i]["double_income"]);
            }
            if (dataList[i].ContainsKey("double_outcome"))  // 翻倍支出
            {
                info.m_doubleOutcome = Convert.ToInt64(dataList[i]["double_outcome"]);
            }
            if (dataList[i].ContainsKey("player_count"))
            {
                info.m_curPlayerCount = Convert.ToInt32(dataList[i]["player_count"]);
            }
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ParamDragonGameModeEarning
{
    public int m_roomId;
    public string m_tableId;

    public ParamDragonGameModeEarning()
    {
        m_roomId = -1;
    }
}

public class ResultDragonGameModeEarning
{
    public const int MODE_NORMAL = 0;
    public const int MODE_FREE_GAME = 1;
    public const int MODE_DOUBLE = 2;
    public static string[] s_mode = { "普通模式", "freegame模式", "翻倍游戏" };

    private Dictionary<int, ResultExpRateParam> m_result =
        new Dictionary<int, ResultExpRateParam>();

    public void reset()
    {
        m_result.Clear();
    }

    public void addModeData(int mode, long income, long outlay)
    {
        ResultExpRateParam r = null;
        if (m_result.ContainsKey(mode))
        {
            r = m_result[mode];
        }
        else
        {
            r = new ResultExpRateParam();
            m_result.Add(mode, r);
        }

        r.m_totalIncome += income;
        r.m_totalOutlay += outlay;
    }

    public ResultExpRateParam getParam(int mode)
    {
        if (m_result.ContainsKey(mode))
            return m_result[mode];

        return null;
    }
}

// 五龙各游戏模式下的盈利率查询
public class QueryDragonGameModeEarning : QueryBase
{
    private ResultDragonGameModeEarning m_result = new ResultDragonGameModeEarning();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamDragonGameModeEarning p =(ParamDragonGameModeEarning)param;
        int tableId = -1;
        if (!string.IsNullOrEmpty(p.m_tableId) && 
            !int.TryParse(p.m_tableId, out tableId))
            return OpRes.op_res_param_not_valid;

        List<IMongoQuery> queryList = new List<IMongoQuery>();
        if (p.m_roomId >= 0)
        {
            queryList.Add(Query.EQ("room_id", BsonValue.Create(p.m_roomId)));
        }
        if (tableId >= 0)
        {
            queryList.Add(Query.EQ("table_id", BsonValue.Create(tableId)));
        }
     
        IMongoQuery imq = queryList.Count > 0 ? Query.And(queryList) : null;

        m_result.reset();
        return query(user, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.DRAGON_TABLE, dip, imq);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        long income = 0, outlay = 0;
        for (int i = 0; i < dataList.Count; i++)
        {
            income = outlay = 0;
            Dictionary<string, object> data = dataList[i];
            if (data.ContainsKey("table_income"))
            {
                income = Convert.ToInt64(data["table_income"]);
            }
            if (data.ContainsKey("normal_outcome"))
            {
                outlay = Convert.ToInt64(data["normal_outcome"]);
            }
            m_result.addModeData(ResultDragonGameModeEarning.MODE_NORMAL, income, outlay);

            income = outlay = 0;
            if (data.ContainsKey("free_outcome"))
            {
                outlay = Convert.ToInt64(data["free_outcome"]);
            }
            m_result.addModeData(ResultDragonGameModeEarning.MODE_FREE_GAME, income, outlay);

            income = outlay = 0;
            if (data.ContainsKey("double_income"))
            {
                income = Convert.ToInt64(data["double_income"]);
            }
            if (data.ContainsKey("double_outcome"))
            {
                outlay = Convert.ToInt64(data["double_outcome"]);
            }
            m_result.addModeData(ResultDragonGameModeEarning.MODE_DOUBLE, income, outlay);
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
//黑红牌局查询
public class ShcdCardsItem 
{
    public string m_time;
    public long m_cardId;
    public int m_cardType;
    public int m_cardValue;
    public string m_resCard;
    public long m_sysOutlay;
    public long m_sysIncome;
    public int m_roomId;
    public string getExParam(int index)  
    {
        if (m_cardId > 0)
        {
            URLParam uParam = new URLParam();
            uParam.m_text = "详情";
            uParam.m_key = "cardId";
            uParam.m_value = m_cardId.ToString();
            uParam.m_url = DefCC.ASPX_GAME_DETAIL_SHCD_CARDS;
            uParam.m_target = "_blank";
            return Tool.genHyperlink(uParam);
        }
        return "";
    }
    public string getExParam1(int index)
    {
        if (m_cardId > 0)
        {
            URLParam uParam = new URLParam();
            uParam.m_text = "详情";
            uParam.m_key = "cardId";
            uParam.m_value = m_cardId.ToString();
            uParam.m_url = DefCC.ASPX_GAME_DETAIL_SHCD_CARDS_PLAYER_LIST;
            uParam.m_target = "_blank";
            uParam.addExParam("roomId", m_roomId);
            return Tool.genHyperlink(uParam);
        }
        return "";
    }
}
public class QueryShcdCardsQuery : QueryBase 
{
    private List<ShcdCardsItem> m_result = new List<ShcdCardsItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        QueryCondition queryCond = new QueryCondition();

        long cardId = 0;
        if (p.m_param != "")
        {
            if (!long.TryParse(p.m_param, out cardId))
            {
                return OpRes.op_res_param_not_valid;
            }
            queryCond.addImq(Query.EQ("id", Convert.ToInt64(p.m_param)));
        }

        queryCond.addImq(Query.EQ("roomId", Convert.ToInt32(p.m_op)));

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_time, ref mint, ref maxt);

        if (!res && p.m_param == "")
        {
            return OpRes.op_res_time_format_error;
        }
        else if (res)
        {
            IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
            IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));
            IMongoQuery imq3 = Query.And(imq1, imq2);
            queryCond.addImq(imq3);
        }

        IMongoQuery imq = queryCond.getImq();
        return query(p, imq, user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(ParamQuery param, IMongoQuery imq, GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.SHCD_CARD_BOARD, imq, dip);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.SHCD_CARD_BOARD,
            dip, imq, (param.m_curPage - 1) * param.m_countEachPage, param.m_countEachPage, null, "genTime", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            ShcdCardsItem info = new ShcdCardsItem();
            info.m_time = Convert.ToDateTime(dataList[i]["genTime"]).ToLocalTime().ToString();
            info.m_cardId = Convert.ToInt64(dataList[i]["id"]);
            info.m_cardType = Convert.ToInt32(dataList[i]["cardType"]);
            info.m_cardValue = Convert.ToInt32(dataList[i]["cardValue"]);
            info.m_sysIncome=Convert.ToInt64(dataList[i]["sysIncome"]);
            info.m_sysOutlay=Convert.ToInt64(dataList[i]["sysOutlay"]);
            info.m_roomId = Convert.ToInt32(dataList[i]["roomId"]);

            m_result.Add(info);
        }
        return OpRes.opres_success;
    }
}
//黑红下注玩家列表
public class QueryShcdCardsPlayerList : QueryBase 
{
    private List<InfoShcd> m_result = new List<InfoShcd>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;

        long cardId = Convert.ToInt64(p.m_param);

        QueryCondition queryCond = new QueryCondition();
        IMongoQuery imq1 = Query.EQ("cardBoardId", cardId);
        queryCond.addImq(imq1);
        IMongoQuery imq2 = Query.EQ("gameId", 10);
        queryCond.addImq(imq2);

        IMongoQuery imq3 = Query.EQ("roomId", Convert.ToInt32(p.m_op));
        queryCond.addImq(imq3);

        IMongoQuery imq = queryCond.getImq();
        return query(user, imq);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_PLAYER_MONEY,
            user.getDbServerID(), DbName.DB_PUMP, imq, 0, 0, null, "playerOutlay", false);
        if (dataList == null || dataList.Count == 0)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];
            InfoShcd info=null;

            if (data.ContainsKey("exInfo"))
            {
                info = BaseJsonSerializer.deserialize<InfoShcd>(Convert.ToString(data["exInfo"]));
            }
            if (info != null)
            {
                info.m_playerId = Convert.ToInt32(data["playerId"]);
                m_result.Add(info);
            }
        }

        return OpRes.opres_success;
    }
}

//黑红牌局详情
public class ShcdCardsDetailItem
{
    public long m_cardId;
    public int m_cardType;
    public Dictionary<int, long> m_area0 = new Dictionary<int, long>(); //黑桃
    public Dictionary<int, long> m_area1 = new Dictionary<int, long>();//红
    public Dictionary<int, long> m_area2 = new Dictionary<int, long>();//梅
    public Dictionary<int, long> m_area3 = new Dictionary<int, long>();//方
    public Dictionary<int, long> m_area4 = new Dictionary<int, long>();//大小王
}

public class QueryShcdCardsResultDetail : QueryBase
{
    private List<ShcdCardsDetailItem> m_result = new List<ShcdCardsDetailItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();

        ParamQuery p = (ParamQuery)param;
        long cardId = Convert.ToInt64(p.m_param);
        return query(user, cardId);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user, long cardId)
    {
        string[] m_fields = { "id","cardType","cardValue", "area0", "area1", "area2", "area3","area4" };

        Dictionary<string, object> data = DBMgr.getInstance().getTableData(TableName.SHCD_CARD_BOARD, "id", cardId, m_fields,
            user.getDbServerID(), DbName.DB_PUMP);

        if (data != null)
        {
            ShcdCardsDetailItem info = new ShcdCardsDetailItem();

            info.m_cardId = Convert.ToInt64(data["id"]);
            info.m_cardType = Convert.ToInt32(data["cardType"]);
            if (data.ContainsKey("area0"))
            {
                Dictionary<string, object> m_area0 = (Dictionary<String, object>)data["area0"];
                info.m_area0.Add(0,Convert.ToInt64(m_area0["robotOutLay"]));
                info.m_area0.Add(1,Convert.ToInt64(m_area0["playerOutLay"]));
            }

            if (data.ContainsKey("area1"))
            {
                Dictionary<string, object> m_area1 = (Dictionary<String, object>)data["area1"];
                info.m_area1.Add(0, Convert.ToInt64(m_area1["robotOutLay"]));
                info.m_area1.Add(1, Convert.ToInt64(m_area1["playerOutLay"]));
            }

            if (data.ContainsKey("area2"))
            {
                Dictionary<string, object> m_area2 = (Dictionary<String, object>)data["area2"];
                info.m_area2.Add(0, Convert.ToInt64(m_area2["robotOutLay"]));
                info.m_area2.Add(1, Convert.ToInt64(m_area2["playerOutLay"]));
            }

            if (data.ContainsKey("area3"))
            {
                Dictionary<string, object> m_area3 = (Dictionary<String, object>)data["area3"];
                info.m_area3.Add(0, Convert.ToInt64(m_area3["robotOutLay"]));
                info.m_area3.Add(1, Convert.ToInt64(m_area3["playerOutLay"]));
            }
            if (data.ContainsKey("area4"))
            {
                Dictionary<string, object> m_area4 = (Dictionary<String, object>)data["area4"];
                info.m_area4.Add(0, Convert.ToInt64(m_area4["robotOutLay"]));
                info.m_area4.Add(1, Convert.ToInt64(m_area4["playerOutLay"]));
            }
            m_result.Add(info);
        }
        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
// 黑红梅方参数查询
public class ResultShcdParam : ResultExpRateParam
{
    public int m_roomId;
    public int m_level;
    public static string[] s_levelName = { "自动控制", "天堂", "普通", "困难", "超难", "最难", };
    public int m_jokerCount;
    public string m_cheatSE;
    // 玩家个数
    public int m_curPlayerCount = 0;

    public string getLevelName()
    {
        return s_levelName[m_level];
    }
}
public class QueryShcdParam : QueryBase
{
    private List<ResultShcdParam> m_result = new List<ResultShcdParam>();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.Clear();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.SHCDCARDS_ROOM,
            dip, null, 0, 0, null, "room_id", true);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultShcdParam info = new ResultShcdParam();
            m_result.Add(info);

            info.m_roomId = Convert.ToInt32(dataList[i]["room_id"]);
            if (dataList[i].ContainsKey("ExpectEarnRate"))
            {
                info.m_expRate = Convert.ToDouble(dataList[i]["ExpectEarnRate"]);
            }
            else
            {
                info.m_expRate = 0.02;
            }

            if (dataList[i].ContainsKey("ExpectEarnMaxRate"))
            {
                info.m_expMaxRate = Convert.ToDouble(dataList[i]["ExpectEarnMaxRate"]);
            }
            else
            {
                info.m_expMaxRate = 0.10;
            }

            if (dataList[i].ContainsKey("room_income")) // 总收入
            {
                info.m_totalIncome = Convert.ToInt64(dataList[i]["room_income"]);
            }
            if (dataList[i].ContainsKey("room_outcome"))  // 总支出
            {
                info.m_totalOutlay = Convert.ToInt64(dataList[i]["room_outcome"]);
            }

            if (dataList[i].ContainsKey("EarnRateControl"))
            {
                info.m_level = Convert.ToInt32(dataList[i]["EarnRateControl"]);
            }
            else
            {
                info.m_level = 0;
            }

//             if (dataList[i].ContainsKey("next_joker_count"))
//             {
//                 info.m_jokerCount = Convert.ToInt32(dataList[i]["next_joker_count"]);
//             }
//             else
//             {
//                 info.m_jokerCount = 0;
//             }

            //if (dataList[i].ContainsKey("beginCheatIndex") && dataList[i].ContainsKey("endCheatIndex"))
            //{
            //    info.m_cheatSE = Convert.ToString(dataList[i]["beginCheatIndex"]) + "-" +
            //        Convert.ToString(dataList[i]["endCheatIndex"]);
            //}
            if (dataList[i].ContainsKey("cheatStr"))
            {
                info.m_cheatSE = Convert.ToString(dataList[i]["cheatStr"]);
            }
            else
            {
                info.m_cheatSE = "1 100";
            }

            if (dataList[i].ContainsKey("player_count"))
            {
                info.m_curPlayerCount = Convert.ToInt32(dataList[i]["player_count"]);
            }
        }

        return OpRes.opres_success;
    }
}

public class ResultIndependentShcd
{
    Dictionary<int, ResultIndependent> m_roomData = new Dictionary<int, ResultIndependent>();

    public string getAreaName(int areaId)
    {
        return StrName.s_shcdArea[areaId - 1];
    }

    public ResultIndependent addRoom(int roomId)
    {
        if (m_roomData.ContainsKey(roomId))
            return m_roomData[roomId];

        ResultIndependent d = new ResultIndependent();
        m_roomData.Add(roomId, d);
        return d;
    }

    public ResultIndependent getRoomData(int roomId)
    {
        if (m_roomData.ContainsKey(roomId))
            return m_roomData[roomId];

        return null;
    }

    public void reset()
    {
        m_roomData.Clear();
    }
}

// 黑红梅方独立数据--各区域的下注，获奖情况
public class QueryIndependentShcd : QueryBase
{
    private ResultIndependentShcd m_result = new ResultIndependentShcd();

    public override OpRes doQuery(object param, GMUser user)
    {
        m_result.reset();
        return query(user);
    }

    public override object getQueryResult()
    {
        return m_result;
    }

    private OpRes query(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.SHCDCARDS_ROOM, dip);

        for (int i = 0; i < dataList.Count; i++)
        {
            for (int k = 1; k <= StrName.s_shcdArea.Length; k++)
            {
                // 总收入
                string totalWinKey = string.Format("WinGold{0}", k);
                // 总支出
                string totalLoseGoldKey = string.Format("LoseGold{0}", k);
                // 押注次数
                string betCountKey = string.Format("BetCount{0}", k);

                long income = 0, outlay = 0, betCount = 0;
                if (dataList[i].ContainsKey(totalWinKey))
                {
                    income = Convert.ToInt64(dataList[i][totalWinKey]);
                }
                if (dataList[i].ContainsKey(totalLoseGoldKey))
                {
                    outlay = Convert.ToInt64(dataList[i][totalLoseGoldKey]);
                }
                if (dataList[i].ContainsKey(betCountKey))
                {
                    betCount = Convert.ToInt64(dataList[i][betCountKey]);
                }

                int roomId = Convert.ToInt32(dataList[i]["room_id"]);
                m_result.addRoom(roomId).addBetCount(k, betCount, 0, income, outlay);
            }
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ParamGameCalfRoping
{
    // 查询套牛的数据参数
    public const int QUERY_CONTROL_PARAM = 0;
    // 独立数据
    public const int QUERY_INDEPENDENT = 1;
    // 关卡数据
    public const int QUERY_LEVEL = 2;
    // 套中的牛的种类
    public const int QUERY_CALF = 3;

    // 查询内容 
    public int m_queryContent;
}

// 套牛独立数据
public class ResultIndependentCalfRoping
{
    // 进入房间次数
    public long m_enterCount;
    // 鼓励奖获得次数
    public long m_norRewardNum;
    // 鼓励奖奖金
    public long m_norRewardGold;
    // 大奖获得次数
    public long m_bigRewardNum;
    // 大奖发放奖金
    public long m_bigRewardGold;
    // 续关次数
    public long m_buyLifeNum;

    public void reset()
    {
        m_enterCount = 0;
        m_norRewardNum = 0;
        m_norRewardGold = 0;
        m_bigRewardNum = 0;
        m_bigRewardGold = 0;
        m_buyLifeNum = 0;
    }
}

// 套牛牛的分类统计
public class ResultCalfRopingLogItem
{
    // 关卡ID
    public int m_passId;
    // 牛的ID
    public int m_calfId;
    public long m_hitCount;

    public string getName()
    {
        CalfRoping_CalfCFGData val = CalfRoping_CalfCFG.getInstance().getValue(m_calfId);
        if (val != null)
        {
            return val.m_calfName;
        }
        return m_calfId.ToString();
    }
}

public class ResultCalfRopingLevelItem
{
    // 关卡ID
    public int m_passId;
    // 丢绳次数
    public long allcount;
    // 套中次数
    public long m_hitCount;

    // 返回命中率
    public string getHitRate()
    {
        return ItemHelp.getRate(m_hitCount, allcount);
    }
}

// 游戏套牛的相关查询
public class QueryGameCalfRoping : QueryBase
{
    // 套牛盈利率等参数
    private List<ResultExpRateParam> m_result = new List<ResultExpRateParam>();

    ResultIndependentCalfRoping m_result1 = new ResultIndependentCalfRoping();

    private List<ResultCalfRopingLogItem> m_result2 = new List<ResultCalfRopingLogItem>();

    private List<ResultCalfRopingLevelItem> m_result3 = new List<ResultCalfRopingLevelItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        OpRes res = OpRes.op_res_failed;
        ParamGameCalfRoping p = (ParamGameCalfRoping)param;
        switch (p.m_queryContent)
        {
            case ParamGameCalfRoping.QUERY_CONTROL_PARAM:
                {
                    m_result.Clear();
                    res = queryParam(user);
                }
                break;
            case ParamGameCalfRoping.QUERY_INDEPENDENT:
                {
                    m_result1.reset();
                    res = queryIndependent(user);
                }
                break;
            case ParamGameCalfRoping.QUERY_CALF:
                {
                    m_result2.Clear();
                    res = queryCalfStat(user);
                    m_result2.Sort(sortLevel);
                }
                break;
            case ParamGameCalfRoping.QUERY_LEVEL:
                {
                    m_result3.Clear();
                    res = queryLevel(user);
                    m_result3.Sort(sortLevel);
                }
                break;
        }

        return res;
    }

    public override object getQueryResult(object param, GMUser user)
    {
        ParamGameCalfRoping p = (ParamGameCalfRoping)param;
        switch (p.m_queryContent)
        {
            case ParamGameCalfRoping.QUERY_CONTROL_PARAM:
                {
                    return m_result;
                }
                break;
            case ParamGameCalfRoping.QUERY_INDEPENDENT:
                {
                    return m_result1;
                }
                break;
            case ParamGameCalfRoping.QUERY_CALF:
                {
                    return m_result2;
                }
                break;
            case ParamGameCalfRoping.QUERY_LEVEL:
                {
                    return m_result3;
                }
                break;
        }
        return null;
    }

    private OpRes queryParam(GMUser user)
    {
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.CALF_ROPING_ROOM,
            user.getDbServerID(), DbName.DB_GAME);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultExpRateParam info = new ResultExpRateParam();
            m_result.Add(info);

            if (dataList[i].ContainsKey("ExpectEarnRate"))
            {
                info.m_expRate = Convert.ToDouble(dataList[i]["ExpectEarnRate"]);
            }
            else
            {
                info.m_expRate = 0.05;
            }

            if (dataList[i].ContainsKey("lobby_income")) // 总收入
            {
                info.m_totalIncome = Convert.ToInt64(dataList[i]["lobby_income"]);
            }
            if (dataList[i].ContainsKey("lobby_outcome"))  // 总支出
            {
                info.m_totalOutlay = Convert.ToInt64(dataList[i]["lobby_outcome"]);
            }
        }

        return OpRes.opres_success;
    }

    private OpRes queryIndependent(GMUser user)
    {
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.CALF_ROPING_ROOM,
            user.getDbServerID(), DbName.DB_GAME);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            if (data.ContainsKey("enter_count"))
            {
                m_result1.m_enterCount = Convert.ToInt64(data["enter_count"]);
            }
            if (data.ContainsKey("NorRewardNum"))
            {
                m_result1.m_norRewardNum = Convert.ToInt64(data["NorRewardNum"]);
            }
            if (data.ContainsKey("NorRewardGold"))
            {
                m_result1.m_norRewardGold = Convert.ToInt64(data["NorRewardGold"]);
            }
            if (data.ContainsKey("BigRewardNum"))
            {
                m_result1.m_bigRewardNum = Convert.ToInt64(data["BigRewardNum"]);
            }
            if (data.ContainsKey("BigRewardGold"))
            {
                m_result1.m_bigRewardGold = Convert.ToInt64(data["BigRewardGold"]);
            }
            if (data.ContainsKey("BuyLifeNum"))
            {
                m_result1.m_buyLifeNum = Convert.ToInt64(data["BuyLifeNum"]);
            }
        }

        return OpRes.opres_success;
    }

    // 套牛牛的分类统计
    private OpRes queryCalfStat(GMUser user)
    {
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.CALF_ROPING_LOG,
            user.getDbServerID(), DbName.DB_PUMP);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultCalfRopingLogItem item = new ResultCalfRopingLogItem();
            m_result2.Add(item);

            Dictionary<string, object> data = dataList[i];

            if (data.ContainsKey("passid"))
            {
                item.m_passId = Convert.ToInt32(data["passid"]);
            }
            if (data.ContainsKey("calfid"))
            {
                item.m_calfId = Convert.ToInt32(data["calfid"]);
            }
            if (data.ContainsKey("hitcount"))
            {
                item.m_hitCount = Convert.ToInt64(data["hitcount"]);
            }
        }

        return OpRes.opres_success;
    }

    private int sortLevel(ResultCalfRopingLogItem p1, ResultCalfRopingLogItem p2)
    {
        return p1.m_passId - p2.m_passId;
    }

    private int sortLevel(ResultCalfRopingLevelItem p1, ResultCalfRopingLevelItem p2)
    {
        return p1.m_passId - p2.m_passId;
    }

    private OpRes queryLevel(GMUser user)
    {
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.CALF_ROPING_PASS_LOG,
            user.getDbServerID(), DbName.DB_PUMP);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultCalfRopingLevelItem item = new ResultCalfRopingLevelItem();
            m_result3.Add(item);

            Dictionary<string, object> data = dataList[i];

            if (data.ContainsKey("passid"))
            {
                item.m_passId = Convert.ToInt32(data["passid"]);
            }
            if (data.ContainsKey("hitcount"))
            {
                item.m_hitCount = Convert.ToInt64(data["hitcount"]);
            }
            if (data.ContainsKey("allcount"))
            {
                item.allcount = Convert.ToInt64(data["allcount"]);
            }
        }

        return OpRes.opres_success;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ParamGrandPrix
{
    public const int QUERY_WEEK_CHAMPION = 1;
    public const int QUERY_MATCH_DAY = 2;

    public int m_queryType;
    public object m_param;
}

public class ParamMatchDay
{
    public string m_matchDay;
    public string m_playerId;
    public bool m_isTop100;
}

public class ResultChampionItem
{
    public string m_time = "";
    public int m_playerId;
    public string m_nickName;
    public int m_bestScore;
}

public class ResultMatchDayItem : ResultChampionItem
{
    public int m_rank;
}

// 大奖赛相关查询
public class QueryGrandPrix : QueryBase
{
    private List<ResultChampionItem> m_result1 = new List<ResultChampionItem>();

    private List<ResultMatchDayItem> m_result2 = new List<ResultMatchDayItem>();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamGrandPrix p = (ParamGrandPrix)param;
        OpRes res = OpRes.op_res_failed;
        switch (p.m_queryType)
        {
            case ParamGrandPrix.QUERY_WEEK_CHAMPION:
                {
                    m_result1.Clear();
                    res = queryWeekChampion(user);
                }
                break;
            case ParamGrandPrix.QUERY_MATCH_DAY:
                {
                    m_result2.Clear();
                    ParamMatchDay pd = (ParamMatchDay)p.m_param;
                    if (pd.m_isTop100)
                    {
                        res = queryMatchDayTop100(user, pd);
                        m_result2.Sort(sortRank);
                    }
                    else
                    {
                        res = queryMatchDayForSpecialPlayer(user, pd);
                    }
                }
                break;
        }

        return res;
    }

    public override object getQueryResult(object param, GMUser user)
    {
        ParamGrandPrix p = (ParamGrandPrix)param;
        switch (p.m_queryType)
        {
            case ParamGrandPrix.QUERY_WEEK_CHAMPION:
                {
                    return m_result1;
                }
                break;
            case ParamGrandPrix.QUERY_MATCH_DAY:
                {
                    return m_result2;
                }
                break;
        }
        return null;
    }

    private OpRes queryWeekChampion(GMUser user)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.MATCH_GRAND_PRIX_WEEK_CHAMPION,
            dip, null, 0, 0, null, "matchTime", false);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultChampionItem info = new ResultChampionItem();
            info.m_time = Convert.ToDateTime(dataList[i]["matchTime"]).ToLocalTime().ToShortDateString();
            info.m_playerId = Convert.ToInt32(dataList[i]["playerId"]);
            info.m_nickName = Convert.ToString(dataList[i]["nickName"]);
            info.m_bestScore = Convert.ToInt32(dataList[i]["bestScore"]);
            m_result1.Add(info);
        }

        return OpRes.opres_success;
    }

    private OpRes queryMatchDayForSpecialPlayer(GMUser user, ParamMatchDay p)
    {
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_matchDay, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        int playerId = 0;
        if (!int.TryParse(p.m_playerId, out playerId))
            return OpRes.op_res_param_not_valid;

        IMongoQuery imq1 = Query.LT("matchTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("matchTime", BsonValue.Create(mint));
        IMongoQuery imq3 = Query.EQ("playerId", BsonValue.Create(playerId));
        var cond = Query.And(imq1, imq2, imq3);

        return queryMatchDay(user, cond);
    }

    private OpRes queryMatchDayTop100(GMUser user, ParamMatchDay p)
    {
        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.m_matchDay, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq = Query.EQ("matchTime", BsonValue.Create(mint));

        return queryMatchDay(user, imq, 100);
    }

    private OpRes queryMatchDay(GMUser user, IMongoQuery cond, int maxCount = 0)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_GAME,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.MATCH_GRAND_PRIX_DAY,
            dip, cond, 0, maxCount);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            ResultMatchDayItem info = new ResultMatchDayItem();
            info.m_time = Convert.ToDateTime(dataList[i]["matchTime"]).ToLocalTime().ToShortDateString();
            info.m_playerId = Convert.ToInt32(dataList[i]["playerId"]);
            info.m_nickName = Convert.ToString(dataList[i]["nickName"]);
            info.m_bestScore = Convert.ToInt32(dataList[i]["bestScore"]);
            info.m_rank = Convert.ToInt32(dataList[i]["rank"]);
            m_result2.Add(info);
        }

        return OpRes.opres_success;
    }

    int sortRank(ResultMatchDayItem left, ResultMatchDayItem right)
    {
        return left.m_rank - right.m_rank;
    }
}

//////////////////////////////////////////////////////////////////////////
public class ParamFishBoss
{
    public int m_roomId;
    public string time;
    public int m_bossId = 0;
}

public class ResultFishBoss
{
    public string m_date;
    public int m_roomId;
    public int m_bossCount;

    public long m_totalgold;//BOSS金币收入
    public int m_totalReleaseChip;//放出碎片

    public int m_bossItemLock;//使用锁定
    public int m_bossItemViolent;//使用狂暴

    public int m_hitBossPersonTime;//攻击人次
    public int m_globalHitPerson;//攻击人数

    public int m_totalReleaseGold;//Boss放出金币

    public long m_consumeGold;
    public int m_bossDieCount;

    public int m_bossId;

    public int m_chipToGold;//碎片转化金币

    public long getBossZheKouTotal()
    {
        return m_totalReleaseGold + m_chipToGold;
    }

    // 返回每boss盈利
    public string getEarnEachBoss()
    {
        return ItemHelp.getRate(m_consumeGold - getBossZheKouTotal(), m_bossDieCount);
    }

    public string getBossName()
    {
        var data = FishCFG.getInstance().getValue(m_bossId);
        if (data != null)
        {
            return data.m_fishName;
        }

        return m_bossId.ToString();
    }

    public string getRoomName() 
    {
        string roomName = "";

        if(StrName.s_roomList.ContainsKey(m_roomId))
            roomName = StrName.s_roomList[m_roomId];

        return roomName;
    }
}

// 捕鱼BOSS
public class QueryFishBoss : QueryBase
{
    static int[] s_boss = { 21, 701, 702, 703, 704, 705, 706};
    private List<ResultFishBoss> m_result = new List<ResultFishBoss>();

    public override OpRes doQuery(object param, GMUser user)
    {
        ParamFishBoss p = (ParamFishBoss)param;
        m_result.Clear();

        DateTime mint = DateTime.Now, maxt = DateTime.Now;
        bool res = Tool.splitTimeStr(p.time, ref mint, ref maxt);
        if (!res)
            return OpRes.op_res_time_format_error;

        IMongoQuery imq1 = Query.LT("genTime", BsonValue.Create(maxt));
        IMongoQuery imq2 = Query.GTE("genTime", BsonValue.Create(mint));

        var imq = Query.And(imq1, imq2);

        //全部
        if (p.m_roomId > 0)
        {
            IMongoQuery imq3 = Query.EQ("roomid", BsonValue.Create(p.m_roomId));
            imq = Query.And(imq, imq3);
        }

        //全部
        IMongoQuery imq4 = null;
        if (p.m_bossId > 0)
        {
            imq4 = Query.EQ("bossId", BsonValue.Create(p.m_bossId));
        }
        else
        {
            imq4 = Query.Or(
                Query.EQ("bossId", 21),
                Query.EQ("bossId", 701),
                Query.EQ("bossId", 702),
                Query.EQ("bossId", 703),
                Query.EQ("bossId", 704),
                Query.EQ("bossId", 705),
                Query.EQ("bossId", 706)
                );
        }
        imq = Query.And(imq, imq4);

        return query(user, imq);
    }

    public override object getQueryResult(object param, GMUser user)
    {
        return m_result;
    }

    private OpRes query(GMUser user, IMongoQuery imq)
    {
        DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(),DbName.DB_PUMP,DbInfoParam.SERVER_TYPE_SLAVE);
        List<Dictionary<string, object>> dataList = DBMgr.getInstance().executeQuery(TableName.PUMP_BOSSINFO, dip, imq, 0, 0, null, "genTime", false);
        if (dataList == null)
            return OpRes.op_res_not_found_data;

        for (int i = 0; i < dataList.Count; i++)
        {
            Dictionary<string, object> data = dataList[i];

            ResultFishBoss info = new ResultFishBoss();
            info.m_date = Convert.ToDateTime(data["genTime"]).ToLocalTime().ToShortDateString();

            info.m_roomId = Convert.ToInt32(data["roomid"]);

            if (data.ContainsKey("boss_count")) //BOSS出现次数
                info.m_bossCount = Convert.ToInt32(data["boss_count"]);

            if (data.ContainsKey("income_gold"))//BOSS金币收入
                info.m_totalgold = Convert.ToInt64(data["income_gold"]);

            if (data.ContainsKey("outlay_chip"))//BOSS放出碎片
                info.m_totalReleaseChip = Convert.ToInt32(data["outlay_chip"]);

            if (data.ContainsKey("bossItemLock"))//锁定
                info.m_bossItemLock = Convert.ToInt32(data["bossItemLock"]);

            if (data.ContainsKey("bossItemViolent"))//狂暴
                info.m_bossItemViolent = Convert.ToInt32(data["bossItemViolent"]);

            if (data.ContainsKey("hitBossPersonTime"))//击打人次
                info.m_hitBossPersonTime = Convert.ToInt32(data["hitBossPersonTime"]);

            //if (data.ContainsKey("gloBalHitPerson")) //击打人数
            //    info.m_globalHitPerson = Convert.ToInt32(data["gloBalHitPerson"]);

            if (data.ContainsKey("totalReleaseGold")) //BOSS放出金币
                info.m_totalReleaseGold = Convert.ToInt32(data["totalReleaseGold"]);

            if (data.ContainsKey("income_gold"))
                info.m_consumeGold = Convert.ToInt64(data["income_gold"]);

            if (data.ContainsKey("bossDieCount"))
                info.m_bossDieCount = Convert.ToInt32(data["bossDieCount"]);
           
            if (data.ContainsKey("bossId"))
                info.m_bossId = Convert.ToInt32(data["bossId"]);

            if (data.ContainsKey("chipToGold"))
                info.m_chipToGold = Convert.ToInt32(data["chipToGold"]);

            m_result.Add(info);
        }

        return OpRes.opres_success;
    }

    bool isBoss(int bossId)
    {
        for (int i = 0; i < s_boss.Length; i++)
        {
            if (s_boss[i] == bossId)
                return true;
        }

        return false;
    }
}








