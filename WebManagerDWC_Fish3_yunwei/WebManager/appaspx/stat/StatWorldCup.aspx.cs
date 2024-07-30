using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace WebManager.appaspx.stat
{
    public partial class StatWorldCup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.DATA_STAT_WORLD_CUP_MATCH, Session, Response);
            m_res.InnerHtml = "";
            if (!IsPostBack)
            { 
                //组别
                Dictionary<int, WorldCupGroup> groupList = WorldCupGroupCFG.getInstance().getAllData();
                foreach (var group in groupList.Values)
                {
                    m_groupId.Items.Add(new ListItem(group.m_groupName,group.m_groupId.ToString()));
                }

                //比赛类型
                m_typeId.Items.Add(new ListItem("小组赛","0"));
                m_typeId.Items.Add(new ListItem("决赛","1"));

                //队伍1  队伍2
                Dictionary<int, WorldCupNationItem> nationList = WorldCupNationCFG.getInstance().getAllData();
                foreach(var nation in nationList.Values)
                {
                    m_team1.Items.Add(new ListItem(nation.m_nationName,nation.m_nationId.ToString()));
                    m_team2.Items.Add(new ListItem(nation.m_nationName, nation.m_nationId.ToString()));
                }
            }
        }

        protected void onReFresh(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            //记录条数>0,服务器禁止刷新
            DbInfoParam dip = ItemHelp.createDbParam(user.getDbServerID(), DbName.DB_PLAYER, DbInfoParam.SERVER_TYPE_SLAVE);
            user.totalRecord = DBMgr.getInstance().getRecordCount(TableName.WORLD_CUP_MATCH_REWARD, null, dip);

            //时间
            DateTime time1 = Convert.ToDateTime("11:59"), time2 = Convert.ToDateTime("12:00");
            DateTime now = Convert.ToDateTime(DateTime.Now.ToString("hh:mm:ss"));

            if (user.totalRecord > 0)
            {
                m_res.InnerHtml = "服务器正在结算奖励，暂不能刷新";
            }
            else if (DateTime.Compare(now, time1) >= 0 && DateTime.Compare(time2, now) >= 0)
            {
                m_res.InnerHtml = "服务器正在结算奖励，暂不能刷新";
            }
            else
            {
                OpRes res = flushToGameServer(1, user);
                m_res.InnerHtml = "刷新服务器：" + OpResMgr.getInstance().getResultString(res);
            }
        }

        // 刷新到游戏服务器
        public OpRes flushToGameServer(int p, GMUser user)
        {
            string fmt = string.Format("cmd=6&op={0}", p);
            string urlIp = DefCC.HTTP_MONITOR;

            DbServerInfo dbInfo = ResMgr.getInstance().getDbInfo(user.m_dbIP);
            if (dbInfo != null)
            {
                if (!string.IsNullOrEmpty(dbInfo.m_monitor))
                    urlIp = dbInfo.m_monitor;
            }
            
            string url = string.Format(urlIp, fmt);
            var ret = HttpPost.Get(new Uri(url));
            if (ret != null)
            {
                string retStr = Encoding.UTF8.GetString(ret);
                if (retStr == "ok")
                    return OpRes.opres_success;
            }
            return OpRes.op_res_failed;
        }
    }
}