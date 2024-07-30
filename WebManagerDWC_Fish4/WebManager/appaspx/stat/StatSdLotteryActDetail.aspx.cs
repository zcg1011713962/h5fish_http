using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatSdLotteryActDetail : System.Web.UI.Page
    {
        private static string[] s_head = new string[] {"玩家名称","玩家ID","对应奖励（次）"};

        private PageStatSdLotteryActDetail m_gen = new PageStatSdLotteryActDetail(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            ////////////////////////////////////////////////////////////////
            string type_id = Request.QueryString["lotteryId"];
            if (string.IsNullOrEmpty(type_id))
                return;

            int typeId = 0;
            if (!int.TryParse(type_id, out typeId))
                return;

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = typeId;
            param.m_time = Request.QueryString["time"];
            param.m_channelNo = Request.QueryString["channel"];
            if (m_gen.parse(Request))
            {
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
            }

            string[] s_Reward = new string[10];


            for (int i = 0; i < 10; i++) 
            {
                int index = (i + 1) + 10 * (param.m_op - 1);
                var lotteryReward = ActivityDrawLotteryCFG.getInstance().getValue(i+1);
                if (lotteryReward != null)
                {
                    var itemList = ItemCFG.getInstance().getValue(lotteryReward.m_itemId);
                    string itemName = "";
                    if (itemList != null)
                        itemName = itemList.m_itemName;

                    s_Reward[i] = itemName + "(" + lotteryReward.m_itemCount + ")";
                }
                else {
                    s_Reward[i] = "";
                }
            }
            
            //////////////////////////////////////////////////////////////
            genTable(m_result, param, user, s_Reward);
        }

        private void genTable(Table table, ParamQuery param, GMUser user, string[] s_reward)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

            string[] m_content = new string[s_head.Length + s_reward.Length - 1];

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatSdLotteryActDetail, user);

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

            List<StatSdLotteryActDetailItem> qresult = (List<StatSdLotteryActDetailItem>)mgr.getQueryResult(QueryType.queryTypeStatSdLotteryActDetail);

            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = m_content.Length;
            td.Text = "日期：" + param.m_time;
            td.ColumnSpan = 4;

            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = m_content.Length;
            td.Text = "  类型：" + ((param.m_op == 1) ? "初级抽奖" : "高级抽奖");
            td.ColumnSpan = 4;

            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = m_content.Length;
            td.Text = " 渠道：" + getChannelName(param.m_channelNo);
            td.ColumnSpan = 4;

            tr = new TableRow();
            table.Rows.Add(tr);
            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = m_content.Length;
            td.Text = "";
            td.ColumnSpan = 12;

            int i = 0, j = 0, index = 0;
            // 表头
            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                if (i == 2)
                {
                    td.RowSpan = 1;
                    td.ColumnSpan = 10;
                }
                else {
                    td.RowSpan = 2;
                    td.ColumnSpan = 1;
                }
                td.Attributes.CssStyle.Value = "vertical-align:middle";
            }

            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0; i < s_reward.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_reward[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }


            for (i = 0; i < qresult.Count; i++)
            {
                index = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                StatSdLotteryActDetailItem item = qresult[i];
                m_content[index++] = item.m_nickname;
                m_content[index++] = item.m_playerId.ToString();
                foreach(var da in item.m_reward)
                {
                    m_content[index++] = da.ToString();
                }

                for (j = 0; j < m_content.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatSdLotteryActDetail.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        public string getChannelName(string channel)
        {
            string channelName = channel;
            var cd = TdChannel.getInstance().getValue(channel);
            if (cd != null)
                channelName = cd.m_channelName;

            return channelName;
        }

    }
}