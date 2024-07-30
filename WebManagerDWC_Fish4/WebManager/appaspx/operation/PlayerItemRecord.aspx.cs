using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operation
{
    public partial class PlayerItemRecord : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","玩家ID", "变化原因", "道具ID", "道具", "起始值", "结束值", "差额", "同步字段" };
        private string[] m_content=new string[s_head.Length];
        private PagePlayerItem m_gen = new PagePlayerItem(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            m_foot.InnerHtml = "";
            m_page.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.OP_PLAYER_ITEM_RECORD, Session, Response);
            if (!IsPostBack)
            {
                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    m_playerId.Text = m_gen.m_playerId;
                    m_itemId.Text = m_gen.m_itemId;
                    m_syncKey.Text = m_gen.m_syncKey;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_playerId = m_playerId.Text;
            param.m_param = m_itemId.Text;
            param.m_channelNo = m_syncKey.Text;//同步字段
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypePlayerItemRecord, user);

            genTable(m_result, res, param, user, mgr);
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
    }
}