using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatTotalConsume : System.Web.UI.Page
    {
        private string[] s_head;
        private string[] m_content;

        private static string[] s_head1 = new string[] { "日期", "普通鱼雷", "青铜鱼雷", "白银鱼雷", "黄金鱼雷","钻石鱼雷","定海神针"};
        private static string[] s_head2 = new string[] { "产出", "消耗", "剩余" };
        private static string[] s_head3 = new string[] { "购买", "邮件领取", "直接发送", "总计" };
        private static string[] s_head4 = new string[] { "使用","南海消耗"};
        private static string[] s_head5 = new string[] { "渔场", "背包" };
 
        private PageGenLottery m_gen = new PageGenLottery(50);
        XmlConfig xml = ResMgr.getInstance().getRes("money_reason.xml");
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_TOTAL_CONSUME, Session, Response);
            if (!IsPostBack)
            {
                m_changeType.Items.Add("系统支出");
                m_changeType.Items.Add("系统收入");
            }
        }

        protected void onStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamTotalConsume param = new ParamTotalConsume();
            param.m_changeType = m_changeType.SelectedIndex;
            param.m_currencyType = Convert.ToInt32(m_currencyType.SelectedValue);
            param.m_time = m_time.Text;

            OpRes res = OpRes.op_res_failed;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            if (param.m_currencyType >= 23 && param.m_currencyType <= 27) //鱼雷
            {
                res = mgr.doQuery(param, QueryType.queryTypeFishlordBulletHeadOutput, user);
                genTable1(m_result, res, user);
            }
            else {
                res = mgr.doQuery(param, QueryType.queryTypeTotalConsume, user);
                genTable(m_result, res, user, mgr, param.m_currencyType);
            }
            
        }

        private void genTable(Table table, OpRes res, GMUser user, QueryMgr mgr, int currType)
        {
            table.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            List<StatTotalConsumeItem> qresult = (List<StatTotalConsumeItem>)mgr.getQueryResult(QueryType.queryTypeTotalConsume);
            if (qresult.Count == 0)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            int i = 0, j = 0, f = 0;

            s_head = new string[qresult[0].m_totalConsume.Count-1]; //日期+1  90 -1 46 -1

            s_head[0] = "日期";
             //获取变化原因 默认值
            foreach (PropertyReasonType type in Enum.GetValues(typeof(PropertyReasonType)))
            {
                if (type == PropertyReasonType.type_max || (int)type == 90 ||(int)type == 46)
                    continue;

                string typeStr = ((int)type).ToString();
                s_head[++i] = xml.getString(typeStr, "");
            }

            m_content = new string[s_head.Length];
            
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.Attributes.CssStyle.Value = "min-width:50px;";
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                if ((i & 1) == 0)
                {
                    tr.CssClass = "alt";
                }
                table.Rows.Add(tr);
                f = 0;
                m_content[f++] = qresult[i].m_time;

                foreach (var da in qresult[i].m_totalConsume) 
                {
                    if (da.Key == 90 || da.Key == 46)
                        continue;

                    //为话费券时，除100
                    m_content[f++] = ((currType == 3)? (da.Value/100.0) : da.Value).ToString();
                }

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }
        }

        //弹头统计 生成表
        private void genTable1(Table table, OpRes res, GMUser user)
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

            List<fishlordBulletHeadOutputStat> qresult = (List<fishlordBulletHeadOutputStat>)user.getQueryResult(QueryType.queryTypeFishlordBulletHeadOutput);

            int i = 0, j = 0, k = 0, l = 0;
            // 表头 第一行
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
                if(i > 0 && i < 7)
                {
                    td.ColumnSpan = 8;
                    td.RowSpan = 1;
                }else
                {
                    td.ColumnSpan = 1;
                    td.RowSpan = 4;
                }
                td.Attributes.CssStyle.Value = "vertical-align:middle";
            }

            // 表头 第二行
            tr = new TableRow();
            table.Rows.Add(tr);
            for (j = 0; j < s_head1.Length - 1; j++)
            {
                for (i = 0; i < s_head2.Length; i++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = s_head2[i];
                    if (i == 0)
                    {
                        td.ColumnSpan = 4;
                        td.RowSpan = 1;
                    }
                    else if (i == 1) {
                        td.ColumnSpan = 3;
                        td.RowSpan = 1;
                    }
                    else
                    {
                        td.ColumnSpan = 1;
                        td.RowSpan = 3;
                    }
                }
            }

            // 表头 第三行
            tr = new TableRow();
            table.Rows.Add(tr);
            for (j = 0; j < s_head1.Length - 1; j++)
            {
                for (i = 0; i < s_head2.Length - 2 ; i++)
                {
                    for (k = 0; k < s_head3.Length; k++) 
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = s_head3[k];
                        td.ColumnSpan = 1;
                        td.RowSpan = 2;
                    }
                    for (k = 0; k < s_head4.Length; k++)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = s_head4[k];

                        if (k == 0)
                        {
                            td.ColumnSpan = 2;
                            td.RowSpan = 1;
                        }
                        else 
                        {
                            td.ColumnSpan = 1;
                            td.RowSpan = 2;
                        }
                    }
                }
            }

            // 表头 第四行
            tr = new TableRow();
            table.Rows.Add(tr);
            for (j = 0; j < s_head1.Length - 1; j++)
            {
                for (i = 0; i < s_head2.Length - 2; i++)
                {
                    for (k = 0; k < s_head4.Length - 1; k++)
                    {
                        for (l = 0; l < s_head5.Length; l++)
                        {
                            td = new TableCell();
                            tr.Cells.Add(td);
                            td.Text = s_head5[l];
                            td.ColumnSpan = 1;
                            td.RowSpan = 1;
                        }
                    }
                }
            }

            long total_output = 0, total_use = 0, output = 0;
            for (i = 0; i < qresult.Count; i++)
            {
                total_output = 0;
                total_use = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                fishlordBulletHeadOutputStat item = qresult[i];
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;

                var dataList = item.m_data.OrderBy(a => a.Key).ToList();
                foreach (var data in dataList)
                {
                    output = 0;
                    long[] da = data.Value;
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da[0].ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da[1].ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da[2].ToString();

                    //某弹头总产出
                    output = da[0] + da[1] + da[2];
                    //当天总产出
                    total_output += output;
                    //当天总使用
                    total_use += da[3] + da[4] + da[5];

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = output.ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da[3].ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da[4].ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = da[5].ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = (output - da[3] - da[4] - da[5]).ToString();
                }
            }
        }
    }
}