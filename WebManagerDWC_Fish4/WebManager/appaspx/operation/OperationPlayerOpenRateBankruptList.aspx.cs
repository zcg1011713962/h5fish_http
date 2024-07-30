using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationPlayerOpenRateBankruptList : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","玩家ID","使用炮倍","花色价值","能量价值","魔石价值","碎片价值","鱼雷价值","当日破产总次数"};
        private string[] m_content = new string[s_head.Length];

        PagePlayerOpenRateBankrupt m_gen = new PagePlayerOpenRateBankrupt(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_PLAYER_OPENRATE_BANKRUPT_LIST, Session, Response);

            if (!IsPostBack) 
            {
                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    m_playerId.Text = m_gen.m_playerId;
                    m_turret.Text = m_gen.m_turret;

                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_playerId = m_playerId.Text;
            param.m_param = m_turret.Text;

            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;

            OpRes res = user.doQuery(param, QueryType.queryTypeOperationPlayerOpenRateBankruptList);
            genTable(m_result, res, user, param);
        }

        //统计表
        private void genTable(Table table, OpRes res, GMUser user, ParamQuery param)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

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

            List<StatOpenRateBankruptListItem> qresult = 
                (List<StatOpenRateBankruptListItem>)user.getQueryResult(QueryType.queryTypeOperationPlayerOpenRateBankruptList);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.Attributes.CssStyle.Value = "background-color:#f1f1f1";
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;

                StatOpenRateBankruptListItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.getTurretName();
                m_content[f++] = item.m_colorValue.ToString();
                m_content[f++] = item.m_energyValue.ToString();
                m_content[f++] = item.m_dimensityValue.ToString();
                m_content[f++] = item.m_chipValue.ToString();
                m_content[f++] = item.m_torpedoValue.ToString();
                m_content[f++] = item.m_bankruptCount.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/operation/OperationPlayerOpenRateBankruptList.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}