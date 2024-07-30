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
        private static string[] s_head = new string[] { "日期","渠道","注册人数",
            "点击开始游戏（百分比）","进入大厅人数","领取开服礼包人数","进入渔场加载人数","进入渔场人数",
            "打出第一发子弹人数","捕获一条鱼人数","第一次升级炮倍","达到10炮倍","第一次使用锁定",
            "中级场兑换人数","第一次兑换VIP经验人数","进入中级场人数","进入龙宫场人数","进入巨鲨场人数","进入高级场人数","第一次登录到领取起航礼包时间(秒)",
            "5炮平均时长(分)","10炮平均时长(分)","20炮平均时长(分)","30炮平均时长(分)","40炮平均时长(分)","50炮平均时长(分)","60炮平均时长(分)","70炮平均时长(分)",
            "80炮平均时长(分)","90炮平均时长(分)","100炮平均时长(分)","150炮平均时长(分)","200炮平均时长(分)","250炮平均时长(分)","300炮平均时长(分)","350炮平均时长(分)",
            "400炮平均时长(分)","450炮平均时长(分)","500炮平均时长(分)"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_NEW_GUILD_LOSE_POINT, Session, Response);
            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("总体", ""));
                m_channel.Items.Add(new ListItem("全渠道", "-1"));
                m_channel.Items.Add(new ListItem("安卓总体", "-2"));
                Dictionary<string, TdChannelInfo> data = TdChannel.getInstance().getAllData();
                foreach (var item in data.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_param = m_channel.SelectedValue;
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

                m_content[f++] = item.m_time.ToShortDateString();
                //渠道
                if (query_param.m_param != "" && query_param.m_param != "-2")
                {
                    m_content[f++] = item.getChannelName();
                }
                else 
                {
                    m_content[f++] = "";
                }
                m_content[f++] = item.m_regeditCount.ToString();
                m_content[f++] = item.m_clickStartGame.ToString();  
                m_content[f++] = item.m_clientEnterPlatform.ToString(); //进入大厅 
                m_content[f++] = item.m_hitFirstGift.ToString(); //点击启航礼包 
                m_content[f++] = item.m_loadGameFromGift.ToString();
                m_content[f++] = item.m_completeLoadGame.ToString();
                m_content[f++] = item.m_fireFirstBullet.ToString(); //第一炮 
                m_content[f++] = item.m_killFirstFish.ToString(); //第一条鱼 
                m_content[f++] = item.m_updateFirstLevel.ToString();  //第一次升炮
                m_content[f++] = item.m_updateSecondLevel.ToString(); //达到10炮倍 
                m_content[f++] = item.m_clientFirstUseLock.ToString(); //第一次使用锁定

                m_content[f++] = item.m_room2ExchangePerson.ToString();
                m_content[f++] = item.m_vipExchangePerson.ToString();
                m_content[f++] = item.m_room2EnterCount.ToString();
                m_content[f++] = item.m_room5EnterCount.ToString();
                m_content[f++] = item.m_room6EnterCount.ToString();
                m_content[f++] = item.m_room3EnterCount.ToString();

                m_content[f++] = item.m_giftTime.ToString();

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