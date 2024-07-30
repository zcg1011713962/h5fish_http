using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatSdActLottery : System.Web.UI.Page
    {

        private static string[] s_head = new string[] { "日期","设备","单次抽奖人数","10连抽人数","单次抽奖次数","10连抽次数","详情"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_SD_ACT_LOTTERY, Session, Response);
            if (!IsPostBack)
            {
                m_queryId.Items.Add("初级");
                m_queryId.Items.Add("高级");

                m_channel.Items.Add(new ListItem("全部", "")); //渠道

                Dictionary<string, TdChannelInfo> cd = TdChannel.getInstance().getAllData();
                foreach (var item in cd.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            TableTdActivation view = new TableTdActivation();
            ParamQuery param = new ParamQuery();
            param.m_op = m_queryId.SelectedIndex + 1;
            param.m_channelNo = m_channel.SelectedValue;
            param.m_time = m_time.Text;

            OpRes res = user.doQuery(param, QueryType.queryTypeStatSdLotteryAct);
            genTable(user, m_resTable, res, param);
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

            List<StatSdLotteryActItem> qresult = (List<StatSdLotteryActItem>)user.getQueryResult(QueryType.queryTypeStatSdLotteryAct);
            foreach (var data in qresult)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                m_content[f++] = data.m_time;
                m_content[f++] = data.getChannelName();
                foreach (var da in data.m_lottery)
                {
                    m_content[f++] = da.Value[0].ToString();
                }
                foreach (var da in data.m_lottery)
                {
                    m_content[f++] = da.Value[1].ToString();
                }

                m_content[f++] = data.getDetail(param.m_op);

                for (int j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}