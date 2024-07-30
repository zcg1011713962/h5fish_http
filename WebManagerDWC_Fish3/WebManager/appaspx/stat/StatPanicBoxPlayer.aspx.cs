using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPanicBoxPlayer : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "玩家ID", "宝箱类型", 
            "抽到奖励1", "抽到奖励2", "抽到奖励3", "抽到奖励4" ,"抽到奖励5","抽到奖励6","抽到奖励7","抽到奖励8"};
        private string[] m_content = new string[s_head.Length];

        private PagePanicBoxPlayer m_gen = new PagePanicBoxPlayer(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            ////////////////////////////////////////////////////////////////
            string box_id = Request.QueryString["boxId"];
            if (string.IsNullOrEmpty(box_id))
                return;

            int boxId = 0;
            if (!int.TryParse(box_id, out boxId))
                return;

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_op = boxId;
            param.m_time = Request.QueryString["time"];
            if(m_gen.parse(Request))
            {
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
            }
            //////////////////////////////////////////////////////////////
            genTable(m_result,param,user);
        }

        private void genTable(Table table, ParamQuery param, GMUser user)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatPanicBoxDetail, user);

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

            List<PanicBoxDetail> qresult = (List<PanicBoxDetail>)mgr.getQueryResult(QueryType.queryTypeStatPanicBoxDetail);

            td = new TableCell();
            tr.Cells.Add(td);
            td.ColumnSpan = 10;
            td.Text = param.m_time;
            int i = 0, j = 0, index = 0;
            // 表头
            tr = new TableRow();
            table.Rows.Add(tr);
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                index = 0;

                tr = new TableRow();
                table.Rows.Add(tr);
                
                PanicBoxDetail item = qresult[i];
                m_content[index++] = item.m_playerId;
                m_content[index++] = item.getBoxTypeName();
                for (int k = 0; k < 8; k++)
                {
                    if (item.m_reward.ContainsKey(k)){
                        m_content[index++] = item.getAwardDetail(k).ToString();
                    }else{
                        m_content[index++] = "";
                    }
                }

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatPanicBoxPlayer.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}