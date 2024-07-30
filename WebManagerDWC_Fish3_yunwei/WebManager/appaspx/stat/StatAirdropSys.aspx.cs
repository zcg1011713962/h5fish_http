﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.stat
{
    public partial class StatAirdropSys : RefreshPageBase
    {
        private static string[] s_head = new string[] { "日期","空投人","空投ID","空投暗码","空投道具：数量","空投状态","领取人","被打开次数","打开人数","打开次数收入总计","操作"};
        private static string[] s_head1 = new string[] { "日期", "空投ID", "空投道具：数量", "发布玩家ID", "领取玩家ID"};
        private string[] m_content = new string[s_head.Length];

        private PageGenAirdropSys m_gen = new PageGenAirdropSys(50);

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_AIR_DROP_SYS, Session, Response);
            if (!IsPostBack)
            {
                m_state.Items.Add("全部");
                m_state.Items.Add("白银鱼雷");
                m_state.Items.Add("黄金鱼雷");
                m_state.Items.Add("钻石鱼雷");

                m_type.Items.Add("发布");
                m_type.Items.Add("打开");

                m_airDropType.Items.Add("玩家");
                m_airDropType.Items.Add("系统");

                if (m_gen.parse(Request)) 
                {
                    m_state.SelectedIndex = m_gen.m_state - 24;
                    m_playerId.Text = m_gen.m_playerId.ToString();
                    m_type.SelectedIndex = m_gen.m_type;

                    m_airDropType.SelectedIndex = 1 - m_gen.m_airDropType;

                    m_time.Text = m_gen.m_time;

                    onQuery(null, null);
                }
            }
        }

        protected void onQuery(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = OpRes.op_res_failed;

            ParamQuery param = new ParamQuery();

            //全部24 白银鱼雷25 黄金鱼雷26 钻石鱼雷27
            param.m_op = m_state.SelectedIndex + 24;
            param.m_playerId = m_playerId.Text;
            param.m_type = m_type.SelectedIndex; //打开0 发布1

            param.m_curPage = m_gen.curPage;
            param.m_countEachPage = m_gen.rowEachPage;
            param.m_param = (1- m_airDropType.SelectedIndex).ToString();

            if (param.m_type == 1) //打开
            {
                param.m_time = m_time.Text;
                res = mgr.doQuery(param, QueryType.queryTypeStatAirDropSysOpen, user);
                genTable1(m_result, res, user, param);
            }
            else  //发布
            {
                res = mgr.doQuery(param, QueryType.queryTypeStatAirDropSys, user);
                genTable(m_result, res, user, param);
            }
        }

        //打开 统计表
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery param)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

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

            List<StatAirdropSysOpenItem> qresult =
                (List<StatAirdropSysOpenItem>)user.getQueryResult(QueryType.queryTypeStatAirDropSysOpen);

            FishBaseInfoData openGold = FishBaseInfoCFG.getInstance().getValue("OpenGold");

            int i = 0, j = 0;
            // 表头
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            for (i = 0; i < qresult.Count; i++)
            {
                StatAirdropSysOpenItem item = qresult[i];
                tr = new TableRow();
                table.Rows.Add(tr);

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;
                td.RowSpan = 1;
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_uuid.ToString();
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.getItemName() + item.m_itemId + "：" + item.m_itemCount;
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_playerId.ToString();
                td.Attributes.CssStyle.Value = "vertical-align:middle";

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_receiveID.ToString();
                td.Attributes.CssStyle.Value = "vertical-align:middle";

            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatAirdropSys.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;

        }

        //发布 统计表
        private void genTable(Table table, OpRes res, GMUser user, ParamQuery param)
        {
            m_page.InnerHtml = "";
            m_foot.InnerHtml = "";

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

            List<StatAirdropSysItem> qresult =
                (List<StatAirdropSysItem>)user.getQueryResult(QueryType.queryTypeStatAirDropSys);

            int i = 0, j = 0, f = 0;
            string[] m_content = new string[s_head.Length];

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
                StatAirdropSysItem item = qresult[i];
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_playerId;
                m_content[f++] = item.m_uuid.ToString();
                m_content[f++] = item.m_pwd;
                m_content[f++] = item.getItemName() + item.m_itemId + "：" + item.m_itemCount;
                m_content[f++] = item.m_state;
                m_content[f++] = item.m_recvId.ToString();
                m_content[f++] = item.m_checkmapCount.ToString();
                m_content[f++] = item.m_checkmapPerson.ToString();
                m_content[f++] = item.m_checkmapTotal.ToString();
                m_content[f++] = "";

                tr = new TableRow();
                table.Rows.Add(tr);
                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                    td.Attributes.CssStyle.Value = "vertical-align:middle";
                    td.RowSpan = 1;

                    if (j == 10)
                    {
                        HtmlGenericControl alink = new HtmlGenericControl();
                        alink.TagName = "a";
                        alink.InnerText = "下架";
                        alink.Attributes.Add("class", "btn btn-primary");
                        alink.Attributes.Add("id", item.m_uuid.ToString());
                        td.Controls.Add(alink);
                    }
                }
            }

            string page_html = "", foot_html = "";
            m_gen.genPage(param, @"/appaspx/stat/StatAirdropSys.aspx", ref page_html, ref foot_html, user);
            m_page.InnerHtml = page_html;
            m_foot.InnerHtml = foot_html;

        }
    }
}