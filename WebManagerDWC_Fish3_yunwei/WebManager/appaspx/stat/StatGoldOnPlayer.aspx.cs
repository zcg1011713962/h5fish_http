using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatGoldOnPlayer : System.Web.UI.Page
    {

        private static string[] s_head = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_STAT_GOLD_ON_PLAYER, Session, Response);
            if (!IsPostBack)
            {
                m_type.Items.Add("全部");

                m_type.Items.Add("新增玩家");
                m_type.Items.Add("活跃玩家");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_type = m_type.SelectedIndex;
            param.m_time = m_time.Text;

            OpRes res = user.doQuery(param, QueryType.queryTypeStatGoldOnPlayer);
            genTable(m_result, res, user, param);
        }

        private void genTable(Table table, OpRes res, GMUser user, ParamQuery p)
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

            int i = 0, k = 0, n = 0;

            s_head = new string[45];
            s_head[0] = "日期";
            s_head[1] = "类型";
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);

                if (i > 1)
                {
                    s_head[i] = i - 1 + "炮";
                    Fish_LevelCFGData openRateLevel = Fish_TurretLevelCFG.getInstance().getValue(i - 1);
                    if (openRateLevel != null)
                        s_head[i] = openRateLevel.m_openRate + "炮";
                }

                td.Attributes.CssStyle.Value = "min-width:70px";
                td.RowSpan = 1;
                td.ColumnSpan = 1;
                td.Text = s_head[i];
            }

            string[] m_content = new string[s_head.Length];

            List<statGoldOnPlayerItem> qresult =
                (List<statGoldOnPlayerItem>)user.getQueryResult(QueryType.queryTypeStatGoldOnPlayer);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;

                tr = new TableRow();
                m_result.Rows.Add(tr);

                statGoldOnPlayerItem item = qresult[i];
                m_content[n++] = item.m_time;

                m_content[n++] = item.m_type == 1 ? "新增玩家" : "活跃玩家";

                foreach(var da in item.m_goldList)
                {
                    m_content[n++] = da.Value.ToString();
                }

                for (k = 0; k < s_head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }
    }
}