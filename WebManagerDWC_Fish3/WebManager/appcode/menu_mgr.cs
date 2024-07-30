using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

public class MenuMgr
{
    private static MenuMgr s_obj = null;

    private List<MainMenu> m_menuList = new List<MainMenu>();

    private Dictionary<int, string> m_menuGroup = new Dictionary<int, string>();

    public static MenuMgr getInstance()
    {
        if (s_obj == null)
        {
            s_obj = new MenuMgr();
            s_obj.init();
        }

        return s_obj;
    }

    public List<MainMenu> menuList
    {
        get { return m_menuList; }
    }

    public string getMenuGroup(int key)
    {
        if (m_menuGroup.ContainsKey(key))
        {
            return m_menuGroup[key];
        }

        return "";
    }

    private void init()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(HttpContext.Current.Server.MapPath("\\data\\Navigation.xml"));
        XmlNode node = doc.SelectSingleNode("Root/mainMenu");
        if (node != null)
        {
            for (; node != null; node = node.NextSibling)
            {
                MainMenu top = new MainMenu();
                top.m_name = node.Attributes["gameName"].Value;
                foreach (XmlNode c in node.ChildNodes)
                {
                    top.add(c.Attributes["url"].Value, c.Attributes["text"].Value);
                }

                m_menuList.Add(top);
            }
        }

        loadMenuGroup();
    }

    private void loadMenuGroup()
    {
        loadMenuGroup(1);
        loadMenuGroup(2);
    }

    private void loadMenuGroup(int group)
    {
        try
        {
            string file = HttpContext.Current.Server.MapPath("\\data\\MenuGroup" + group + ".xml");
            byte[] all = File.ReadAllBytes(file);
            string str = Encoding.UTF8.GetString(all);
            m_menuGroup.Add(group, str);
        }
        catch (System.Exception ex)
        {
        }
    }
}

public class SubMenu
{
    public string m_url = "";
    public string m_text = "";
}

public class MainMenu
{
    public string m_name = "";

    public List<SubMenu> m_subList = new List<SubMenu>();

    public BulletedList m_dst = new BulletedList();

    public MainMenu()
    {
        m_dst.DisplayMode = BulletedListDisplayMode.HyperLink;
    }

    public void add(string url, string text)
    {
        SubMenu s = new SubMenu();
        s.m_url = url;
        s.m_text = text;
        m_subList.Add(s);

        ListItem item = new ListItem();
        item.Text = text;
        item.Value = url;
        m_dst.Items.Add(item);
    }
}



