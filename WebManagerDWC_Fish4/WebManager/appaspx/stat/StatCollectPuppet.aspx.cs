using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatCollectPuppet : System.Web.UI.Page
    {
        private static string[] s_head_1 = new string[] { "日期", "任务名称", "发放玩偶数量"};
        private static string[] s_head_2 = new string[] { "日期", "捐赠档位", "达成人数","领取奖励人数" };
        private static string[] s_head_3 = new string[] { "日期", "捐赠档位", "领取人数" };
        private static string[] s_head_4 = new string[] { "日期","捐赠次数","捐赠数量"};
        private static string[] s_head_5 = new string[] { "排名","玩家ID", "玩家昵称", "捐赠玩偶总数", "活动期间的充值"};
        private static string[] s_head_6 = new string[] { "排名","玩家ID","玩家昵称","玩偶获得总数","活动期间的充值"};
        private string[] m_content_1 = new string[s_head_1.Length];
        private string[] m_content_2 = new string[s_head_2.Length];
        private string[] m_content_3 = new string[s_head_3.Length];
        private string[] m_content_4 = new string[s_head_4.Length];
        private string[] m_content_5 = new string[s_head_5.Length+1];
        private PageCollectPuppet m_gen = new PageCollectPuppet(20);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_LABA_LOTTERY, Session, Response);
            if (!IsPostBack)
            {
                m_queryType.Items.Add("玩偶发放数量统计");
                m_queryType.Items.Add("玩家捐赠档位统计");
                m_queryType.Items.Add("服务器档位奖励领取统计");
                m_queryType.Items.Add("服务器总捐赠玩偶统计");
                m_queryType.Items.Add("20个捐赠玩家排行榜");
                m_queryType.Items.Add("20个累计获得玩偶玩家排行榜");

                m_type.Items.Add("当前活动");
                m_type.Items.Add("历史活动");

                if (m_gen.parse(Request))
                {
                    m_queryType.SelectedIndex = m_gen.m_way;
                    if (m_queryType.SelectedIndex == 4 || m_queryType.SelectedIndex == 5)
                    {
                        m_type.SelectedIndex = Convert.ToInt32(m_gen.m_type-1);
                    }
                    else 
                    {
                        m_time.Text = m_gen.m_time;
                    }
                    
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_way = (QueryWay)m_queryType.SelectedIndex;
            if (param.m_way == QueryWay.by_way0) //玩偶发放数量统计
            {
                param.m_time = m_time.Text;
                param.m_showWay = "-1";
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
                OpRes res = user.doQuery(param, QueryType.queryTypePuppetActStat);
                genTable1(m_result, res, user, param);
            }

            if (param.m_way == QueryWay.by_way1) //玩家捐赠档位统计
            {
                param.m_time = m_time.Text;
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
                param.m_param = "1";
                param.m_showWay = "-1";
                OpRes res = user.doQuery(param, QueryType.queryTypePuppetRewardRecv);
                genTable2(m_result, res, user, param,s_head_2,m_content_2);
            }

            if (param.m_way == QueryWay.by_way2) //服务器档位奖励领取统计
            {
                param.m_time = m_time.Text;
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
                param.m_param = "2";
                param.m_showWay = "-1";
                OpRes res = user.doQuery(param, QueryType.queryTypePuppetRewardRecv);
                genTable2(m_result, res, user, param,s_head_3,m_content_3);
            }
            if (param.m_way == QueryWay.by_way3)//服务器总捐赠玩偶统计
            {
                param.m_showWay = "-1";
                param.m_time = m_time.Text;
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
                OpRes res = user.doQuery(param, QueryType.queryTypePuppetSvrDonate);
                genTable3(m_result, res, user, param);
            }

            if (param.m_way == QueryWay.by_way4)//20个捐赠玩家排行榜
            {
                param.m_showWay = (m_type.SelectedIndex + 1).ToString(); //活动属性
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
                OpRes res = user.doQuery(param, QueryType.queryTypePuppetPlayerDonateRank);
                genTable4(m_result, res, user, s_head_5, QueryType.queryTypePuppetPlayerDonateRank, param.m_showWay, param);
            }

            if(param.m_way==QueryWay.by_way5)//20个累计获得玩偶玩家排行榜
            {
                param.m_showWay = (m_type.SelectedIndex + 1).ToString();
                param.m_curPage = m_gen.curPage;
                param.m_countEachPage = m_gen.rowEachPage;
                OpRes res = user.doQuery(param, QueryType.queryTypePuppetPlayerGainRank);
                genTable4(m_result, res, user, s_head_6, QueryType.queryTypePuppetPlayerGainRank,param.m_showWay,param);
            }
        }
        //生成表 玩偶发放数量统计
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                m_page.InnerHtml = "";
                m_foot.InnerHtml = "";
                return;
            }

            List<PuppetActItem> qresult = (List<PuppetActItem>)user.getQueryResult(QueryType.queryTypePuppetActStat);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head_1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                PuppetActItem item = qresult[i];

                m_content_1[f++] = item.m_time;
                m_content_1[f++] = getActName(item.m_actId);
                m_content_1[f++] = item.m_puppetCount.ToString();
                for (j = 0; j < s_head_1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_1[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatCollectPuppet.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //生成表 玩家捐赠档位统计
        private void genTable2(Table table, OpRes res, GMUser user, ParamQuery query_param,string[] s_head,string[] m_content)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                m_page.InnerHtml = "";
                m_foot.InnerHtml = "";
                return;
            }

            List<PuppetRewardRecvItem> qresult = (List<PuppetRewardRecvItem>)user.getQueryResult(QueryType.queryTypePuppetRewardRecv);

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
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                PuppetRewardRecvItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = getDonateCount(item.m_rewardId);
                if(item.m_rewardType==1) //玩家
                {
                    m_content[f++] = item.m_reachCount.ToString();
                }
                m_content[f++] = item.m_recvCount.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatCollectPuppet.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //生成表 服务器档位奖励领取统计
        private void genTable3(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);
                m_page.InnerHtml = "";
                m_foot.InnerHtml = "";
                return;
            }

            List<PuppetSvrDonateItem> qresult = (List<PuppetSvrDonateItem>)user.getQueryResult(QueryType.queryTypePuppetSvrDonate);

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head_4.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_4[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                PuppetSvrDonateItem item = qresult[i];

                m_content_4[f++] = item.m_time;
                m_content_4[f++] = item.m_donateCount.ToString();
                m_content_4[f++] = item.m_donateAmount.ToString();

                for (j = 0; j < s_head_4.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_4[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatCollectPuppet.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //生成表 20个捐赠玩家排行榜/20个累计获得玩偶玩家排行榜
        private void genTable4(Table table, OpRes res, GMUser user, string[] s_head, QueryType type, string actType, ParamQuery query_param)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            TableCell td = null;

            if (res != OpRes.opres_success)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = OpResMgr.getInstance().getResultString(res);

                m_page.InnerHtml = "";
                m_foot.InnerHtml = "";
                return;
            }

            List<PuppetPlayerRankItem> qresult = (List<PuppetPlayerRankItem>)user.getQueryResult(type);

            int i = 0, j = 0;
            // 表头
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = s_head[0];

            int len = s_head.Length;
            if (Convert.ToInt32(actType) == 2) //如果是历史活动就在第二列加一列时间
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "日期";
            }
            
            for (i = 1; i < len; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            if (Convert.ToInt32(actType) == 2)
            {
                len += 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                PuppetPlayerRankItem item = qresult[i];

                m_content_5[f++] = item.m_rank.ToString();
                if(Convert.ToInt32(actType)==2)
                {
                    m_content_5[f++] = item.m_time;
                }
                m_content_5[f++] = item.m_playerId.ToString();
                m_content_5[f++] = item.m_nickName;
                m_content_5[f++] = item.m_puppetCount.ToString();
                m_content_5[f++] = item.m_totalRecharge.ToString();

                for (j = 0; j < len; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content_5[j];
                }
            }
            string page_html = "", foot_html = "";
            m_gen.genPage(query_param, @"/appaspx/stat/StatCollectPuppet.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }

        //任务名
        public string getActName(int key)
        {
            string name = key.ToString();
            if(key==0)
            {
                name = "玩家购买";
            }
            var data = ActivityPuppetCFG.getInstance().getValue(key);
            if (data != null)
            {
                name = data.m_actName;
            }
            return name;
        }
        public string getDonateCount(int key) 
        {
            string donateCount = key+"档位";
            var data = ActivityPuppetDonateRewardCFG.getInstance().getValue(key);
            if (data != null)
            {
                donateCount = data.m_donateCount.ToString();
            }
            return donateCount;
        }
    }
}