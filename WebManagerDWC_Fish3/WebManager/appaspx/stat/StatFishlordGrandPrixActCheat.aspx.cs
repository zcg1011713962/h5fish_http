using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordGrandPrixActCheat : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID", "昵称", "当日排行积分", "赛季排行积分" };
        private string[] m_content = new string[s_head.Length];

        private DateTime time_start = Convert.ToDateTime("23:50:00");
        private DateTime time_end = Convert.ToDateTime("23:59:59");

        protected void Page_Load(object sender, EventArgs e)
        {
            m_res.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_FISHLORD_GRAND_PRIX_ACT_CHEAT, Session, Response);
            if (!IsPostBack)
            {
                m_type.Items.Add("今日排行");
                m_type.Items.Add("赛季排行");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            ParamQuery p = new ParamQuery();
            p.m_playerId = m_playerId.Text;
            OpRes res = mgr.doQuery(p, QueryType.queryTypeStatFishlordGrandPrixActCheat, user);
            genTable1(m_result, res, user, mgr);
        }

        private void genTable1(Table table, OpRes res, GMUser user, QueryMgr mgr)
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
            List<FishlordGrandPrixActRankItem> qresult =
                (List<FishlordGrandPrixActRankItem>)mgr.getQueryResult(QueryType.queryTypeStatFishlordGrandPrixActCheat);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                FishlordGrandPrixActRankItem item = qresult[i];
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_points.ToString();
                m_content[f++] = item.m_points2.ToString();
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                }
            }
        }

        protected void onEdit(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            OpRes res = OpRes.op_res_failed;

            ParamQuery p = new ParamQuery();
            p.m_type = m_type.SelectedIndex;

            //每天的23:50-23:59时，提示不能修改积分
            DateTime dt = DateTime.Now;
            if (DateTime.Compare(time_start, dt) <= 0 && DateTime.Compare(dt, time_end) <= 0)
            {
                m_res.InnerHtml = "注：每天的23:50-23:59不能修改积分";
            }
            else if (!string.IsNullOrEmpty(m_score.Text))
            {
                CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
                buffer.Writer.Write(m_playerId.Text);
                buffer.Writer.Write(p.m_type);
                buffer.Writer.Write(m_score.Text);
                buffer.Writer.Write(m_nickName.Text);
                CommandBase cmd = CommandMgr.processCmd(CmdName.AlterFishlordGrandPrixActPlayerScore, buffer, user);
                res = cmd.getOpRes();
                string str = OpResMgr.getInstance().getResultString(res);
                m_res.InnerHtml = str;
            }

            onQuery(null, null);
        }
    }
}