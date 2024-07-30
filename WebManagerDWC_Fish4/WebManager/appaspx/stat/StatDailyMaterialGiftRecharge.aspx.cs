using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatDailyMaterialGiftRecharge : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_DAILY_MATERIAL_GIFT, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            OpRes res = user.doQuery(param, QueryType.queryTypeMaterialGiftRecharge);
            genTable(m_result, res, user);
        }

        //查询表
        protected void genTable(Table table, OpRes res, GMUser user)
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

            ResultMaterialGiftRecharge qresult = (ResultMaterialGiftRecharge)user.getQueryResult(QueryType.queryTypeMaterialGiftRecharge);

            var fields = from f in qresult.m_fields orderby f ascending select f;

            // 生成行标题  表头
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "日期";

            // 生成行标题  表头
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "礼包名称";

            foreach (var reason in fields)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = reason+"钻石";
            }

            //内容
            for (int i = 0; i < qresult.m_result.Count(); i++)
            {
                MaterialGiftItem item = qresult.m_result[i];
                tr = new TableRow();
                m_result.Rows.Add(tr);
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time.ToShortDateString();

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = qresult.getName(item.m_giftId);

                // 生成这个结果
                foreach (var reason in fields)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_number.ContainsKey(reason))
                    {
                        td.Text = item.m_number[reason].ToString();
                    }
                    else
                    {
                        td.Text = "0";
                    }
                }
            }

        }
    }
}