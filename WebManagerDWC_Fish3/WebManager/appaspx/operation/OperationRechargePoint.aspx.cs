using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class OperationRechargePoint : System.Web.UI.Page
    {
        static string[] s_secondTile = { "日期", "渠道", "充值金额", "充值次数" };
        static string[] s_secondTile1= { "日期","渠道","充值金额","充值金额"};
        static string[] s_secondTile2 = { "日期","渠道","充值次数","充值次数"};
        private string[] s_head;
        private string[] m_content;

        private PageGenLottery m_gen = new PageGenLottery(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_RECHARGE_POINT, Session, Response);
            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("全部", ""));

                Dictionary<string, TdChannelInfo> data = TdChannel.getInstance().getAllData();
                foreach (var item in data.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }
                m_showWay.Items.Add(new ListItem("全部","0"));
                m_showWay.Items.Add(new ListItem("充值金额","1"));
                m_showWay.Items.Add(new ListItem("充值次数","2"));
            }
        }
        //付费点统计查询
        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = genParamQuery();
            OpRes res = user.doQuery(param, QueryType.queryTypeRechargePointStat);
            genTable(res, user,param.m_showWay,param.m_param);
        }

        ParamQuery genParamQuery()
        {
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_param = m_channel.SelectedValue;
            param.m_showWay = m_showWay.SelectedValue;
            return param;
        }

        //查询表
        private void genTable(OpRes res, GMUser user, String way,String channel)
        {
            TableRow tr = new TableRow();
            TableCell td = null;

            ResultRechargePoint qresult = (ResultRechargePoint)user.getQueryResult(QueryType.queryTypeRechargePointStat);
            if (qresult.m_fields.Count == 0)
            {
                m_result.Rows.Add(tr);
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            qresult.sortHeadField();
            genFirstHead(qresult, way);
            genSecondHead(qresult, way);

            for (int i = 0; i < qresult.m_result.Count(); i++)
            {
               // int f = 0;
                RechargePointItemNew item = qresult.m_result[i];
                tr = new TableRow();
                m_result.Rows.Add(tr);
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time.ToShortDateString();

                td = new TableCell();  //渠道
                tr.Cells.Add(td);
                if (channel != "")
                {
                    td.Text = qresult.getChannelName(item.m_channel);
                }
                else 
                {
                    td.Text = "";
                }
                
                long totalValue = 0;
                long totalCount = 0;

                if (way == "1")
                {
                    foreach (var reason in qresult.m_sortFields)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        if (item.m_number.ContainsKey(reason))
                        {
                            totalValue += item.m_number[reason][0];
                            td.Text = item.m_number[reason][0].ToString();
                        }
                        else
                        {
                            td.Text = "";
                        }
                    }
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = totalValue.ToString();
                }
                else if (way == "2")
                {
                    foreach (var reason in qresult.m_sortFields)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        if (item.m_number.ContainsKey(reason))
                        {
                            totalCount += item.m_number[reason][1];
                            td.Text  = item.m_number[reason][1].ToString();
                        }
                        else
                        {
                            td.Text = "";
                        }
                    }
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = totalCount.ToString();
                }
                else 
                {
                    foreach (var reason in qresult.m_sortFields)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        if (item.m_number.ContainsKey(reason))
                        {
                            totalValue += item.m_number[reason][0];
                            totalCount+=item.m_number[reason][1];
                            td.Text = item.m_number[reason][0].ToString();
                            td = new TableCell();
                            tr.Cells.Add(td);
                            td.Text=item.m_number[reason][1].ToString();
                        }
                        else
                        {
                            td.Text = "";
                            td = new TableCell();
                            tr.Cells.Add(td);
                            td.Text = "";
                        }
                    }
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = totalValue.ToString();
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = totalCount.ToString();
                }

            }
        }

        void genFirstHead(ResultRechargePoint qresult,String way)
        {
            int i = 0;
            TableCell td = null;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);

            s_head = new string[2 + qresult.m_fields.Count+1];
            s_head[i++] = "";
            s_head[i++] = "";
            foreach (var r in qresult.m_sortFields)
            {                 
                s_head[i++] = ResultRechargePoint.getPayName(r);
            }

            s_head[i++]="合计";
        
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "";
                if (i >= 2)
                {
                    td.Text = s_head[i];
                    if (way == "0")
                    {
                        td.ColumnSpan = 2;
                    }
                    else
                    {
                        td.ColumnSpan = 1;
                    }
                }
            }
        }
        void genSecondHead(ResultRechargePoint qresult,String way)
        {
            TableCell td = null;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            for (int i = 0; i < 2; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_secondTile[i];
            }
            if(way=="1"){
                int count = qresult.m_fields.Count;
                for (int i = 0; i < count; i++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = s_secondTile1[2];
                }
                s_head = new string[3 + count];
                m_content = new string[3 + count];
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_secondTile1[3];
            }else if(way=="2"){
                int count = qresult.m_fields.Count;
                for (int i = 0; i < count; i++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = s_secondTile2[2];
                }
                s_head = new string[3 + count];
                m_content = new string[3 + count];
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_secondTile2[3];
            }else{
                int count = (qresult.m_fields.Count+1) * 2;
                for (int i = 0; i < count; i++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = s_secondTile[2 + i % 2];
                }

                s_head = new string[2 + count];
                m_content = new string[2 + count];
            }
            
        }
    }
}