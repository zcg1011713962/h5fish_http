using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationPlayerActTurretBySingle : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","注册时间","渠道","玩家ID","当日炮倍","上线天数","当日游戏时间","当日金币","总充值金额","次留","三留","七留"};
        private string[] m_content = new string[s_head.Length];

        private PageGenPlayerTurretAct m_gen = new PageGenPlayerTurretAct(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_PLAYER_ACT_TURRET_BY_SINGLE, Session, Response);
            if (!IsPostBack) 
            {
                m_days.Items.Add(new ListItem("全部7天", "-1"));
                for (int i = 1; i <= 7; i++)
                {
                    m_days.Items.Add(new ListItem("第"+ i +"天",i.ToString()));
                }

                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    m_days.SelectedValue = m_gen.m_days.ToString();
                    m_turret.Text = m_gen.m_param;

                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_op = Convert.ToInt32(m_days.SelectedValue);
            param.m_param = m_turret.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypeOperationPlayerActTurretBySingle);
            genTable(m_result, res, user, param);
        }

        //导出Excel
        protected void onExport(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_op = Convert.ToInt32(m_days.SelectedValue);
            param.m_param = m_turret.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;

            ExportMgr mgr = user.getSys<ExportMgr>(SysType.sysTypeExport);
            OpRes res = mgr.doExport(param, ExportType.exportTypeOperationPlayerActTurretBySingle, user);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
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

            List<PlayerActTurretBySingleItem> qresult = (List<PlayerActTurretBySingleItem>)user.getQueryResult(QueryType.queryTypeOperationPlayerActTurretBySingle);

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
                PlayerActTurretBySingleItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_createTime;
                m_content[f++] = item.getChannelName();
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.getTurretName();
                m_content[f++] = item.m_loginDays.ToString();
                m_content[f++] = item.transTimeTohms(item.m_playTime);
                m_content[f++] = item.m_gold.ToString();

                m_content[f++] = item.m_totalRecharged.ToString();
                m_content[f++] = item.m_remain2.ToString();
                m_content[f++] = item.m_remain3.ToString();
                m_content[f++] = item.m_remain7.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/operation/OperationPlayerActTurretBySingle.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;

        }
    }
}