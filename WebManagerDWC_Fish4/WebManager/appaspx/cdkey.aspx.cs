﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebManager.appaspx
{
    public partial class cdkey : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RightMgr.getInstance().opCheck(RightDef.OTHER_CD_KEY, Session, Response);
        }
    }
}