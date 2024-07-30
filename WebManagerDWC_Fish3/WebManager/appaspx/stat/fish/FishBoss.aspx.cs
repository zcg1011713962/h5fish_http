using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.fish
{
    public partial class FishBoss : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "日期","场次","BOSS名称", "BOSS出现次数", "BOSS死亡次数", "金币消耗", "放出碎片","放出魔石",
            "使用锁定", "使用狂暴", "攻击人次","BOSS放出金币","碎片金币折合总计" ,"每BOSS系统盈利"};
        private string[] m_content = new string[s_head.Length];

        private static int[] s_boss = { 21, 701, 702, 703, 704, 705, 706, 708, 709, 710, 711 };

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_BOSS_CONSUME, Session, Response);

            if (!IsPostBack)
            {
                m_room.Items.Add(new ListItem("全部", "0"));
                foreach(var room in StrName.s_roomList)
                {
                    if (string.IsNullOrEmpty(room.Value))
                        continue;
                    m_room.Items.Add(new ListItem(room.Value,room.Key.ToString()));
                }

                //boss鱼
                m_bossList.Items.Add(new ListItem("全部", "0"));
                foreach(int bossId in s_boss)
                {
                    var Fish = FishCFG.getInstance().getValue(bossId);
                    if (Fish != null)
                    {
                        m_bossList.Items.Add(new ListItem(Fish.m_fishName, bossId.ToString()));
                    }
                    else {
                        m_bossList.Items.Add(new ListItem(bossId.ToString(), bossId.ToString()));
                    }
                }//

            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamFishBoss param = new ParamFishBoss();
            param.m_roomId = Convert.ToInt32(m_room.SelectedValue);
            param.time = m_time.Text;
            param.m_bossId = Convert.ToInt32(m_bossList.SelectedValue);
            OpRes res = user.doQuery(param, QueryType.queryTypeFishBoss);
            genTable(m_result, res, user);
        }

        private void genTable(Table table, OpRes res, GMUser user)
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

            int i = 0, k = 0, n = 0 ;

            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            List<ResultFishBoss> qresult =
                (List<ResultFishBoss>)user.getQueryResult(0,QueryType.queryTypeFishBoss);

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                if ((i & 1) == 0)
                {
                    //tr.CssClass = "alt";
                }
                m_result.Rows.Add(tr);
                ResultFishBoss item = qresult[i];

                n = 0;
                m_content[n++] = item.m_date;
                m_content[n++] = item.getRoomName();
                m_content[n++] = item.getBossName();
                m_content[n++] = item.m_bossCount.ToString();
                m_content[n++] = item.m_bossDieCount.ToString();
                m_content[n++] = item.m_consumeGold.ToString();
                m_content[n++] = item.m_totalReleaseChip.ToString(); //放出碎片
                m_content[n++] = item.m_totalReleaseDimensity.ToString();//放出魔石
                m_content[n++] = item.m_bossItemLock.ToString(); //使用锁定
                m_content[n++] = item.m_bossItemViolent.ToString();//使用狂暴

                m_content[n++] = item.m_hitBossPersonTime.ToString(); //攻击人次
                m_content[n++] = item.m_totalReleaseGold.ToString(); //BOSS放出金币

                m_content[n++] = item.getBossZheKouTotal().ToString();
                m_content[n++] = item.getEarnEachBoss();

                for (k = 0; k < s_head.Length; k++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[k];
                }
            }
        }
    }
}