using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPumpVipRecord : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "打开人数", "打开次数",
            "vip1玩家人数", "vip2玩家人数","vip3玩家人数", "vip4玩家人数","vip5玩家人数", "vip6玩家人数",
            "vip7玩家人数","vip8玩家人数","vip9玩家人数","vip10玩家人数","未领取人数"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_PUMP_VIP_GIFT, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatVipRecord, user);
            genTable(m_result, res, param, user, mgr);
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
            List<StatVipRecordItem> qresult = (List<StatVipRecordItem>)mgr.getQueryResult(QueryType.queryTypeStatVipRecord);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                StatVipRecordItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_openPerson.ToString();
                m_content[f++] = item.m_openCount.ToString();

                foreach (var da in item.m_vipLevel) 
                {
                    m_content[f++] = da.Value.ToString();
                }

                m_content[f++] = item.m_missCount.ToString();

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