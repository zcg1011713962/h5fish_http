using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

namespace WebManager.appaspx
{
    public partial class cdkeyNew : RefreshPageBase
    {
        private static string[] s_head = new string[] { "CDkey","生成时间","失效时间","礼包内容","最大可用次数","当前使用次数","备注","删除"};
        private string[] m_content=new string[s_head.Length];

        protected void Page_Load(object sender, EventArgs e)
        {
            m_res.InnerHtml = "";
            m_opRes.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.OTHER_CD_KEY, Session, Response);

            GMUser user = (GMUser)Session["user"];
            if (!IsPostBack) 
            {
                var allData = M_CDKEY_GiftCFG.getInstance().getAllData();
                Dictionary<string, object> jdGift = new Dictionary<string, object>();
                m_gift.Items.Add(new ListItem("自定义", "0"));
                foreach (var d in allData)
                {
                    m_gift.Items.Add(new ListItem(d.Value.m_name, d.Key.ToString()));
                }
                genTable(user);
            } 
        }

        protected void onSubmit(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];

            if (IsRefreshed)
            {
                genTable(user);
                return;
            }

            ParamCDKEY param = new ParamCDKEY();
            param.m_deadTime = m_deadTime.Text;
            param.m_giftId = m_gift.SelectedValue;
            param.m_pici = m_cdkeyCode.Text;
            param.m_count = m_maxUseCount.Text;
            param.m_comment = m_comment.Text;
            param.m_type = 1;
            param.m_op = 1;//生成
            param.m_itemList = m_customItem.Text;
            DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
            OpRes res = mgr.doDyop(param, DyOpType.opTypeGiftCodeNew,user);
            genTable(user);
            if (res == OpRes.op_res_has_exist)
            {
                m_res.InnerHtml = "该礼包码已经占用";
            }
            else
            {
                m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);
            }
        }

        // 生成统计表
        private void genTable(GMUser user)
        {
            m_result.GridLines = GridLines.Both;
            TableRow tr = new TableRow();
            m_result.Rows.Add(tr);
            TableCell td = null;
           
            int i = 0, j = 0, f = 0 ;
            for (i = 0; i < s_head.Length; i++)
            {
                td = new TableCell();
                tr.Cells.Add(td);
                td.Text = s_head[i];
            }
            OpRes res = user.doQuery(null, QueryType.queryTypeGiftCodeNewList);
            List<giftCodeNewItem> qresult = (List<giftCodeNewItem>)user.getQueryResult(QueryType.queryTypeGiftCodeNewList);
            for (i = 0; i < qresult.Count; i++) 
            {
                f = 0;
                tr = new TableRow();
                m_result.Rows.Add(tr);

                giftCodeNewItem item = qresult[i];
                m_content[f++] = item.m_cdKey;
                m_content[f++] = item.m_time;
                m_content[f++] = item.m_deadTime;
                m_content[f++] = item.m_giftName;
                m_content[f++] = item.m_maxUseCount.ToString();
                m_content[f++] = item.m_curUseCount.ToString();
                m_content[f++] = item.m_comment;
                m_content[f++] = item.m_id;

                for (j = 0; j < s_head.Length; j++)
                {
                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.Attributes.CssStyle.Value = "vertical-align:middle";
                    if (j == 7)
                    {
                        HtmlGenericControl aBtn = new HtmlGenericControl("input");
                        aBtn.Attributes.Add("style", "margin-left:20px;");
                        aBtn.Attributes.Add("type", "button");
                        aBtn.Attributes.Add("value", "点击删除");
                        aBtn.Attributes.Add("class", "btn btn-primary btn_delete");
                        aBtn.Attributes.Add("id",m_content[j]);
                        aBtn.Attributes.Add("index",(i+1).ToString());
                        td.Controls.Add(aBtn);
                    }
                    else 
                    {
                        td.Text = m_content[j];
                    }
                }
            }
        }

        protected void onRefresh(object sender, EventArgs e)
        {
            GMUser user=(GMUser)Session["user"];
            OpRes res = flushToGameServer(0,user);
            m_res.InnerHtml = OpResMgr.getInstance().getResultString(res);

            if (IsRefreshed)
            {
                genTable(user);
                return;
            }
            genTable(user);
        }

        // 刷新到游戏服务器
        public OpRes flushToGameServer(int p, GMUser user)
        {
            string fmt = string.Format("cmd=2&op={0}", p);

            string url = string.Format(DefCC.HTTP_MONITOR, fmt);
            var ret = HttpPost.Get(new Uri(url));
            if (ret != null)
            {
                string retStr = Encoding.UTF8.GetString(ret);
                if (retStr == "ok")
                {
                    return OpRes.opres_success;
                }
            }
            return OpRes.op_res_failed;
        }

    }
}