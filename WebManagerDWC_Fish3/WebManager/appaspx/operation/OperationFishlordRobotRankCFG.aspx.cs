using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationFishlordRobotRankCFG : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_FISHLORD_ROBOT_RANK_CFG, Session, Response);
            GMUser user = (GMUser)Session["user"];
            if (!IsPostBack) 
            {
                for (int i = 1; i <= 17; i++)
                {
                    m_type.Items.Add(getRankItemName(i));
                }

                genTable(user, null);
            }

            m_res.InnerHtml = "";
        }

        protected void onEdit(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            OpRes res = OpRes.op_res_failed;
            ParamQuery p = new ParamQuery();
            p.m_op = m_type.SelectedIndex + 1;
            p.m_param = m_param.Text;

            res = user.doDyop(p, DyOpType.opTypeOperationFishlordRobotRankCFG);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
            genTable(user, p);
        }

        private void genTable(GMUser user, ParamQuery param)
        {
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
           
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeOperationRobotRankCFG, user);
            List<OperationRobotRankCFGItem> qresult =
                (List<OperationRobotRankCFGItem>)mgr.getQueryResult(QueryType.queryTypeOperationRobotRankCFG);
           
            TableCell td = null;
            tr = new TableRow();
            m_result.Rows.Add(tr);
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "机器人积分管理";
            td.ColumnSpan = 2;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            for (int i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);

                OperationRobotRankCFGItem item = qresult[i];

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = getRankItemName(item.m_type);
                td.ColumnSpan = 1;
                td.RowSpan = 1;

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_rankPointMin.ToString() + ',' + item.m_rankPointMax.ToString();
            }
        }

        public string getRankItemName(int type) 
        {
            string name = type.ToString();
            switch(type)
            {
                case 1:
                    name = "中级场幸运榜";break;
                case 2:
                    name="中级场牛人榜";break;
                case 3:
                    name = "高级场排行榜";break;
                case 4:
                    name="武装场排行榜";break;
                case 5:
                    name="巨鲲场排行榜";break;
                case 6:
                    name="欢乐炸-普通";break;
                case 7:
                    name="欢乐炸-青铜";break;
                case 8:
                    name="欢乐炸-白银";break;
                case 9:
                    name="欢乐炸-黄金";break;
                case 10:
                    name="欢乐炸-钻石";break;
                case 11:
                    name="衰神炸-普通";break;
                case 12:
                    name="衰神炸-青铜";break;
                case 13:
                    name="衰神炸-白银";break;
                case 14:
                    name="衰神炸-黄金";break;
                case 15:
                    name="衰神炸-钻石";break;
                case 16:
                    name = "圣兽场排行榜";break;
                case 17:
                    name = "竞技场";break;
            }

            return name;
        }
    }
}