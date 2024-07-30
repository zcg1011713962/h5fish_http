using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

// 捕鱼盈利率调整
public class TableStatFishlordControl
{
    private static string[] s_head = new string[] { "选择" ,"房间","系统总收入", "系统总支出", "盈亏情况",
        "实际盈利率", "当前人数", "废弹","码量起始控制值","最大盈利率","控制盈利率大","期望盈利率","控制盈利率小","最小盈利率",
        "盈利率奖励支出","盈利率惩罚收入","充值奖励","新用户奖励",""};
    private string[] m_content = new string[s_head.Length];

    private static string[] s_head1 = new string[] { "房间", "BOSS收入", "BOSS支出", "池子收入", "池子支出", "实际池子盈利率" };

    public TableStatFishlordControl()
    {
    }

    public OpRes onModifyExpRate(GMUser user, string expRate, string roomList, DyOpType dyType)
    {
        ParamFishlordNew p = new ParamFishlordNew();
        p.m_isReset = false;
        p.m_expRate = expRate;
        p.m_roomList = roomList;
        p.m_rightId = RightDef.FISH_PARAM_CONTROL;
        OpRes res = user.doDyop(p, dyType/*DyOpType.opTypeFishlordParamAdjust*/);
       // genExpRateTable(m_expRateTable, user);

        return res;
    }

    public OpRes onReset(GMUser user, string roomList, DyOpType dyType)
    {
        ParamFishlordParamAdjust p = new ParamFishlordParamAdjust();
        p.m_isReset = true;
        p.m_roomList = roomList;
        p.m_rightId = RightDef.FISH_PARAM_CONTROL;
        OpRes res = user.doDyop(p, dyType/*DyOpType.opTypeFishlordParamAdjust*/);
       // genExpRateTable(m_expRateTable, user);
        return res;
    }

    // 期望盈利率表格
    public void genExpRateTable(Table table, GMUser user, QueryType qType)
    {
        table.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        table.Rows.Add(tr);

        for (int i=0; i < s_head.Length; i++)
        {
            TableCell td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        long totalIncome = 0;
        long totalOutlay = 0;
        long totalAbandonedbullets = 0;
        long totalHitLucky = 0;
        long totalHitUnlucky = 0;

        long totalChargeOutlay = 0;
        long totalNewPlayerOutlay = 0;

        OpRes res = user.doQuery(null, qType/*QueryType.queryTypeFishlordParam*/);
        Dictionary<int, ResultFishlordNewExpRate> qresult
            = (Dictionary<int, ResultFishlordNewExpRate>)user.getQueryResult(qType/*QueryType.queryTypeFishlordParam*/);

        foreach(var room in StrName.s_roomList)
        {
            int f = 0;
            m_content[f++] = Tool.getCheckBoxHtml("roomList", room.Key.ToString(), false);   //选择
            m_content[f++] = room.Value;    //房间
            if (qresult.ContainsKey(room.Key))
            {
                ResultFishlordNewExpRate r = new ResultFishlordNewExpRate();
                r = qresult[room.Key];

                m_content[f++] = r.m_totalIncome.ToString();
                m_content[f++] = r.m_totalOutlay.ToString();
                m_content[f++] = (r.m_totalIncome - r.m_totalOutlay).ToString();
                m_content[f++] = r.getFactExpRate();
                m_content[f++] = r.m_curPlayerCount.ToString();
                m_content[f++] = r.m_abandonedbullets.ToString();
                m_content[f++] = r.m_startEarnValue.ToString();

                m_content[f++] = r.m_maxEarnValue.ToString();
                m_content[f++] = r.m_maxControlEarnValue.ToString();
                m_content[f++] = r.m_expEarn.ToString();
                m_content[f++] = r.m_minControlEarnValue.ToString();
                m_content[f++] = r.m_minEarnValue.ToString();

                m_content[f++] = r.m_totalHitLucky.ToString();
                m_content[f++] = r.m_totalHitUnlucky.ToString();
                m_content[f++] = r.m_totalChargeOutlay.ToString();
                m_content[f++] = r.m_totalNewPlayerOutlay.ToString();

                totalIncome += r.m_totalIncome;
                totalOutlay += r.m_totalOutlay;
                totalAbandonedbullets += r.m_abandonedbullets;
                totalHitLucky += r.m_totalHitLucky;
                totalHitUnlucky += r.m_totalHitUnlucky;
                totalChargeOutlay += r.m_totalChargeOutlay;
                totalNewPlayerOutlay += r.m_totalNewPlayerOutlay;
            }
            else 
            {
                m_content[f++] = m_content[f++] = m_content[f++] = m_content[f++] = m_content[f++] = "0";
                m_content[f++] = m_content[f++] = m_content[f++] = m_content[f++] = m_content[f++] = "0";
                m_content[f++] = m_content[f++] = m_content[f++] = m_content[f++] = m_content[f++] = m_content[f++] = "0";
            }

            tr = new TableRow();
            table.Rows.Add(tr);
            for (int j = 0; j < s_head.Length; j++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];

                if (j == 4) 
                {
                    setColor(td, m_content[j]);
                }
                else if (j == 18) 
                {
                    HtmlGenericControl alink = new HtmlGenericControl();
                    alink.TagName = "a";
                    alink.InnerText = "新算法参数修改";
                    alink.Attributes.Add("class", "btn btn-primary");
                    alink.Attributes.Add("roomId", room.Key.ToString());
                    td.Controls.Add(alink);
                }
            }
        }
       
        //总计
        addStatFoot(table, totalIncome, totalOutlay, totalAbandonedbullets, totalHitLucky,totalHitUnlucky,totalChargeOutlay,totalNewPlayerOutlay);
    }

    public void getBossTable(Table table, GMUser user, QueryType qType, string midT, string highT)
    {
        table.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        table.Rows.Add(tr);

        int i = 0;
        for (; i < s_head1.Length; i++)
        {
            TableCell td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head1[i];
        }

        ParamFishlordBoss param = new ParamFishlordBoss();
        param.m_midTime = midT;
        param.m_highTime = highT;
        OpRes res = user.doQuery(param, qType);
        List<ResultFishlordExpRate> qresult
            = (List<ResultFishlordExpRate>)user.getQueryResult(param, qType/*QueryType.queryTypeFishlordParam*/);

        for (i = 0; i < qresult.Count; i++)
        {
            ResultFishlordExpRate item = qresult[i];
            m_content[0] = StrName.s_roomName[item.m_roomId - 1];
            m_content[1] = item.m_robotIncome.ToString();
            m_content[2] = item.m_robotOutlay.ToString();

            m_content[3] = item.m_totalIncome.ToString();
            m_content[4] = item.m_totalOutlay.ToString();
            m_content[5] = item.getFactExpRate();

            tr = new TableRow();
            table.Rows.Add(tr);
            for (int j = 0; j < s_head1.Length; j++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
    }

    // 增加统计页脚
    protected void addStatFoot(Table table, long totalIncome, long totalOutlay, long totalAbandonedbullets, 
        long totalHitLucky, long totalHitUnlucky, long totalChargeOutlay, long totalNewPlayerOutlay)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        int f=0;
        m_content[f++]="";
        m_content[f++] = "总计";
        //m_content[1] = "";   //期望盈利率
        // 总收入
        m_content[f++] = totalIncome.ToString();
        // 总支出
        m_content[f++] = totalOutlay.ToString();
        // 总盈亏
        m_content[f++] = (totalIncome - totalOutlay).ToString();
        m_content[f++] = "";
        m_content[f++] = "";
        m_content[f++] = totalAbandonedbullets.ToString();
        m_content[f++] = m_content[f++] = m_content[f++] = m_content[f++] = m_content[f++] = m_content[f++] = "";
        m_content[f++] = totalHitLucky.ToString();
        m_content[f++] = totalHitUnlucky.ToString();
        m_content[f++] = totalChargeOutlay.ToString();
        m_content[f++] = totalNewPlayerOutlay.ToString();
        m_content[f++] = "";

        for (int j = 0; j < s_head.Length; j++)
        {
            TableCell td = new TableCell();
            tr.Cells.Add(td);
            td.Text = m_content[j];

            if (j == 4)  //4   值为负数 红 否则 绿
            {
                setColor(td, m_content[j]);
            }
        }
    }

    protected void setColor(TableCell td, string num)
    {
        if (num[0] == '-')
        {
            td.ForeColor = Color.Red;
        }
        else
        {
            td.ForeColor = Color.Green;
        }
    }
}

// 大水池参数调整(新)
public class TableStatFishlordNewCtrl
{
    private static string[] s_head = new string[] { "选择","场次","大水池总收入","大水池总支出","充值奖励","新用户奖励","盈亏情况","玩家抽水率",
        "控制盈利率大","控制盈利率小","码量控制值",
        "当前人数","玩法支出","玩法收入","废弹","盈利率奖励支出","盈利率惩罚收入","",""};
    private string[] m_content = new string[s_head.Length];

    private static string[] s_head1 = new string[] { "房间", "BOSS收入", "BOSS支出", "池子收入", "池子支出", "实际池子盈利率" };

    public TableStatFishlordNewCtrl()
    {
    }

    // 期望盈利率表格
    public void genExpRateTable(Table table, GMUser user, QueryType qType)
    {
        table.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        table.Rows.Add(tr);

        int i = 0, f = 0;
        for (i = 0; i < s_head.Length; i++)
        {
            TableCell td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        long totalIncome = 0, totalOutlay = 0;
        long totalRechargePool = 0, totalNewPlayerPool = 0;
        int totalPlayerCount = 0;
        long totalTrickOutLay = 0, totalTrickIncome = 0;
        long totalRewardOutLay = 0, totalPunishIncome = 0, totalAbandonedbullets = 0;


        OpRes res = user.doQuery(null, qType);
        Dictionary<int, FishlordNewParamItem> qresult = (Dictionary<int, FishlordNewParamItem>)user.getQueryResult(qType);

        foreach (var room in StrName.s_roomList)
        {
            f = 0;

            if (qresult.ContainsKey(room.Key))
            {
                m_content[f++] = Tool.getCheckBoxHtml("roomList", room.Key.ToString(), false);   //选择
                m_content[f++] = room.Value;    //房间
                FishlordNewParamItem r = new FishlordNewParamItem();
                r = qresult[room.Key];

                m_content[f++] = r.m_totalIncome.ToString();
                m_content[f++] = r.m_totalOutlay.ToString();
                m_content[f++] = r.m_rechargePool.ToString();
                m_content[f++] = r.m_newPlayerPool.ToString();
                m_content[f++] = (r.m_totalIncome - r.m_totalOutlay).ToString();
                m_content[f++] = r.getRate(r.m_totalIncome, r.m_totalOutlay);

                m_content[f++] = r.m_earnRatemCtrMax.ToString();
                m_content[f++] = r.m_earnRatemCtrMin.ToString();
                m_content[f++] = r.m_incomeThreshold.ToString();

                m_content[f++] = r.m_playerCount.ToString();
                m_content[f++] = r.m_trickOutLay.ToString();
                m_content[f++] = r.m_trickIncome.ToString();
                m_content[f++] = r.m_abandonedbullets.ToString();

                m_content[f++] = r.m_rewardOutlay.ToString();
                m_content[f++] = r.m_punishIncome.ToString();
                m_content[f++] = "";
                m_content[f++] = "";

                totalIncome += r.m_totalIncome;
                totalOutlay += r.m_totalOutlay;

                totalRechargePool += r.m_rechargePool;
                totalNewPlayerPool += r.m_newPlayerPool;

                totalPlayerCount += r.m_playerCount;
                totalTrickOutLay += r.m_trickOutLay;
                totalTrickIncome += r.m_trickIncome;
                totalRewardOutLay += r.m_rewardOutlay;
                totalPunishIncome += r.m_punishIncome;
                totalAbandonedbullets += r.m_abandonedbullets;

                tr = new TableRow();
                table.Rows.Add(tr);
                for (int j = 0; j < s_head.Length; j++)
                {
                    TableCell td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];

                    if (j == 6 || j == 7)
                    {
                        setColor(td, m_content[j]);
                    }
                    else if (j == 17)
                    {
                        HtmlGenericControl alink = new HtmlGenericControl();
                        alink.TagName = "a";
                        alink.InnerText = "新算法参数修改";
                        alink.Attributes.Add("class", "btn btn-primary btn-edit");
                        alink.Attributes.Add("roomId", room.Key.ToString());
                        alink.Attributes.Add("jsckpotGrandPump", r.m_jsckpotGrandPump.ToString());
                        alink.Attributes.Add("jsckpotSmallPump", r.m_jsckpotSmallPump.ToString());
                        alink.Attributes.Add("normalFishRoomPoolPumpParam", r.m_normalFishRoomPoolPumpParam.ToString());
                        alink.Attributes.Add("rateCtr",r.m_baseRate.ToString());
                        alink.Attributes.Add("checkRate",r.m_checkRate.ToString());
                        alink.Attributes.Add("trickDeviationFix", r.m_trickDeviationFix.ToString());
                        alink.Attributes.Add("legendaryFishRate", r.m_legendaryFishRate.ToString());

                        alink.Attributes.Add("mythicalScoreTurnRate", r.m_mythicalScoreTurnRate.ToString());
                        alink.Attributes.Add("mythicalFishRate", r.m_mythicalFishRate.ToString());
                        td.Controls.Add(alink);
                    }
                    else if (j == 18)
                    {
                        HtmlGenericControl alink = new HtmlGenericControl();
                        alink.TagName = "a";
                        alink.InnerText = "详情";
                        alink.Attributes.Add("href", DefCC.ASPX_STAT_FISHLORD_CTRL_NEW_DETAIL + "?roomId=" + room.Key.ToString());
                        alink.Attributes.Add("class", "btn btn-primary btn-detail");
                        alink.Attributes.Add("target", "_blank");
                        td.Controls.Add(alink);
                    }
                }
            }
            else
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                for (int j = 0; j < s_head.Length; j++)
                {
                    TableCell td = new TableCell();
                    tr.Cells.Add(td);
                    if (j == 0){
                        td.Text = Tool.getCheckBoxHtml("roomList", room.Key.ToString(), false);   //选择  
                    }
                    else if (j == 1) {
                        td.Text = room.Value; //房间
                    }
                    else
                    {
                        td.Text = "";
                    }
                }
            }
        }

