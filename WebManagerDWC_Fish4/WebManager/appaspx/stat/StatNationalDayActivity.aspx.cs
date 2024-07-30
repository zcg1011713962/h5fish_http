using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatNationalDayActivity : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "排名", "玩家ID", "玩家昵称", "击杀数量"};
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {
            m_resNote.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.DATA_NATIONAL_DAY_ACTIVITY, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("活动领取奖励人数统计");
                m_queryType.Items.Add("活动排行榜");
                m_queryType.Items.Add("活动作弊功能设置");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_op = m_queryType.SelectedIndex;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            switch(param.m_op)
            {
                case 0:
                        OpRes res = mgr.doQuery(param, QueryType.queryTypeNdActRecvCount, user);
                        genTable0(m_result, res, param, user, mgr);
                        break;
                case 1:
                        OpRes res_1 = mgr.doQuery(param, QueryType.queryTypeNdActRankList, user);
                        genTable1(m_result, res_1, param, user, mgr);
                        break;
                case 2:
                        CMemoryBuffer buffer = CommandMgr.getMemoryBuffer(user, true);
                        buffer.Writer.Write(m_playerId.Text);
                        buffer.Writer.Write(m_fishCount.Text);
                        CommandBase cmd = CommandMgr.processCmd(CmdName.SetNdActFishCount, buffer, user);
                        OpRes res_2 = cmd.getOpRes();
                        m_resNote.InnerHtml = OpResMgr.getInstance().getResultString(res_2);
                        break;
            }
        }

        //领取奖励生成查询表
        private void genTable0(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
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
            NdActRecvItem qresult = (NdActRecvItem)mgr.getQueryResult(QueryType.queryTypeNdActRecvCount);
            int i = 0, j = 0;
            // 表头
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "日期";

            foreach(var d in qresult.m_data.Keys)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = d;
            }

            for (int k = 0; k< 3;k++ ) 
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);


                td = new TableCell();
                tr.Cells.Add(td);
                switch (k)
                {
                    case 0: td.Text = "人数"; break;
                    case 1: td.Text = "当日登录人数"; break;
                    case 2: td.Text = "领取比例"; break;
                }

                foreach (var d in qresult.m_data.Values)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    if(k==2)
                    {
                        td.Text = qresult.getPercent(d[0],d[1]); //领取人数  登陆人数
                    }else
                    { 
                        td.Text = d[k].ToString();
                    }
                }
            }
        }

        //活动排行榜生成查询表
        private void genTable1(Table table, OpRes res, ParamQuery query_param, GMUser user, QueryMgr mgr)
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
            List<NdActRankList> qresult = (List<NdActRankList>)mgr.getQueryResult(QueryType.queryTypeNdActRankList);
            int i = 0, j = 0,f = 0;
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
                NdActRankList item = qresult[i];
                m_content[f++] = item.m_rankId.ToString();
                m_content[f++] = item.m_playerId;
                m_content[f++] = item.m_nickName;
                m_content[f++] = item.m_fishCount.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            
        }
    }
}