using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.td
{
    public partial class TdNewPlayerRechargeMonitor : System.Web.UI.Page
    {
        private static string[] s_head = new string[] {"注册时间", "玩家ID","渠道","当前炮倍","累计付费","累计游戏时间",
            "初次付费日期","初次付费游戏时间","初次付费项目","初次付费时拥有金币","初次付费炮倍",
            "再次付费日期","再次付费游戏时间","再次付费项目","再次付费时拥有金币","再次付费炮倍",
            "碎片结余","碎片累计获得"};
        private string[] m_content = new string[s_head.Length];

        private PageTdNewPlayerRechargeMonitor m_gen = new PageTdNewPlayerRechargeMonitor(50);
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.TD_NEW_PLAYER_RECHARGE_MONITOR, Session, Response);
            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("全渠道", "-1"));
                Dictionary<string, TdChannelInfo> data = TdChannel.getInstance().getAllData();
                foreach (var item in data.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }

                if (m_gen.parse(Request))
                {
                    m_time.Text = m_gen.m_time;
                    m_channel.SelectedValue = m_gen.m_channel;
                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();

            param.m_channelNo = m_channel.SelectedValue;
            param.m_time = m_time.Text;

            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = OpRes.op_res_failed;

            res = mgr.doQuery(param, QueryType.queryTypeTdNewPlayerMonitor, user);
            genTable(m_result, res, user, param);
        }

        //抽奖统计
        private void genTable(Table table, OpRes res, GMUser user, ParamQuery param)
        {
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

            List<TdNewPlayerMonitorItem> qresult =
                (List<TdNewPlayerMonitorItem>)user.getQueryResult(QueryType.queryTypeTdNewPlayerMonitor);

            int i = 0, j = 0, f = 0;

            // 表头
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);
                TdNewPlayerMonitorItem item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.getChannelName();
                m_content[f++] = item.getOpenRate(item.m_turretLevel);
                m_content[f++] = item.m_totalRecharge.ToString();
                m_content[f++] = item.getTimehsm(item.m_totalOnlineTime);

                //初次付费
                m_content[f++] = item.m_firstRechargeTime;
                m_content[f++] = item.getTimehsm(item.m_firstRechargePlayedTime);
                m_content[f++] = item.getPayName(item.m_firstRechargePayId);
                m_content[f++] = item.m_firstRechargePlayerMoney.ToString();
                m_content[f++] = item.getOpenRate(item.m_firstRechargePlayerTurretLevel);

                //再次付费
                m_content[f++] = item.m_secondRechargeTime;
                m_content[f++] = item.getTimehsm(item.m_secondRechargePlayedTime);
                m_content[f++] = item.getPayName(item.m_secondRechargePayId);
                m_content[f++] = item.m_secondRechargePlayerMoney.ToString();
                m_content[f++] = item.getOpenRate(item.m_secondRechargePlayerTurretLevel);

                m_content[f++] = item.m_torpedoChip.ToString();
                m_content[f++] = item.m_totalGainChip.ToString();

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/td/TdNewPlayerRechargeMonitor.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;
        }
    }
}