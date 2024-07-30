using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordBaojinBasicData : System.Web.UI.Page
    {
        TableStatFishlordBaojinData m_common = new TableStatFishlordBaojinData(@"/appaspx/stat/StatFishlordBaojinBasicData.aspx");
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_STAT_FISHLORD_BAOJIN, Session, Response);
           
            GMUser user = (GMUser)Session["user"];

            PageFishBaojin gen=m_common.m_gen;

            if (!IsPostBack)
            {
                if (gen.parse(Request))
                {
                    ParamQuery param=new ParamQuery();
                    Table table=new Table();
                    param.m_op = gen.m_op;
                    param.m_time = gen.m_time;
                    param.m_param = gen.m_param.ToString();

                    m_time.Text = gen.m_time;
                    string pageParam = "";
                    switch(gen.m_op)
                    {
                        case 1: pageParam = m_common.onQuery(user, gen, param, m_result1, QueryType.queryTypeFishlordBaojinStat); break;
                        case 4: pageParam = m_common.onQuery(user, gen, param, m_result4, QueryType.queryTypeFishlordBaojinStat); break;
                        case 5: pageParam = m_common.onQuery(user, gen, param, m_result5, QueryType.queryTypeFishlordBaojinStat); break;
                    }
                      
                    if(pageParam!="")
                    {
                        string[] str = Tool.split(pageParam, '#');
                        m_page.InnerHtml = str[0];
                        m_foot.InnerHtml = str[1];
                    }
                }
            }

        }
    }
}