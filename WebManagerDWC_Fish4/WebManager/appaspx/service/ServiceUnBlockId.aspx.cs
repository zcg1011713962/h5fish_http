using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.service
{
    public partial class ServiceUnBlockId : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "停封时间", "ID", "选择" };
        private string[] m_content = new string[s_head.Length];
        private string m_idList = "";
        private PageGen m_gen = new PageGen(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.SVR_UN_BLOCK_PLAYER_ID, Session, Response);
            if (IsPostBack)
            {
                m_idList = Request["sel"];
                if (m_idList == null)
                {
                    m_idList = "";
                }
            }
            else 
            {
                if (m_gen.parse(Request))
                {
                    onQueryBlockId(null, null);
                }
            }
        }
        protected void onQueryBlockId(object sender, EventArgs e) //查询已停封玩家ID
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_param = m_playerId.Text;

            OpRes res = user.doQuery(param, QueryType.queryTypeServiceUnBlockIdList);
            genTable(m_result, user, res, param);
        }

        protected void onUnBlockPlayerId(object sender, EventArgs e) //解封玩家ID
        {
            GMUser user = (GMUser)Session["user"];
            ParamBlock p = new ParamBlock();
            p.m_isBlock = false;
            p.m_param = m_idList;
            DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
            if (p.m_param != string.Empty)
            {
                OpRes res = mgr.doDyop(p, DyOpType.opTypeBlockId, user);
                m_res.InnerHtml = "解封玩家ID" + OpResMgr.getInstance().getResultString(res);
            }
            else 
            {
                m_res.InnerHtml = "请选择需要解封的玩家ID......";
            }

            onQueryBlockId(null,null);
        }
        private void genTable(Table table, GMUser user, OpRes res, ParamQuery pageParam)
        {
            table.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            List<ResultBlock> qresult = (List<ResultBlock>)user.getQueryResult(QueryType.queryTypeServiceUnBlockIdList);

            if (qresult.Count == 0)
            {
                btn_unBlockId.Attributes.CssStyle.Value = "display:none";
                res = OpRes.op_res_not_found_data;
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                return;
            }
            else 
            {
                btn_unBlockId.Attributes.CssStyle.Value = "display:block";
            }

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

                m_content[0] = qresult[i].m_time;
                m_content[1] = qresult[i].m_param;
                m_content[2] = Tool.getCheckBoxHtml("sel", m_content[1], false);
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(pageParam, @"/appaspx/service/ServiceUnBlockId.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}