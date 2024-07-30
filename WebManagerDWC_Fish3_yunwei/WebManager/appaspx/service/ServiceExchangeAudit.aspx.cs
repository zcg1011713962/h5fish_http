using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.service
{
    public partial class ServiceExchangeAudit : System.Web.UI.Page
    {
        private static string[] s_head = new string[] {"玩家ID", "历史充值金额","历史发放金额","手机号","绑定手机号", 
            "道具名称","数量", "领取时间","发放时间", "审核时间","审核状态", "审核ID","操作",""};
        private string[] m_content = new string[s_head.Length];
        private PageGift m_gen = new PageGift(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SVR_EXCHANGE_AUDIT, Session, Response);
            if (!IsPostBack)
            {
                m_filter.Items.Add("审核中");
                m_filter.Items.Add("已通过");
                m_filter.Items.Add("未通过");
                m_filter.Items.Add("已兑换");
                m_filter.Items.Add("全部");

                if (m_gen.parse(Request))
                {
                    m_filter.SelectedIndex = m_gen.m_state;
                    m_param.Text = m_gen.m_playerId;
                    onSearch(null, null);
                }
            }
        }

        protected void onSearch(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQueryGift param = new ParamQueryGift();
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_state = m_filter.SelectedIndex;
            param.m_param = m_param.Text;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeExchangeAudit, user);

            genGiftTable(GiftTable, res, param, user, mgr);
        }

        private void genGiftTable(Table table, OpRes res, ParamQueryGift param, GMUser user, QueryMgr mgr)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

            table.GridLines = GridLines.Both;
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

            List<ExchangeItem> qresult = (List<ExchangeItem>)mgr.getQueryResult(QueryType.queryTypeExchangeAudit);

            int i = 0, j = 0, f = 0;

            // 表头
            for (; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                if (i == s_head.Length - 2)
                {
                    td.ColumnSpan = 2;
                    i++;
                }
                else
                {
                    td.ColumnSpan = 1;
                }

                td.Attributes.CssStyle.Value = "vertical-align:middle";
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);
                
                m_content[f++] = qresult[i].m_playerId.ToString(); //玩家ID
                m_content[f++] = qresult[i].m_recharged.ToString(); //历史充值金额
                m_content[f++] = qresult[i].m_historyMoney.ToString(); //历史发放金额
                m_content[f++] = qresult[i].m_phone; //手机号
                m_content[f++] = qresult[i].m_bindPhone; //绑定手机号
                m_content[f++] = qresult[i].getChgItem()[0]; //道具名称
                m_content[f++] = qresult[i].getChgItem()[1]; //数量
                m_content[f++] = qresult[i].m_exchangeTime; //申请时间
                m_content[f++] = qresult[i].m_giveOutTime; //领取时间
                m_content[f++] = qresult[i].m_verifyTime; //审核时间
                m_content[f++] = qresult[i].getStateName(); //审核状态
                m_content[f++] = qresult[i].m_opName; //审核ID
                m_content[f++] = "";
                m_content[f++] = "";
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.ColumnSpan = 1;
                    td.RowSpan = 1;
                    td.Attributes.CssStyle.Value = "vertical-align:middle";

                    if(qresult[i].m_status != 0 || qresult[i].m_flag == 0) //审核中 0 才有通过
                        continue;

                    if (j == 12)
                    {
                        HtmlGenericControl alink = new HtmlGenericControl();
                        alink.TagName = "a";
                        alink.InnerText = "通过";
                        alink.Attributes.Add("class", "btn btn-primary btn-op");
                        alink.Attributes.Add("id", qresult[i].m_exchangeId);
                        alink.Attributes.Add("op", "2");
                        td.Controls.Add(alink);
                    }
                    else if(j == 13)
                    {
                        HtmlGenericControl alink = new HtmlGenericControl();
                        alink.TagName = "a";
                        alink.InnerText = "未通过";
                        alink.Attributes.Add("class", "btn btn-primary btn-op");
                        alink.Attributes.Add("id", qresult[i].m_exchangeId);
                        alink.Attributes.Add("op", "1");
                        td.Controls.Add(alink);
                    }
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/service/ServiceExchangeAudit.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

    }
}