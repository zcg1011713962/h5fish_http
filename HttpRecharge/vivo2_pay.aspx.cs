using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    /*
       游戏名：至尊电玩捕鱼
        包名：com.yoyangs.zhizun.vivo
        CPID：20151207111019198783
        APPKEY：83525dd6cad0b89ee67300cdc9d87900
        APPID：963e674f4d48a7b91d85df0cebaab18b
     */
    public partial class vivo2_pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackVIVO pay = new PayCallbackVIVO();
            pay.PayTableName = PayTable.VIVO2_PAY;
            pay.VivoCpKey = PayTable.VIVO_CP_KEY2;

            string str = pay.notifyPay(Request);
            if (str != "success")
            {
                Response.StatusCode = 201;
            }
            Response.Write(str);
        }
    }
}