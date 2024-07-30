using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationActivityCfg : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "活动ID","活动名称","活动时间","操作"};
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_ACTIVITY_CFG, Session, Response);
            if (!IsPostBack)
                genTable();
        }
        //生成查询表 玩法收入统计
        private void genTable()
        {
            string[] m_content = new string[s_head.Length];

            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            
            // 表头
            int i = 0, j = 0, f = 0;
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            //表内容
            string path = HttpRuntime.BinDirectory + "..\\" + "data";
            string file = Path.Combine(path, "M_ActivityCFG.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlNode node = doc.SelectSingleNode("/Root");

            for (node = node.FirstChild; node != null; node = node.NextSibling)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;

                int itemId = Convert.ToInt32(node.Attributes["ID"].Value);
                m_content[f++] = itemId.ToString();
                m_content[f++] = node.Attributes["ActivityName"].Value;
                string time1 = node.Attributes["StartTime"].Value; 
                string time2 = node.Attributes["EndTime"].Value;
                m_content[f++] = time1 + "至" + time2;
                m_content[f++] = "编辑";
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.Attributes.CssStyle.Value = "vertical-align:middle";
                    if (j == 3) 
                    {
                        HtmlGenericControl alink = new HtmlGenericControl();
                        alink.TagName = "a";
                        alink.InnerText = "编辑";
                        alink.Attributes.Add("class", "btn btn-primary btn-edit");
                        alink.Attributes.Add("itemId", itemId.ToString());
                        alink.Attributes.Add("startTime", time1);
                        alink.Attributes.Add("endTime", time2);

                        td.Controls.Add(alink);
                    }
                }
            }
        }
    }
}