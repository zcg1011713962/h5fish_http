using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    // 百万街机捕鱼
    public partial class huawei_baiwan_pay : System.Web.UI.Page
    {
        // 公钥
        public static string PUBLIC_KEY = "<RSAKeyValue><Modulus>ngER4vFxMUgo0LAuYUDIMQGpxL5Z9Bx+hsTc0VH5zpMmsX9gQRplCRyP8YXW3r3TEM8Y/jmiX6sVhHDxFTKnSl8gYRKl09d9a9mzoQ9PUPTfyCNIVoOKmFnklztgGcTcDYfhroTRI5hjeL8uUF7/jZmSuF1DWoq3H7GdOg4M2u+pGSSxPeDUTKe8W7UGXYidg1+NVS/MZzl0Awcc01rc5IrvaRxeYoBWONIQZlyoDfisn7CvFsYNW+yJHYHg1uNqdBNgFPW6UkPHD4GTfKUd03eqjEHzFNtXdqMyDDXWcSjEDBXfUbCcvX/pfbms53677yO1+2RldM0plSOnTPXCmQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackHuaWei pay = new PayCallbackHuaWei();
            pay.PublicKey = PUBLIC_KEY;
            pay.PayTableName = PayTable.HUAWEI_BAIWAN_PAY;
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}