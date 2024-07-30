using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatVipExclusive : System.Web.UI.Page
    {
        private static string[] s_head1 = new string[] { "日期","百万","千万1","千万2","千万3","千万4","千万5","亿万1",
            "亿万2","亿万3","亿万4","亿万5","富甲天下","支出金币"};
        private static string[] s_head2 = new string[] { "日期","购买1阶","购买2阶","购买3阶","购买4阶","购买5阶","购买6阶",
            "购买7阶","购买8阶","购买9阶","购买10阶","支出金币"};
        private static string[] s_head3 = new string[] { "日期","认证数量","支出金币"};
        private static string[] s_head4 = new string[] { "日期", "物品1购买次数", "物品2购买次数", "物品3购买次数", "物品4购买次数", 
            "物品5购买次数", "物品6购买次数", "物品7购买次数", "物品8购买次数", "物品9购买次数", "物品10购买次数", "消耗积分" };
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_VIP_EXCLUSIVE, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("富豪计划");
                m_queryType.Items.Add("VIP礼包");
                m_queryType.Items.Add("VIP认证");
                m_queryType.Items.Add("积分商城");
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_op = m_queryType.SelectedIndex;
            OpRes res = OpRes.op_res_failed;

            switch(param.m_op){
                case 0: //富豪计划
                    res = user.doQuery(param, QueryType.queryTypeStatVipExclusiveTask);
                    genTable(m_resTable, s_head1, res, user, QueryType.queryTypeStatVipExclusiveTask);
                    break;
                case 1: //VIP宝库
                    res = user.doQuery(param, QueryType.queryTypeStatVipExclusiveDiamondMall);
                    genTable(m_resTable, s_head2, res, user, QueryType.queryTypeStatVipExclusiveDiamondMall);
                    break;
                case 2: //VIP认证
                    res = user.doQuery(param, QueryType.queryTypeStatVipExclusiveBindPhone);
                    genTable(m_resTable, s_head3, res, user, QueryType.queryTypeStatVipExclusiveBindPhone);
                    break;
                case 3://积分商城
                    res = user.doQuery(param, QueryType.queryTypeStatVipExclusivePointExchange);
                    genTable(m_resTable, s_head4, res, user, QueryType.queryTypeStatVipExclusivePointExchange);
                    break;
            }
        }

        private void genTable(Table table,string[] s_head, OpRes res, GMUser user, QueryType queryTypeStatVipExclusive)
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

            List<StatVipExclusiveTaskItem> qresult =
                (List<StatVipExclusiveTaskItem>)user.getQueryResult(queryTypeStatVipExclusive);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                StatVipExclusiveTaskItem item = qresult[i];

                m_content[f++] = item.m_time;
                foreach (var da in item.m_task)
                {
                    m_content[f++] = da.ToString();
                }
                m_content[f++] = item.m_goldoutlay.ToString();
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