using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.stat
{
    public partial class StatFishLordJingjiData : System.Web.UI.Page
    {
        private static string[] s_head_1 = new string[] { "日期", "参与人数", "参与次数", "门票消耗", "导弹消耗", "锁定消耗", "钻石累计消耗", "钻石任务奖励" };
        private static string[] s_head_2 = new string[] { "日期", "龙珠产出", "金弹头产出", "银弹头产出", "铜弹头产出", "钻石弹头产出" };
        private static string[] s_head_3 = new string[] { "日期","任务ID","接取人数","完成人数","所属组"};

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.FISH_LORD_JINGJI_DATA_STAT,Session,Response);
            if(!IsPostBack)
            {
                m_item.Items.Add("竞技场消耗统计");
                m_item.Items.Add("竞技场产出统计");
                m_item.Items.Add("竞技场任务统计");
                m_item.Items.Add("竞技场玩家分布统计");
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            ParamQuery param = new ParamQuery();
            param.m_way = (QueryWay)m_item.SelectedIndex;
            param.m_time = m_time.Text;
            OpRes res=OpRes.op_res_failed;
            switch(param.m_way)
            {
                case QueryWay.by_way0://竞技场消耗统计
                                res= user.doQuery(param, QueryType.queryTypeFishlordJingjiConsumeStat);
                                genTable0(m_result, res, user, param);
                                break;
                case QueryWay.by_way1: //竞技场产出统计
                                res = user.doQuery(param, QueryType.queryTypeFishlordJingjiOutlayStat);
                                genTable1(m_result, res, user, param);
                                break;
                case QueryWay.by_way2: //竞技场任务统计
                                res = user.doQuery(param, QueryType.queryTypeFishlordJingjiTaskStat);
                                genTable2(m_result, res, user, param);
                                break;
                case QueryWay.by_way3: //竞技场玩家分布统计
                                res = user.doQuery(param, QueryType.queryTypeFishlordJingjiPlayerStat);
                                genTable3(m_result, res, user, param);
                                break;
            }
            
        }
        //竞技场消耗统计
        private void genTable0(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content=new string[s_head_1.Length];

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

            List<JingjiConsumeStat> qresult = (List<JingjiConsumeStat>)user.getQueryResult(QueryType.queryTypeFishlordJingjiConsumeStat);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head_1.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.RowSpan = 1;
                td.Text = s_head_1[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                JingjiConsumeStat item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_joinPerson.ToString();
                m_content[f++] = item.m_joinCount.ToString();
                m_content[f++] = item.m_enterTicket.ToString();
                m_content[f++] = item.m_item2.ToString();
                m_content[f++] = item.m_item1.ToString();
                m_content[f++] = item.m_itemTotal.ToString();
                m_content[f++] = item.m_quest.ToString();

                for (j = 0; j < s_head_1.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //竞技场产出统计
        private void genTable1(Table table, OpRes res, GMUser user, ParamQuery query_param)
        {
            string[] m_content=new string[s_head_2.Length];

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

            List<JingjiOutlayStat> qresult = (List<JingjiOutlayStat>)user.getQueryResult(QueryType.queryTypeFishlordJingjiOutlayStat);

            int i = 0, j = 0, f = 0;
            // 表头
            for (i = 0; i < s_head_2.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.RowSpan = 1;
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.Text = s_head_2[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);
                JingjiOutlayStat item = qresult[i];

                m_content[f++] = item.m_time;
                m_content[f++] = item.m_dbOutlay.ToString();
                m_content[f++] = item.m_goldBullet.ToString();
                m_content[f++] = item.m_silverBullet.ToString();
                m_content[f++] = item.m_copperBullet.ToString();
                m_content[f++] = item.m_diamondBullet.ToString();

                for (j = 0; j < s_head_2.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.RowSpan = 1;
                    td.Text = m_content[j];
                }
            }
        }

        //竞技场任务统计
        private void genTable2(Table table, OpRes res, GMUser user, ParamQuery query_param)
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

            List<JingjiTaskStat> qresult = (List<JingjiTaskStat>)user.getQueryResult(QueryType.queryTypeFishlordJingjiTaskStat);

            int i = 0, k = 0;
            // 表头
            for (i = 0; i < s_head_3.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.RowSpan = 1;
                td.Attributes.CssStyle.Value = "min-width:140px";
                td.Text = s_head_3[i];
            }

            for (i = 0; i < qresult.Count; i++)
            {
                JingjiTaskStat item = qresult[i];
                List<TaskItem> taskList = item.m_taskList;
                for (k = 0; k<taskList.Count; k++ )
                {
                    tr = new TableRow();
                    m_result.Rows.Add(tr);
                    if(k == 0)
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);
                        td.Text = item.m_time;
                        td.RowSpan = item.m_idNum;
                        td.Attributes.CssStyle.Value = "vertical-align:middle";
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = taskList[k].m_taskId.ToString();
                    td.RowSpan = 1;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = taskList[k].m_taskJoin.ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = taskList[k].m_taskFinish.ToString();

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = taskList[k].m_groupId.ToString();
                }
            }
        }

        //竞技场玩家分布统计
        private void genTable3(Table table, OpRes res, GMUser user, ParamQuery query_param)
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

            List<JingjiPlayerStat> qresult = (List<JingjiPlayerStat>)user.getQueryResult(QueryType.queryTypeFishlordJingjiPlayerStat);

            int i = 0, j = 0;
            // 表头
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "日期";
            td.Attributes.CssStyle.Value = "vertical-align:middle;";
            td.RowSpan = 2;

            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "等级";
            td.Attributes.CssStyle.Value = "min-width:80px;";

            for (i = 19; i <= 59; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = "Level_"+i;
            }

            tr = new TableRow();
            tr.Cells.Clear();
            m_result.Rows.Add(tr);
            td = new TableCell();
            tr.Cells.Add(td);
            td.Text = "倍率";

            for (i = 19; i <= 59; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);

                Fish_LevelCFGData data = Fish_LevelCFG.getInstance().getValue(i);
                if (data != null)
                {
                    td.Text = data.m_openRate.ToString();
                }else 
                {
                    td.Text = "Level_" + i;
                }
            }

            for (i = 0; i < qresult.Count; i++)
            {
                tr = new TableRow();
                m_result.Rows.Add(tr);
                JingjiPlayerStat item = qresult[i];

                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = item.m_time;

                if(i==0)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = "参与人数";
                    td.Attributes.CssStyle.Value = "vertical-align:middle;";
                    td.RowSpan = qresult.Count;
                }
                for (int k = 0; k<item.m_data.Length;k++ ) 
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = item.m_data[k].ToString();
                    td.ColumnSpan = 1;
                }
            }
        }
    }
}