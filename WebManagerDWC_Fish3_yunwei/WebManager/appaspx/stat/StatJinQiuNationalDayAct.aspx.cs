using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatJinQiuNationalDayAct : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "排名榜", "玩家ID", "玩家昵称", "累计月饼","最高炮台倍","VIP等级","是否机器人" };
        private static string[] s_head2 = new string[] { "日期","抽奖类型","当日金币消耗","当日月饼产出","当日抽取人次","当日抽取人数",
            "奖励1人次","奖励2人次","奖励3人次","奖励4人次","奖励5人次","奖励6人次","奖励7人次","奖励8人次","奖励9人次", "奖励总支出","详情"};

        private static string[] s_head3 = new string[] { "时间", "中级场消耗金币", "高级场消耗金币", "vip场消耗金币", "龙宫场消耗金币", 
            "中级场击杀鱼数量", "高级场击杀鱼数量", "vip场击杀鱼数量", "龙宫场击杀鱼数量" };

        private static string[] s_head4 = new string[] { "日期", "类型", "任务1人次", "任务2完人次", "任务3人次","详情"};

        private static int[] s_roomId = new int[] {2, 3, 4, 20};

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_JINQIU_NATIONAL_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("排行榜统计");
                m_queryType.Items.Add("抽奖统计");
                m_queryType.Items.Add("场次统计");
                m_queryType.Items.Add("任务统计");

                m_time1.Items.Add(new ListItem("当前活动"));
                m_time1.Items.Add(new ListItem("第一周"));
                m_time1.Items.Add(new ListItem("第二周"));
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_op = m_queryType.SelectedIndex;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = OpRes.op_res_failed;
            switch (param.m_op)
            {
                case 0:
                    param.m_param = m_time1.SelectedIndex.ToString();
                    res = mgr.doQuery(param, QueryType.queryTypeJinQiuNationalDayActRank, user);
                    genTable0(m_result, res, param, user, mgr);
                    break;
                case 1:
                    param.m_param = m_time2.Text;
                    res = mgr.doQuery(param, QueryType.queryTypeJinQiuNationalDayActLottery, user);
                    genTable1(m_result, res, param, user, mgr);
                    break;
                case 2:
                    param.m_param = m_time2.Text;
                    res = mgr.doQuery(param, QueryType.queryTypeJinQiuNationDayActRoomStat, user);
                    genTable2(m_result, res, param, user, mgr);
                    break;
                case 3:
                    param.m_param = m_time2.Text;
                    res = mgr.doQuery(param, QueryType.queryTypeJinQiuNationalDayActTaskStat, user);
                    genTable3(m_result, res, param, user, mgr);
                    break;
            }
        }
        //活动排行榜生成查询表
        private void genTable0(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head1.Length];
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            List<StatRankItem> qresult = (List<StatRankItem>)mgr.getQueryResult(QueryType.queryTypeJinQiuNationalDayActRank);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
                td.RowSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                StatRankItem item = qresult[i];
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_playerId;
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_moonCakeCount.ToString();
                m_content[f++] = item.fishLevelName();
                m_content[f++] = item.m_vipLevel.ToString();
                m_content[f++] = item.m_isRobot;

                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                }
            }
        }

        //活动抽奖统计
        private void genTable1(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head2.Length];
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            List<StatLotteryList> qresult = (List<StatLotteryList>)mgr.getQueryResult(QueryType.queryTypeJinQiuNationalDayActLottery);
            int i = 0;
            // 表头
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head2[i];
            }

            long totalRewardOutlay = 0;
            for (i = 0; i < qresult.Count; i++)
            {
                StatLotteryList item = qresult[i];

                int m = 0;
                foreach(var da in item.m_data)
                {
                    totalRewardOutlay = 0;

                    tr = new TableRow();
                    m_result.Rows.Add(tr);

                    if (m == 0) {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = da.m_time;
                        td.RowSpan = item.m_data.Count;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.getLotteryTypeName();
                    td.RowSpan = 1;

                    if (m == 0) {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_goldIncome.ToString();
                        td.RowSpan = item.m_data.Count;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_dropCount.ToString();
                        td.RowSpan = item.m_data.Count;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.m_lotteryCount.ToString();
                    td.RowSpan = 1;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.m_lotteryPerson.ToString();

                    //少月饼抽奖 多月饼抽奖
                    int index = da.m_lotteryType - 1;
                    for (int k = 0; k < 9; k++)
                    {
                        string str_content = "";
                        if (da.m_reward.ContainsKey(k)) 
                        {
                            totalRewardOutlay += da.m_reward[k] * da.getLotteryAward(k);
                            str_content = da.m_reward[k].ToString();
                        }
                            
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = str_content;
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = totalRewardOutlay.ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.getExParam(i);

                    m++;
                }
            }
        }

        //场次统计
        private void genTable2(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            List<ActRoomStatList> qresult = (List<ActRoomStatList>)mgr.getQueryResult(QueryType.queryTypeJinQiuNationDayActRoomStat);
            int i = 0, k = 0, len = s_roomId.Length;
            // 表头
            for (i = 0; i < s_head3.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head3[i];
                td.RowSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                ActRoomStatList item = qresult[i];

                tr = new TableRow();
                m_result.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;
                td.RowSpan = 1;

                for (k = 0; k < len; k++)
                {
                    int index = s_roomId[k];
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_data.ContainsKey(index))
                    {
                        td.Text = item.m_data[index].m_goldIncome.ToString();
                    }else{
                        td.Text = "";
                    }
                    td.RowSpan = 1;
                }

                for (k = 0; k < len; k++)
                {
                    int index = s_roomId[k];
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_data.ContainsKey(index))
                    {
                        td.Text = item.m_data[index].m_killCount.ToString();
                    }else{
                        td.Text = "";
                    }
                    td.RowSpan = 1;
                }
            }
        }

        //任务统计
        private void genTable3(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            List<ActTaskItem> qresult = (List<ActTaskItem>)mgr.getQueryResult(QueryType.queryTypeJinQiuNationalDayActTaskStat);
            int i = 0;
            // 表头
            for (i = 0; i < s_head4.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head4[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                ActTaskItem item = qresult[i];

                tr = new TableRow();
                m_result.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;
                td.RowSpan = 2;
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "完成";
                td.RowSpan = 1;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_t1Finish.ToString();
                td.RowSpan = 1;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_t2Finish.ToString();

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_t3Finish.ToString();

                td = new TableCell();
                tr.Cells.Add(td);
                td.RowSpan = 2;
                td.Text = item.getExParam(i);
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                tr = new TableRow();
                m_result.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "领取";
                td.RowSpan = 1;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_t1Receive.ToString();

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_t2Receive.ToString();

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_t3Receive.ToString();
            }
        }
    }
}