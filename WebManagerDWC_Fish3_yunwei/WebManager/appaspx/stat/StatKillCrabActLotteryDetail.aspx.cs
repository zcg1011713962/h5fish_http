using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatKillCrabActLotteryDetail : System.Web.UI.Page
    {
        private PageJiuQiuNationalActPlayer m_gen = new PageJiuQiuNationalActPlayer(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            ////////////////////////////////////////////////////////////////
            string lottery_id = Request.QueryString["lotteryId"];
            if (string.IsNullOrEmpty(lottery_id))
                return;

            int lotteryId = 0;
            if (!int.TryParse(lottery_id, out lotteryId))
                return;

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = lotteryId;
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

            int s_head = 0;
            if (param.m_op == 1)
                s_head = 6;
            else
                s_head = 7;

            string[] m_content = new string[s_head + 1];

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatKillCrabActLotteryDetail, user);

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

            List<KillCrabActLotteryDetail> qresult = 
                (List<KillCrabActLotteryDetail>)mgr.getQueryResult(QueryType.queryTypeStatKillCrabActLotteryDetail);

            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = s_head + 1;
            td.Text = "日期：" + param.m_time + "&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;抽奖类型：&ensp;" + getLotteryTypeName(param.m_op);
            int i = 0, j = 0, index = 0;
            // 表头
            tr = new TableRow();
            table.Rows.Add(tr);

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "玩家ID";
            td.ColumnSpan = 1;

            for (i = 0; i < s_head; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = getLotteryAwardDetail(param.m_op, i);
            }

            for (i = 0; i < qresult.Count; i++)
            {
                index = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                KillCrabActLotteryDetail item = qresult[i];
                m_content[index++] = item.m_playerId;

                for (int k = 0; k < s_head; k++)
                {
                    if (item.m_reward.ContainsKey(k))
                    {
                        m_content[index++] = item.m_reward[k].ToString();
                    }
                    else
                    {
                        m_content[index++] = "";
                    }
                }

                for (j = 0; j < s_head + 1; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatKillCrabActLotteryDetail.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //抽奖类型
        public string getLotteryTypeName(int key)
        {
            string typeName = "";
            switch (key)
            {
                case 1: typeName = "100宝剑抽奖"; break;
                case 2: typeName = "1000宝剑抽奖"; break;
            }
            return typeName;
        }

        public string getLotteryAwardDetail(int type,int index)
        {
            PanicBoxInfo awardInfo = FishActKillCrabLotteryCFG.getInstance().getValue(type);
            if (awardInfo != null)
            {
                var arr_awardItem = awardInfo.m_awardsItemIds.Split(',');
                int itemId = Convert.ToInt32(arr_awardItem[index]);
                ItemCFGData itemCfg = ItemCFG.getInstance().getValue(itemId);
                string itemName = "";
                if (itemCfg != null)
                    itemName = itemCfg.m_itemName;

                var arr_awardCount = awardInfo.m_awardsItemCounts.Split(',');
                int itemCount = Convert.ToInt32(arr_awardCount[index]);

                return itemName + "(" + itemCount + ")";
            }
            return "";
        }
    }
}