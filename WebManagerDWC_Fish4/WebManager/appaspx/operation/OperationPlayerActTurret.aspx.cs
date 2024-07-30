using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationPlayerActTurret : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "注册日期", "", "1炮", "5炮", "10炮", "20炮", "30炮", "40炮", "50炮", "60炮", "70炮", "80炮", "90炮", "100炮",
        "150炮","200炮","250炮","300炮","350炮","400炮","450炮","500炮","550炮","600炮","650炮","700炮","750炮","800炮","850炮","900炮","950炮","1000炮",
        "1500炮","2000炮","2500炮","3000炮","3500炮","4000炮","4500炮","5000炮","6000炮","7000炮","8000炮","9000炮","10000炮"};
        private string[] m_content = new string[s_head.Length];
        public string[] s_day = new string[] { "第一天", "第二天", "第三天", "第四天", "第五天", "第六天", "第七天" };

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_PLAYER_ACT_TURRET, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            OpRes res = user.doQuery(param, QueryType.queryTypeOperationPlayerActTurret);
            genTable(m_result, res, user, param);
        }

        //统计表
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

            List<PlayerActTurretItem> qresult = (List<PlayerActTurretItem>)user.getQueryResult(QueryType.queryTypeOperationPlayerActTurret);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.Attributes.CssStyle.Value = "background-color:#f1f1f1";
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                PlayerActTurretItem item = qresult[i];

                foreach (var da in item.m_data)
                {
                    tr = new TableRow();
                    table.Rows.Add(tr);

                    if (da.Key == 1) //时间
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_time;
                        td.ColumnSpan = 1;
                        td.RowSpan = 7;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = s_day[da.Key-1];
                    td.ColumnSpan = 1;
                    td.RowSpan = 1;
                    td.Attributes.CssStyle.Value = "min-width:60px;";

                    foreach (var turret in da.Value)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = turret.ToString();
                        td.Attributes.CssStyle.Value = "min-width:70px;";
                    }
                }
            }
        }
    }
}