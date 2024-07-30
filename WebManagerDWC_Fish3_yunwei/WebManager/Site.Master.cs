using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.UI.HtmlControls;

namespace WebManager
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            if (user != null)
            {
                DbServerInfo info = ResMgr.getInstance().getDbInfo(user.m_dbIP);
                if (info != null)
                {
                    //Label1.Text = "当前操作的服务器地址：" + info.m_serverName;
                }
                else
                {
                    //Label1.Text = "当前操作的数据库地址：" + user.m_dbIP;
                }
                user.preURL = Request.Url.AbsolutePath;

                userMenu.InnerHtml = MenuMgr.getInstance().getMenuGroup(user.getMenuGroup());
            }

            //genMenu();
           // genShortCutMenu();
        }

        // 动态生成菜单
        private void genMenu()
        {
            Dictionary<string, DbServerInfo> db = ResMgr.getInstance().getAllDb();
            foreach(DbServerInfo info in db.Values)
            {
                string url = "/appaspx/SelectMachine.aspx?db=" + info.m_serverIp;

                HtmlGenericControl li = new HtmlGenericControl();
                li.TagName = "li";
                li.Attributes.Add("role", "presentation");

                HtmlGenericControl a = new HtmlGenericControl();
                a.TagName = "a";
                a.InnerText = info.m_serverName;
                a.Attributes.Add("href", url);
                li.Controls.Add(a);
//                 MenuItem item = new MenuItem();
//                 item.Text = info.m_serverName;
//                 item.NavigateUrl = "/appaspx/SelectMachine.aspx?db=" + info.m_serverIp;
//                 NavigationMenu.Items.Add(item);

                //NavigationMenu.Controls.Add(li);
            }
        }

        private void genShortCutMenu()
        {
            try
            {
                // 生成快捷菜单
                List<MainMenu> menuList = MenuMgr.getInstance().menuList;
                foreach (var m in menuList)
                {
                    TableCell td = new TableCell();
                    Label L = new Label();
                    L.Text = m.m_name;
                    L.CssClass = "cShortCutTitle";
                    td.Controls.Add(L);

                    td.Controls.Add(m.m_dst);
                    //shortCutMenu.Cells.Add(td);
                }
            }
            catch (System.Exception ex)
            {
                LOGW.Info(ex.ToString());
            }
        }
    }
}
