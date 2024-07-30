using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    /*
     * 游戏名：电玩城捕鱼2
        包名：com.tjzl.dianwan.yxf
        gameid：1879
        appid：1107
        appkey：bd3bb71204f976c413d7e038b39cd0df
        agent：default
     */
    public partial class fan_pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackBase pay = new PayCallbacGameFan();
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}