﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatWjlwGoldBetPlayerList : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "玩家ID", "玩家昵称", "装填数量" };
        private string[] m_content = new string[s_head.Length];

        private PageGoldBetPlayer m_gen = new PageGoldBetPlayer(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = Request.QueryString["turnId"];
            param.m_time = Request.QueryString["time"];
            if (m_gen.parse(Request))
            {
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
            }
            genTable(m_result, param, user);
        }

        private void genTable(Table table, ParamQuery param, GMUser user)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeWjlwGoldBetPlayerList, user);

            table.GridLines = GridLines.Both;
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

            List<WjlwGoldBetPlayerItem> qresult = (List<WjlwGoldBetPlayerItem>)mgr.getQueryResult(QueryType.queryTypeWjlwGoldBetPlayerList);

            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = 4;
            td.Text = "局ID：" + param.m_param + "&ensp;&ensp;&ensp;&ensp;开始时间：" + param.m_time;
            int i = 0, j = 0, index = 0;
            // 表头
            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                index = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                WjlwGoldBetPlayerItem item = qresult[i];
                m_content[index++] = item.m_time;
                m_content[index++] = item.m_playerId == -1 ? "机器人" : item.m_playerId.ToString();
                m_content[index++] = item.m_nickName;
                m_content[index++] = item.m_equipCount.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatWjlwGoldBetPlayerList.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}