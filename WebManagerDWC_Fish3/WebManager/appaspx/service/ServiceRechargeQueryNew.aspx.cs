using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace WebManager.appaspx.service
{
    public partial class ServiceRechargeQueryNew : System.Web.UI.Page
    {
        private static string[] s_head = new string[] {"创建时间", "充值时间", "玩家ID", "玩家账号", "订单ID", "付费点", "充值金额(元)", "订单状态", "渠道"};
        private static string[] s_head1 = new string[] { "充值时间","充值次数", "充值人数", "金额" };
        private static string[] s_head2 = new string[] { "订单ID", "出现次数" };
        private static string[] s_head_1 = new string[] {"充值时间", "玩家ID", "玩家账号", "订单ID", "付费点", "充值金额(元)", "订单状态", "渠道","是否处理"};

        private string[] m_content = new string[s_head.Length];
        private PageGenRecharge m_gen = new PageGenRecharge(50);

        private string pattern_num = @"^\d*$";

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SVR_RECHARGE_QUERY, Session, Response);
            m_res.InnerHtml = "";
            if (IsPostBack)   //客户端回发而加载
            {
                m_gen.m_way = m_queryWay.SelectedIndex;
            }
            else
            {
                m_queryWay.Items.Add("通过玩家id查询");
                m_queryWay.Items.Add("通过账号查询");
                m_queryWay.Items.Add("通过订单号查询");

                m_rechargeResult.Items.Add(new ListItem("全部", PayState.PAYSTATE_ALL.ToString()));
                m_rechargeResult.Items.Add(new ListItem("已到账", PayState.PAYSTATE_SUCCESS.ToString()));
                m_rechargeResult.Items.Add(new ListItem("已请求", PayState.PAYSTATE_HAS_REQ.ToString()));
                m_rechargeResult.Items.Add(new ListItem("已支付", PayState.PAYSTATE_HAS_PAY.ToString()));

                m_channel.Items.Add(new ListItem("全部", "")); //渠道

                Dictionary<string, TdChannelInfo> cd = TdChannel.getInstance().getAllData();
                foreach (var item in cd.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }

                ///////////////////////////  付费点  /////////////////////////////////////////////
                m_rechargePoint.Value = "0_全部";
                var rp = RechargeCFG.getInstance().getAllData();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<select name=\"filter1\" >");
                sb.Append("<option value=\"0_全部\">0_全部</option>");
                foreach (var d in rp)
                {
                    sb.Append("<option value=\"" + d.Value.m_payId + "_" + d.Value.m_name + "\">" + d.Value.m_payId + "_" + d.Value.m_name + "</option>");
                }
                sb.Append("</select>");
                m_rechargePoint_list.InnerHtml = sb.ToString();
                ///////////////////////////////////////////////////////////////////////////////////////
                if (m_gen.parse(Request))
                {
                    m_queryWay.SelectedIndex = m_gen.m_way;
                    m_param.Text = m_gen.m_param;
                    m_time.Text = m_gen.m_time;
                    
                    for (int i = 0; i < m_rechargeResult.Items.Count; i++)
                    {
                        if (m_rechargeResult.Items[i].Value == m_gen.m_result.ToString())
                        {
                            m_rechargeResult.Items[i].Selected = true;
                            break;
                        }
                    }

                    m_range.Text = m_gen.m_range;

                    //付费点
                    int point = Convert.ToInt32(m_gen.m_rechargePoint);
                    if (point == 0) m_rechargePoint.Value = "0_全部";
                    var pointItem = RechargeCFG.getInstance().getValue(point);
                    if (pointItem != null)
                        m_rechargePoint.Value = pointItem.m_payId.ToString()+'_'+pointItem.m_name;

                    for (int i = 0; i < m_channel.Items.Count; i++)
                    {
                        if (m_channel.Items[i].Value == m_gen.m_channelNo)
                        {
                            m_channel.Items[i].Selected = true;
                            break;
                        }
                    }

                    if (m_gen.m_op == 1)
                    {
                        onQueryByAibei(null,null);
                    }
                    else 
                    {
                        onQueryRecharge(null, null);
                    }
                }
            }
        }

        // 开始查询充值记录
        protected void onQueryRecharge(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQueryRecharge param = genParamQueryRecharge();
            if (param == null)
            {
                m_res.InnerHtml = "输入参数非法";
                m_page.InnerHtml = "";
                m_foot.InnerHtml = "";
                return;
            }
            param.m_op = 0;//表示总体
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeRechargeNew, user);
            genTable(m_result, res, s_head, param, user, mgr, param);
        }

        // 开始统计充值记录
        protected void onStatRecharge(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQueryRecharge param = genParamQueryRecharge();
            if (param == null)
            {
                m_res.InnerHtml = "输入参数非法";
                m_page.InnerHtml = "";
                m_foot.InnerHtml = "";
                return;
            }
            StatMgr mgr = user.getSys<StatMgr>(SysType.sysTypeStat);
            OpRes res = mgr.doStat(param, StatType.statTypeRechargeNew, user);
            genStatTable(m_result, res, mgr, user);
        }
        //导出excel
        protected void onExport(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQueryRecharge param = genParamQueryRecharge();
            if (param == null)
            {
                m_res.InnerHtml = "输入参数非法";
                m_page.InnerHtml = "";
                m_foot.InnerHtml = "";
                return;
            }
            ExportMgr mgr = user.getSys<ExportMgr>(SysType.sysTypeExport);
            OpRes res = mgr.doExport(param, ExportType.exportTypeRechargeNew, user);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }

        protected void onSameOrder(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQueryRecharge param = genParamQueryRecharge();

            if (param == null)
            {
                m_res.InnerHtml = "输入参数非法";
                m_page.InnerHtml = "";
                m_foot.InnerHtml = "";
                return;
            }

            StatMgr mgr = user.getSys<StatMgr>(SysType.sysTypeStat);
            OpRes res = mgr.doStat(param, StatType.statTypeSameOrderId, user);
            genSameOrderTable(m_result, res, mgr, user);
        }

        //爱贝充值查询
        protected void onQueryByAibei(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQueryRecharge param = genParamQueryRecharge();
            param.m_op = 1; //表示爱贝
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeRechargeNew, user);
            genTable(m_result, res, s_head_1, param, user, mgr, param);
        }

        private ParamQueryRecharge genParamQueryRecharge()
        {
            ParamQueryRecharge param = new ParamQueryRecharge();
            param.m_way = (QueryWay)m_queryWay.SelectedIndex;
            param.m_param = m_param.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_time = m_time.Text;
            param.m_result = Convert.ToInt32(m_rechargeResult.SelectedValue);
            param.m_range = m_range.Text;
            param.m_channelNo = m_channel.SelectedValue;

            //////////////  考虑分页 只有 id    否则 id_name  ///////////////////
            string idStr = m_rechargePoint.Value;
            if (string.IsNullOrEmpty(idStr))
                return null;
            //防止自定义输入
            //纯数字  或者 数字_名称     =====避免  输入 数字名称通过
            if (!Regex.IsMatch(idStr, pattern_num)) //非纯数字   
            {
                if (!Regex.IsMatch(idStr, @"^[0-9]+_"))
                    return null;

                idStr = idStr.Split('_')[0];
            }
            param.m_rechargePoint = idStr;
            //param.m_rechargePoint = m_rechargePoint.SelectedValue;
            ////////////////////////////////////////////////////////////////////////
            return param;
        }

        //生成查询表
        private void genTable(Table table, OpRes res, string[] s_head,ParamQueryRecharge query_param, GMUser user, QueryMgr mgr,ParamQueryRecharge param)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            List<RechargeItemNew> qresult = (List<RechargeItemNew>)mgr.getQueryResult(QueryType.queryTypeRechargeNew);
            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }
            int f = 0;
            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                RechargeItemNew item = qresult[i];

                if(param.m_op!=1)
                {
                    m_content[f++] = item.m_createTime;
                }
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_account;
                m_content[f++] = item.m_orderId;
                m_content[f++] = item.getPayName();
                m_content[f++] = item.m_totalPrice.ToString();
                m_content[f++] = StrName.s_rechargeState[item.m_status];
                m_content[f++] = item.getChannelName();
                if(param.m_op==1)
                {
                    m_content[f++] = item.m_process == false ? "未处理" : "已处理";
                }

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/service/ServiceRechargeQueryNew.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        // 生成统计表
        private void genStatTable(Table table, OpRes res, StatMgr mgr, GMUser user)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            int i = 0, j = 0;
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
            }
            List<ResultStatRecharge> rs = (List<ResultStatRecharge>)mgr.getStatResult(StatType.statTypeRechargeNew);
            //for (i = 0; i < rs.Count; i++) //升序
            for (i = (rs.Count - 1); i >= 0; i--)  //降序
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                m_content[f++] = rs[i].m_rechargeTime.ToString();
                m_content[f++] = rs[i].m_rechargeCount.ToString();
                m_content[f++] = rs[i].m_rechargePersonNum.ToString();
                m_content[f++] = rs[i].m_total.ToString();

                for (j = 0; j < s_head1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }

        private void genSameOrderTable(Table table, OpRes res, StatMgr mgr, GMUser user)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }

            int i = 0, j = 0;
            for (i = 0; i < s_head2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head2[i];
            }

            List<ResultSameOrderIdItem> result = (List<ResultSameOrderIdItem>)mgr.getStatResult(StatType.statTypeSameOrderId);

            for (i = 0; i < result.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);

                m_content[0] = result[i].m_orderId;
                m_content[1] = result[i].m_count.ToString();

                for (j = 0; j < s_head2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}