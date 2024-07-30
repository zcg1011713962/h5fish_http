using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordAdvancedRoomAct : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "时间","排名","玩家ID","昵称","积分"};
        private static string[] s_head = new string[] { "日期","抽奖人数","抽奖次数","奖池收入","奖池支出","大奖支出","系统回收","一等奖","二等奖","三等奖","详情"};
      

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_ADVANCED_ROOM_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("排行榜统计");
                m_queryType.Items.Add("奖池统计");

                m_rankType.Items.Add(new ListItem("当前活动"));
                m_rankType.Items.Add(new ListItem("历史排行"));
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
                    param.m_param = m_rankType.SelectedIndex.ToString(); //0 当前  1历史
                    if (Convert.ToInt32(param.m_param) == 1)
                        param.m_time = m_time.Text;

                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordAdvancedRoomActRank, user);
                    genTable0(m_result, res, user, param);
                    break;
                case 1:
                    param.m_param = m_time.Text;
                    res = mgr.doQuery(param, QueryType.queryTypeStatFishlordAdvancedRoomAct, user);
                    genTable1(m_result, res, user, param);
                    break;
            }
        }

        //统计表
        private void genTable0(Table table, OpRes res, GMUser user, ParamQuery param)
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

            List<StatFishlordAdvancedRoomActRankItem> qresult =
                (List<StatFishlordAdvancedRoomActRankItem>)user.getQueryResult(QueryType.queryTypeStatFishlordAdvancedRoomActRank);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head1.Length];

            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                StatFishlordAdvancedRoomActRankItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_score.ToString();

                tr = new TableRow();
                table.Rows.Add(tr);
                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        //统计表
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery param)
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

            List<StatFishlordAdvancedRoomActItem> qresult = (List<StatFishlordAdvancedRoomActItem>)user.getQueryResult(QueryType.queryTypeStatFishlordAdvancedRoomAct);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head.Length];
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                StatFishlordAdvancedRoomActItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_lotteryPerson.ToString();
                m_content[f++] = item.m_lotteryCount.ToString();
                m_content[f++] = item.m_poolIncome.ToString();
                m_content[f++] = item.m_poolOutlay.ToString();
                m_content[f++] = item.m_dailyAdvanceRoomGrandPriceOutlay.ToString();//大奖支出
                m_content[f++] = item.m_recycleGold.ToString();
                m_content[f++] = item.m_level1.ToString();
                m_content[f++] = item.m_level2.ToString();
                m_content[f++] = item.m_level3.ToString();
                m_content[f++] = i == 0 ? " " : item.getDetail();

                tr = new TableRow();
                table.Rows.Add(tr);
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}