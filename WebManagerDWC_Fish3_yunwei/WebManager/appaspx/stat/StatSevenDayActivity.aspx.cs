using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatSevenDayActivity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_SEVEN_DAY_ACTIVITY, Session, Response);
            if(!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("全部", "")); //渠道
                Dictionary<string, TdChannelInfo> cd = TdChannel.getInstance().getAllData();
                foreach (var item in cd.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_channelNo = m_channel.SelectedValue;
            OpRes res = user.doQuery(param, QueryType.queryTypeSevenDayActivity);
            genTable(m_result,res,user,param.m_channelNo);
        }

        //查询表
        protected void genTable(Table table,OpRes res, GMUser user,string channel)
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

            ResultSevenDayActivity qresult = (ResultSevenDayActivity)user.getQueryResult(QueryType.queryTypeSevenDayActivity);

            var fields = from f in qresult.m_fields orderby f ascending select f;

            int[] day = {0,0,0,0,0,0,0};
            foreach (var reason in fields)
            {
                if (reason.IndexOf('1') == 0) { day[0]++; }
                if (reason.IndexOf('2') == 0) { day[1]++; }
                if (reason.IndexOf('3') == 0) { day[2]++; }
                if (reason.IndexOf('4') == 0) { day[3]++; }
                if (reason.IndexOf('5') == 0) { day[4]++; }
                if (reason.IndexOf('6') == 0) { day[5]++; }
                if (reason.IndexOf('7') == 0) { day[6]++; }
            }

            string[] dayName = { "第一天", "第二天", "第三天", "第四天", "第五天", "第六天", "第七天"};

            // 生成行标题  表头
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "日期";
            td.RowSpan = 2;
            td.Attributes.CssStyle.Value = "vertical-align:middle";

            if(channel!="")
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "渠道";
                td.RowSpan = 2;
                td.Attributes.CssStyle.Value = "vertical-align:middle;min-width:120px";
            }

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "当日新增人数";
            td.RowSpan = 2;
            td.ColumnSpan = 1;
            td.Attributes.CssStyle.Value = "vertical-align:middle;min-width:120px";
            //表头1 第几天
            for (int i = 0; i<day.Count();i++ ) 
            {
                if(day[i]!=0)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text =dayName[i];
                    td.RowSpan = 1;
                    td.ColumnSpan = day[i];
                }
            }
            //表头2
            tr = new TableRow();
            m_result.Rows.Add(tr);
            foreach (var reason in fields)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = qresult.getName(reason);
                td.Attributes.CssStyle.Value = "min-width:160px";
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            //内容
            for (int i = 0; i < qresult.m_result.Count(); i++)
            {
                SevenDayItem item = qresult.m_result[i];
                tr = new TableRow();
                m_result.Rows.Add(tr);
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time.ToShortDateString();

                if(channel!="")  //单独渠道
                {
                    td = new TableCell();  //渠道
                    tr.Cells.Add(td);
                    td.Text = qresult.getChannelName(item.m_channel);
                }
                
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_regeditCount.ToString();

                // 生成这个结果
                foreach (var reason in fields)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if (item.m_number.ContainsKey(reason))
                    {
                        td.Text = item.getPercent(item.m_number[reason], item.m_regeditCount);
                        //td.Text = item.m_number[reason].ToString();
                    }
                    else 
                    {
                        td.Text = "0";
                    }
                }
            }
        }
    }
}