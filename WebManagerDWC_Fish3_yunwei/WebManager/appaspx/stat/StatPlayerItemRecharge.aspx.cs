using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatPlayerItemRecharge : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","渠道","锁定购买人数 | 次数","冰冻购买人数 | 次数",
            "狂暴购买人数 | 次数","火力符文购买人数 | 次数","瞄准符文购买人数 | 次数","炮管符文购买人数 | 次数","炮座符文购买人数 | 次数",
            "召唤购买人数 | 次数", "普通鱼雷购买人数 | 次数","升级石购买人数 | 次数"};

        private static string[] s_head1 = new string[] { "日期","渠道","玩家ID","购买锁定次数",
            "购买冰冻次数","购买狂暴次数","购买火力符文次数","购买瞄准符文次数","购买炮管符文次数","购买炮座符文次数","购买召唤次数",
            "购买普通鱼雷次数","购买升级石次数"};

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DTAT_STAT_PLAYER_ITEM_RECHARGE, Session, Response);

            if(!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("全部", "")); //渠道

                Dictionary<string, TdChannelInfo> cd = TdChannel.getInstance().getAllData();
                foreach (var item in cd.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }

                m_objectId.Items.Add("全部");
                m_objectId.Items.Add("个人");
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;

            param.m_op = m_objectId.SelectedIndex;
            if (param.m_op == 1){
                param.m_playerId = m_playerId.Text;
            }
            else {
                param.m_channelNo = m_channel.SelectedValue;
            }
            
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(param, QueryType.queryTypeStatPlayerItemRecharge, user);

            if (param.m_op == 1) //个人
            {
                genTable(m_result, res, user, mgr, param, s_head1);
            }
            else {
                genTable(m_result, res, user, mgr, param, s_head);
            }
        }

        //生成查询表
        private void genTable(Table table, OpRes res, GMUser user, QueryMgr mgr, ParamQuery param, string[] s_head)
        {
            string[] m_content = new string[s_head.Length];

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
            List<StatItemRecharge> qresult = (List<StatItemRecharge>)mgr.getQueryResult(QueryType.queryTypeStatPlayerItemRecharge);
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
                StatItemRecharge item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = param.m_op == 1 ? item.getChannelName(item.m_channelNo) : (param.m_channelNo == "" ? "" : item.getChannelName(param.m_channelNo));
                if(param.m_op == 1)
                    m_content[f++] = item.m_playerId.ToString();

                foreach (var da in item.m_rechargeItem)
                {
                    m_content[f++] = (param.m_op == 1) ? da.Value[1].ToString() : (da.Value[0] + " | " + da.Value[1].ToString());
                }

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Attributes.CssStyle.Value = "min-width:100px";
                    td.Text = m_content[j];
                }
            }
        }

    }
}