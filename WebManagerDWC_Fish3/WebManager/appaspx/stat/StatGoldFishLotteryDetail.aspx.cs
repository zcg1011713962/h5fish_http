﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatGoldFishLotteryDetail : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID", "抽奖类型", "抽到奖励1", "抽到奖励2", "抽到奖励3", "抽到奖励4",
            "抽到奖励5", "抽到奖励6"};
        private string[] m_content = new string[s_head.Length];

        private PagePanicBoxPlayer m_gen = new PagePanicBoxPlayer(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            ////////////////////////////////////////////////////////////////
            string gear_id = Request.QueryString["boxId"];
            if (string.IsNullOrEmpty(gear_id))
                return;

            int gearId = 0;
            if (!int.TryParse(gear_id, out gearId))
                return;

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = gearId;
            param.m_time = Request.QueryString["time"];
            if (m_gen.parse(Request))
            {
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
            }
            //////////////////////////////////////////////////////////////
            genTable(m_result, param, user);
        }

        private void genTable(Table table, ParamQuery param, GMUser user)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatGoldFishLotteryDetail, user);

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

            List<StatGoldFishLotteryDetailItem> qresult = 
                (List<StatGoldFishLotteryDetailItem>)mgr.getQueryResult(QueryType.queryTypeStatGoldFishLotteryDetail);

            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = 8;
            td.Text = param.m_time;
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

                StatGoldFishLotteryDetailItem item = qresult[i];
                m_content[index++] = item.m_playerId.ToString();
                m_content[index++] = item.getGearName();
                m_content[index++] = item.getReward(1);
                m_content[index++] = item.getReward(2);
                m_content[index++] = item.getReward(3);
                m_content[index++] = item.getReward(4);
                m_content[index++] = item.getReward(5);
                m_content[index++] = item.getReward(6);
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatGoldFishLotteryDetail.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}