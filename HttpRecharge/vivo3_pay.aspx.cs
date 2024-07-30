using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    /*
     * 公司名：深圳市三巨头科技有限公司

        名称：电玩捕鱼大富豪

        渠道：VIVO

        包名： com.yoyangs.fuhao.vivo

        Cp-ID：1b572d7781ab0ee5ee5d
        App-key：a59f4610b846445c7d8b8bb5d13ec26d
        App-ID：19c85d7e41e0b4ea1956645ee54204ad
     */
    public partial class vivo3_pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackVIVO pay = PayCallbackVIVO.createVivoPay(Request);//new PayCallbackVIVO();
            pay.PayTableName = PayTable.VIVO3_PAY;
            pay.VivoCpKey = PayTable.VIVO_CP_KEY3;

            string str = pay.notifyPay(Request);
            if (str != "success")
            {
                Response.StatusCode = 201;
            }
            Response.Write(str);
        }
    }
}