        //总计
        addStatFoot(table, totalIncome, totalOutlay, totalRechargePool, totalNewPlayerPool,
            totalPlayerCount, totalTrickOutLay, totalTrickIncome, totalRewardOutLay, totalPunishIncome, totalAbandonedbullets);
    }

    // 增加统计页脚
    protected void addStatFoot(Table table, long totalIncome, long totalOutlay, long totalRechargePool, long totalNewPlayerPool,
        int totalPlayerCount, long totalTrickOutLay, long totalTrickIncome, long totalRewardOutLay, long totalPunishIncome,
        long totalAbandonedbullets)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        int f = 0;
        m_content[f++] = "";
        m_content[f++] = "总计";
        // 总收入
        m_content[f++] = totalIncome.ToString();
        // 总支出
        m_content[f++] = totalOutlay.ToString();

        m_content[f++] = totalRechargePool.ToString();
        m_content[f++] = totalNewPlayerPool.ToString();

        // 总盈亏
        m_content[f++] = (totalIncome - totalOutlay).ToString();
        m_content[f++] = "";

        m_content[f++] = "";
        m_content[f++] = "";
        m_content[f++] = "";

        m_content[f++] = totalPlayerCount.ToString();
        m_content[f++] = totalTrickOutLay.ToString();
        m_content[f++] = totalTrickIncome.ToString();
        m_content[f++] = totalAbandonedbullets.ToString();

        m_content[f++] = totalRewardOutLay.ToString();
        m_content[f++] = totalPunishIncome.ToString();
        m_content[f++] = "";
        m_content[f++] = "";

        for (int j = 0; j < s_head.Length; j++)
        {
            TableCell td = new TableCell();
            tr.Cells.Add(td);
            td.Text = m_content[j];

            if (j == 6)  //值为负数 红 否则 绿
            {
                setColor(td, m_content[j]);
            }
        }
    }

    //重置
    public OpRes onReset(GMUser user, string roomList, DyOpType dyType)
    {
        ParamFishlordParamAdjust p = new ParamFishlordParamAdjust();
        p.m_isReset = true;
        p.m_roomList = roomList;
        p.m_rightId = RightDef.FISH_PARAM_CONTROL;
        OpRes res = user.doDyop(p, dyType/*DyOpType.opTypeFishlordParamAdjust*/);
        // genExpRateTable(m_expRateTable, user);
        return res;
    }

    protected void setColor(TableCell td, string num)
    {
        if (num[0] == '-')
        {
            td.ForeColor = Color.Red;
        }
        else
        {
            td.ForeColor = Color.Green;
        }
    }
}


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public class TableStatFishlordRoomNewCtrl
{
    //private static string[] s_head = new string[] { "时间","鱼ID","实际系统收入","实际系统支出","实际玩家收益率","兑换次数","盈亏情况"};
    private static string[] s_head = new string[] { "日期","玩法收入","玩法支出","废弹", "每日大水池收入", "每日大水池支出", "盈亏情况", "实际盈利率", "充值奖励", "新用户奖励" };
    private string[] m_content = new string[s_head.Length];

    public TableStatFishlordRoomNewCtrl()
    {
    }

    public void genExpRateTable(Table table, GMUser user, QueryType qType, ParamQuery param)
    {
        table.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        table.Rows.Add(tr);

        int i = 0, f = 0;
        for (i = 0; i < s_head.Length; i++)
        {
            TableCell td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        long totalIncome = 0, totalOutlay = 0, totalRechargeReward = 0, totalNewReward = 0;

        OpRes res = user.doQuery(param, qType);
        List<FishlordRoomItemNew> qresult = (List<FishlordRoomItemNew>)user.getQueryResult(qType);

        for (i = 0; i < qresult.Count; i++)
        {
            f = 0;
            FishlordRoomItemNew da = qresult[i];

            m_content[f++] = da.m_time;

            m_content[f++] = da.m_dailyTrickIncome.ToString();
            m_content[f++] = da.m_dailyTrickOutlay.ToString();
            m_content[f++] = da.m_abandonedbullets.ToString();

            m_content[f++] = da.m_dailyIncome.ToString();
            m_content[f++] = da.m_dailyOutlay.ToString();
            m_content[f++] = (da.m_dailyIncome - da.m_dailyOutlay).ToString();
            m_content[f++] = da.getRate(da.m_dailyIncome, da.m_dailyOutlay);
            m_content[f++] = da.m_rechargeReward.ToString();
            m_content[f++] = da.m_newReward.ToString();

            totalIncome += da.m_dailyIncome;
            totalOutlay += da.m_dailyOutlay;
            totalRechargeReward += da.m_rechargeReward;
            totalNewReward += da.m_newReward;

            tr = new TableRow();
            table.Rows.Add(tr);
            for (int j = 0; j < s_head.Length; j++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];

                if (j == 6 || j == 7)
                    setColor(td, m_content[j]);
            }
        }
    }

    protected void setColor(TableCell td, string num)
    {
        if (num[0] == '-')
        {
            td.ForeColor = Color.Red;
        }
        else
        {
            td.ForeColor = Color.Green;
        }
    }
}

//个人后台管理
public class TableStatFishlordNewSingleCtrl 
{
     private static string[] s_head = new string[] { "炮倍","玩家总收入","玩家总支出","实际盈利率","当前人数","无价值用户数",
         "盈利率奖励支出","盈利率惩罚收入",""};
    private string[] m_content = new string[s_head.Length];

    public TableStatFishlordNewSingleCtrl()
    {
    }

    // 期望盈利率表格
    public void genExpRateTable(Table table, GMUser user, QueryType qType)
    {
        table.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        table.Rows.Add(tr);

        int i = 0;
        for (i = 0; i < s_head.Length; i++)
        {
            TableCell td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        OpRes res = user.doQuery(null, qType);
        List<FishlordNewSingleItem> qresult = (List<FishlordNewSingleItem>)user.getQueryResult(qType);

        for (i = 0; i < qresult.Count; i++)
        {
            tr = new TableRow();
            table.Rows.Add(tr);

            TableCell td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "1-100";

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_100TurretPlayerIncome.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_100TurretPlayerOutlay.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].getRate(qresult[i].m_100TurretPlayerIncome, qresult[i].m_100TurretPlayerOutlay);
            setColor(td, td.Text);

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_100TurretPlayerCount.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_100TurretNoValueCount.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_100TurretRewardOutlay.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_100TurretPunishIncome.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "";
            td.RowSpan = 4;
            td.Attributes.CssStyle.Value = "vertical-align:middle";
            HtmlGenericControl alink = new HtmlGenericControl();
            alink.TagName = "a";
            alink.InnerText = "新算法参数修改";
            alink.Attributes.Add("class", "btn btn-primary btn-edit");
            alink.Attributes.Add("baseRate", qresult[i].m_baseRate.ToString());
            alink.Attributes.Add("deviationFix", qresult[i].m_deviationFix.ToString());
            alink.Attributes.Add("noValuePlayerRate", qresult[i].m_noValuePlayerRate.ToString());
            td.Controls.Add(alink);

            ///////////
            tr = new TableRow();
            table.Rows.Add(tr);

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "100-1000";
            td.RowSpan = 1;

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_1000TurretPlayerIncome.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_1000TurretPlayerOutlay.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].getRate(qresult[i].m_1000TurretPlayerIncome, qresult[i].m_1000TurretPlayerOutlay);
            setColor(td, td.Text);

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_1000TurretPlayerCount.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_1000TurretNoValueCount.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_1000TurretRewardOutlay.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_1000TurretPunishIncome.ToString();

            ///////////
            tr = new TableRow();
            table.Rows.Add(tr);

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "1000-5000";

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_5000TurretPlayerIncome.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_5000TurretPlayerOutlay.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].getRate(qresult[i].m_5000TurretPlayerIncome, qresult[i].m_5000TurretPlayerOutlay);
            setColor(td, td.Text);

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_5000TurretPlayerCount.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_5000TurretNoValueCount.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_5000TurretRewardOutlay.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_5000TurretPunishIncome.ToString();

            ///////////
            tr = new TableRow();
            table.Rows.Add(tr);

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "5000-10000";

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_10000TurretPlayerIncome.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_10000TurretPlayerOutlay.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].getRate(qresult[i].m_10000TurretPlayerIncome, qresult[i].m_10000TurretPlayerOutlay);
            setColor(td, td.Text);

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_10000TurretPlayerCount.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_10000TurretNoValueCount.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_10000TurretRewardOutlay.ToString();

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult[i].m_10000TurretPunishIncome.ToString();
        }
    }

    protected void setColor(TableCell td, string num)
    {
        if (num[0] == '-')
        {
            td.ForeColor = Color.Red;
        }
        else
        {
            td.ForeColor = Color.Green;
        }
    }
}

