using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.service
{
    public partial class ServiceReplacementOrder : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "操作时间", "操作原因", "操作ID", "补单项目", "补单补贴/客服回访福利", "操作账号","备注"};
        private string[] m_content = new string[s_head.Length];
        private PageGenRepairOrder m_gen = new PageGenRepairOrder(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            m_res.InnerHtml = "";
            m_resInfo.InnerHtml = "";

            RightMgr.getInstance().opCheck(RightDef.SVR_REPLACEMENT_ORDER, Session, Response);

            GMUser user = (GMUser)Session["user"];
            if (IsPostBack)
            {
                m_gen.m_time = m_time.Text;
            }
            else {
                m_opReason.Items.Add("补单");
                m_opReason.Items.Add("换包福利");
                m_opReason.Items.Add("访问客服福利");
                m_opReason.Items.Add("大户回流引导");

                m_item.Items.Add(new ListItem("非补单不用修改本项", "-1"));
                var allData = RechargeCFG.getInstance().getAllData();
                foreach (var d in allData)
                {
                    m_item.Items.Add(new ListItem(d.Value.m_name, d.Key.ToString()));
                }

                m_bonus.Items.Add(new ListItem("非奖励不用修改此选项", "-1"));
                var repairOrderData = RepairOrderItem.getInstance().getAllData();
                foreach (var d in repairOrderData)
                {
                    m_bonus.Items.Add(new ListItem(d.Value.m_itemName, d.Key.ToString()));
                }

                if (m_gen.parse(Request)) 
                {
                    m_time.Text = m_gen.m_time;
                    onQuery(null, null);
                }

            }
        }

        protected void onSubmit(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamRepairOrder param = new ParamRepairOrder();
            param.m_op = Convert.ToInt32(m_opReason.SelectedIndex);
            param.m_playerId = m_playerIdList.Text;
            param.m_itemId = Convert.ToInt32(m_item.SelectedValue);
            param.m_bonusId = Convert.ToInt32(m_bonus.SelectedValue);
            param.m_comments = m_comment.Text;

            if (param.m_op == 0) //补单
            {
                param.m_rtype = (int)RechargeType.rechargeRMB;
                ListItem selItem = m_item.Items[m_item.SelectedIndex];
                param.m_param = selItem.Value;
            }

            DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
            OpRes res = mgr.doDyop(param, DyOpType.opTypeRepairOrder, user);

            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }

        //系统操作历史查询
        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeRepairOrder, user);
            genTable(m_result, res, param, user, mgr);
        }

        //系统操作历史查询导出excel
        protected void onExportExcel(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            ExportMgr mgr = user.getSys<ExportMgr>(SysType.sysTypeExport);
            OpRes res = mgr.doExport(param, ExportType.exportTypeRepairOrder, user);
            m_resInfo.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }
        //活动排行榜生成查询表
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
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
            List<ParamRepairOrderItem> qresult = (List<ParamRepairOrderItem>)mgr.getQueryResult(QueryType.queryTypeRepairOrder);
            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                f = 0;
                ParamRepairOrderItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.getOpreason();
                m_content[f++] = item.m_playerId;
                m_content[f++] = item.getRepairOrderName();
                m_content[f++] = item.getRepairBonusName();
                m_content[f++] = item.m_operator;
                m_content[f++] = item.m_comments;

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/service/ServiceReplacementOrder.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

    }
}