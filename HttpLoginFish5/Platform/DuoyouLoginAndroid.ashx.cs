using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    // 原多游android是通过quick sdk实现，单独实现，仍然写到表 quicksdk_acc中。
    public class DuoyouLoginAndroid : IHttpHandler
    {
        const string DUOYOU_CHANNEL = "971";
        const string APP_ID = "FC0A563E8BB41792998A345AA73449BF";

        public void ProcessRequest(HttpContext context)
        {
            CDuoYouLogin objLogin = new CDuoYouLogin();
            objLogin.AppId = APP_ID;
            objLogin.ChannelId = DUOYOU_CHANNEL;
            //objLogin.Platform = LoginTable.PLATFORM_QUICKSDK;
            objLogin.doLogin(context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}