//爆金比赛场参数调整
public class TableStatFishlordBaojinControl 
{
    private static string[] s_head = new string[] {"房间","系统总收入（门票）","系统总支出（档位奖励）","盈亏情况","实际盈利率","当前人数"};
    private string [] m_content=new string[s_head.Length];
    public TableStatFishlordBaojinControl()
    {
    }
    // 期望盈利率表格
    public void genExpRateTable(Table table, GMUser user, QueryType qType)
    {
        table.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        table.Rows.Add(tr);

        int i = 0;
        for (; i < s_head.Length; i++)
        {
            TableCell td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }
        OpRes res = user.doQuery(null, qType/*QueryType.queryTypeFishlordBaojinParam*/);
        List<FishlordBaojinParamItem> qresult = (List<FishlordBaojinParamItem>)user.getQueryResult(qType);
        string[] strName = new string[] {"富豪竞技场"};
        for (i = 0; i < strName.Length; i++)
        {
            int f = 0;
            //m_content[f++] = Tool.getCheckBoxHtml("roomList", (qresult[i].m_roomId).ToString(), false);   //选择
            m_content[f++] = strName[i];    //房间
            FishlordBaojinParamItem r = qresult[i];
            m_content[f++] = r.m_sysTotalIncome.ToString();
            m_content[f++] = r.m_sysTotalOutlay.ToString();
            m_content[f++] = (r.m_sysTotalIncome - r.m_sysTotalOutlay).ToString();
            m_content[f++] = r.m_expRate.ToString();
            m_content[f++] = r.m_currentPersonNum.ToString();
            //m_content[f++] = r.m_pumpRate.ToString();

            tr = new TableRow();
            table.Rows.Add(tr);
            for (int j = 0; j < s_head.Length; j++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        };
    }
}

//竞技场得分修改
public class TableStatFishlordBaojinScoreControl
{
    private static string[] s_head = new string[] { "玩家ID", "昵称", "日最高积分", "周最高积分", "是否机器人"};
    private string[] m_content = new string[s_head.Length];
    public TableStatFishlordBaojinScoreControl()
    {
    }
    // 竞技场玩家得分信息
    public void genExpRateTable(Table table, ParamQuery param, GMUser user, QueryType qType)
    {
        table.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;

        OpRes res = user.doQuery(param, qType/*QueryType.queryTypeFishlordBaojinScoreParam*/);
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }
        //表头
        for (int i = 0; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        List<FishlordBaojinScoreParamItem> qresult = (List<FishlordBaojinScoreParamItem>)user.getQueryResult(qType);
        for (int i = 0; i < qresult.Count; i++)
        {
            int f = 0;
            tr = new TableRow();
            table.Rows.Add(tr);
            FishlordBaojinScoreParamItem r = qresult[i];
            m_content[f++] = r.m_playerId.ToString();
            m_content[f++] = r.m_nickName;
            m_content[f++] = r.m_todayMaxScore.ToString();
            m_content[f++] = r.m_weekMaxScore.ToString();
            m_content[f++] = r.m_isRobot ? "是" : "否";

            for (int j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
                if(j==2)
                {
                    td.Attributes.Add("id", "todayMaxScore");
                }
            }
        }
    }
}

//魔石数量修改
public class TableStatDragonScaleControl 
{
    private static string[] s_head = new string[] { "玩家ID", "昵称", "周最高魔石"};
    private string[] m_content = new string[s_head.Length];
    public TableStatDragonScaleControl()
    {
    }
    // 玩家魔石数量信息
    public void genExpRateTable(Table table, ParamQuery param, GMUser user, QueryType qType)
    {
        table.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;

        OpRes res = user.doQuery(param, qType);
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }
        //表头
        for (int i = 0; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        List<FishlordBaojinScoreParamItem> qresult = (List<FishlordBaojinScoreParamItem>)user.getQueryResult(qType);
        for (int i = 0; i < qresult.Count; i++)
        {
            int f = 0;
            tr = new TableRow();
            table.Rows.Add(tr);
            FishlordBaojinScoreParamItem r = qresult[i];
            m_content[f++] = r.m_playerId.ToString();
            m_content[f++] = r.m_nickName;
            m_content[f++] = r.m_weekMaxScore.ToString();

            for (int j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
                if(j==2)
                {
                    td.Attributes.Add("id", "weekDragonScale");
                }
            }
        }
    }
}

//中秋国庆活动修改月饼
public class TableStatJinQiuNationalDayActCtrl 
{
    private static string[] s_head = new string[] { "玩家ID","昵称","月饼量","获取时间"};
    private string[] m_content = new string[s_head.Length];
    public TableStatJinQiuNationalDayActCtrl() 
    {
    }
    //玩家月饼数量信息
    public void genExpRateTable(Table table, ParamQuery param, GMUser user, QueryType qType)
    {
        table.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;

        OpRes res = user.doQuery(param, qType);
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }
        //表头
        for (int i = 0; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        List<FishlordBaojinScoreParamItem> qresult = (List<FishlordBaojinScoreParamItem>)user.getQueryResult(qType);
        for (int i = 0; i < qresult.Count; i++)
        {
            int f = 0;
            tr = new TableRow();
            table.Rows.Add(tr);
            FishlordBaojinScoreParamItem r = qresult[i];
            m_content[f++] = r.m_playerId.ToString();
            m_content[f++] = r.m_nickName;
            m_content[f++] = r.m_weekMaxScore.ToString();
            m_content[f++] = r.m_time;

            for (int j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
                if (j == 2)
                {
                    td.Attributes.Add("id", "weekDragonScale");
                }
            }
        }
    }
}

//围剿龙王
public class TableStatWjlwDefRechargeReward 
{
    private static string[] s_head = new string[] { "昵称", "奖项", "操作" };
    private string[] m_content = new string[s_head.Length];

    public TableStatWjlwDefRechargeReward() 
    {
    }
    public void genTable(Table table, GMUser user)
    {
        table.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;

        OpRes res = user.doQuery(null, QueryType.queryTypeWjlwRechargeReward);
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }
        //表头
        int i = 0, f = 0;
        for (i = 0; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        List<WjlwRechargeRewardItem> qresult
            = (List<WjlwRechargeRewardItem>)user.getQueryResult(QueryType.queryTypeWjlwRechargeReward);
        for (i = 0; i < qresult.Count; i++)
        {
            f = 0;
            WjlwRechargeRewardItem item = qresult[i];
            m_content[f++] = item.m_nickname;
            m_content[f++] = item.getRewardName();
            m_content[f++] = "";

            tr = new TableRow();
            table.Rows.Add(tr);
            for (int j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
                td.Attributes.CssStyle.Value = "vertical-align:middle";
                if (j == 2)
                {
                    HtmlGenericControl alink = new HtmlGenericControl();
                    alink.TagName = "a";
                    alink.InnerText = "移除";
                    alink.Attributes.Add("class", "btn btn-primary btn_remove");
                    alink.Attributes.Add("_id", item.m_id);
                    td.Controls.Add(alink);
                }
            }
        }
    }
}

//爆金场数据
public class TableStatFishlordBaojinData 
{
    private static string[] s_head_1 = new string[] { "日期", "比赛参与总次数","比赛门票收入","玩家胜率","比赛参与人数","比赛活跃人数", "比赛档位奖励总计","比赛1次人数", "比赛3次人数",
            "比赛5次人数", "比赛10次人数", "比赛20次人数" ,"比赛平均时间","比赛1档爆机次数","比赛2档爆机次数","比赛3档爆机次数","比赛4档爆机次数","比赛5档爆机次数"};
    private string[] m_content_1 = new string[s_head_1.Length];

    private static string[] s_head_4 = new string[] { "日期", "排名", "昵称", "玩家ID", "日最高积分", "VIP等级", "最高炮台倍率" };
    private string[] m_content_4 = new string[s_head_4.Length];

    private static string[] s_head_5 = new string[] { "日期", "排名", "昵称", "玩家ID", "该玩家累计充值", "冠军累计获得次数", "前10名累计获得次数" };
    private string[] m_content_5 = new string[s_head_5.Length];

    public PageFishBaojin m_gen = new PageFishBaojin(1);

    private string m_page = "";
    private string m_foot = "";
    private string m_callURL;

    public TableStatFishlordBaojinData(string callURL)
    {
        m_callURL = callURL;
    }

    public string getPage() { return m_page; }
    public string getFoot() { return m_foot; }

    public string onQuery(GMUser user,PageFishBaojin m_gen,ParamQuery param,Table table,QueryType qType)
    {
        param.m_curPage = m_gen.curPage;
        param.m_countEachPage = m_gen.rowEachPage;

        OpRes res = user.doQuery(param, qType);

        switch(Convert.ToInt32(param.m_op))
        {
            case 1: return genTable1(table, res, user, param, m_gen);
            case 4: return genTable4(s_head_4, m_content_4, table, res, user, param, m_gen);
            case 5: return genTable4(s_head_5, m_content_5, table, res, user, param, m_gen);
        }
        return "";
    }

    //爆金比赛场
    public String genTable1(Table table, OpRes res, GMUser user, ParamQuery query_param,PageFishBaojin m_gen)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;

        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return "";
        }

        List<FishlordBaojinStatItem> qresult = (List<FishlordBaojinStatItem>)user.getQueryResult(QueryType.queryTypeFishlordBaojinStat);

        int i = 0, j = 0;
        // 表头
        for (i = 0; i < s_head_1.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Attributes.CssStyle.Value = "min-width:140px";
            td.Text = s_head_1[i];
        }

        for (i = 0; i < qresult.Count; i++)
        {
            int f = 0;
            tr = new TableRow();
            table.Rows.Add(tr);

            FishlordBaojinStatItem item = qresult[i];

            m_content_1[f++] = item.m_time;
            m_content_1[f++] = item.m_joinCount.ToString();
            m_content_1[f++] = String.Format("{0:N0}", item.m_ticketIncome);
            m_content_1[f++] = item.m_winRate.ToString();
            m_content_1[f++] = item.m_personCount.ToString();
            m_content_1[f++] = item.m_activeCount.ToString();

            m_content_1[f++] = item.m_giveOutGold.ToString();
            m_content_1[f++] = item.m_matchPerson1.ToString();
            m_content_1[f++] = item.m_matchPerson2.ToString();
            m_content_1[f++] = item.m_matchPerson3.ToString();
            m_content_1[f++] = item.m_matchPerson4.ToString();
            m_content_1[f++] = item.m_matchPerson5.ToString();
            m_content_1[f++] = item.m_matchTime.ToString();
            m_content_1[f++] = item.m_baoji1.ToString();
            m_content_1[f++] = item.m_baoji2.ToString();
            m_content_1[f++] = item.m_baoji3.ToString();
            m_content_1[f++] = item.m_baoji4.ToString();
            m_content_1[f++] = item.m_baoji5.ToString();

            for (j = 0; j < s_head_1.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content_1[j];
            }
        }
        string page_html = "", foot_html = "";
        m_gen.genPage(query_param, @"/appaspx/stat/StatFishlordBaojinBasicData.aspx", ref page_html, ref foot_html, user);

