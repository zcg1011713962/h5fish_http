using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatYuanDanAct : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "签到1天人数","签到2天人数","签到3天人数","签到4天人数",
            "签到5天人数","签到6天人数","签到7天人数","签到8天人数","签到9天人数","签到10天人数","连续签到人数"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_YUAN_DAN_SIGN, Session, Response);
            OnStat(null, null);
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = OpRes.op_res_failed;

            res = mgr.doQuery(param, QueryType.queryTypeStatYuandanSign, user);
            genTable0(m_resTable, res, user, param);
        }

        //签到
        private void genTable0(Table table, OpRes res, GMUser user, ParamQuery param)
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

            StatYuandanSignItem qresult =
                (StatYuandanSignItem)user.getQueryResult(QueryType.queryTypeStatYuandanSign);

            // 表头
            for (int i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            tr = new TableRow();
            table.Rows.Add(tr);

            foreach (var da in qresult.m_sign) 
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = da.Value.ToString();
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = qresult.m_signCount.ToString();
            td.RowSpan = 1;
            td.ColumnSpan = 1;
        }
    }
}