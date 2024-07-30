using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx.service
{
    public partial class ServiceGuideTheLossOfPlayers : System.Web.UI.Page
    {
        private static string[] s_head = new string[] { "引导编号","接收引导时间","玩家ID","用户名","手机号","渠道",
            "所选时间内充值金额","vip等级","注册时间","最后登录时间","剩余金币","回流引导时间","备注"};
        private string[] m_content = new string[s_head.Length];
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void onSubmit(object sender, EventArgs e)
        {

        }

        protected void onQuery(object sender, EventArgs e)
        {

        }
    }
}