        return page_html + "#" + foot_html;
        //m_page.InnerHtml = page_html;
        //m_foot.InnerHtml = foot_html;
    }

    //爆金排行榜 日\周（历史）
    public string genTable4(string[] s_head, string[] m_content, Table table, OpRes res, GMUser user, ParamQuery query_param, PageFishBaojin m_gen)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;

        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return "";
        }
        List<FishlordBaojinRankItem> qresult = (List<FishlordBaojinRankItem>)user.getQueryResult(QueryType.queryTypeFishlordBaojinRank);

        int i = 0, j = 0;
        // 表头
        for (i = 0; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        for (i = 0; i < qresult.Count; i++)
        {
            int f = 0;
            tr = new TableRow();
            table.Rows.Add(tr);

            FishlordBaojinRankItem item = qresult[i];
            m_content[f++] = item.m_time;
            m_content[f++] = item.m_rank.ToString();
            m_content[f++] = item.m_nickName;
            m_content[f++] = item.m_playerId.ToString();
            if (Convert.ToInt32(query_param.m_param) == 1) //周
            {
                m_content[f++] = item.m_totalRecharge.ToString();
                m_content[f++] = item.m_weekChampionCount.ToString();
                m_content[f++] = item.m_weekTop10Count.ToString();
            }
            else if (Convert.ToInt32(query_param.m_param) == 0)//日
            {
                m_content[f++] = item.m_maxScore.ToString();
                m_content[f++] = item.m_vipLevel.ToString();
                m_content[f++] = item.m_fishLevel.ToString();
            }

            for (j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
        string page_html = "", foot_html = "";
        m_gen.genPage(query_param, @"/appaspx/stat/StatFishlordBaojinBasicData.aspx", ref page_html, ref foot_html, user);
        //m_page.InnerHtml = page_html;
        //m_foot.InnerHtml = foot_html;
        return page_html + "#" + foot_html;
    }
}
//////////////////////////////////////////////////////////////////////////
// 捕鱼桌子盈利率查询
public class TableStatFishlordDeskEarningsRate
{
    private static string[] s_head = new string[] { "桌子ID", "系统总收入", "系统总支出", "盈亏情况", "实际盈利率", "废弹" };
    private string[] m_content = new string[s_head.Length];
    private string m_page = "";
    private string m_foot = "";
    private string m_callURL;

    public TableStatFishlordDeskEarningsRate(string callURL)
    {
        m_callURL = callURL;
    }

    public string getPage() { return m_page; }
    public string getFoot() { return m_foot; }

    public void onQueryDesk(GMUser user, 
                            PageGift gen, 
                            int roomId, 
                            int roomIndex,
                            Table table, 
                            QueryType qType)
    {
        ParamQueryGift param = new ParamQueryGift();
        param.m_curPage = gen.curPage;
        param.m_countEachPage = gen.rowEachPage;
        param.m_state = roomIndex;
        param.m_roomId = roomId;
        user.doQuery(param, qType/*QueryType.queryTypeFishlordDeskParam*/);
        genDeskTable(table, user, param, qType, gen);
    }

    // 桌子的盈利率表格
    protected void genDeskTable(Table table, GMUser user, ParamQueryGift param, QueryType qType, PageGift gen)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);

        int i = 0;
        for (; i < s_head.Length; i++)
        {
            TableCell td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        List<ResultFishlordExpRate> qresult
            = (List<ResultFishlordExpRate>)user.getQueryResult(qType/*QueryType.queryTypeFishlordDeskParam*/);

        bool alt = true;
        foreach (var info in qresult)
        {
            tr = new TableRow();
            table.Rows.Add(tr);

            if (alt)
            {
                tr.CssClass = "alt";
            }
            alt = !alt;

            m_content[0] = info.m_roomId.ToString(); //桌子ID
            m_content[1] = info.m_totalIncome.ToString();
            m_content[2] = info.m_totalOutlay.ToString();
            m_content[3] = info.getDelta().ToString();
            m_content[4] = info.getFactExpRate();
            m_content[5] = info.m_abandonedbullets.ToString();

            for (int j = 0; j < s_head.Length; j++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }

        string page_html = "", foot_html = "";
        gen.genPage(param, m_callURL/*@"/appaspx/stat/StatFishlordDeskEarningsRate.aspx"*/,
            ref page_html, ref foot_html, user);
        
        m_page = page_html;
        m_foot = foot_html;
    }
}

//////////////////////////////////////////////////////////////////////////
// 捕鱼算法阶段分析
public class TableStatFishlordStage
{
    private static string[] s_head = new string[] { "时间", "房间名称", "阶段", "收入", "支出", "盈利率" };
    private string[] m_content = new string[s_head.Length];

    private string m_page = "";
    private string m_foot = "";
    private string m_callURL;

    public TableStatFishlordStage(string callURL)
    {
        m_callURL = callURL;
    }

    public string getPage() { return m_page; }
    public string getFoot() { return m_foot; }

    public void onQuery(GMUser user, 
                           string time, 
                           int roomId, 
                           PageGift gen, 
                           Table table,
                           QueryType qType)
    {
        ParamQueryGift param = new ParamQueryGift();
        param.m_param = time;
        param.m_state = roomId;
        param.m_curPage = gen.curPage;
        param.m_countEachPage = gen.rowEachPage;

        OpRes res = user.doQuery(param, qType/*QueryType.queryTypeFishlordStage*/);
        genTable(table, res, param, user, qType, gen);
    }

    private void genTable(Table table, OpRes res, ParamQueryGift param, GMUser user, QueryType qType, PageGift gen)
    {
        table.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        List<FishlordStageItem> qresult = (List<FishlordStageItem>)user.getQueryResult(qType/*QueryType.queryTypeFishlordStage*/);
        int i = 0, j = 0;
        // 表头
        for (i = 0; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        for (i = 0; i < qresult.Count; i++)
        {
            tr = new TableRow();
            if ((i & 1) == 0)
            {
                tr.CssClass = "alt";
            }
            table.Rows.Add(tr);

            m_content[0] = qresult[i].m_time;
            m_content[1] = StrName.s_roomName[qresult[i].m_roomId - 1];
            m_content[2] = StrName.s_stageName[qresult[i].m_stage];
            m_content[3] = qresult[i].m_income.ToString();
            m_content[4] = qresult[i].m_outlay.ToString();
            m_content[5] = qresult[i].getFactExpRate();

            for (j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }

        string page_html = "", foot_html = "";
        gen.genPage(param, m_callURL, ref page_html, ref foot_html, user);
        
        m_page = page_html;
        m_foot = foot_html;
    }
}

//////////////////////////////////////////////////////////////////////////
// 鱼的统计
public class TableStatFish
{
    private static string[] s_head = new string[] {"日期", "房间", "鱼ID", "名称", "击中次数", "死亡次数", "命中率", 
        "支出", "收入", "盈利率"};//, "总计折合盈利率"};
    private string[] m_content = new string[s_head.Length];

    private string m_page = "";
    private string m_foot = "";
    private string m_callURL;

    public TableStatFish(string callURL)
    {
        m_callURL = callURL;
    }

    public string getPage() { return m_page; }
    public string getFoot() { return m_foot; }

    public void onClearFishTable(GMUser user, string tableName, Table table)
    {
        OpRes res = user.doDyop(tableName, DyOpType.opTypeClearFishTable);
        table.Rows.Clear();
    }

    public void onQuery(GMUser user, Table table, ParamQuery param, QueryType qtype, PageStatFish gen)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);

        int i = 0, f = 0;
        for (; i < s_head.Length; i++)
        {
            TableCell td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        OpRes res = user.doQuery(param, qtype/*QueryType.queryTypeFishStat*/);
        List<ResultFish> qresult = (List<ResultFish>)user.getQueryResult(qtype);

        foreach (var data in qresult)
        {
            f = 0;
            m_content[f++] = data.m_time;
            if (data.m_roomId > 0)
            {
                m_content[f++] = StrName.s_roomList[data.m_roomId];
            }
            else
            {
                m_content[f++] = "";
            }
            m_content[f++] = data.m_fishId.ToString();
            FishCFGData fishInfo = null;
            fishInfo = FishCFG.getInstance().getValue(data.m_fishId);
            if (fishInfo != null)
            {
                m_content[f++] = fishInfo.m_fishName;
            }
            else
            {
                m_content[f++] = "";
            }
            m_content[f++] = data.m_hitCount.ToString();
            m_content[f++] = data.m_dieCount.ToString();
            m_content[f++] = data.getHit_Die();
            m_content[f++] = ItemHelp.getNumByComma(data.m_outlay);
            m_content[f++] = ItemHelp.getNumByComma(data.m_income);
            m_content[f++] = data.getOutlay_Income();
            //m_content[f++] = data.getZheheEarn();

            tr = new TableRow();
            table.Rows.Add(tr);
            for (int j = 0; j < s_head.Length; j++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }

        string page_html = "", foot_html = "";
        gen.genPage(param, m_callURL, ref page_html, ref foot_html, user);

        m_page = page_html;
        m_foot = foot_html;

    }
}

//////////////////////////////////////////////////////////////////////////
// Td活跃数据
public class TableTdActivation
{
    private static string[] s_head = new string[] {"日期", "渠道", "注册", "设备激活", "活跃","设备活跃", "收入","进入渔场人数", "平均每个设备生成账号", 
        "付费人数", "付费次数","人均付费次数",
        "次日留存率", "3日留存率", "7日留存率" ,"30日留存率", "ARPU", "ARPPU", "付费率",
        "非新增首次付费次数","老用户充值人数","老用户充值占比","新增付费人数","新增用户付费", "新增ARPPU","新增用户付费率","次日付费率","3日付费率","7日付费率",
    "次日设备留存率"/*15*/, "3日设备留存率", "7日设备留存率" ,"30日设备留存率","付费次日留存","付费3日留存","付费7日留存"};
    private string[] m_content = new string[s_head.Length];

    private static string[] s_head_1 = new string[] { "日期", "渠道", "爱贝充值", "总充值"};
    private string[] m_content_1 = new string[s_head_1.Length];

    public void genTable(GMUser user, Table table, OpRes res)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0, f = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
            if(i==0||i==1)
            {
                td.Attributes.CssStyle.Value="min-width:120px";
            }
        }

        i = 0;
        List<ResultActivationItem> qresult = (List<ResultActivationItem>)user.getQueryResult(QueryType.queryTypeTdActivation);
        foreach (var data in qresult)
        {
            f = 0;
            m_content[f++] = data.m_genTime.ToShortDateString();
            TdChannelInfo info = TdChannel.getInstance().getValue(data.m_channel);
            if (info != null)
            {
                m_content[f++] = info.m_channelName;
            }
            else
            {
                m_content[f++] = data.m_channel;
            }

            m_content[f++] = data.m_regeditCount.ToString();
            m_content[f++] = data.m_deviceActivationCount.ToString();
            m_content[f++] = data.m_activeCount.ToString();
            m_content[f++] = data.m_deviceLoginCount.ToString();
            m_content[f++] = data.m_totalIncome.ToString();
            m_content[f++] = data.m_enterFishRoomCount.ToString(); // 进入渔场人数
            m_content[f++] = data.getAccNumberPerDev();
            m_content[f++] = data.m_rechargePersonNum.ToString(); //付费人数
            m_content[f++] = data.m_rechargeCount.ToString();
            m_content[f++] = getRate(data.m_rechargeCount, data.m_rechargePersonNum);

            m_content[f++] = data.get2DayRemain();
            m_content[f++] = data.get3DayRemain();
            m_content[f++] = data.get7DayRemain();
            m_content[f++] = data.get30DayRemain();
            m_content[f++] = data.getARPU();
            m_content[f++] = data.getARPPU();
            m_content[f++] = data.getRechargeRate(); //付费率

            m_content[f++] = data.m_oldFirstRechargePersonCount.ToString();// 非新增首次付费次数
            m_content[f++] = (data.m_rechargePersonNum - data.m_newAccRechargePersonNum).ToString();//老用户付费人数
            m_content[f++] = getRate(data.m_rechargePersonNum-data.m_newAccRechargePersonNum, data.m_rechargePersonNum);
            m_content[f++] = data.m_newAccRechargePersonNum.ToString();  //新增付费人数
            m_content[f++] = data.m_newAccIncome > -1 ? data.m_newAccIncome.ToString() : "";
            m_content[f++] = data.getNewARPPU(); //新增ARPUU
            m_content[f++] = data.getNewAccRechargeRate(); //新增用户付费率

            m_content[f++] = data.getRemainPercent(data.m_2DayRechargePersonNum, data.m_regeditCount); //次日付费率
            m_content[f++] = data.getRemainPercent(data.m_3DayRechargePersonNum, data.m_regeditCount);
            m_content[f++] = data.getRemainPercent(data.m_7DayRechargePersonNum, data.m_regeditCount);

            m_content[f++] = data.get2DayDevRemain();
            m_content[f++] = data.get3DayDevRemain();
            m_content[f++] = data.get7DayDevRemain();
            m_content[f++] = data.get30DayDevRemain();

            //付费次日留存率
            m_content[f++] = data.getRemainPercent(data.m_2DayRemainCountRecharge, data.m_newAccRechargePersonNum);
            m_content[f++] = data.getRemainPercent(data.m_3DayRemainCountRecharge, data.m_newAccRechargePersonNum);
            m_content[f++] = data.getRemainPercent(data.m_7DayRemainCountRecharge, data.m_newAccRechargePersonNum);

            tr = new TableRow();
            table.Rows.Add(tr);
            i++;

            for (int j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
    }
	
    public string getRate(int key1, int key2) {
        return Math.Round(key1 * 1.0 / key2, 2).ToString();
    }

    public void genTable1(GMUser user,Table table,OpRes res) 
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        for (int i = 0; i < s_head_1.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head_1[i];
        }

        List<rechargeByAiBeiItem> qresult = (List<rechargeByAiBeiItem>)user.getQueryResult(QueryType.queryTypeRechargeByAibei);
        int f = 0;
        foreach (var data in qresult)
        {
            f = 0;
            m_content_1[f++] = data.m_time;
            TdChannelInfo info = TdChannel.getInstance().getValue(data.m_channel);
            if (info != null)
            {
                m_content_1[f++] = info.m_channelName;
            }
            else
            {
                m_content_1[f++] = data.m_channel;
            }

            m_content_1[f++]=data.m_recharge.ToString();
            m_content_1[f++]=data.m_totalRecharge.ToString();

            tr = new TableRow();
            table.Rows.Add(tr);
            for (int j = 0; j < s_head_1.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content_1[j];
            }
        }
    }
}
//Td老用户活跃数据
public class TableTdActivationOfOldPlayer
{
    private static string[] s_head = new string[] {"日期", "渠道", "注册","活跃","老用户活跃","老用户充值金额",
        "老用户充值人数","老用户付费率","老用户ARPU","老用户充值占比"};
    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0, f = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
            if (i == 0 || i == 1)
            {
                td.Attributes.CssStyle.Value = "min-width:120px";
            }
        }

        i = 0;
        int oldActive = 0, oldRechargePersonNum = 0, oldIncome = 0;
        List<ResultActivationItem> qresult = (List<ResultActivationItem>)user.getQueryResult(QueryType.queryTypeTdActivation);
        foreach (var data in qresult)
        {
            f = 0;
            m_content[f++] = data.m_genTime.ToShortDateString();
            TdChannelInfo info = TdChannel.getInstance().getValue(data.m_channel);
            if (info != null)
            {
                m_content[f++] = info.m_channelName;
            }
            else
            {
                m_content[f++] = data.m_channel;
            }

            m_content[f++] = data.m_regeditCount.ToString();
            m_content[f++] = data.m_activeCount.ToString();

            oldActive = data.m_activeCount - data.m_regeditCount;
            m_content[f++] = oldActive.ToString();

            oldIncome = data.m_totalIncome - data.m_newAccIncome;
            m_content[f++] = oldIncome.ToString();

            oldRechargePersonNum = data.m_rechargePersonNum - data.m_newAccRechargePersonNum;
            m_content[f++] = oldRechargePersonNum.ToString();//老用户付费人数
            m_content[f++] = data.getRemainPercent(oldRechargePersonNum, oldActive); //老用户付费率
            m_content[f++] = data.getARPU(oldIncome, oldActive);//老用户ARPU
            m_content[f++] = getRate(oldIncome, data.m_totalIncome);

            tr = new TableRow();
            table.Rows.Add(tr);
            i++;

            for (int j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
    }

    public string getRate(int key1, int key2)
    {
        if (key2 == 0)
            return key1.ToString();
        return Math.Round(key1 * 1.0 / key2, 2).ToString();
    }
}
//////////////////////////////////////////////////////////////////////////
// 最高在线玩家个数
public class TableMaxOnline
{
    private static string[] s_head = new string[] { "日期", "最高在线时间点", "在线人数" };
    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        i = 0;
        List<ResultMaxOnlineItem> qresult = (List<ResultMaxOnlineItem>)user.getQueryResult(QueryType.queryTypeMaxOnline);
        foreach (var data in qresult)
        {
            m_content[0] = data.m_date;
            m_content[1] = data.m_timePoint;
            m_content[2] = data.m_playerNum.ToString();

            tr = new TableRow();
            table.Rows.Add(tr);
            if ((i & 1) == 0)
            {
                tr.CssClass = "alt";
            }
            i++;

            for (int j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 玩家金币总和
public class TablePlayerTotalMoney
{
    private static string[] s_head = new string[] { "日期", "总金币", "保险箱内总金币" };
    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        i = 0;
        List<ResultTotalPlayerMoneyItem> qresult = (List<ResultTotalPlayerMoneyItem>)user.getQueryResult(QueryType.queryTypeTotalPlayerMoney);
        foreach (var data in qresult)
        {
            m_content[0] = data.m_date;
            m_content[1] = string.Format("{0:N0}", data.m_money);
            if (data.m_safeBox > -1)
            {
                m_content[2] = string.Format("{0:N0}", data.m_safeBox);
            }
            else
            {
                m_content[2] = "";
            }

            tr = new TableRow();
            table.Rows.Add(tr);
//             if ((i & 1) == 0)
//             {
//                 tr.CssClass = "alt";
//             }
            i++;

            for (int j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 平均价值
public class TableTdLTVBase
{
    protected static string[] s_head = new string[] {"渠道", "日期", "注册", "1日价值", "3日价值", "7日价值", "14日价值", "30日价值", 
        "60日价值", "90日价值"};
    protected string[] m_content = new string[s_head.Length];

    public static TableTdLTVBase create(int type)
    {
        return new TableTdLTVChannel();
        //switch (type)
        //{
        //    case 0: //全部平均
        //    case 1: //安卓总体
        //        return new TableTdLTVFull();
        //        break;
        //    default:
        //        return new TableTdLTVChannel();
        //        break;
        //}
        //return null;
    }

    public virtual void genTable(GMUser user, Table table, OpRes res)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        fillData(user, table);
    }

    public virtual void fillData(GMUser user, Table table) { }
    public virtual OpRes query(GMUser user, ParamQuery param) { return OpRes.op_res_failed; }
}

public class TableTdLTVChannel : TableTdLTVBase
{
    public override void fillData(GMUser user, Table table)
    {
        TableRow tr = null;
        TableCell td = null;
        int i = 0;

        List<ResultLTVItem> qresult = (List<ResultLTVItem>)user.getQueryResult(QueryType.queryTypeLTV);
        foreach (var data in qresult)
        {
            TdChannelInfo info = TdChannel.getInstance().getValue(data.m_channel);
            if (info != null)
            {
                m_content[0] = info.m_channelName;
            }
            else
            {
                m_content[0] = data.m_channel;
            }

            m_content[1] = data.m_genTime;
            m_content[2] = data.m_regeditCount.ToString();
            m_content[3] = data.get1DayAveRecharge();
            m_content[4] = data.get3DayAveRecharge();
            m_content[5] = data.get7DayAveRecharge();
            m_content[6] = data.get14DayAveRecharge();
            m_content[7] = data.get30DayAveRecharge();
            m_content[8] = data.get60DayAveRecharge();
            m_content[9] = data.get90DayAveRecharge();
            
            tr = new TableRow();
            table.Rows.Add(tr);
//             if ((i & 1) == 0)
//             {
//                 tr.CssClass = "alt";
//             }
            i++;

            for (int j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
    }

    public override OpRes query(GMUser user, ParamQuery param) 
    {
        OpRes res = user.doQuery(param, QueryType.queryTypeLTV);
        return res;
    }
}

// 全体平均
public class TableTdLTVFull : TableTdLTVBase
{
    public override void fillData(GMUser user, Table table)
    {
        TableRow tr = null;
        TableCell td = null;
        int i = 0;

        List<ResultLTVItem> qresult = (List<ResultLTVItem>)user.getStatResult(StatType.statTypeLTV);
        foreach (var data in qresult)
        {
            m_content[0] = "";
            m_content[1] = data.m_genTime;
            m_content[2] = data.m_regeditCount.ToString();
            m_content[3] = data.get1DayAveRecharge();
            m_content[4] = data.get3DayAveRecharge();
            m_content[5] = data.get7DayAveRecharge();
            m_content[6] = data.get14DayAveRecharge();
            m_content[7] = data.get30DayAveRecharge();
            m_content[8] = data.get60DayAveRecharge();
            m_content[9] = data.get90DayAveRecharge();

            tr = new TableRow();
            table.Rows.Add(tr);
//             if ((i & 1) == 0)
//             {
//                 tr.CssClass = "alt";
//             }
            i++;

            for (int j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
    }

    public override OpRes query(GMUser user, ParamQuery param)
    {
        OpRes res = user.doStat(param, StatType.statTypeLTV);
        return res;
    }
}

//////////////////////////////////////////////////////////////////////////
// 捕鱼3种道具 锁定、急速、散射
public class TableFishConsumeItem
{
    private static string[] s_head = new string[] { "日期", "初级场（道具使用）", "中级场", "高级场", "碎片场", "巨鲨场","初级龙宫场","高级龙宫场","巨鲲场","圣兽场"};
    //冰冻、锁定、召唤、狂暴、普通鱼雷、青铜鱼雷、白银鱼雷、黄金鱼雷、钻石鱼雷
    private static string[] s_head2 = new string[]{"冰冻","锁定","召唤","狂暴","普通鱼雷","青铜鱼雷","白银鱼雷","黄金鱼雷","钻石鱼雷"};
    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res,string r)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0, j = 0;
        //表头 第一行
        for (i = 0 ; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
            if (i == 0)
            {
                td.RowSpan = 2;
                td.ColumnSpan = 1;
            }
            else 
            {
                td.RowSpan = 1;
                td.ColumnSpan = 9;
            }
            td.Attributes.CssStyle.Value = "vertical-align:middle";
        }
        //表头第二行
        tr = new TableRow();
        table.Rows.Add(tr);
        for (j = 0; j < s_head.Length - 1; j++) 
        {
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head2[i];
                td.ColumnSpan = 1;
            }
        }

        ResultConsumeItem qresult = (ResultConsumeItem)user.getQueryResult(null, QueryType.queryTypeFishConsume);
        var timeList = qresult.timeList;
        //冰冻、锁定、召唤、狂暴、普通鱼雷、青铜鱼雷、白银鱼雷、黄金鱼雷、钻石鱼雷
        int[] itemIds = new int[]{8,5,9,17,23,24,25,26,27};
        int[] roomIds = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
       
        foreach (var t in timeList)
        {
            tr = new TableRow();
            table.Rows.Add(tr);

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = t.ToShortDateString();

            foreach (int m in roomIds) //{ "初级场", "中级场", "高级场","碎片场","龙宫场"};
            {
                RooomItemConsume ric = qresult.getRooomItemConsume(t, m);
                if (ric == null)
                {
                    for (i = 0; i < s_head2.Length; i++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "";
                    }
                }
                else 
                {
                    foreach(int itemId in itemIds)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        if (ric.m_dic.ContainsKey(itemId))
                        {
                            if (ric.m_dic[itemId] != null)
                            {
                                td.Text = ric.m_dic[itemId].m_useCount.ToString();
                            }
                            else
                            {
                                td.Text = "";
                            }
                        }
                        else {
                            td.Text = "";
                        }
                    }
                }
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 大R流失
public class TableRLose
{
    private static string[] s_head = new string[] { "玩家id", "昵称", "VIP等级", "金币", "钻石", "龙珠", "最后登录时间" };

    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        i = 0;
        List<RLoseItem> qresult = (List<RLoseItem>)user.getQueryResult(QueryType.queryTypeRLose);
        for (; i < qresult.Count; i++)
        {
            tr = new TableRow();
            table.Rows.Add(tr);

            RLoseItem item = qresult[i];
            m_content[0] = item.m_playerId.ToString();
            m_content[1] = item.m_nickName;
            m_content[2] = item.m_vipLevel.ToString();
            m_content[3] = item.m_gold.ToString();
            m_content[4] = item.m_gem.ToString();
            m_content[5] = item.m_dragonBall.ToString();
            m_content[6] = item.m_lastLoginTime;

            for (int k = 0; k < s_head.Length; k++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[k];
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 充值用户统计
public class TableRechargePlayer
{
    //private static string[] s_head = new string[] { "玩家id", "渠道","充值次数", "充值金额", 
    //    "注册时间", "上线次数", "剩余金币","最后上线时间", "曾经最大金币",
    //    StrName.getGameName3((int)GameId.fishlord),StrName.getGameName3((int)GameId.shcd),
    //    StrName.getGameName3((int)GameId.cows), StrName.getGameName3((int)GameId.dragon),
    //    StrName.getGameName3((int)GameId.crocodile),StrName.getGameName3((int)GameId.baccarat),
    //    StrName.getGameName3((int)GameId.dice)};

    private static string[] s_head = new string[] { "玩家id","绑定手机号码", "渠道","充值次数", "充值金额", 
        "注册时间", "上线次数", "剩余金币","最后上线时间", "曾经最大金币",
        StrName.getGameName3((int)GameId.fishlord),
        //StrName.getGameName3((int)GameId.shcd),
        //StrName.getGameName3((int)GameId.cows),StrName.getGameName3((int)GameId.crocodile)
    };

    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0, f = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }
        
        i = 0;
        List<RechargePlayerItem> qresult = (List<RechargePlayerItem>)user.getStatResult(StatType.statTypeRechargePlayer);
        for (; i < qresult.Count; i++)
        {
            tr = new TableRow();
            table.Rows.Add(tr);

            f = 0;
            RechargePlayerItem item = qresult[i];
            m_content[f++] = item.m_playerId.ToString();
            m_content[f++] = item.m_bindPhone;
            m_content[f++] = item.getChannelName();
            m_content[f++] = item.m_rechargeCount.ToString();
            m_content[f++] = item.m_rechargeMoney.ToString();
            m_content[f++] = item.m_regTime.ToString(); // 注册时间
            m_content[f++] = item.m_loginCount.ToString();// 上线次数
            m_content[f++] = item.m_remainGold.ToString(); // 剩余金币
            m_content[f++] = item.m_lastLoginTime.ToString(); // 最后上线时间
            m_content[f++] = item.m_mostGold.ToString(); // 曾经最大金币

            m_content[f++] = item.getEnterCount((int)GameId.fishlord).ToString();
            //m_content[f++] = item.getEnterCount((int)GameId.shcd).ToString();
            //m_content[f++] = item.getEnterCount((int)GameId.cows).ToString();
            //m_content[f++] = item.getEnterCount((int)GameId.dragon).ToString();
            //m_content[f++] = item.getEnterCount((int)GameId.crocodile).ToString();
            //m_content[f++] = item.getEnterCount((int)GameId.baccarat).ToString();
            //m_content[f++] = item.getEnterCount((int)GameId.dice).ToString();

            for (int k = 0; k < s_head.Length; k++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[k];
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 玩家龙珠监控
public class TablePlayerDragonBall
{
    private static string[] s_head = new string[] { "玩家ID", "注册时间", 
       "打到龙珠",  "送出龙珠", "收取龙珠", "龙珠兑换","初始龙珠", "龙珠结余",
       "金币充值获得", "金币其余获得", "金币消耗","初始金币", "结余金币",
       "钻石充值获得", "钻石其余获得", "钻石消耗", "初始钻石", "结余钻石",
       "总充值", "时间段内充值"
        };

    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0, f = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        i = 0;
        List<StatPlayerDragonBallItem> qresult = (List<StatPlayerDragonBallItem>)user.getStatResult(StatType.statTypePlayerDragonBall);
        for (; i < qresult.Count; i++)
        {
            tr = new TableRow();
            table.Rows.Add(tr);

            f = 0;
            StatPlayerDragonBallItem item = qresult[i];
            m_content[f++] = item.m_playerId.ToString();
            m_content[f++] = item.m_regTime.ToString();
            m_content[f++] = item.m_dbgain.ToString();
            m_content[f++] = item.m_dbsend.ToString(); 
            m_content[f++] = item.m_dbaccept.ToString();
            m_content[f++] = item.m_dbexchange.ToString(); 
            m_content[f++] = item.m_dbStart.ToString(); 
            m_content[f++] = item.m_dbRemain.ToString();

            m_content[f++] = item.m_goldByRecharge.ToString();
            m_content[f++] = item.m_goldByOther.ToString();
            m_content[f++] = item.m_goldConsume.ToString();
            m_content[f++] = item.m_goldStart.ToString();
            m_content[f++] = item.m_goldRemain.ToString();

            m_content[f++] = item.m_gemByRecharge.ToString();
            m_content[f++] = item.m_gemByOther.ToString();
            m_content[f++] = item.m_gemConsume.ToString();
            m_content[f++] = item.m_gemStart.ToString();
            m_content[f++] = item.m_gemRemain.ToString();

            m_content[f++] = item.m_rechargeFromReg.ToString();
            m_content[f++] = item.m_todayRecharge.ToString();

            for (int k = 0; k < s_head.Length; k++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[k];
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 每日龙珠
public class TableDragonBallDaily
{
    private static string[] s_head = new string[] { "日期", "每日充值", 
       "龙珠产出",  "龙珠消耗", "当日消耗", "盈利折算RMB"};

    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0, f = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        i = 0;
        List<DragonBallDailyItem> qresult = (List<DragonBallDailyItem>)user.getQueryResult(QueryType.queryTypeDragonBallDaily);
        for (; i < qresult.Count; i++)
        {
            tr = new TableRow();
            table.Rows.Add(tr);

            f = 0;
            DragonBallDailyItem item = qresult[i];
            m_content[f++] = item.m_time.ToShortDateString();
            m_content[f++] = item.m_todayRecharge.ToString();
            m_content[f++] = item.m_dragonBallGen.ToString();
            m_content[f++] = item.m_dragonBallConsume.ToString();
            m_content[f++] = item.m_dragonBallRemain.ToString();
            m_content[f++] = item.m_rmb.ToString();

            for (int k = 0; k < s_head.Length; k++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[k];
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 充值玩家监控
public class TableRechargePlayerMonitor
{
    private static string[] s_head = new string[] { "玩家ID","渠道", "当前炮数", 
       "累计付费",  "累计游戏时间", 
       "初次付费日期", "初次付费游戏时间","初次付费项目","初次付费时拥有金币","初次付费炮数",
       "再次付费日期", "再次付费游戏时间","再次付费项目","再次付费时拥有金币","再次付费炮数",
       "注册时间", "龙珠结余", "龙珠累计获得", "龙珠累计转出"
        };

    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0, f = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        i = 0;
        List<RechargePlayerMonitorItem> qresult = (List<RechargePlayerMonitorItem>)user.getQueryResult(QueryType.queryTypeRechargePlayerMonitor);
        for (; i < qresult.Count; i++)
        {
            tr = new TableRow();
            table.Rows.Add(tr);

            f = 0;
            RechargePlayerMonitorItem item = qresult[i];
            m_content[f++] = item.m_playerId.ToString();

            TdChannelInfo info = TdChannel.getInstance().getValue(item.m_channel);
            if (info != null)
            {
                m_content[f++] = info.m_channelName;
            }
            else
            {
                m_content[f++] = item.m_channel;
            }

            m_content[f++] = item.getOpenRate(item.m_curFishLevel);
            m_content[f++] = item.m_totalRecharge.ToString();
            m_content[f++] = item.getGameTime(item.m_totalGameTime);

            m_content[f++] = item.m_firstRechargeTime.ToString();
            m_content[f++] = item.getGameTime(item.m_firstRechargeGameTime);
            m_content[f++] = item.getRechargePoint(item.m_firstRechargePoint);
            m_content[f++] = item.m_firstRechargeGold.ToString();
            m_content[f++] = item.getOpenRate(item.m_firstRechargeFishLevel);

            m_content[f++] = item.m_secondRechargeTime.Ticks == 0 ? "" : item.m_secondRechargeTime.ToString();
            m_content[f++] = item.getGameTime(item.m_secondRechargeGameTime);
            m_content[f++] = item.getRechargePoint(item.m_secondRechargePoint);
            m_content[f++] = item.m_secondRechargeGold.ToString();
            m_content[f++] = item.getOpenRate(item.m_secondRechargeFishLevel);

            m_content[f++] = item.m_regTime.ToString();
            m_content[f++] = item.m_remainDragon.ToString();
            m_content[f++] = item.m_gainDragon.ToString();
            m_content[f++] = item.m_sendDragon.ToString();

            for (int k = 0; k < s_head.Length; k++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[k];
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 玩家收支统计
public class TablePlayerIncomeExpenses
{
    private static string[] s_head = new string[] { "日期", "活跃人数", "",
       "初始值",  "免费获得总计", "免费获得人均", "充值获得总计","充值获得人均", "消耗总计","消耗人均",
       "结余总计","结余人均", "数据库结余"
        };
    private static string[] s_head1 = { "金币", "钻石", "龙珠", "话费券" };

    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res, ParamIncomeExpenses param)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        i = 0;
        List<StatIncomeExpensesItem> qresult = (List<StatIncomeExpensesItem>)user.getStatResult(StatType.statTypePlayerIncomeExpenses);
        for (; i < qresult.Count; i++)
        {
            StatIncomeExpensesItem item = qresult[i];
            genDataCol(item, table, param);
        }
    }

    private void genDataCol(StatIncomeExpensesItem item, Table table, ParamIncomeExpenses param)
    {
        int curRow = table.Rows.Count;
        int rowCount = s_head1.Length;
        if (param.m_property > -1)
        {
            rowCount = 1;
        }

        // 每块数据生成足够行
        for (int i = 0; i < rowCount; i++)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
        }

        // 日期列
        TableCell td = new TableCell();
        table.Rows[curRow].Cells.Add(td);
        td.RowSpan = rowCount;
        td.Text = item.m_genTime.ToShortDateString();

        td = new TableCell();
        table.Rows[curRow].Cells.Add(td);
        td.RowSpan = rowCount;
        td.Text = item.m_playerCount.ToString();

        if (rowCount > 1)
        {
            for (int i = 0; i < s_head1.Length; i++)
            {
                fillRow(item, i, i, table.Rows[curRow + i]);
            }
        }
        else
        {
            fillRow(item, 0, param.m_property, table.Rows[curRow]);
        }
    }

    private void fillRow(StatIncomeExpensesItem item, int curRow, int property, TableRow tr)
    {
        switch (property)
        {
            case 0:  // 金币
                {
                    genCell(tr, "金币");
                    genCell(tr, item.m_goldStart.ToString());       // 初始值
                    genCell(tr, item.m_goldFreeGain.ToString());    // 免费获得总计
                    genCell(tr, ItemHelp.getRate(item.m_goldFreeGain, item.m_playerCount, 1));    // 免费获得人均

                    genCell(tr, item.m_goldRechargeGain.ToString()); // 充值获得总计
                    genCell(tr, ItemHelp.getRate(item.m_goldRechargeGain, item.m_playerCount, 1)); // 充值获得人均

                    genCell(tr, item.m_goldConsume.ToString()); // 消耗总计
                    genCell(tr, ItemHelp.getRate(item.m_goldConsume, item.m_playerCount, 1)); // 消耗人均

                    genCell(tr, item.m_goldRemain.ToString()); // 结余总计
                    genCell(tr, ItemHelp.getRate(item.m_goldRemain, item.m_playerCount, 1)); // 结余人均

                    genCell(tr, item.getDataBaseRemain(item.m_dataBaseGoldRemain));
                }
                break;
            case 1:  // 钻石
                {
                    genCell(tr, "钻石");
                    genCell(tr, item.m_gemStart.ToString());       // 初始值
                    genCell(tr, item.m_gemFreeGain.ToString());    // 免费获得总计
                    genCell(tr, ItemHelp.getRate(item.m_gemFreeGain, item.m_playerCount, 1));    // 免费获得人均

                    genCell(tr, item.m_gemRechargeGain.ToString());
                    genCell(tr, ItemHelp.getRate(item.m_gemRechargeGain, item.m_playerCount, 1));

                    genCell(tr, item.m_gemConsume.ToString());
                    genCell(tr, ItemHelp.getRate(item.m_gemConsume, item.m_playerCount, 1));

                    genCell(tr, item.m_gemRemain.ToString());
                    genCell(tr, ItemHelp.getRate(item.m_gemRemain, item.m_playerCount, 1));

                    genCell(tr, item.getDataBaseRemain(item.m_dataBaseGemRemain));
                }
                break;
            case 2: // 龙珠
                {
                    genCell(tr, "龙珠");
                    genCell(tr, item.m_dbStart.ToString());       // 初始值
                    genCell(tr, item.m_dbFreeGain.ToString());    // 免费获得总计
                    genCell(tr, ItemHelp.getRate(item.m_dbFreeGain, item.m_playerCount, 1));    // 免费获得人均

                    genCell(tr, "");
                    genCell(tr, "");

                    genCell(tr, item.m_dbConsume.ToString());
                    genCell(tr, ItemHelp.getRate(item.m_dbConsume, item.m_playerCount, 1));

                    genCell(tr, item.m_dbRemain.ToString());
                    genCell(tr, ItemHelp.getRate(item.m_dbRemain, item.m_playerCount, 1));

                    genCell(tr, item.getDataBaseRemain(item.m_dataBaseDbRemain));
                }
                break;
            case 3: // 话费券
                {
                    genCell(tr, "话费券");
                    genCell(tr, item.m_chipStart.ToString());       // 初始值
                    genCell(tr, item.m_chipFreeGain.ToString());    // 免费获得总计
                    genCell(tr, ItemHelp.getRate(item.m_chipFreeGain, item.m_playerCount, 1));    // 免费获得人均

                    genCell(tr, "");
                    genCell(tr, "");

                    genCell(tr, item.m_chipConsume.ToString());
                    genCell(tr, ItemHelp.getRate(item.m_chipConsume, item.m_playerCount, 1));

                    genCell(tr, item.m_chipRemain.ToString());
                    genCell(tr, ItemHelp.getRate(item.m_chipRemain, item.m_playerCount, 1));

                    genCell(tr, item.getDataBaseRemain(item.m_dataBaseChipRemain));
                }
                break;
        }
    }

    private TableCell genCell(TableRow tr, string text)
    {
        TableCell td = new TableCell();
        tr.Cells.Add(td);
        td.Text = text;
        return td;
    }
}

//////////////////////////////////////////////////////////////////////////
// 游戏台账
public class TableGameStandingBook
{
    private static string[] s_head = new string[] { "日期", "前日库存", "今日系统金币减少",
       "今日系统金币增加",  "今日实际库存", "今日记录差值比"};

    private static string[] s_head1 = { "金币", "钻石", "龙珠", "话费券" };

    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res, ParamIncomeExpenses param)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        i = 0;
        List<StatIncomeExpensesItem> qresult = (List<StatIncomeExpensesItem>)user.getStatResult(StatType.statTypePlayerIncomeExpenses);
        for (; i < qresult.Count; i++)
        {
            StatIncomeExpensesItem item = qresult[i];
            //genDataCol(item, table, param);

            tr = new TableRow();
            table.Rows.Add(tr);
            fillRow(item, 0, param.m_property, tr);
        }
    }

    private void genDataCol(StatIncomeExpensesItem item, Table table, ParamIncomeExpenses param)
    {
        int curRow = table.Rows.Count;
        int rowCount = s_head1.Length;
        if (param.m_property > -1)
        {
            rowCount = 1;
        }

        // 每块数据生成足够行
        for (int i = 0; i < rowCount; i++)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
        }

        // 日期列
        TableCell td = new TableCell();
        table.Rows[curRow].Cells.Add(td);
        td.RowSpan = rowCount;
        td.Text = item.m_genTime.ToShortDateString();

        td = new TableCell();
        table.Rows[curRow].Cells.Add(td);
        td.RowSpan = rowCount;
        td.Text = item.m_playerCount.ToString();

        if (rowCount > 1)
        {
            for (int i = 0; i < s_head1.Length; i++)
            {
                fillRow(item, i, i, table.Rows[curRow + i]);
            }
        }
        else
        {
            fillRow(item, 0, param.m_property, table.Rows[curRow]);
        }
    }

    private void fillRow(StatIncomeExpensesItem item, int curRow, int property, TableRow tr)
    {
        genCell(tr, item.m_genTime.ToShortDateString());
        switch (property)
        {
            case 1:  // 金币
                {
                   // genCell(tr, "金币");
                    genCell(tr, item.m_goldStart.ToString("N0"));       // 初始值
                    genCell(tr, (item.m_goldFreeGain + item.m_goldRechargeGain).ToString("N0"));
                    genCell(tr, item.m_goldConsume.ToString("N0")); // 消耗总计
                    genCell(tr, item.m_goldRemain.ToString("N0")); // 结余总计
                    genCell(tr,
                        relativeErrorBeyond(item.m_goldStart + item.m_goldFreeGain + item.m_goldRechargeGain - item.m_goldConsume,
                        item.m_goldRemain));
                }
                break;
            case 2:  // 钻石
                {
                   // genCell(tr, "钻石");
                    genCell(tr, item.m_gemStart.ToString("N0"));       // 初始值
                    genCell(tr, (item.m_gemFreeGain + item.m_gemRechargeGain).ToString("N0"));
                    genCell(tr, item.m_gemConsume.ToString("N0"));
                    genCell(tr, item.m_gemRemain.ToString("N0"));

                    genCell(tr,
                        relativeErrorBeyond(item.m_gemStart + item.m_gemFreeGain + item.m_gemRechargeGain - item.m_gemConsume,
                        item.m_gemRemain));
                }
                break;
            case 14: // 龙珠
                {
                   // genCell(tr, "龙珠");
                    genCell(tr, item.m_dbStart.ToString("N0"));       // 初始值
                    genCell(tr, item.m_dbFreeGain.ToString("N0"));    // 免费获得总计
                    genCell(tr, item.m_dbConsume.ToString("N0"));
                    genCell(tr, item.m_dbRemain.ToString("N0"));

                    genCell(tr,
                        relativeErrorBeyond(item.m_dbStart + item.m_dbFreeGain - item.m_dbConsume,
                        item.m_dbRemain));
                }
                break;
            case 11: // 话费券
                {
                  //  genCell(tr, "话费券");
                    genCell(tr, item.m_chipStart.ToString("N0"));       // 初始值
                    genCell(tr, item.m_chipFreeGain.ToString("N0"));    // 免费获得总计
                    genCell(tr, item.m_chipConsume.ToString("N0"));
                    genCell(tr, item.m_chipRemain.ToString("N0"));

                    genCell(tr,
                        relativeErrorBeyond(item.m_chipStart + item.m_chipFreeGain - item.m_chipConsume,
                        item.m_chipRemain));
                }
                break;
        }
    }

    private TableCell genCell(TableRow tr, string text)
    {
        TableCell td = new TableCell();
        tr.Cells.Add(td);
        td.Text = text;
        return td;
    }

    string relativeErrorBeyond(double cur, double accuracy)
    {
        if (accuracy == 0.0)
            return "";

        double delta = Math.Abs(cur - accuracy);
        double e = delta / accuracy;
        return (Math.Round(e * 100, 2)).ToString() + "%";
    }
}

//////////////////////////////////////////////////////////////////////////
// 每小时付费
public class TableTdRechargePerHour
{
    private static string[] s_head = new string[] {"日期", 
        "0点累计","1点累计", "2点累计", "3点累计", "4点累计", "5点累计", "6点累计", "7点累计", 
        "8点累计","9点累计", "10点累计", "11点累计", "12点累计", "13点累计", "14点累计", "15点累计", 
        "16点累计","17点累计", "18点累计", "19点累计", "20点累计", "21点累计", "22点累计", "23点累计"};
    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0, f = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        i = 0;
        DataEachDay qresult = (DataEachDay)user.getQueryResult(QueryType.queryTypeRechargePerHour);
        var allData = qresult.getData();
        foreach (var data in allData)
        {
            f = 0;
            m_content[f++] = data.m_time.ToShortDateString();

            for (int k = 0; k < data.getCount(); k++)
            {
                m_content[f++] = data.getData(k).ToString();
            }

            tr = new TableRow();
            table.Rows.Add(tr);
            if ((i & 1) == 0)
            {
                tr.CssClass = "alt";
            }
            i++;

            for (int j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
// 每小时的实时在线
public class TableTdOnlinePlayerNumPerHour
{
    private static string[] s_head = new string[] {"日期", 
        "最高在线","最高在线时间点", "最低在线", "最低在线时间点", "平均在线"};
    private string[] m_content = new string[s_head.Length];

    public void genTable(GMUser user, Table table, OpRes res)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0, f = 0;
        for (; i < s_head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[i];
        }

        i = 0;
        DataEachDay qresult = (DataEachDay)user.getQueryResult(QueryType.queryTypeOnlinePlayerNumPerHour);
        var allData = qresult.getData();
        foreach (var data in allData)
        {
            f = 0;
            m_content[f++] = data.m_time.ToShortDateString();
            m_content[f++] = data.getData(data.m_maxIndex).ToString();
            m_content[f++] = data.m_maxIndex.ToString() + "点";
            m_content[f++] = data.getData(data.m_minIndex).ToString();
            m_content[f++] = data.m_minIndex.ToString() + "点";
            m_content[f++] = qresult.average(data);

            tr = new TableRow();
            table.Rows.Add(tr);
            if ((i & 1) == 0)
            {
                tr.CssClass = "alt";
            }
            i++;

            for (int j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
    }
}

//////////////////////////////////////////////////////////////////////////
public class TableHVBase
{
    protected string[] m_headH;
    protected string[] m_headV;

    public TableCell genCell(TableRow tr, string text)
    {
        TableCell td = new TableCell();
        tr.Cells.Add(td);
        td.Text = text;
        td.RowSpan = 1;
        return td;
    }

    public void genTable(GMUser user, Table table, OpRes res, object param)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        int i = 0;
        for (; i < m_headH.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = m_headH[i];
        }

        fillTableData(user, table, param);
    }

    public void fillHeadV(Table table, int startRow)
    {
        for (int i = 0; i < m_headV.Length; i++)
        {
            genCell(table.Rows[startRow + i], m_headV[i]);
        }
    }

    public virtual void fillTableData(GMUser user, Table table, object param) { }
}

// 游戏金币流动统计总计
public class TableStatServerEarningsTotal : TableHVBase
{
    // 横向标题
    private static string[] s_head = new string[] { "日期", "", "捕鱼","鳄鱼大亨", "牛牛","黑红梅方", "水浒传", "奔驰宝马","水果机","宝石迷阵", "总计" };
    private static string[] s_head1 = new string[] { "系统总收入", "系统总支出", "盈利值", "盈利率", "活跃人数" };

    public TableStatServerEarningsTotal()
    {
        m_headH = s_head;
        m_headV = s_head1;
    }

    public override void fillTableData(GMUser user, Table table, object param)
    {
        int[] ids = { (int)GameId.fishlord,(int)GameId.crocodile, (int)GameId.cows, (int)GameId.shcd, (int)GameId.shuihz, (int)GameId.bz,(int)GameId.fruit,(int)GameId.jewel};
        ResultServerEarningsTotal qresult = (ResultServerEarningsTotal)user.getQueryResult(null, QueryType.queryTypeServerEarnings);
        PlayerTypeData<EarningItem> total = qresult.sum(ids);
        genDataCol(total, table, "总计", ids);

        var arr = qresult.getAllDescByTime();
        foreach (var item in arr)
        {
            genDataCol(item.Value, table, item.Key.ToShortDateString(), ids);
        }
    }

    private void genDataCol(PlayerTypeData<EarningItem> item, Table table, string time, int[] ids)
    {
        int curRow = table.Rows.Count;
        int rowCount = m_headV.Length;
        
        // 每块数据生成足够行
        for (int i = 0; i < rowCount; i++)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
        }

        // 日期列
        TableCell td = new TableCell();
        table.Rows[curRow].Cells.Add(td);
        td.RowSpan = rowCount;
        td.Text = time;
        td.Attributes.CssStyle.Value = "vertical-align:middle";

        fillHeadV(table, curRow);

        long totalIncome = 0, totalOutlay = 0, totalEarn = 0, totalActPerson = 0;
        
        for (int i = 0; i < ids.Length; i++)
        {
            var dataItem = item.getData(ids[i]);
            fillColDataBlock(dataItem, table, curRow);

            if (dataItem != null)
            {
                totalIncome += dataItem.getRoomIncome(6);
                totalOutlay += dataItem.getRoomOutlay(6);
                totalEarn += dataItem.getDelta(6);
                totalActPerson += dataItem.getRoomIncome(0);
            }
        }

        fillSum(totalIncome, totalOutlay, totalEarn, totalActPerson, table, curRow);
    }

    private void fillColDataBlock(EarningItem item, Table table, int startRow)
    {
        int i = 0;
        if (item == null)
        {
            genCell(table.Rows[startRow + i], "");
            i++;
            genCell(table.Rows[startRow + i], "");
            i++;
            genCell(table.Rows[startRow + i], "");
            i++;
            genCell(table.Rows[startRow + i], "");
            i++;
            genCell(table.Rows[startRow + i], "");
        }
        else
        {
            genCell(table.Rows[startRow + i], ItemHelp.getNumByComma(item.getRoomIncome(6)));
            i++;
            genCell(table.Rows[startRow + i], ItemHelp.getNumByComma(item.getRoomOutlay(6)));
            i++;
            genCell(table.Rows[startRow + i], ItemHelp.getNumByComma(item.getDelta(6)));
            i++;
            genCell(table.Rows[startRow + i], item.getFactExpRate(6).ToString());
            i++;
            genCell(table.Rows[startRow + i], ItemHelp.getNumByComma(item.getRoomIncome(0)));
        }
    }

    private void fillSum(long totalIncome, long totalOutlay, long totalEarn,long totalActPerson, Table table, int startRow)
    {
        int i = 0;
        genCell(table.Rows[startRow + i], ItemHelp.getNumByComma(totalIncome));
        i++;
        genCell(table.Rows[startRow + i], ItemHelp.getNumByComma(totalOutlay));
        i++;
        genCell(table.Rows[startRow + i], ItemHelp.getNumByComma(totalEarn));
        i++;
        genCell(table.Rows[startRow + i], "");
        i++;
        genCell(table.Rows[startRow + i], ItemHelp.getNumByComma(totalActPerson));
    }
}

//////////////////////////////////////////////////////////////////////////
public class TableHeadInfo
{
    public string m_title;
    public int m_rowSpan;
    public int m_colSpan;
}

public class TableHHBase
{
    protected string[] m_headH1;
    protected string[] m_headH2;

    public TableCell genCell(TableRow tr, string text)
    {
        TableCell td = new TableCell();
        tr.Cells.Add(td);
        td.Text = text;
        return td;
    }

    public void genTable(GMUser user, Table table, OpRes res, object param)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        genHead(m_headH1, tr);
        tr = new TableRow();
        table.Rows.Add(tr);
        genHead(m_headH2, tr);

        fillTableData(user, table, param);
    }

    List<TableHeadInfo> getHeadList(string[] head)
    {
        List<TableHeadInfo> res = new List<TableHeadInfo>();
        for (int i = 0; i < head.Length; i++)
        {
            string[] tmp = Tool.split(head[i], '#', StringSplitOptions.RemoveEmptyEntries);
            TableHeadInfo ti = new TableHeadInfo();
            ti.m_title = tmp[0];
            ti.m_rowSpan = Convert.ToInt32(tmp[1]);
            ti.m_colSpan = Convert.ToInt32(tmp[2]);
            res.Add(ti);
        }
        return res;
    }

    void genHead(string[] head, TableRow tr)
    {
        List<TableHeadInfo> infoList = getHeadList(head);
        TableCell td = null;
        int i = 0;
        for (; i < infoList.Count; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = infoList[i].m_title;
            td.ColumnSpan = infoList[i].m_colSpan;
            td.RowSpan = infoList[i].m_rowSpan;
        }
    }
    public virtual void fillTableData(GMUser user, Table table, object param) { }
}

// 用户下注情况查询
public class TablePlayerGameBet : TableHHBase
{
    private static string[] s_head = new string[] { "日期#2#1", "玩家ID#2#1", "携带量#1#3", "下注量#1#3", "赢钱数#1#3", "输钱数#1#3",
        "当日流水#2#1", "输赢详情#2#1" };
    private static string[] s_head1 = new string[] { 
        "平均#1#1", "最大#1#1", "最小#1#1", 
        "平均#1#1", "最大#1#1", "最小#1#1",
        "平均#1#1", "最大#1#1", "最小#1#1",
        "平均#1#1", "最大#1#1", "最小#1#1",};

    public TablePlayerGameBet()
    {
        m_headH1 = s_head;
        m_headH2 = s_head1;
    }

    public override void fillTableData(GMUser user, Table table, object param)
    {
        List<ResultItemPlayerGameBet> qresult =
            (List<ResultItemPlayerGameBet>)user.getQueryResult(QueryType.queryTypePlayerGameBet);
        for (int i = 0; i < qresult.Count; i++)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            genDataRow(qresult[i], tr);
        }
    }

    void genDataRow(ResultItemPlayerGameBet data, TableRow tr)
    {
        genCell(tr, data.m_time.ToShortDateString());
        genCell(tr, data.getPlayerId());
        genCell(tr, data.getAve(ResultItemPlayerGameBet.CARRY));
        genCell(tr, data.getMax(ResultItemPlayerGameBet.CARRY));
        genCell(tr, data.getMin(ResultItemPlayerGameBet.CARRY));

        genCell(tr, data.getAve(ResultItemPlayerGameBet.OUTLAY));
        genCell(tr, data.getMax(ResultItemPlayerGameBet.OUTLAY));
        genCell(tr, data.getMin(ResultItemPlayerGameBet.OUTLAY));

        genCell(tr, data.getAve(ResultItemPlayerGameBet.WIN));
        genCell(tr, data.getMax(ResultItemPlayerGameBet.WIN));
        genCell(tr, data.getMin(ResultItemPlayerGameBet.WIN));

        genCell(tr, data.getAve(ResultItemPlayerGameBet.LOSE));
        genCell(tr, data.getMax(ResultItemPlayerGameBet.LOSE));
        genCell(tr, data.getMin(ResultItemPlayerGameBet.LOSE));

        genCell(tr, data.getRw());

        if (data.m_playerId > 0)
        {
            genCell(tr, genLink(data));
        }
        else
        {
            genCell(tr, "");
        }
    }

    string genLink(ResultItemPlayerGameBet data)
    {
        URLParam uParam = new URLParam();
        uParam.m_text = "详情";
        uParam.m_key = "param";
        uParam.m_value = data.m_playerId.ToString();
        uParam.m_url = "/appaspx/operation/OperationMoneyQueryDetail.aspx";
        uParam.m_target = "_blank";
        uParam.addExParam("time", data.m_time.ToShortDateString());
        uParam.addExParam("property", 1);
        uParam.addExParam("filter", (int)PropertyReasonType.type_reason_single_round_balance);
        uParam.addExParam("gameId", data.m_gameId);
        return Tool.genHyperlink(uParam);
    }
}

//////////////////////////////////////////////////////////////////////////
public class TableNormalBase
{
    protected string[] m_headH;

    public TableCell genCell(TableRow tr, string text, int row = 1)
    {
        TableCell td = new TableCell();
        tr.Cells.Add(td);
        td.Text = text;
        td.RowSpan = row;
        td.Attributes.CssStyle.Value = "vertical-align:middle";
        return td;
    }

    public void genTable(GMUser user, Table table, OpRes res, object param)
    {
        TableRow tr = new TableRow();
        table.Rows.Add(tr);
        TableCell td = null;
        if (res != OpRes.opres_success)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = OpResMgr.getInstance().getResultString(res);
            return;
        }

        genHead(m_headH, tr);
        fillTableData(user, table, param);
    }

    void genHead(string[] head, TableRow tr)
    {
        TableCell td = null;
        int i = 0;
        for (; i < head.Length; i++)
        {
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = head[i];
        }
    }
    public virtual void fillTableData(GMUser user, Table table, object param) { }
}

public class TableRank : TableNormalBase
{
    private static string[] s_head = new string[] { "玩家id","渠道", "排行", "增长值", "累计充值","当日累计充值", "最后登录" };

    public TableRank()
    {
        m_headH = s_head;
    }

    public override void fillTableData(GMUser user, Table table, object param)
    {
        var rankList = (List<ResultRankItem>)param;
        if (rankList == null)
            return;
        for (int i = 0; i < rankList.Count; i++)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            genDataRow(rankList[i], tr, i + 1);
        }
    }

    void genDataRow(ResultRankItem data, TableRow tr, int rank)
    {
        genCell(tr, data.m_playerId.ToString());
        genCell(tr, data.m_channel.ToString());
        genCell(tr, rank.ToString());
        genCell(tr, data.m_value.ToString());
        genCell(tr, data.m_rechargeTotal.ToString());
        genCell(tr, data.m_dailyRecharge.ToString());
        genCell(tr, data.m_lastLogin.ToString());
    }
}

///////////////////////////////////
//财富榜
public class TablePlayerRichesRank : TableNormalBase
{
    private static string[] s_head = new string[] { "玩家ID", "玩家昵称", "排行", "总量", "当日充值", "最后登录时间" };

    public TablePlayerRichesRank()
    {
        m_headH = s_head;
    }

    public override void fillTableData(GMUser user, Table table, object param)
    {
        var rankList = (List<ResultRichesRankItem>)param;
        if (rankList == null)
            return;
        table.GridLines = GridLines.Both;
        for (int i = 0; i < rankList.Count; i++)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            genDataRow(rankList[i], tr);
        }
    }

    void genDataRow(ResultRichesRankItem data, TableRow tr)
    {
        genCell(tr, data.m_playerId.ToString());
        genCell(tr, data.m_nickName.ToString());
        genCell(tr, data.m_rank.ToString());
        genCell(tr, data.m_rechargeTotal.ToString());
        genCell(tr, data.m_dailyRecharge.ToString());
        genCell(tr, data.m_lastLogin.ToString());
    }
}
//////////////////////////////////////////////////////////////////////////
public class TableBlockIdList : TableNormalBase
{
    private static string[] s_head = new string[] { "停封时间", "ID", "选择" };

    public TableBlockIdList()
    {
        m_headH = s_head;
    }

    public override void fillTableData(GMUser user, Table table, object param)
    {
        var blockList = (List<ResultBlock>)param;
        if (blockList == null)
            return;
        for (int i = 0; i < blockList.Count; i++)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            genDataRow(blockList[i], tr);
        }
    }

    void genDataRow(ResultBlock data, TableRow tr)
    {
        genCell(tr, data.m_time);
        genCell(tr, data.m_param);
        genCell(tr, Tool.getCheckBoxHtml("sel", data.m_param, false));
    }
}

public class StatFishLordBaojin 
{

}

//////////////////////////////////////////////////////////////////////////
public class TableChannel100003 : TableNormalBase
{
    //private static string[] s_head = new string[] { "日期","累计赢取金币20万", "累计赢取金币500万", "累计赢取金币2000万", "累计赢取金币5000万",
    //    "累计赢取金币2亿","累计赢取金币6亿","累计赢取金币9亿","累计赢取金币15亿","累计赢取金币30亿","累计赢取金币50亿","累计赢取金币80亿","累计赢取金币140亿","累计赢取金币260亿",
    //    "累计赢取金币450亿","累计赢取金币800亿","累计赢取金币1500亿","累计充值6元","累计充值30元","累计充值98元","累计充值198元","累计充值398元","累计充值998元","累计充值1588元",
    //    "累计充值3288元","累计充值6488元","累计充值10000元","累计充值15000元","累计充值20000元" };

    private static string[] s_head = new string[] {"日期","广告编号","渠道","注册人数","级别","级别描述","奖励RMB","人数","转化率"};

    static int GOLD_INDEX_COUNT = 16;
    static int RECHARGE_INDEX_COUNT = 12;

    public TableChannel100003()
    {
        m_headH = s_head;
    }

    public override void fillTableData(GMUser user, Table table, object param)
    {
        List<Channel100003> channelList =
            (List<Channel100003>)user.getQueryResult(QueryType.queryTypeQueryChannel100003ActCount);
        if (channelList == null)
            return;

        for (int i = 0; i < channelList.Count; i++)
        {
            int k = 1, l = 0, m = 0;
            for (l = 0; l < GOLD_INDEX_COUNT; ++l)
            {
                TableRow tr = new TableRow();
                table.Rows.Add(tr);
                if (l == 0)
                    genCell(tr, channelList[i].m_time.ToShortDateString(), 28);

                genCell(tr, "闲玩一期");
                genCell(tr, "闲玩");
                genCell(tr, channelList[i].m_regeditCount.ToString());
                genCell(tr, k.ToString());
                genCell(tr, getChannelLaunchName(k)[0]);
                genCell(tr, getChannelLaunchName(k)[1]);

                //人数
                int goldCount = channelList[i].getGoldCount(l);
                genCell(tr, goldCount.ToString()); //人数
                genCell(tr, getRate(goldCount, channelList[i].m_regeditCount));
                k++;
            }

            for (m = 0; m < RECHARGE_INDEX_COUNT; ++m)
            {
                TableRow tr = new TableRow();
                table.Rows.Add(tr);

                genCell(tr, "闲玩一期");
                genCell(tr, "闲玩");
                genCell(tr, channelList[i].m_regeditCount.ToString());
                genCell(tr, k.ToString());
                genCell(tr, getChannelLaunchName(k)[0]);
                genCell(tr, getChannelLaunchName(k)[1]);

                //人数
                int rechargeCount = channelList[i].getRechargeCount(m);
                genCell(tr, rechargeCount.ToString()); //人数
                genCell(tr, getRate(rechargeCount, channelList[i].m_regeditCount));
                k++;
            }
        }
    }

    public string getRate(int key1, int key2)
    {
        string rate = "";
        if (key2 == 0)
        {
            rate = key1.ToString();
        }
        else
        {
            rate = Math.Round(key1 * 1.0 / key2, 2)*100 + "%";
        }

        return rate;
    }


    public string[] getChannelLaunchName(int lv)
    {
        string[] item = new string[2]{"",""};
        ChannelLaunchData itemChannel = ChannelLaunchCFG.getInstance().getValue(lv);
        if (itemChannel != null) 
        {
            item[0] = itemChannel.m_lvName;
            item[1] = itemChannel.m_RMB.ToString();
        }

        return item;
    }

    //void genDataRow(Channel100003 data, TableRow tr)
    //{
    //    genCell(tr, data.m_time.ToShortDateString());
    //    for (int i = 0; i < GOLD_INDEX_COUNT; ++i)
    //    {
    //        genCell(tr, data.getGoldCount(i).ToString());
    //    }
    //    for (int i = 0; i < RECHARGE_INDEX_COUNT; ++i)
    //    {
    //        genCell(tr, data.getRechargeCount(i).ToString());
    //    }
    //}
}


































