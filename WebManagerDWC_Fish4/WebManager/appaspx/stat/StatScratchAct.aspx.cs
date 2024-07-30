using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatScratchAct : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "场次", "参与次数", "支付金币", "兑换金币", "金币盈利（支付-兑换）" ,
            "支付钻石","兑换钻石","钻石盈利（支付-兑换）"};
        private string[] m_content = new string[s_head.Length];

        private static string[] s_head1 = new string[] { "场次","奖项ID","奖项","中奖次数","产出金币","产出钻石"};
        private string[] m_content1=new string[s_head1.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_SCRATCH_ACT, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("运营状况");
                m_queryType.Items.Add("兑奖状况");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_op = m_queryType.SelectedIndex;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res=OpRes.op_res_failed;
            switch (param.m_op) 
            {
                case 0: res = mgr.doQuery(param, QueryType.queryTypeScratchActEarn, user);
                        genTable(m_result, res, param, user, mgr);
                        break;
                case 1: res = mgr.doQuery(param, QueryType.queryTypeScratchActExchangeRes, user);
                        genTable1(m_result, res, param, user, mgr);
                        break;
            }
        }

        //生成查询表
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            List<ScratchActEarnItem> qresult = (List<ScratchActEarnItem>)mgr.getQueryResult(QueryType.queryTypeScratchActEarn);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            int totalJoinCount = 0;
            long totalPayGold = 0, totalExchangeGold = 0, totalPayGem = 0, totalExchangeGem = 0;

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                ScratchActEarnItem item = qresult[i];
                m_content[f++] = getChangCiName(item.m_changCiId);
                m_content[f++] = item.m_joinCount.ToString();
                m_content[f++] = item.m_payGold.ToString();
                m_content[f++] = item.m_exchangeGold.ToString();
                m_content[f++] = (item.m_payGold - item.m_exchangeGold).ToString();

                m_content[f++] = item.m_payGem.ToString();
                m_content[f++] = item.m_exchangeGem.ToString();
                m_content[f++] = (item.m_payGem - item.m_exchangeGem).ToString();

                totalJoinCount += item.m_joinCount;
                totalPayGold += item.m_payGold;
                totalExchangeGold += item.m_exchangeGold;
                totalPayGem += item.m_payGem;
                totalExchangeGem += item.m_exchangeGem;

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }

            tr = new TableRow();
            table.Rows.Add(tr);
            f = 0;
            m_content[f++] = "累计";
            m_content[f++] = totalJoinCount.ToString();
            m_content[f++] = totalPayGold.ToString();
            m_content[f++] = totalExchangeGold.ToString();
            m_content[f++] = (totalPayGold - totalExchangeGold).ToString();
            m_content[f++] = totalPayGem.ToString();
            m_content[f++] = totalExchangeGem.ToString();
            m_content[f++] = (totalPayGem - totalExchangeGem).ToString();
            for (j = 0; j < s_head.Length; j++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }


        }

        private void genTable1(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            List<ScratchActItem> qresult =
                (List<ScratchActItem>)mgr.getQueryResult(QueryType.queryTypeScratchActExchangeRes);
            int  f = 0,k = 0, len = 0 ;
            // 表头
            for (int i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
            }

            for (int i = 0; i < qresult.Count; i++)
            {
                f = k = 0;
                tr = new TableRow();
                table.Rows.Add(tr);
                ScratchActItem it = qresult[i];
                //按照键升序
                Dictionary<int, ScratchActExchangeResItem> data = it.m_data.OrderBy(o => o.Key).ToDictionary(o=>o.Key,o=>o.Value);
                foreach(var d in data.Values)
                {
                    tr = new TableRow();
                    table.Rows.Add(tr);
                    f = 0;
                    if (k == 0)
                    {
                        m_content1[f++] = getChangCiName(it.m_changCiId);
                        len = s_head1.Length;
                    }
                    else 
                    {
                        len = s_head1.Length - 1;
                    }
                    m_content1[f++] = d.m_rewardId.ToString();
                    m_content1[f++] = getRewardName(d.m_rewardId);
                    m_content1[f++] = d.m_winCount.ToString();
                    m_content1[f++] = d.m_exchangeGold.ToString();
                    m_content1[f++] = d.m_exchangeGem.ToString();

                    for (int j = 0; j < len; j++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = m_content1[j];
                        if (j == 0 && k==0)
                        {
                            td.RowSpan = it.m_data.Count;
                        }
                        else
                        {
                            td.RowSpan = 1;
                        }
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }
                    k++;
                }
            }
        }

        public string getChangCiName(int key) 
        {
            string changCName = "";
            switch(key)
            {
                case 1: changCName = "普通场"; break;
                case 2: changCName = "黄金场"; break;
                case 3: changCName = "钻石场"; break;
                default: changCName = key.ToString(); break;
            }
            return changCName;
        }

        public string getRewardName(int key) 
        {
            string m_rewardName=key.ToString();

            ScratchCFGData data = ScratchTicketCFG.getInstance().getValue(key);
            if(data!=null)
            {
                if (data.m_randType == 2)
                {
                    m_rewardName = "随机金币";
                }
                else 
                {
                    switch(data.m_itemId)
                    {
                        case 1:m_rewardName = data.m_itemCount + "金币";break;
                        case 2: m_rewardName = data.m_itemCount + "钻石"; break;
                    }
                }
            }
            return m_rewardName;
        }
    }
}