﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatWjlwGoldEarn : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "当日总收入", "当日总支出","盈利率","详情"};
        private string[] m_content = new string[s_head.Length];
        private PagePanicBoxPlayer m_gen = new PagePanicBoxPlayer(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            m_res.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_WJLW_GOLD_EARN, Session, Response);
            if (!IsPostBack) 
            {
                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypeWjlwGoldEarn);
            genTable(m_result, res, user, param);
        }

        protected void onConfirm(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = m_expRate.Text;
            OpRes res = user.doDyop(param, DyOpType.opTypeWjlwGoldEditCurrExpRate);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);

            if (!string.IsNullOrEmpty(m_time.Text))
                onQuery(null, null);
        }

        //金币玩法统计表
        private void genTable(Table table, OpRes res, GMUser user,ParamQuery param)
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

            List<WjlwGoldEarnStatItem> qresult = (List<WjlwGoldEarnStatItem>)user.getQueryResult(QueryType.queryTypeWjlwGoldEarn);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                WjlwGoldEarnStatItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_income.ToString();
                m_content[f++] = item.m_outlay.ToString();
                m_content[f++] = item.getEarnRate();
                m_content[f++] = item.getDetail();
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
                if (item.m_expRate != -1)
                    m_expRate.Text = item.m_expRate.ToString();
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatWjlwGoldEarn.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}