using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace WebManager.appaspx.operation
{
    public partial class PlayerItemRecord : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期", "玩家ID", "变化原因", "道具ID", "道具", "起始值", "结束值", "差额", "同步字段" };
        private string[] m_content=new string[s_head.Length];
        private PagePlayerItem m_gen = new PagePlayerItem(50);

        XmlConfig xml = ResMgr.getInstance().getRes("money_reason.xml");
        private string pattern_num = @"^\d*$";

        protected void Page_Load(object sender, EventArgs e)
        {
            m_res.InnerHtml = "";
            m_foot.InnerHtml = "";
            m_page.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.OP_PLAYER_ITEM_RECORD, Session, Response);
            if (!IsPostBack)
            {
                    if (xml != null)  //变化原因
                    {
                        ///////////////////////////////////////////////////////////////////////////////////
                        m_reason.Value = "0_全部";
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<select name=\"filter1\" >");
                        sb.Append("<option value=\"0_全部\">0_全部</option>");
                        foreach (PropertyReasonType type in Enum.GetValues(typeof(PropertyReasonType)))
                        {
                            if (type == PropertyReasonType.type_max)
                                continue;

                            string typeStr = ((int)type).ToString();
                            sb.Append("<option value=\"" + typeStr + "_" + xml.getString(typeStr, "") + "\">" + typeStr + "_" + xml.getString(typeStr, "") + "</option>");
                        }
                        sb.Append("</select>");
                        m_reason_list.InnerHtml = sb.ToString();
                    }

                    if (m_gen.parse(Request))
                    {
                        m_time.Text = m_gen.m_time;
                        m_playerId.Text = m_gen.m_playerId;
                        m_itemId.Text = m_gen.m_itemId;
                        m_syncKey.Text = m_gen.m_syncKey;

                        ///////////////////  变化原因数字ID转为名称 //////////////////
                        if (m_gen.m_op == 0)
                        {
                            m_reason.Value = "0_全部";
                        }else
                        {
                            m_reason.Value = m_gen.m_op.ToString() + '_' + xml.getString(m_gen.m_op.ToString(), "");
                        }
                        onQuery(null, null);
                    }
                }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = getParamQuery();
            if (param == null)
            {
                m_res.InnerHtml = "输入参数非法";
                m_page.InnerHtml = "";
                m_foot.InnerHtml = "";
                return;
            }
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypePlayerItemRecord, user);

            genTable(m_result, res, param, user, mgr);
        }

        private ParamQuery getParamQuery() {
            ParamQuery param = new ParamQuery();

            //////////////  考虑分页 只有 id    否则 id_name  ///////////////////
            string idStr = m_reason.Value;
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
            param.m_op = Convert.ToInt32(idStr);   //变化原因
            ////////////////////////////////////////////////////////////////////////
            param.m_time = m_time.Text;
            param.m_playerId = m_playerId.Text;
            param.m_param = m_itemId.Text;
            param.m_channelNo = m_syncKey.Text;//同步字段
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            return param;
        }

        private void genTable(Table table, OpRes res, ParamQuery param, GMUser user, QueryMgr mgr)
        {
            table.GridLines = GridLines.Both;
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

            List<playerItemRecord> qresult = (List<playerItemRecord>)mgr.getQueryResult(QueryType.queryTypePlayerItemRecord);
            int i = 0, j = 0;
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
                table.Rows.Add(tr);

                int index = 0;
                m_content[index++] = qresult[i].m_time;
                m_content[index++] = qresult[i].m_playerId;
                m_content[index++] = qresult[i].getActionName(); 
                int itemId = qresult[i].m_itemIId;
                m_content[index++] = itemId.ToString();

                ItemCFGData item = ItemCFG.getInstance().getValue(itemId);
                if (item != null)
                {
                    m_content[index++] = item.m_itemName;
                }else 
                {
                    m_content[index++] = "";
                }
                m_content[index++] = qresult[i].m_itemOldCount.ToString();
                m_content[index++] = qresult[i].m_itemNewCount.ToString();
                m_content[index++] = qresult[i].m_itemAddCount.ToString();
                m_content[index++] = qresult[i].m_syncKey;
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/operation/PlayerItemRecord.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        protected void onExport(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = getParamQuery();
            if (param == null)
            {
                m_res.InnerHtml = "输入参数非法";
                m_page.InnerHtml = "";
                m_foot.InnerHtml = "";
                return;
            }
            ExportMgr mgr = user.getSys<ExportMgr>(SysType.sysTypeExport);
            OpRes res = mgr.doExport(param, ExportType.exportTypePlayerItemRecord, user);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }
    }
}