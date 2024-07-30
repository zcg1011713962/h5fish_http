using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishlordAdvancedRoomCtrl : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "奖励等级", "名额（人）", "奖金比例（%）", " ",""};
        protected void Page_Load(object sender, EventArgs e)
        {
            m_res.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_ADVANCED_ROOM_CTRL, Session, Response);
            if (!IsPostBack)
            {
                onQuery(null, null);
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            OpRes res = user.doQuery(param, QueryType.queryTypeStatFishlordAdvancedRoom);
            genTable(m_result, res, user);
        }

        //生成表
        private void genTable(Table table, OpRes res, GMUser user)
        {
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

            List<ParamFishlordAdvancedRoomItem> qresult = 
                (List<ParamFishlordAdvancedRoomItem>)user.getQueryResult(QueryType.queryTypeStatFishlordAdvancedRoom);

            string[] m_levels = new string[] { "一等奖", "二等奖", "三等奖"};

            int i = 0;

            tr = new TableRow();
            m_result.Rows.Add(tr);

            ////////////////////////////////////////////////////////////////////////////////////////
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "奖池期望值";
            td.Attributes.CssStyle.Value = "vertical-align:middle;height:50px;";
            td.RowSpan = 1;
            td.ColumnSpan = 1;

            td = new TableCell();
            tr.Cells.Add(td);
            HtmlGenericControl aInput0 = new HtmlGenericControl("input");
            aInput0.Attributes.Add("style", "height:30px;");
            aInput0.Attributes.Add("value", qresult[3].m_ratio.ToString());
            aInput0.Attributes.Add("type", "text");
            aInput0.Attributes.Add("class", "input_data rewardRatio");
            td.Controls.Add(aInput0);

            td.RowSpan = 1;
            HtmlGenericControl aBtn0 = new HtmlGenericControl("input");
            aBtn0.Attributes.Add("style", "margin-left:20px;");
            aBtn0.Attributes.Add("type", "button");
            aBtn0.Attributes.Add("value", "提交");
            aBtn0.Attributes.Add("class", "btn btn-primary btn_edit");
            aBtn0.Attributes.Add("op", "3");
            td.Controls.Add(aBtn0);
            /////////////////////////////////////////////////////////////////////////

            tr = new TableRow();
            m_result.Rows.Add(tr);
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "控制系数（百分比）";
            td.Attributes.CssStyle.Value = "vertical-align:middle;height:50px;";
            td.RowSpan = 1;
            td.ColumnSpan = 1;

            td = new TableCell();
            tr.Cells.Add(td);
            HtmlGenericControl aInput1 = new HtmlGenericControl("input");
            aInput1.Attributes.Add("style", "height:30px;");
            aInput1.Attributes.Add("value", qresult[4].m_ratio.ToString());
            aInput1.Attributes.Add("type", "text");
            aInput1.Attributes.Add("class", "input_data rewardRatio");
            td.Controls.Add(aInput1);

            td.RowSpan = 1;
            HtmlGenericControl aBtn1 = new HtmlGenericControl("input");
            aBtn1.Attributes.Add("style", "margin-left:20px;");
            aBtn1.Attributes.Add("type", "button");
            aBtn1.Attributes.Add("value", "提交");
            aBtn1.Attributes.Add("class", "btn btn-primary btn_edit");
            aBtn1.Attributes.Add("op", "4");
            td.Controls.Add(aBtn1);


            tr = new TableRow();
            m_result.Rows.Add(tr);
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "小奖抽水（百分比）";
            td.Attributes.CssStyle.Value = "vertical-align:middle;height:50px;";
            td.RowSpan = 1;
            td.ColumnSpan = 1;

            td = new TableCell();
            tr.Cells.Add(td);
            HtmlGenericControl aInput2 = new HtmlGenericControl("input");
            aInput2.Attributes.Add("style", "height:30px;");
            aInput2.Attributes.Add("value", qresult[5].m_ratio.ToString());
            aInput2.Attributes.Add("type", "text");
            aInput2.Attributes.Add("class", "input_data rewardRatio");
            td.Controls.Add(aInput2);

            td.RowSpan = 1;
            HtmlGenericControl aBtn2 = new HtmlGenericControl("input");
            aBtn2.Attributes.Add("style", "margin-left:20px;");
            aBtn2.Attributes.Add("type", "button");
            aBtn2.Attributes.Add("value", "提交");
            aBtn2.Attributes.Add("class", "btn btn-primary btn_edit");
            aBtn2.Attributes.Add("op", "5");
            td.Controls.Add(aBtn2);

            tr = new TableRow();
            m_result.Rows.Add(tr);
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "";
            td.RowSpan = 1;
            td.ColumnSpan = 5;

            // 表头 第一行
            tr = new TableRow();
            m_result.Rows.Add(tr);
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
                td.Attributes.CssStyle.Value = "vertical-align:middle;height:50px;";
            }

            for (i = 0; i < 3; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_levels[i];

                td = new TableCell();
                tr.Cells.Add(td);
                HtmlGenericControl aInput11 = new HtmlGenericControl("input");
                aInput11.Attributes.Add("style", "height:30px;");
                aInput11.Attributes.Add("value", qresult[i].m_maxWinCount.ToString());
                aInput11.Attributes.Add("type", "text");
                aInput11.Attributes.Add("class", "input_data maxWinCount");
                td.Controls.Add(aInput11);

                td = new TableCell();
                tr.Cells.Add(td);
                HtmlGenericControl aInput12 = new HtmlGenericControl("input");
                aInput12.Attributes.Add("style", "height:30px;");
                aInput12.Attributes.Add("value", qresult[i].m_ratio.ToString());
                aInput12.Attributes.Add("type", "text");
                aInput12.Attributes.Add("class", "input_data rewardRatio");
                td.Controls.Add(aInput12);

                if (i == 0) {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = "";
                    td.RowSpan = 3;
                }

                td = new TableCell();
                tr.Cells.Add(td);
                td.RowSpan = 1;
                HtmlGenericControl aBtn = new HtmlGenericControl("input");
                aBtn.Attributes.Add("style", "margin-left:20px;");
                aBtn.Attributes.Add("type", "button");
                aBtn.Attributes.Add("value", "提交");
                aBtn.Attributes.Add("class", "btn btn-primary btn_edit");
                aBtn.Attributes.Add("op", (i).ToString());
                td.Controls.Add(aBtn);
            }
            ///////////////////////////////////////////////////////////
        }
    }
}