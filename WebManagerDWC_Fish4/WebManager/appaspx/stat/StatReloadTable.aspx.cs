using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace WebManager.appaspx.stat
{
    public partial class StatReloadTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            m_res.InnerHtml = "";
            m_res1.InnerHtml = "";
            RightMgr.getInstance().opCheck(RightDef.DATA_RELOAD_TABLE, Session, Response);

            if (!IsPostBack)
            {
                m_table.Items.Add("经典捕鱼鱼表");
                m_table.Items.Add("平台账号表");
            }
        }

        protected void onLoad(object sender, EventArgs e)
        {
            GMUser user = (GMUser)Session["user"];
            
            OpRes  res = OpRes.op_res_failed;
            if (m_table.SelectedIndex == 1) //平台账号表
            {
                string param = "cmd=4";
                res = flushToGameServer(param);
                m_res.InnerHtml = "刷新服务器加载数据" + OpResMgr.getInstance().getResultString(res);
            }
            else 
            {
                DyOpMgr mgr = user.getSys<DyOpMgr>(SysType.sysTypeDyOp);
                res = mgr.doDyop(m_table.SelectedIndex, DyOpType.opTypeReLoadTable, user);
                m_res.InnerHtml = "加载" + OpResMgr.getInstance().getResultString(res);
            }
        }

        //开 性能测试日志功能
        protected void onOpen(object sender, EventArgs e)
        {
            string param = "cmd=5&open=1";
            OpRes res = flushToGameServer(param);
            m_res1.InnerHtml = "开启性能测试日志功能：" + OpResMgr.getInstance().getResultString(res);
        }
        //关 性能测试日志功能
        protected void onClose(object sender, EventArgs e)
        {
            string param = "cmd=5&open=0";
            OpRes res = flushToGameServer(param);
            m_res1.InnerHtml = "关闭性能测试日志功能：" + OpResMgr.getInstance().getResultString(res);
        }

        // 刷新到游戏服务器
        public OpRes flushToGameServer(string param)
        {
            string url = string.Format(DefCC.HTTP_MONITOR, param);
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