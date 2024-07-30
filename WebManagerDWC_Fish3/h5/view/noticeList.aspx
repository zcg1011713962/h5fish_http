<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="noticeList.aspx.cs" Inherits="h5.view.noticeList" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>公告栏</title>
    <meta name="referrer" content="always"/>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width,initial-scale=1, maximum-scale=1,minimum-scale=1, user-scalable=no,orientation=landscape"/>  
    <meta http-equiv="x-dns-prefetch-control" content="no"/>
    <script type="text/javascript" src="../scripts/jquery-2.1.1.js"></script>
    <style>
        * { 
            padding:0;margin:0;
            /*消除点击阴影*/
            -webkit-tap-highlight-color: transparent;
        }
        /*旋转屏幕时，字体大小调整*/
        html, body, form, fieldset, p, div, h1, h2, h3, h4, h5, h6 {
          -webkit-text-size-adjust:100%;
        }
        ul, li { 
            list-style-type:none;
        }
        a {
            text-decoration-line:none;
        }
        body {
    		font-family:"微软雅黑","Microsoft YaHei",Helvetica,sansation,Verdana,Arial,sans-serif;
            background-color:#004967;
        }
        #box {
            margin:20px;padding:0;
        }
        #box #title {
            float:left;
            width:25%;
            text-align:center;
            padding:15px 0px;
            height:280px;
            border-radius:20px;
            overflow:hidden;
            overflow-y:scroll;
        }
        #box #title li a {
            background:url("../images/noticeList/btn_no_selected_bg.png") no-repeat;
            background-size:100% 100%;
            font-size:20px;
            margin-bottom:5px;
            display:inline-block;
            width:95%; height:45px;line-height:40px;
            color:#0c3b89;
            padding-right:7%;
        }
        #box #title #first a{
            color:#7c1d04;
            background:url('../images/noticeList/btn_selected_bg.png') no-repeat;
            background-size:100% 100%;
        }
        #box #container {
            width:75%;
            background-color:#012C47;
            height:310px;
            float:left;
            border-radius:20px;
        }
        #box #container #content {
            margin:15px 18px;
            height:280px;
            overflow:hidden;
            overflow-y:scroll;
        }
        #box #container, #box #title {
            border:none;
        }
        #box #footer {
            clear:both;
        }
    </style>
    <script>
        $(function () {

            $.ajax({
                type: "POST",
                url: "/api/NoticeList.ashx",
                data: {},
                success: function (data) {
                    var data_msg = JSON.parse(data);
                    var msg = data_msg["result"];
                    var list = []; //存储所有公告消息
                    if (msg == 0) //成功
                    {
                        //console.log(data_msg["resList"]);
                        var resList = $.parseJSON(data_msg["resList"]);
                        $.each(resList, function (index, item){
                            list[index] = item;
                            //初始化页面标题和内容
                            if (index == 0)
                            {
                                $("#box #title ul").append("<li id='first'><a>" + item.m_title + "</a></li>");
                                $("#box #content").append(item.m_content);
                            } else
                            {
                                $("#box #title ul").append("<li><a>" + item.m_title + "</a></li>");
                            }
                        });
                    }
                    //点击切换
                    $("#box #title ul li").each(function () { 
                        $(this).click(function () {
                            $(this).attr("id", "first").siblings().removeAttr("id");
                            var index = $(this).index();
                            $("#box #content").html(list[index].m_content);
                        });
                    });
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="box">
            <div id="title"> 
                <ul>
                    <!--公告标题列表li-->
                </ul>
            </div>
            <div id="container">
                <div id="content"></div>
            </div>
            <div id="footer"></div>
        </div>
    </form>
</body>
</html>
