using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.operation
{
    public partial class word2LogicItemError : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { 
            "添加时间", "玩家ID", "添加原因", "失败原因", "是否确认失败", "道具列表", "记录生成时间","同步字段","是否已处理","操作"};
        private string[] m_content = new string[s_head.Length];
        private PageGenItemError m_gen = new PageGenItemError(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            m_foot.InnerHtml = "";
            m_page.InnerHtml = "";
            m_span.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.OP_WORD2_LOGIC_ITEM_ERROR, Session, Response);
            if (!IsPostBack)
            {
                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    m_playerId.Text = m_gen.m_playerId;
                    m_syncKey.Text = m_gen.m_syncKey;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_playerId = m_playerId.Text;
            param.m_channelNo = m_syncKey.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeWord2LogicItemError, user);

            genTable(m_result, res, param, user, mgr);
        }

        private void genTable(Table table, OpRes res, ParamQuery param, GMUser user, QueryMgr mgr)
        {
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

            List<addItemError> qresult = (List<addItemError>)mgr.getQueryResult(QueryType.queryTypeWord2LogicItemError);
            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                if(i==4)
                    td.Attributes.CssStyle.Value = "min-width:150px;";
                if(i==5)
                    td.Attributes.CssStyle.Value = "min-width:220px;";
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);

                int index = 0;
                m_content[index++] = qresult[i].m_time;
                m_content[index++] = qresult[i].m_playerId;

                XmlConfig xml = ResMgr.getInstance().getRes("money_reason.xml");
                if (xml != null)
                {
                    m_content[index++] = xml.getString(qresult[i].m_addItemReason.ToString(), "");
                }
                else 
                {
                    m_content[index++] = qresult[i].m_addItemReason.ToString();
                }

                m_content[index++] = getFailReason(qresult[i].m_failReason);
                m_content[index++] = "";
                m_content[index++] = getRewardList(qresult[i].m_rewardList);//道具列表
                m_content[index++] = qresult[i].m_recCreateTime;
                m_content[index++] = qresult[i].m_syncKey;
                m_content[index++] = qresult[i].m_isDeal==true?"是":"否";
                m_content[index++] = "";

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.Attributes.CssStyle.Value = "vertical-align:middle";

                    //是否确认失败
                    if (j == 4)
                    {
                        if (qresult[i].m_failReason == 2)
                        {   //存疑
                            string time = qresult[i].m_time.Split(' ')[0];
                            HtmlGenericControl alink = new HtmlGenericControl();
                            alink.TagName = "a";
                            alink.InnerText = "存疑";
                            alink.Attributes.Add("class", "btn btn-default");
                            alink.Attributes.Add("target","_blank");
                            if (qresult[i].m_isDeal)
                                alink.Attributes.CssStyle.Value = "color:red";
                            alink.Attributes.Add("href", DefCC.ASPX_PLAYER_ITEM_RECORD
                                + "?playerId=" + qresult[i].m_playerId
                                + "&syncKey=" + qresult[i].m_syncKey
                                + "&time=" + time);
                            td.Controls.Add(alink);
                        }
                        else 
                        {   //确认
                            HtmlGenericControl aspan = new HtmlGenericControl();
                            aspan.TagName = "span";
                            aspan.InnerText = "确认>>>";
                            if (qresult[i].m_isDeal)
                                aspan.Attributes.CssStyle.Value = "color:red";
                            td.Controls.Add(aspan);
                        }

                        //补单
                        HtmlGenericControl alink1 = new HtmlGenericControl();
                        alink1.TagName = "a";
                        alink1.InnerText = "补单";
                        alink1.Attributes.Add("class", "btn btn-primary");
                        alink1.Attributes.Add("target", "_blank");
                        alink1.Attributes.CssStyle.Value = "margin-left:10px;";
                        alink1.Attributes.Add("href", DefCC.ASPX_SERVICE_MAIL
                            + "?playerId=" + qresult[i].m_playerId
                            + "&title=" + System.Web.HttpUtility.UrlEncode("游戏道具补偿")
                            + "&sender=" + System.Web.HttpUtility.UrlEncode("游戏运营团队")
                            + "&gift=" + BaseJsonSerializer.serialize(qresult[i].m_rewardList));
                        td.Controls.Add(alink1);
                    }

                    if(j==s_head.Length-1 && !qresult[i].m_isDeal) 
                    {
                        HtmlGenericControl alink = new HtmlGenericControl();
                        alink.TagName = "a";
                        alink.InnerText = "设置";
                        alink.Attributes.Add("class", "btn btn-primary");
                        alink.Attributes.Add("id", qresult[i].m_id);
                        td.Controls.Add(alink);
                    }
                }
            }
            m_span.InnerHtml = "*红色表示已处理";
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/operation/word2LogicItemError.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //道具详情
        public string getRewardList(List<giftErrorItem> rewardList)
        {
            string result = "";
            string name = "";
            for (int i = 0; i < rewardList.Count; i++)
            {
                Fish_ItemCFGData data = Fish_ItemCFG.getInstance().getValue(rewardList[i].m_gameItemId);
                if (data != null)
                {
                    name = data.m_itemName;
                }
                else
                {
                    name = "";
                }
                result += string.Format("id : {0}, name ：{1}, count : {2}",
                    rewardList[i].m_gameItemId, name, rewardList[i].m_count);
                result += "<br />";
            }
            return result;
        }

        //失败原因
        public string getFailReason(int key) 
        {
            string reason = "";
            switch(key)
            {
                case (int)AddItemFailReason.fail_reason_player_not_in_fish: 
                        reason = "不在捕鱼服务器内"; 
                        break;
                case (int)AddItemFailReason.fail_reason_player_offline:
                        reason = "玩家掉线，与logic断开连接"; 
                        break;
                case (int)AddItemFailReason.fail_reason_beyond_verity_time:
                        reason = "超出确认时间"; 
                        break;
                case (int)AddItemFailReason.fail_reason_not_find_player:
                        reason = "没找到玩家"; 
                        break;
            }
            return reason;
        }
    }
}