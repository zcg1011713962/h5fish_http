using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpFish : System.Web.UI.Page
    {
        //骰子鱼
        private static string[] s_head1 = new string[] { "日期","系统收入","系统支出","场次","鱼死亡次数","翻倍0次","翻倍1次","翻倍2次","翻倍3次"};
        //贝壳鱼
        private static string[] s_head2 = new string[] { "日期", "系统收入", "系统支出", "场次", "鱼死亡次数", "1个贝壳", "2个贝壳", "3个贝壳", "4个贝壳", 
            "5个贝壳", "6个贝壳", "7个贝壳", "8个贝壳", "9个贝壳","10个贝壳","11个贝壳","12个贝壳" };

        //钻头虾、炸弹蟹
        private static string[] s_head3 = new string[] { "日期","系统收入","系统支出","场次","鱼死亡次数"};

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PUMP_FISH, Session, Response);
            if (!IsPostBack)
            {
                m_item.Items.Add(new ListItem("骰子鱼", "309"));
                m_item.Items.Add(new ListItem("钻头虾", "306"));
                m_item.Items.Add(new ListItem("贝壳鱼", "308"));
                m_item.Items.Add(new ListItem("炸弹蟹", "307"));

                //场次
                m_roomId.Items.Add(new ListItem("全部", "-1"));
                foreach (var room in StrName.s_roomList)
                {
                    m_roomId.Items.Add(new ListItem(room.Value, room.Key.ToString()));
                }
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = Convert.ToInt32(m_item.SelectedValue);
            param.m_time = m_time.Text;
            param.m_type = Convert.ToInt32(m_roomId.SelectedValue);
            OpRes res = OpRes.op_res_failed;
            switch (param.m_op)
            {
                case 309: //骰子鱼
                    res = user.doQuery(param, QueryType.queryTypeStatDiceGame);
                    genTable(m_result, res, user, param, s_head1, QueryType.queryTypeStatDiceGame);
                    break;
                case 306: //钻头虾
                case 307: //炸弹蟹
                    res = user.doQuery(param, QueryType.queryTypeStatXiaXieGame);
                    genTable(m_result, res, user, param, s_head3, QueryType.queryTypeStatXiaXieGame);
                    break;
                case 308: //贝壳鱼
                    res = user.doQuery(param, QueryType.queryTypeStatShellGame);
                    genTable(m_result, res, user, param, s_head2, QueryType.queryTypeStatShellGame);
                    break;
            }
        }

        private void genTable(Table table, OpRes res, GMUser user, ParamQuery query_param, string[] s_head, QueryType queryType)
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

            List<StatFishItem> qresult = (List<StatFishItem>)user.getQueryResult(queryType);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                StatFishItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_income.ToString();
                m_content[f++] = item.m_outlay.ToString();
                m_content[f++] = item.getRoomName(query_param.m_type);
                m_content[f++] = item.m_deadCount.ToString();

                //为空就是钻头虾或者炸弹蟹
                if (item.m_winList.Count != 0)
                {
                    foreach (var da in item.m_winList)
                    {
                        m_content[f++] = da.ToString();
                    }
                }

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }
    }
}