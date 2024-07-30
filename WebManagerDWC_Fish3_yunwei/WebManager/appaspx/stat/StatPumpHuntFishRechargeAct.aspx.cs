using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpHuntFishRechargeAct : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "日期","100条鱼","200条鱼","500条鱼","1000条鱼","3000条鱼","5000条鱼","7000条鱼","10000条鱼",
            "5只蟹将","10只蟹将","15只蟹将","20只蟹将","30只蟹将","50只蟹将","100只蟹将","200只蟹将","金币产出","详情"};
        private static string[] s_head2 = new string[] { "日期","6元","20元","50元","150元","400元","1000元","2500元","6000元","金币产出","详情"};

        private static string[] s_head3 = new string[] { "日期","场次人数","高级礼包购买人数","支出金币","任务类型",
            "令牌任务1","令牌任务2","令牌任务3","令牌任务4","令牌任务5"};

        private static string[] s_head4 = new string[] { "日期","玩家获得银币","支出金币"};

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PUMP_HUNT_FISH_RECHARGE_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_actId.Items.Add("圣兽活动");
                m_actId.Items.Add("巨鲨活动");
                m_actId.Items.Add("捕鱼活动");
                m_actId.Items.Add("累充奖励");
                m_actId.Items.Add("捞鱼活动");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = m_actId.SelectedIndex;
            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = OpRes.op_res_failed;
            if (param.m_op == 0 || param.m_op == 1) //圣兽活动 巨鲨活动
            {
                res = mgr.doQuery(param, QueryType.queryTypeStatRoomQuestAct, user);
                genTable2(m_result, res, param, user, mgr);

            }else if(param.m_op == 2) //捕鱼活动
            {
                res = mgr.doQuery(param, QueryType.queryTypeStatPumpHuntFishActivity, user);
                genTable(m_result, res, param, user, mgr);
            }
            else if(param.m_op == 3)  //累充奖励
            {
                res = mgr.doQuery(param, QueryType.queryTypeStatPumpRechargeActivity, user);
                genTable1(m_result, res, param, user, mgr);
            }else if(param.m_op == 4) //捞鱼活动
            {
                res = mgr.doQuery(param, QueryType.queryTypeStatPumpCatchFishActivity, user);
                genTable3(m_result, res, param, user, mgr);
            }
        }

        //生成查询表 玩法收入统计
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
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
            List<StatHuntFishActItem> qresult = (List<StatHuntFishActItem>)mgr.getQueryResult(QueryType.queryTypeStatPumpHuntFishActivity);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                StatHuntFishActItem item = qresult[i];
                m_content[f++] = item.m_time;
                foreach(var da in item.m_task)
                {
                    m_content[f++] = da.Value.ToString();
                }
                m_content[f++] = item.m_outlay.ToString();
                m_content[f++] = item.getDetail();
                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        //生成查询表 孵化巨鲲统计
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
            List<StatRechargeActItem> qresult = (List<StatRechargeActItem>)mgr.getQueryResult(QueryType.queryTypeStatPumpRechargeActivity);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head2[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                StatRechargeActItem item = qresult[i];
                m_content[f++] = item.m_time;
                foreach(var da in item.m_task)
                {
                    m_content[f++] = da.Value.ToString();
                }
                m_content[f++] = item.m_outlay.ToString();
                m_content[f++] = item.getDetail();

                for (j = 0; j < s_head2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        //生成查询表 圣兽活动巨鲨活动
        private void genTable2(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head3.Length];

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
            List<StatRoomQuestActItem> qresult = (List<StatRoomQuestActItem>)mgr.getQueryResult(QueryType.queryTypeStatRoomQuestAct);
            int i = 0, f = 0;
            // 表头
            for (i = 0; i < s_head3.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head3[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
               
                StatRoomQuestActItem item = qresult[i];

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;
                td.RowSpan = 2;
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_roomCount.ToString();
                td.RowSpan = 2;
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_giftCount.ToString();
                td.RowSpan = 2;
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_goldOutlay.ToString();
                td.RowSpan = 2;
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                f=0;
                foreach (var da in item.m_data.Values) 
                {
                    if (f == 0)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "普通任务";
                        td.RowSpan = 1;
                    }
                    else if (f == 5)
                    {
                        tr = new TableRow();
                        table.Rows.Add(tr);
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "礼包任务";
                        td.RowSpan = 1;
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da.ToString();
                    td.RowSpan = 1;

                    f++;
                }

            }
        }

        //生成查询表 孵化巨鲲统计
        private void genTable3(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            string[] m_content = new string[s_head4.Length];

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
            List<StatPumpCatchFishItem> qresult = 
                (List<StatPumpCatchFishItem>)mgr.getQueryResult(QueryType.queryTypeStatPumpCatchFishActivity);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head4.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head4[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                StatPumpCatchFishItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_gainCoin.ToString();
                m_content[f++] = item.m_outlay.ToString();

                for (j = 0; j < s_head4.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}