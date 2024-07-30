﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpLogin.Platform
{
    // 聚焰登录，一个平台，内置了很多游戏，点击某个图标处，可以链接到h5客户端地址
    // 从地址栏获取到平台传过来的玩家ID参数， 然后客户端将这个ID传给服务器进行登录
    public class JuYanLogin : IHttpHandler
    {
        const string JUYAN_CHANNEL = "100007";

        public void ProcessRequest(HttpContext context)
        {
            LoginBase obj = new LoginBase();

            if (obj.chekOrReply(context.Request, context.Response, LoginBase.CHK_PARAM_ACCOUNT))
            {
                LoginBaseParam param = new LoginBaseParam();
                param.m_acc = JUYAN_CHANNEL + "_" + context.Request.Form["acc"];
                param.m_platform = "juyan";
                param.m_channelId = JUYAN_CHANNEL;
                param.m_deviceId = context.Request.Form["deviceID"];
                param.Request = context.Request;
                string str = obj.startLogin(param);

                context.Response.ContentType = "text/plain";
                context.Response.Write(str);
            }
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