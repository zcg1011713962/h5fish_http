using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.operationNew
{
    public partial class OperationNewTurretTimes : System.Web.UI.Page
    {
        private static string[] s_head = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OPNEW_TURRET_TIMES, Session, Response);
            if (!IsPostBack)
            {
                m_channel.Items.Add(new ListItem("总体", "-1"));
                m_channel.Items.Add(new ListItem("安卓总体","-2"));
                Dictionary<string, TdChannelInfo> data = TdChannel.getInstance().getAllData();
                foreach (var item in data.Values)
                {
                    m_channel.Items.Add(new ListItem(item.m_channelName, item.m_channelNo));
                }
            }
        }

        protected void OnStat(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            ParamQuery param = new ParamQuery();
            param.m_param = m_channel.SelectedValue;
            param.m_time = m_time.Text;
            OpRes res = user.doQuery(param, QueryType.queryTypeOpnewTurretTimes);
            genTable(user, m_resTable, res);
        }

        public void genTable(GMUser user, Table table, OpRes res)
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

            int i = 0, f = 0;

            s_head = new string[45];
            string[] m_content = new string[s_head.Length];

            s_head[0] = "日期";
            s_head[1] = "活跃人数";

            for(i = 1; i <= 43; i++)
            {
                var turretLevel = Fish_TurretLevelCFG.getInstance().getValue(i);
                
                int openRate = i;
                if(turretLevel != null)
                    openRate = turretLevel.m_openRate;

                s_head[i+1] = openRate + "倍人数";
            }

            for (i = 0 ; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
                td.RowSpan = 1;
                td.ColumnSpan = 1;
            }

            List<TurretTimesItem> qresult = (List<TurretTimesItem>)user.getQueryResult(QueryType.queryTypeOpnewTurretTimes);
            foreach (var data in qresult)
            {
                tr = new TableRow();
                table.Rows.Add(tr);
                f = 0;
                m_content[f++] = data.m_time;
                m_content[f++] = data.m_active.ToString();
                foreach (var da in data.m_turretCount) 
                {
                    m_content[f++] = da.Value.ToString();
                }

                for (int j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Text = m_content[j];
                }
            }
        }
    }
}