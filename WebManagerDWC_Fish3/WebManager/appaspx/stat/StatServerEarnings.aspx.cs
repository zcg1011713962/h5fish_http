using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatServerEarnings : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", 
                "初级场收入", "初级场产出", "盈利值", "盈利率" ,
                "中级场收入", "中级场产出", "盈利值", "盈利率" ,
                "高级场收入", "高级场产出", "盈利值", "盈利率" ,
                "碎片场收入", "碎片场产出", "盈利值", "盈利率",
                "巨鲨场收入","巨鲨场产出","盈利值","盈利率",
                "初级龙宫场收入","初级龙宫场产出","盈利值","盈利率",
                "高级龙宫场收入","高级龙宫场产出","盈利值","盈利率",
                "巨鲲场收入","巨鲲场产出","盈利值","盈利率",
                "圣兽场收入","圣兽场产出","盈利值","盈利率",
                "聚宝金龟场收入","聚宝金龟场产出","盈利值","盈利率",
                 "竞技场收入","竞技场场产出","盈利值","盈利率",
                "总收入", "总支出", "总盈利值", "总盈利率","活跃人数",
                "初级场废弹", "中级场废弹", "高级场废弹", "碎片场废弹", "巨鲨场废弹","初级龙宫场废弹","高级龙宫场废弹","巨鲲场废弹","圣兽场废弹","聚宝金龟场废弹", "竞技场废弹"};
        private string[] m_content = new string[s_head.Length];

      //  private int[] m_roomList = new int[] { 1, 2, 3, 4, 6, 5, 7, 8, 9, 10};
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_MONEY_FLOW, Session, Response);
        }

        protected void onStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            ParamServerEarnings param = new ParamServerEarnings();
            
            param.m_time = m_time.Text;
            OpRes res = mgr.doQuery(param, QueryType.queryTypeServerEarnings, user);
            m_result.Rows.Clear();
            //只有经典捕鱼
            genDetailGameTableFishlord(m_result, res, user, mgr, param.m_gameId);
        }

        ///////////////////////////////////经典捕鱼数据///////////////////////////////////////
        private void genDetailGameTableFishlord(Table table, OpRes res, GMUser user, QueryMgr mgr, int gameId)
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

            List<ServerEarningItem> qresult = (List<ServerEarningItem>)user.getQueryResult(QueryType.queryTypeServerEarnings);
            int i = 0, j = 0, k = 0;
            // 表头
            tr.CssClass = "alt";
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.Attributes.CssStyle.Value = "vertical-align:middle";
            }

            int len = qresult.Count;
            long roomIncome = 0, roomOutlay = 0;

            ServerEarningItem item_total = qresult[0];

            for (i = 1; i < len; i++)
            {
                k = 0;
                ServerEarningItem item = qresult[i];
                tr = new TableRow();
                tr.Cells.Clear();
                tr.CssClass = null;

                m_result.Rows.Add(tr);

                m_content[k++] = item.m_time;
                /////////////////////////////////////////初级场

                roomIncome = 0;
                roomOutlay = 0;

                foreach (var roomId in QueryServerEarningsNew.s_roomList) 
                {
                    m_content[k++] = string.Format("{0:N0}", item.m_incomeOutlay[roomId][0]);
                    m_content[k++] = string.Format("{0:N0}", item.m_incomeOutlay[roomId][1]);
                    m_content[k++] = string.Format("{0:N0}", item.m_incomeOutlay[roomId][0] - item.m_incomeOutlay[roomId][1]);
                    m_content[k++] = item.getEarnRate(item.m_incomeOutlay[roomId][0], item.m_incomeOutlay[roomId][1]);

                    roomIncome += item.m_incomeOutlay[roomId][0];
                    roomOutlay += item.m_incomeOutlay[roomId][1];
                }

                //总
                m_content[k++] = string.Format("{0:N0}", roomIncome);
                m_content[k++] = string.Format("{0:N0}", roomOutlay);
                m_content[k++] = string.Format("{0:N0}", roomIncome - roomOutlay);
                m_content[k++] = item.getEarnRate(roomIncome, roomOutlay);

                //活跃人数
                m_content[k++] = string.Format("{0:N0}", item.m_activeCount);

                //废弹
                foreach (var roomId in QueryServerEarningsNew.s_roomList) 
                {
                    m_content[k++] = string.Format("{0:N0}", item.m_incomeOutlay[roomId][2]);
                }

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Style.Clear();
                    td.Text = m_content[j];
                }
                for (k = 3; k < 44; k += 4)
                {
                    setRedColor(tr.Cells[k]);
                    setRedColor(tr.Cells[k + 1]);
                }
            }
            addDetailStatFootFishlord(table, item_total);
        }
        // 增加经典捕鱼统计页脚
        protected void addDetailStatFootFishlord(Table table, ServerEarningItem item)
        {
            TableRow tr = new TableRow();
            tr.Cells.Clear();
            table.Rows.Add(tr);
            TableCell td = null;

            tr.CssClass = "alt";
            int k = 0, j = 0;
            long roomIncome = 0, roomOutlay = 0;

            m_content[k++] = item.m_time;
            foreach (var roomId in QueryServerEarningsNew.s_roomList) 
            {
                m_content[k++] = string.Format("{0:N0}", item.m_incomeOutlay[roomId][0]);
                m_content[k++] = string.Format("{0:N0}", item.m_incomeOutlay[roomId][1]);
                m_content[k++] = string.Format("{0:N0}", item.m_incomeOutlay[roomId][0] - item.m_incomeOutlay[roomId][1]);
                m_content[k++] = item.getEarnRate(item.m_incomeOutlay[roomId][0], item.m_incomeOutlay[roomId][1]);

                roomIncome += item.m_incomeOutlay[roomId][0];
                roomOutlay += item.m_incomeOutlay[roomId][1];
            }

            //总
            m_content[k++] = string.Format("{0:N0}", roomIncome);
            m_content[k++] = string.Format("{0:N0}", roomOutlay);
            m_content[k++] = string.Format("{0:N0}", roomIncome - roomOutlay);
            m_content[k++] = item.getEarnRate(roomIncome, roomOutlay);

            //活跃人数
            m_content[k++] = string.Format("{0:N0}", item.m_activeCount);

            //废弹
            foreach (var roomId in QueryServerEarningsNew.s_roomList) 
            {
                m_content[k++] = string.Format("{0:N0}", item.m_incomeOutlay[roomId][2]);
            }

            for (j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Style.Clear();
                td.Text = m_content[j];
            }
            for (k = 3; k < 44; k += 4)
            {
                setRedColor(tr.Cells[k]);
                setRedColor(tr.Cells[k + 1]);
            }
        }

        protected void setRedColor(TableCell td)
        {
            td.Style.Clear();
            if (td.Text[0] == '-')
            {
                td.Style.Add("color", "red");
            }else {
                td.Style.Add("color", "green");
            }
        }
    }
}