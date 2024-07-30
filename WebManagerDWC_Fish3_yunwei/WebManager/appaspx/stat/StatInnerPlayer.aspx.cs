using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatInnerPlayer : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","玩家ID","金币数量","收入码量","支出码量","库存","中级场兑换",
            "魔石兑换","使用轰炸机","巨鲨抽奖","使用鱼雷","追击蟹将","获得鱼雷"};
        private string[] m_content = new string[s_head.Length];

        private PageInnerPlayer m_gen = new PageInnerPlayer(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_INNER_PLAYER, Session, Response);
            if (!IsPostBack)
            {
                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatInnerPlayer, user);
            genTable(m_result, res, param, user, mgr);
        }

        //生成查询表
        private void genTable(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
        {
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
            List<InnerPlayerItem> qresult = (List<InnerPlayerItem>)mgr.getQueryResult(QueryType.queryTypeStatInnerPlayer);
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
                InnerPlayerItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_totalGold.ToString();
                m_content[f++] = item.m_income.ToString();
                m_content[f++] = item.m_outlay.ToString();
                m_content[f++] = item.m_store.ToString();
                m_content[f++] = item.m_exchangeGold.ToString();
                m_content[f++] = item.m_exchangeDimensity.ToString();
                m_content[f++] = item.m_bombGold.ToString();
                m_content[f++] = item.m_sharkLotteryGold.ToString();
                m_content[f++] = item.m_torpedoGold.ToString();
                m_content[f++] = item.m_crabGold.ToString();
                m_content[f++] = item.m_getTorpedo.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatInnerPlayer.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}