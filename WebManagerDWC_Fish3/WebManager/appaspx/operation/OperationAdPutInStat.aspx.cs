using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationAdPutInStat : System.Web.UI.Page
    {
        private PageGenOnlineReward m_gen = new PageGenOnlineReward(3);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_STAT_AD_PUTIN, Session, Response);
            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("闲玩", "100003"));
                m_channel.Items.Add(new ListItem("蛋蛋赚", "100009"));
                m_channel.Items.Add(new ListItem("葫芦星球", "100010"));
                m_channel.Items.Add(new ListItem("有赚", "100011"));
                m_channel.Items.Add(new ListItem("聚享游", "100013"));
                m_channel.Items.Add(new ListItem("麦子赚", "100012"));
                m_channel.Items.Add(new ListItem("小啄", "100014"));
                m_channel.Items.Add(new ListItem("泡泡赚", "100015"));
                m_channel.Items.Add(new ListItem("豆豆赚", "100016"));

                if (m_gen.parse(Request))
                {
                    m_channel.SelectedValue = m_gen.m_channelID;
                    m_time.Text = m_gen.m_time;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            ParamQuery param = new ParamQuery();
            param.m_channelNo = m_channel.SelectedValue;
            param.m_time = m_time.Text;

            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            OpRes res = user.doQuery(param, QueryType.queryTypeQueryChannel100003ActCount);

            switch(param.m_channelNo){
                case "100003": //闲玩
                    TableChannel100003 view = new TableChannel100003();
                    view.genTable(user, m_result, res, null);
                    break;
                case "100009":  //蛋蛋赚
                    TableChannel100009 view1 = new TableChannel100009();
                    view1.genTable(user, m_result, res, null);
                    break;
                case "100010": //葫芦星球
                    TableChannel100010 view2 = new TableChannel100010();
                    view2.genTable(user, m_result, res, null);
                    break;
                case "100011": //有赚
                    TableChannel100011 view3 = new TableChannel100011();
                    view3.genTable(user, m_result, res, null);
                    break;
                case "100012": //麦子赚
                    TableChannel100012 view4 = new TableChannel100012();
                    view4.genTable(user, m_result, res, null);
                    break;
                case "100013": //聚享游
                    TableChannel100013 view5 = new TableChannel100013();
                    view5.genTable(user, m_result, res, null);
                    break;
                case "100014": //小啄
                    TableChannel100014 view6 = new TableChannel100014();
                    view6.genTable(user, m_result, res, null);
                    break;
                case "100015": //泡泡赚
                    TableChannel100015 view7 = new TableChannel100015();
                    view7.genTable(user, m_result, res, null);
                    break;
                case "100016": //豆豆赚
                    TableChannel100016 view8 = new TableChannel100016();
                    view8.genTable(user, m_result, res, null);
                    break;
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/operation/OperationAdPutInStat.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}