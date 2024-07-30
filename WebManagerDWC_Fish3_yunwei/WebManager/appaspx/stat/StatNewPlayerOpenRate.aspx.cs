using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatNewPlayerOpenRate : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","渠道","注册人数",
            "达到1倍人数","达到5倍人数","达到10倍人数","达到20倍人数","达到30倍人数",
            "达到40倍人数","达到50倍人数","达到60倍人数","达到70倍人数","达到80倍人数",
            "达到90倍人数","达到100倍人数", "达到150倍人数","达到200倍人数","达到250倍人数",
            "达到300倍人数","达到300+倍人数"};

        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_NEW_PLAYER_OPENRATE, Session, Response);
            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("总体", ""));
                m_channel.Items.Add(new ListItem("全渠道", "-1"));
                Dictionary<string, TdChannelInfo> data = TdChannel.getInstance().getAllData();
                foreach (var item in data.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_channelNo = m_channel.SelectedValue;
            param.m_time = m_time.Text;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = OpRes.op_res_failed;

            res = mgr.doQuery(param, QueryType.queryTypeStatNewPlayerOpenRate, user);
            genTable(m_result, res, user, param);
        }


        private void genTable(Table table, OpRes res, GMUser user, ParamQuery param)
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

            List<StatNewPlayerOpenRateItem> qresult =
                (List<StatNewPlayerOpenRateItem>)user.getQueryResult(QueryType.queryTypeStatNewPlayerOpenRate);

            int i = 0, j = 0, f = 0;

            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);
                StatNewPlayerOpenRateItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.getChannelName(item.m_channelNo);
                m_content[f++] = item.m_regeditCount.ToString();
                foreach(var da in item.m_openRateData)
                {
                    m_content[f++] = da.Value.ToString();
                }

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}