using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.fish
{
    public partial class StatFishlordConsume : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_CONSUME, Session, Response);

            if (!IsPostBack)
            {
                //moneyType.Items.Add("金币");
                //moneyType.Items.Add("钻石");
                moneyType.Items.Add("物品");
            }
        }

        protected void genTable(Table table, GMUser user)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            ParamTotalConsume param = new ParamTotalConsume();
            param.m_time = m_time.Text;
            param.m_currencyType = moneyType.SelectedIndex + 1;
            OpRes res = user.doQuery(param, QueryType.queryTypeFishConsume);
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            ResultTotalConsume qresult = (ResultTotalConsume)user.getQueryResult(QueryType.queryTypeFishConsume);

            var fields = from f in qresult.m_fields orderby f ascending select f;

            int i = 0, k = 0;
            
           
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "";
            td.ColumnSpan = 1;
            td.RowSpan = 1;

            // 生成行标题
            foreach (var reason in fields)
            {
                tr = new TableRow();
                table.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = qresult.getFishReason(reason);
                td.ColumnSpan = 1;
                td.RowSpan = 1;
            }

            for (i = 0; i < qresult.getResultCount(); i++)
            {
                TotalConsumeItem item = qresult.m_result[i];

                td = new TableCell();

                k = 0;
                tr = table.Rows[k];
                tr.Cells.Add(td);
                td.Text = item.m_time.ToShortDateString();
                td.ColumnSpan = 1;
                td.RowSpan = 1;

                // 生成这个结果
                foreach (var reason in fields)
                {
                    k++;
                    tr = table.Rows[k];

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.ColumnSpan = 1;
                    td.RowSpan = 1;
                    ConsumeOneItem citem = item.getValue(reason);
                    if (citem != null)
                    {
                        td.Text = citem.m_totalValue.ToString();
                    }
                    else
                    {
                        td.Text = "";
                    }
                }
            }
        }

        protected void btnStat_Click(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            int type =  2;
            if (type == 2)
            {
                ParamTotalConsume param = new ParamTotalConsume();
                param.m_time = m_time.Text;
                param.m_currencyType = type + 1;
                param.m_roomType = "0";
                OpRes res = user.doQuery(param, QueryType.queryTypeFishConsume);
                TableFishConsumeItem view = new TableFishConsumeItem();
                view.genTable(user, tabResult, res,param.m_roomType);
            }
            else
            {
                genTable(tabResult, user);
            }
        }
    }
}