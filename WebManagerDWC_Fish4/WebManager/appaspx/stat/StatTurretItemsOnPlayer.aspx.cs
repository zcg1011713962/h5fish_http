using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatTurretItemsOnPlayer : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "炮倍", "锁定", "冰冻", "狂暴", "召唤", "使用锁定", "使用冰冻", "使用狂暴", "使用召唤", 
            "升级石", "明日buff","黑桃","红心","梅花","方块","小丑帽","普通鱼雷","青铜鱼雷","白银鱼雷","黄金鱼雷","钻石鱼雷"};
        private string[] m_content = new string[s_head.Length];

        private PageGenPlayerTurretAct m_gen = new PageGenPlayerTurretAct(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_STAT_TURRET_ITEMS_ON_PLAYER, Session, Response);
            if (!IsPostBack)
            {
                m_type.Items.Add("活跃玩家");
                m_type.Items.Add("新增玩家");

                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    m_turret.Text = m_gen.m_param;
                    m_type.SelectedIndex = m_gen.m_days;

                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_op = Convert.ToInt32(m_type.SelectedIndex); 
            param.m_param = m_turret.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypeStatTurretItemsOnPlayer);
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

            List<StatTurretItemsOnPlayerItem> qresult =
                (List<StatTurretItemsOnPlayerItem>)user.getQueryResult(QueryType.queryTypeStatTurretItemsOnPlayer);

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
                StatTurretItemsOnPlayerItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.getOpenRate();

                foreach (var da in item.m_items) 
                {
                    m_content[f++] = da.ToString();
                }

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatTurretItemsOnPlayer.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}