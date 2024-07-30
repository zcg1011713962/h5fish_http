using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.service
{
    public partial class ServiceExchange : RefreshPageBase
    {
        private static string[] s_head = new string[] { "选择", "玩家ID", "渠道", "历史充值金额","历史发放金额","手机号","绑定手机号", 
            "兑换ID", "道具名称","数量", "领取时间", "成功时间", "兑换状态" };
        private string[] m_content = new string[s_head.Length];

        // 所选择的checkbox
        private string m_selectStr = "";
        private PageExchange m_gen = new PageExchange(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SVR_EXCHANGE_MGR, Session, Response);
            if (IsPostBack)
            {
                m_selectStr = Request["aaa"];
                if (m_selectStr == null)
                    m_selectStr = "";

                return;
            }
            else
            {
                //渠道
                m_channel.Items.Add(new ListItem("总体", ""));
                m_channel.Items.Add(new ListItem("安卓总体", "-2"));
                Dictionary<string, TdChannelInfo> data = TdChannel.getInstance().getAllData();
                foreach (var item in data.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }

                m_type.Items.Add("实物"); //话费券和京东卡
                m_type.Items.Add("其他");

                m_filter.Items.Add("已兑现");
                m_filter.Items.Add("未兑现");
                m_filter.Items.Add("全部");

                if (m_gen.parse(Request))
                {
                    m_filter.SelectedIndex = m_gen.m_state;
                    m_type.SelectedIndex = m_gen.m_type;
                    m_playerId.Text = m_gen.m_playerId;
                    m_channel.SelectedValue = m_gen.m_channel;
                    m_phone.Text = m_gen.m_bindPhone;
                    onSearch(null, null);
                }
            }
        }

        protected void onActivateGift(object sender, EventArgs e)
        {
            if (IsRefreshed)
            {
                onSearch(null, null);
                return;
            }

            if (m_selectStr != "")
            {
                GMUser user = (GMUser)Session["user"];
                DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
                OpRes res = mgr.doDyop(m_selectStr, DyOpType.opTypeExchange, user);
                onSearch(null, null);
            }
        }

        protected void onSearch(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQueryGift param = new ParamQueryGift();
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_state = m_filter.SelectedIndex; //兑换状态
            param.m_type = m_type.SelectedIndex; //兑换类型

            param.m_playerId = m_playerId.Text;//玩家id 
            param.m_param = m_phone.Text;//电话
            param.m_channelNo = m_channel.SelectedValue;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeExchange, user);

            genGiftTable(GiftTable, res, param, user, mgr);

            if (param.m_state == (int)ExState.wait)  //1
            {
                m_btnActive.Visible = true;
            }else
            {
                m_btnActive.Visible = false;
            }
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

            List<ExchangeItem> qresult = (List<ExchangeItem>)mgr.getQueryResult(QueryType.queryTypeExchange);

            int i = 0, j = 0,f = 0;
            if (param.m_state != 1)
                i++;

            // 表头
            for (; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);
                if (param.m_state == 1)
                {
                    m_content[f++] = Tool.getCheckBoxHtml("aaa", qresult[i].m_exchangeId, false);
                    j = 0;
                }else 
                {
                    f++;
                    j = 1;
                }
                m_content[f++] = qresult[i].m_playerId.ToString();
                m_content[f++] = qresult[i].getChannelName();
                m_content[f++] = qresult[i].m_recharged.ToString();
                m_content[f++] = qresult[i].m_historyMoney.ToString();
                m_content[f++] = qresult[i].m_phone;
                m_content[f++] = qresult[i].m_bindPhone;
                m_content[f++] = qresult[i].m_exchangeId;
                m_content[f++] = qresult[i].getChgItem()[0];
                m_content[f++] = qresult[i].getChgItem()[1];
                m_content[f++] = qresult[i].m_exchangeTime;
                m_content[f++] = qresult[i].m_giveOutTime;
                m_content[f++] = qresult[i].getStateName();

                for (; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/service/ServiceExchange.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}