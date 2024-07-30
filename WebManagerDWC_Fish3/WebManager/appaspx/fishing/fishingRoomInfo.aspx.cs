using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.fishing
{
    public partial class fishingRoomInfo : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","场次","人数","人均时长","活跃玩家参与率","破产率"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISHING_ROOM_INFO, Session, Response);

            if (!IsPostBack)   //客户端回发而加载
            {
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
            param.m_time = m_time.Text;
            param.m_op = Convert.ToInt32(m_roomId.SelectedValue);
            OpRes res = user.doQuery(param, QueryType.queryTypeStatFishingRoomInfo);
            genTable(m_resTable, res, user, param.m_param);
        }

        //生成表
        private void genTable(Table table, OpRes res, GMUser user, string playerList)
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

            List<fishingRoomItem> qresult = (List<fishingRoomItem>)user.getQueryResult(QueryType.queryTypeStatFishingRoomInfo);

            int i = 0, f = 0;
            // 表头 第一行
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.ColumnSpan = 1;
                td.RowSpan = 1;
            }

            foreach (var data in qresult)
            {
                f = 0;

                foreach (var time in data.m_avgTime) 
                {
                    tr = new TableRow();
                    table.Rows.Add(tr);

                    if (f == 0)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = data.m_time;
                        td.RowSpan = data.m_avgTime.Count;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = data.getRoomName(time.Key);
                    td.RowSpan = 1;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = data.m_personCount[time.Key].ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = data.transTimeTohms(time.Value);

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = data.getRate(data.m_personCount[time.Key], data.m_activeCount);

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = data.getRate(data.m_brokenCount[time.Key], data.m_personCount[time.Key]);

                    f++;
                }
            }
        }
    }
}