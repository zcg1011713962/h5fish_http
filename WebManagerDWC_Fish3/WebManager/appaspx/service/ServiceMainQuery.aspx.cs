using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.service
{
    public partial class ServiceMainQuery : System.Web.UI.Page
    {
        private static string[] s_head = new string[] {"玩家ID","日期", "邮件名称","邮件内容","邮件附件","邮件状态（未领取/已领取）"};
        private string[] m_content = new string[s_head.Length];

        private static string[] s_head_1 = new string[] { "玩家ID","日期","排行类别","名次"};
        private string[] m_content_1 = new string[s_head_1.Length];

        private PageGenMail m_gen = new PageGenMail(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.SVR_MAIL_QUERY, Session, Response);
            if (!IsPostBack)
            {
                m_queryWay.Items.Add("通过玩家id查询");
                m_queryWay.Items.Add("通过账号查询");
                m_queryWay.Items.Add("通过昵称查询");

                m_isDel.Items.Add("未删除");
                m_isDel.Items.Add("已删除");
                m_isDel.Items.Add("已领取");
                m_isDel.Items.Add("竞技场领取奖励发送失败");//竞技场邮件发送失败

                if (m_gen.parse(Request))
                {
                    m_isDel.SelectedIndex = m_gen.m_isDel;
                    m_param.Text = m_gen.m_param;
                    m_queryWay.SelectedIndex = m_gen.m_way;
                    m_time.Text = m_gen.m_time;
                    onQueryMail(null, null);
                }
            }
        }

        protected void onQueryMail(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_way = (QueryWay)m_queryWay.SelectedIndex;
            param.m_param = m_param.Text;
            param.m_op = m_isDel.SelectedIndex;//0未删除 1已删除 2已领取 3发送失败
            param.m_time = m_time.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeMailQuery, user);
            if (param.m_op == 3) //发送失败
            {
                genTable1(m_result, res, param, user, mgr);
            }
            else 
            {
                genTable(m_result, res, param, user, mgr);
            }
        }

        private void genTable(Table table, OpRes res, ParamQuery param, GMUser user, QueryMgr mgr)
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

            List<mainQuery> qresult = (List<mainQuery>)mgr.getQueryResult(QueryType.queryTypeMailQuery);
            int i = 0, j = 0,f = 0 ;
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
                m_content[f++] = qresult[i].m_playerId;
                m_content[f++] = qresult[i].m_time;
                m_content[f++] = qresult[i].m_title;
                m_content[f++] = qresult[i].m_content;
                m_content[f++] = getRewardList(qresult[i].m_rewardList);//邮件附件
                m_content[f++] = getMailState(qresult[i].m_isReceive);

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.Attributes.CssStyle.Value = "vertical-align:middle";
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/service/ServiceMainQuery.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //发送失败
        private void genTable1(Table table, OpRes res, ParamQuery param, GMUser user, QueryMgr mgr)
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

            List<mailSendFail> qresult = (List<mailSendFail>)mgr.getQueryResult(param , QueryType.queryTypeMailQuery, user);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head_1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                m_content_1[f++] = qresult[i].m_playerId;
                m_content_1[f++] = qresult[i].m_time;
                m_content_1[f++] = qresult[i].m_rankType;
                m_content_1[f++] = qresult[i].m_rank.ToString();

                for (j = 0; j < s_head_1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_1[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/service/ServiceMainQuery.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
        
        //邮件附件
        public string getRewardList(List<mailGiftItem> rewardList)
        {
            string result = "";
            string name = "";
            for (int i = 0; i < rewardList.Count; i++)
            {
                ItemCFGData data = ItemCFG.getInstance().getValue(rewardList[i].m_giftId);
                if (data != null)
                {
                    name = data.m_itemName;
                }
                else
                {
                    name = "";
                }
                result += string.Format("id：{0}， name：{1}， count：{2} ， receive：{3}", 
                    rewardList[i].m_giftId, name, rewardList[i].m_count,getMailState(rewardList[i].m_receive));
                result += "<br />";
            }
            return result;
        }
        //是否领取
        public string getMailState(bool isReceive) 
        {
            string stateStr = "";
            if (isReceive)
            {
                stateStr = "已领取";
            }
            else 
            {
                stateStr = "未领取";
            }
            return stateStr;
        }
    }
}