﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebManager.appaspx.stat.fish
{
    public partial class FishlordBulletHeadStat :  RefreshPageBase
    {
        private static string[] s_head1 = new string[] { "鱼雷ID","渔场","背包"};
        private static string[] s_head2 = new string[] { "使用数量", "系统总支出", "平均支出"};
        private string[] m_content=new string[s_head2.Length];

        private static string[] s_head_1 = new string[] { "日期", "普通鱼雷_23", "青铜鱼雷_24", "白银鱼雷_25", "黄金鱼雷_26", "钻石鱼雷_27" };
        private static string[][] s_head_2 = new string[3][]{
            new string[]{"使用次数","总计支出金币","总计平均金币"},
            new string[]{"渔场次数","渔场人数","渔场支出金币","渔场平均金币"},
            new string[]{"背包次数","背包人数","背包支出金币","背包平均金币"},
        };
        private string[] m_content_1=new string[s_head_1.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            m_res.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_BULLET_HEAD_HEAD, Session, Response);
            if (!IsPostBack) 
            {
                m_queryType.Items.Add("全部");
                m_queryType.Items.Add("渔场");
                m_queryType.Items.Add("背包");
                onQuery(null, null);
            }
        }

        protected void onQuery(object sender, EventArgs e) 
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            OpRes res = user.doQuery(param, QueryType.queryTypeFishlordBulletHeadStat);
            genTable(m_result, res, user);
        }

        protected void onReset(object sender, EventArgs e)
        {
            if (IsRefreshed) 
            {
                onQuery(null,null);
                return;
            }
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            OpRes res = user.doDyop(param, DyOpType.opTypeFishlordBulletHeadReset);
            onQuery(null, null);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
        }

        //弹头查询
        protected void OnStat(object sender, EventArgs e)
        {
            onQuery(null, null);

            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_time = m_time.Text;
            param.m_op = m_queryType.SelectedIndex;
            OpRes res = user.doQuery(param, QueryType.queryTypeFishlordBulletHeadQuery);
            genTable1(m_resTable, res, user,param);
        }

        //生成表
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

            List<fishlordBulletHeadStat> qresult = (List<fishlordBulletHeadStat>)user.getQueryResult(QueryType.queryTypeFishlordBulletHeadStat);

            int i = 0, j = 0, f = 0;
            // 表头 第一行
            for (i = 0; i < s_head1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head1[i];
                if (i == 0)
                {
                    td.ColumnSpan = 1;
                    td.RowSpan = 2;
                }
                else {
                    td.ColumnSpan = 3;
                    td.RowSpan = 1;
                }
                td.Attributes.CssStyle.Value = "vertical-align:middle";
            }

            // 表头 第二行
            tr = new TableRow();
            table.Rows.Add(tr);
            for (j = 0; j < s_head1.Length-1; j++ ) 
            {
                for (i = 0; i < s_head2.Length; i++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = s_head2[i];
                    td.ColumnSpan = 1;
                    td.RowSpan = 1;
                }
            }
           
            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                fishlordBulletHeadStat item = qresult[i];
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_bulletHeadId.ToString();

                if (!item.m_data.ContainsKey(1))
                {
                    fishlordBulletHeadDetail tmp = new fishlordBulletHeadDetail();
                    item.m_data[1] = tmp;
                }

                if (!item.m_data.ContainsKey(2))
                {
                    fishlordBulletHeadDetail tmp = new fishlordBulletHeadDetail();
                    item.m_data[2] = tmp;
                }

                var dataList = item.m_data.OrderBy(a=>a.Key).ToList();
                foreach (var data in dataList)
                {
                    fishlordBulletHeadDetail tmp = data.Value;
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = tmp.m_useCount.ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = tmp.m_outlayGold.ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = tmp.m_avgoutlayGold.ToString();
                }
            }
        }

        //弹头查询表
        private void genTable1(Table table, OpRes res, GMUser user,ParamQuery param)
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

            List<fishlordBulletHeadQuery> qresult = (List<fishlordBulletHeadQuery>)user.getQueryResult(QueryType.queryTypeFishlordBulletHeadQuery);

            int i = 0, j = 0;
            // 表头 第一行
            for (i = 0; i < s_head_1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head_1[i];
                if (i == 0)
                {
                    td.RowSpan = 2;
                    td.ColumnSpan = 1;
                }
                else {
                    td.RowSpan = 1;
                    td.ColumnSpan = (param.m_op == 0) ? 3 : 4;
                }
                td.Attributes.CssStyle.Value = "vertical-align:middle";
            }

            //表头 第二行
            tr = new TableRow();
            table.Rows.Add(tr);
            for (j = 0; j < 5; j++)
            {
                for (i = 0; i < s_head_2[param.m_op].Length; i++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = s_head_2[param.m_op][i];
                    td.RowSpan = 1;
                    td.ColumnSpan = 1;
                }
            }
            int[] itemIds = new int[] { 23, 24, 25, 26, 27 };
            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                table.Rows.Add(tr);

                fishlordBulletHeadQuery item = qresult[i];
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;
               
                td.RowSpan = 1;
               
                foreach(int itemId in itemIds)
                {
                    if (item.m_data.ContainsKey(itemId))
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_data[itemId][0].ToString();

                        if (param.m_op != 0) {
                            td = new TableCell();
                            tr.Cells.Add(td);
                            td.Text = item.m_data[itemId][2].ToString();
                        }

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_data[itemId][1].ToString();

                        td = new TableCell();
                        tr.Cells.Add(td);
                        if (item.m_data[itemId][0] != 0)
                        {
                            td.Text = Math.Round(item.m_data[itemId][1] * 1.0 / item.m_data[itemId][0]).ToString();
                        }
                        else
                        {
                            td.Text = item.m_data[itemId][1].ToString();
                        }
                    }
                    else 
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "";

                        if (param.m_op != 0) {
                            td = new TableCell();
                            tr.Cells.Add(td);
                            td.Text = "";
                        }

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "";

                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = "";
                    }
                }

            }
        }
    }
}