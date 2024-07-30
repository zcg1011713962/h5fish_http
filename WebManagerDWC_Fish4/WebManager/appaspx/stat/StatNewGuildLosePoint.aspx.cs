using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatNewGuildLosePoint : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","注册人数",
            "点击开始游戏（百分比）","进入大厅人数","领取开服礼包人数","进入渔场加载人数","进入渔场人数",
            "打出第一发子弹人数","捕获一条鱼人数","第一次升级炮倍","达到10炮倍","第一次使用锁定",
            "中级场兑换人数","第一次兑换VIP经验人数","进入中级场人数","进入龙宫场人数","进入巨鲨场人数","进入高级场人数",
            "5炮平均时长(分)","10炮平均时长","20炮平均时长","30炮平均时长","40炮平均时长","50炮平均时长","60炮平均时长","70炮平均时长",
            "80炮平均时长","90炮平均时长","100炮平均时长","150炮平均时长","200炮平均时长","250炮平均时长","300炮平均时长","350炮平均时长",
            "400炮平均时长","450炮平均时长","500炮平均时长"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_NEW_GUILD_LOSE_POINT, Session, Response);
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatNewGuildLosePoint, user);
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
            List<StatNewGuildLosePointItem> qresult = (List<StatNewGuildLosePointItem>)mgr.getQueryResult(QueryType.queryTypeStatNewGuildLosePoint);
            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            //内容
            for (i = 0; i < qresult.Count; i++)
            {
                int f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                StatNewGuildLosePointItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_regeditCount.ToString();
                m_content[f++] = item.getRate(item.m_clickStartGame);  
                m_content[f++] = item.getRate(item.m_clientEnterPlatform); //进入大厅 
                m_content[f++] = item.getRate(item.m_hitFirstGift); //点击启航礼包 
                m_content[f++] = item.getRate(item.m_loadGameFromGift);
                m_content[f++] = item.getRate(item.m_completeLoadGame);
                m_content[f++] = item.getRate(item.m_fireFirstBullet); //第一炮 
                m_content[f++] = item.getRate(item.m_killFirstFish); //第一条鱼 
                m_content[f++] = item.getRate(item.m_updateFirstLevel);  //第一次升炮
                m_content[f++] = item.getRate(item.m_updateSecondLevel); //达到10炮倍 
                m_content[f++] = item.getRate(item.m_clientFirstUseLock); //第一次使用锁定

                m_content[f++] = item.getRate(item.m_room2ExchangePerson);
                m_content[f++] = item.getRate(item.m_vipExchangePerson);
                m_content[f++] = item.getRate(item.m_room2EnterCount);
                m_content[f++] = item.getRate(item.m_room5EnterCount);
                m_content[f++] = item.getRate(item.m_room6EnterCount);
                m_content[f++] = item.getRate(item.m_room3EnterCount);

                foreach(var da in item.m_turretTime)
                {
                    m_content[f++] = Convert.ToInt32(da.Value / 60).ToString();
                }

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