using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat.gamedetail
{
    public partial class GameDetailFruit : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "押注区域", "倍率", "押注", "得奖" };
        private string[] m_content = new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck("", Session, Response);
            string indexStr = Request.QueryString["index"];
            if (string.IsNullOrEmpty(indexStr))
                return;

            int index = 0;
            if (!int.TryParse(indexStr, out index))
                return;

            GMUser user = (GMUser)Session["user"];
            GameDetailInfo ginfo = GameDetail.parseGameInfo(GameId.fruit, index, user);
            genInfoPanel(ginfo);
        }

        private void genInfoPanel(GameDetailInfo ginfo)
        {
            if (ginfo == null)
                return;

            MoneyItem item = ginfo.m_item;
            InfoCrocodile info = (InfoCrocodile)ginfo.m_detailInfo;
            divHead.InnerText = item.m_genTime;
            // 玩家ID
            tdPlayerFruit.InnerText = "玩家ID:" + item.m_playerId.ToString();

            fillResultInfo(info);

            genBetTable(tableBet, info);
        }

        private void fillResultInfo(InfoCrocodile info)
        {
            if (info == null)
                return;

            e_award_type_def type = info.getResultType((int)GameId.fruit);
            switch (type)
            {
                case e_award_type_def.e_type_normal: //正常
                        addResultImg(divNormalResult, info);
                        break;

                case e_award_type_def.e_type_spot_light: //射灯
                        addResultImg(divSpotLightResult, info);
                        break;

                default: //三七机
                    {
                        DbCrocodileResultType t = info.m_resultType[0];

                        if(type == e_award_type_def.e_type_st_slot) //三七机
                            tdAllPrizesResult.InnerText = t.ts_reward + "倍";
                        
                        if(type==e_award_type_def.e_type_boss_award) //联机大奖
                            tdAllPrizesResult.InnerText = t.ts_reward + "金币";
                    }
                    break;
            }
        }

        private void addResultImg(System.Web.UI.HtmlControls.HtmlGenericControl div, InfoCrocodile info)
        {
            foreach (var res in info.m_resultType)
            {
                if (string.IsNullOrEmpty(res.param))
                    continue;

                string[] m_ids = res.param.Split(',');
                Panel p = new Panel();
                foreach (var id in m_ids)
                {
                    int posId = Convert.ToInt32(id);
                    if (posId == -1 || posId == 11 || posId == 23)
                        continue;

                    Fruit_PosData data = Fruit_PosCFG.getInstance().getValue(posId);
                    if (data != null)
                    {
                        Image img = new Image();
                        img.ImageUrl = "/data/image/fruit/" + data.m_icon;
                        p.CssClass = "clr";
                        p.Controls.Add(img);
                    }
                }
                div.Controls.Add(p);
            }
        }

        // 下注表
        protected void genBetTable(Table table, InfoCrocodile info)
        {
            GMUser user = (GMUser)Session["user"];

            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            int i = 0;
            //表头
            for (; i < s_head.Length; i++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            long totalBet = 0, totalWin = 0;
            // 1-8个区域
            int index = 0;
            for (i = 1; i < 9; i++)
            {
                index = i;
                if (i > 1)
                    index = i + 1;

                Crocodile_RateCFGData data = Fruit_RateCFG.getInstance().getValue(index);
                m_content[0] = data.m_name;
                BetInfoCrocodile item = info.getBetInfo(i);
                if (item != null)
                {
                    m_content[1] = "";
                    m_content[2] = item.bet_count.ToString();
                    totalBet += item.bet_count;

                    m_content[3] = item.award_count.ToString();
                    totalWin += item.award_count;
                }
                else
                {
                    m_content[1] = m_content[2] = m_content[3] = "";
                }

                tr = new TableRow();
                table.Rows.Add(tr);
                for (int j = 0; j < s_head.Length; j++)
                {
                    TableCell td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }

            DbCrocodileResultType t = info.m_resultType[0];
            tr = new TableRow();
            table.Rows.Add(tr);
            string ts_reward = "";
            if(t.type=="st_slot") //三七机  //倍率
            {
                long reward_gold=Convert.ToInt32(t.ts_reward)*totalBet;
                totalWin += reward_gold;
                ts_reward = reward_gold.ToString();
                
            }
            string[] m_data1 = { "三七机", "", "", ts_reward};

            foreach(var d in m_data1)
            {
                TableCell td1 = new TableCell();
                tr.Cells.Add(td1);
                td1.Text = d;
            }
            /////////////////////////////////////////////////////////////////////////////////////
            tr = new TableRow();
            table.Rows.Add(tr);
            if (t.type == "boss_award") //联机大奖
            {
                ts_reward = t.ts_reward;
                totalWin += Convert.ToInt32(t.ts_reward);
            }
            else 
            {
                ts_reward = "";
            }
            string[] m_data2 = { "联机大奖", "", "", ts_reward};

            foreach (var d in m_data2)
            {
                TableCell td1 = new TableCell();
                tr.Cells.Add(td1);
                td1.Text = d;
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
            //tr = new TableRow();
            //table.Rows.Add(tr);
            //string bs_gold = "";
            //if(!string.IsNullOrEmpty(t.bs_gold))
            //{
            //    bs_gold = t.bs_gold;
            //    totalWin += Convert.ToInt32(t.bs_gold);
            //}
            //string[] m_data3 = { "比大小投入", "", "比大小盈利", t.bs_gold};
            //foreach (var d in m_data3)
            //{
            //    TableCell td1 = new TableCell();
            //    tr.Cells.Add(td1);
            //    td1.Text = d;
            //}

            addStatFoot(table, totalBet, totalWin);
        }

        private void addImg(ControlCollection parent, Crocodile_RateCFGData data, string cssName = "")
        {
            if (data != null)
            {
                Panel p = new Panel();
                Image img = new Image();
                img.ImageUrl = "/data/image/fruit/" + data.m_icon;
                p.Controls.Add(img);

                Panel tmp = new Panel();
                tmp.CssClass = "clr";
                parent.Add(tmp);
            }
        }

        // 增加统计页脚
        protected void addStatFoot(Table table, long totalBet, long totalWin)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            m_content[0] = "总计";
            m_content[1] = "";
            m_content[2] = totalBet.ToString();
            m_content[3] = totalWin.ToString();

            for (int j = 0; j < s_head.Length; j++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];
            }
        }
    }
}