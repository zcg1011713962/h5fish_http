﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class xiaomi_fishingJoy2_2_pay : System.Web.UI.Page
    {
        const string APPSECRET = "85ed49ed-c65e-9f54-9969-530d600cb73d";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbacXiaoMi pay = new PayCallbacXiaoMi();
            pay.AppSecret = APPSECRET;
            pay.PayTableName = PayTable.XIAOMI_FISHING_JOY2_2_PAY;
            string str = pay.notifyPay(Request);
            Response.Write(str);
        }
    }
}