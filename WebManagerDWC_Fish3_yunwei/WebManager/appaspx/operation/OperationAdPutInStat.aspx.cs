using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationAdPutInStat : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","广告编号","渠道","注册人数","级别","级别描述","奖励RMB","人数","转化率"};
        private string[] m_content = new string[s_head.Length];

        private PageGenOnlineReward m_gen = new PageGenOnlineReward(2);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_STAT_AD_PUTIN, Session, Response);
            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("全部", "")); //渠道
                m_channel.Items.Add(new ListItem("闲玩", "100003"));
                //Dictionary<string, TdChannelInfo> cd = TdChannel.getInstance().getAllData();
                //foreach (var item in cd.Values)
                //{
                //    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                //}

                //if (m_gen.parse(Request))
                //{
                //    m_channel.SelectedValue = m_gen.m_channelID;
                //    m_time.Text = m_gen.m_time;
                //    onQuery(null, null);
                //}

                if (m_gen.parse(Request))
                {
                    m_channel.SelectedValue = m_gen.m_channelID;
                    m_time.Text = m_gen.m_time;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            ParamQuery param = new ParamQuery();
            param.m_channelNo = m_channel.SelectedValue;
            param.m_time = m_time.Text;

            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypeQueryChannel100003ActCount);

            TableChannel100003 view = new TableChannel100003();
            //Table table = new Table();
            view.genTable(user, m_result, res, null);

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/operation/OperationAdPutInStat.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        public void genTable(GMUser user, Table table, OpRes res, ParamQuery param)
        {
            string[] m_content = new string[s_head.Length];

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

            int i = 0, f = 0;
            for (; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.ColumnSpan = 1;
                td.RowSpan = 1;
            }

            List<StatOnlineRewardItem> qresult = (List<StatOnlineRewardItem>)user.getQueryResult(QueryType.queryTypeQueryChannel100003ActCount);
            foreach (var data in qresult)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                m_content[f++] = data.m_time;
                m_content[f++] = data.getChannelName();
                foreach (var count in data.m_recvCount)
                {
                    m_content[f++] = count.ToString();
                }

                for (int j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            //string page_html = "", foot_html = "";
            //m_gen.genPage(param, @"/appaspx/stat/StatOnlineReward.aspx", ref page_html, ref foot_html, user);
            //m_page.InnerHtml = page_html;
            //m_foot.InnerHtml = foot_html;

        }
    }
}