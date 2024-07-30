using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.playerinfo
{
    public partial class PlayerBasicInfo : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "注册时间","玩家ID","昵称","渠道","等级","当前炮倍","VIP等级","充值BUFF","新手BUFF","特殊充值BUFF","特殊新手BUFF",
            "当前贡献","历史贡献","总消耗金币数量","金币数量","钻石数量","话费数量","碎片数量",
            "最大金币数","最大钻石数","总充值额","充值次数","最大充值金额","最后充值时间","最后在线时间",
            "狂暴数量","精准数量","冰冻数量","召唤数量","使用狂暴数量","使用精准数量","使用冰冻数量","使用召唤数量","领取救济金次数","背包"};
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.PLAYER_PLAYER_BASIC_INFO, Session, Response);
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_param = m_playerIds.Text;
            OpRes res = user.doQuery(param, QueryType.queryTypeStatPlayerBasicInfo);
            genTable(m_resTable, res, user, param.m_param);
        }

        //生成表
        private void genTable(Table table, OpRes res, GMUser user, string playerList)
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

            List<playerBasicInfo> qresult = (List<playerBasicInfo>)user.getQueryResult(QueryType.queryTypeStatPlayerBasicInfo);

            int i = 0, j = 0, f = 0;
            // 表头 第一行
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.ColumnSpan = 1;
                td.RowSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                table.Rows.Add(tr);

                playerBasicInfo item = qresult[i];
                m_content[f++] = item.m_createTime;
                m_content[f++] = item.m_playerId.ToString();
                m_content[f++] = item.m_nickname;
                m_content[f++] = item.getChannelName();
                m_content[f++] = item.m_playerLv.ToString();
                m_content[f++] = item.getTurretRate();
                m_content[f++] = item.m_vipLv.ToString();

                m_content[f++] = item.m_playerRecDominantGoldPool.ToString();
                m_content[f++] = item.m_newPlayerDominantGoldPool.ToString();
                m_content[f++] = item.m_sPlayerRecDominantGoldPool.ToString();
                m_content[f++] = item.m_sNewPlayerDominantGoldPool.ToString();

                m_content[f++] = item.m_contributionValue.ToString();
                m_content[f++] = item.m_contributeHistory.ToString();
                m_content[f++] = item.m_playerGoldOutlay.ToString(); //玩家总金币消耗

                m_content[f++] = item.m_gold.ToString();
                m_content[f++] = item.m_diamond.ToString();
                m_content[f++] = (item.m_ticket/100.0).ToString();

                m_content[f++] = item.m_torpedoChip.ToString();//碎片数量

                m_content[f++] = item.m_maxGold.ToString();
                m_content[f++] = item.m_maxDiamond.ToString();
                m_content[f++] = item.m_totalRecharge.ToString();
                m_content[f++] = item.m_rechargeCount.ToString();
                m_content[f++] = item.m_maxRecharge.ToString();
                m_content[f++] = item.m_lastRechargeTime;
                m_content[f++] = item.m_lastLogoutTime;

                m_content[f++] = item.m_item17.ToString();
                m_content[f++] = item.m_item5.ToString();
                m_content[f++] = item.m_item8.ToString();
                m_content[f++] = item.m_item9.ToString();
                m_content[f++] = item.m_useItem17.ToString();
                m_content[f++] = item.m_useItem5.ToString();
                m_content[f++] = item.m_useItem8.ToString();
                m_content[f++] = item.m_useItem9.ToString();
                m_content[f++] = item.m_hasRecvAlmsCount.ToString();
                m_content[f++] = item.getExParam(); //背包详情

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