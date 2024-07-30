using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordTicketRoomAct : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "日期","奖券产出","金币收入","金元宝产出"};
        private static string[] s_head2 = new string[] { "日期","任务1", "任务2", "任务3", "任务4", "任务5", "任务6", "任务7", "任务8", "任务9" };

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_FISHLORD_TICKET_ROOM_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_item.Items.Add("奖券场统计");
                m_item.Items.Add("每日挑战");
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = m_item.SelectedIndex;
            param.m_time = m_time.Text;

            OpRes res = OpRes.op_res_failed;
            switch (param.m_op)
            {
                case 0: //奖券场统计
                    res = user.doQuery(param, QueryType.queryTypeStatFishlordTicketRoomStat);
                    genTable0(m_result, res, user, param);
                    break;
                case 1: //每日挑战
                    res = user.doQuery(param, QueryType.queryTypeStatFishlordTicketRoomQuest);
                    genTable1(m_result, res, user, param);
                    break;
            }
        }

        //圣兽场
        private void genTable0(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[s_head1.Length];

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

            List<StatFishlordTicketRoomStatItem> qresult =
                (List<StatFishlordTicketRoomStatItem>)user.getQueryResult(QueryType.queryTypeStatFishlordTicketRoomStat);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.Text = s_head1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatFishlordTicketRoomStatItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_tombolaOutlay.ToString();
                m_content[f++] = item.m_goldIncome.ToString();
                m_content[f++] = item.m_goldingotOutlay.ToString();

                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //巨鲨场
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content = new string[s_head2.Length];

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

            List<StatFishlordTicketRoomQuestItem> qresult =
                (List<StatFishlordTicketRoomQuestItem>)user.getQueryResult(QueryType.queryTypeStatFishlordTicketRoomQuest);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.RowSpan = 1;
                td.Text = s_head2[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatFishlordTicketRoomQuestItem item = qresult[i];

                m_content[f++] = item.m_time;
                foreach (var da in item.m_taskList) 
                {
                    m_content[f++] = da.Value.ToString();
                }

                for (j = 0; j < s_head2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }
    }
}