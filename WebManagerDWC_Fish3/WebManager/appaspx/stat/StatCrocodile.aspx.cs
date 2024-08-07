﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace WebManager.appaspx.stat
{
    public partial class StatCrocodile : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "房间", "期望盈利率", "系统总收入", "系统总支出", "盈亏情况", "实际盈利率", "当前人数",
            "广告版下注阀值","大天堂放分概率（万分值）","小天堂放分概率（万分值）","大地狱杀分概率","小地狱杀分概率", "选择" };
        private string[] m_content = new string[s_head.Length];
        private string m_roomList = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.CROD_PARAM_CONTROL, Session, Response);

            if (!IsPostBack)
            {
                m_paramItem.Items.Add(new ListItem("期望盈利率"));
                m_paramItem.Items.Add(new ListItem("广告版下注阀值"));
                m_paramItem.Items.Add(new ListItem("大天堂放分概率"));
                m_paramItem.Items.Add(new ListItem("小天堂放分概率"));
                m_paramItem.Items.Add(new ListItem("大地狱杀分概率"));
                m_paramItem.Items.Add(new ListItem("小地狱杀分概率"));
                genExpRateTable(m_expRateTable);
            }
            else
            {
                m_roomList = Request["roomList"];
                if (m_roomList == null)
                {
                    m_roomList = "";
                }
            }
        }

        protected void onModifyExpRate(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            ParamFishlordParamAdjust p = new ParamFishlordParamAdjust();
            p.m_isReset = false;
            p.m_expRate = m_expRate.Text;
            p.m_op = m_paramItem.SelectedIndex;

            if (m_roomList == "")
            {
                m_res.InnerHtml = "请选择房间";
                genExpRateTable(m_expRateTable);
            }
            else 
            {
                p.m_roomList = m_roomList;
                DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
                OpRes res = mgr.doDyop(p, DyOpType.opTypeCrocodileParamAdjust, user);
                m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
                genExpRateTable(m_expRateTable);
            } 
        }

        protected void onReset(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            ParamFishlordParamAdjust p = new ParamFishlordParamAdjust();
            p.m_isReset = true;
            p.m_roomList = m_roomList;

            DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
            OpRes res = mgr.doDyop(p, DyOpType.opTypeCrocodileParamAdjust, user);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
            genExpRateTable(m_expRateTable);
        }

        // 期望盈利率表格
        protected void genExpRateTable(Table table)
        {
            GMUser user = (GMUser)Session["user"];

            table.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            int i = 0;
            for (; i < s_head.Length; i++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }

            long totalIncome = 0;
            long totalOutlay = 0;

            QueryMgr mgr = user.getSys<QueryMgr>(SysType.sysTypeQuery);
            OpRes res = mgr.doQuery(null, QueryType.queryTypeCrocodileParam, user);
            Dictionary<int, ResultFishlordExpRate> qresult
                = (Dictionary<int, ResultFishlordExpRate>)mgr.getQueryResult(QueryType.queryTypeCrocodileParam);

            for (i = 1; i <= StrName.s_roomName.Length; i++)
            {
                m_content[0] = StrName.s_roomName[i - 1];
                if (qresult.ContainsKey(i))
                {
                    ResultFishlordExpRate r = qresult[i];
                    m_content[1] = r.m_expRate.ToString();
                    m_content[2] = r.m_totalIncome.ToString();
                    m_content[3] = r.m_totalOutlay.ToString();
                    m_content[4] = (r.m_totalIncome - r.m_totalOutlay).ToString();
                    m_content[5] = r.getFactExpRate();
                    m_content[6] = r.m_curPlayerCount.ToString();
                    m_content[7] = r.m_appearAdvertisementLessBet.ToString();
                    m_content[8] = r.m_bigParadiseGiveScoreProb.ToString();
                    m_content[9] = r.m_smallParadiseGiveScoreProb.ToString();
                    m_content[10] = r.m_bigHellKillScoreProb.ToString();
                    m_content[11] = r.m_smallHellKillScoreProb.ToString();
                    totalIncome += r.m_totalIncome;
                    totalOutlay += r.m_totalOutlay;
                }
                else
                {
                    m_content[1] = "0.05";
                    m_content[2] = "0";
                    m_content[3] = "0";
                    m_content[4] = "0";
                    m_content[5] = "0";
                    m_content[6] = "0";
                    m_content[7] = "0";
                    m_content[8] = "0";
                    m_content[9] = "0";
                    m_content[10] = "0";
                    m_content[11] = "0";
                }
                m_content[12] = Tool.getCheckBoxHtml("roomList", i.ToString(), false);

                tr = new TableRow();
                table.Rows.Add(tr);
                for (int j = 0; j < s_head.Length; j++)
                {
                    TableCell td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];

                    if (j == 4)
                    {
                        setColor(td, m_content[j]);
                    }
                }
            }

            addStatFoot(table, totalIncome, totalOutlay);
        }

        // 增加统计页脚
        protected void addStatFoot(Table table, long totalIncome, long totalOutlay)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);
            m_content[0] = "总计";
            m_content[1] = "";
            // 总收入
            m_content[2] = totalIncome.ToString();
            // 总支出
            m_content[3] = totalOutlay.ToString();
            // 总盈亏
            m_content[4] = (totalIncome - totalOutlay).ToString();
            m_content[5] = "";
            m_content[6] = "";
            m_content[7] = "";
            m_content[8] = "";
            m_content[9] = "";
            m_content[10] = "";
            m_content[11] = "";
            m_content[12] = "";
            for (int j = 0; j < s_head.Length; j++)
            {
                TableCell td = new TableCell();
                tr.Cells.Add(td);
                td.Text = m_content[j];

                if (j == 4)
                {
                    setColor(td, m_content[j]);
                }
            }
        }

        protected void setColor(TableCell td, string num)
        {
            if (num[0] == '-')
            {
                td.ForeColor = Color.Red;
            }
            else
            {
                td.ForeColor = Color.Green;
            }
        }
    }
}