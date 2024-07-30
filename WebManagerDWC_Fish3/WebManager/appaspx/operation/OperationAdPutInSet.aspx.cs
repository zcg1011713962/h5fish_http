using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationAdPutInSet : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "活动ID", "活动名称", "广告编号", "活动时间", "操作" };
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_ACTIVITY_CFG, Session, Response);
            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("闲玩", "100003"));
                m_channel.Items.Add(new ListItem("蛋蛋赚", "100009"));
                m_channel.Items.Add(new ListItem("葫芦星球", "100010"));
                m_channel.Items.Add(new ListItem("有赚", "100011"));
                m_channel.Items.Add(new ListItem("麦子赚", "100012"));
                m_channel.Items.Add(new ListItem("聚享游", "100013"));
                m_channel.Items.Add(new ListItem("小啄", "100014"));
                m_channel.Items.Add(new ListItem("泡泡赚", "100015"));
                m_channel.Items.Add(new ListItem("豆豆赚", "100016"));

                GMUser user = (GMUser)Session["user"];
                ParamQuery param = new ParamQuery();

                OpRes res = user.doQuery(param, QueryType.queryTypeOperationAdActSet);
                genTable(m_result, res, user);
            }
        }

        private void genTable(Table table, OpRes res, GMUser user)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            //不存在，新增
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);

                return;
            }
           
            int i = 0, n = 0, j = 0;
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            List<AdActItem> qresult =
                (List<AdActItem>)user.getQueryResult(QueryType.queryTypeOperationAdActSet);

            for (i = 0; i < qresult.Count; i++)
            {
                n = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                AdActItem item = qresult[i];
                m_content[n++] = item.m_key.ToString();
                m_content[n++] = getActName(item.m_key);
                m_content[n++] = getQihaoName(item.m_qihao);
                m_content[n++] = item.m_startTime.ToString() + " 至 " + item.m_endTime.ToString();
                m_content[n++] = "编辑";

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.Attributes.CssStyle.Value = "vertical-align:middle";
                    if (j == 4)
                    {
                        HtmlGenericControl alink = new HtmlGenericControl();
                        alink.TagName = "a";
                        alink.InnerText = "编辑";
                        alink.Attributes.Add("class", "btn btn-primary btn-edit");
                        alink.Attributes.Add("itemId", item.m_key.ToString());
                        alink.Attributes.Add("qihao", item.m_qihao.ToString());
                        alink.Attributes.Add("startTime", item.m_startTime);
                        alink.Attributes.Add("endTime", item.m_endTime);

                        td.Controls.Add(alink);
                    }
                }
            }
        }
        
        public string getActName(int key)
        {
            switch(key)
            {
                case 1: return "100003 - 闲玩";
                case 2: return "100009 - 蛋蛋赚"; 
                case 3: return "100010 - 葫芦星球";
                case 4: return "100011 - 有赚";
                case 5: return "100012 - 麦子赚";
                case 6: return "100013 - 聚享游";
                case 7: return "100014 - 小啄";
                case 8: return "100015 - 泡泡赚";
                case 9: return "100016 - 豆豆赚";
            }
            return "";
        }

        public string getQihaoName(int key) 
        {
            switch(key)
            {
                case 1: return "一期";
                case 2: return "二期";
                case 3: return "三期";
                case 4: return "四期";
                case 5: return "五期";
                case 6: return "六期";
                case 7: return "七期";
                case 8: return "八期";
                case 9: return "九期";
                case 10: return "十期";
            }
            return "";
        }
    }
}