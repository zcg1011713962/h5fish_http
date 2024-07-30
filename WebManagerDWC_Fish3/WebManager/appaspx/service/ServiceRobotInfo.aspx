<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServiceRobotInfo.aspx.cs" Inherits="WebManager.appaspx.service.ServiceRobotInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="../../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        seajs.use("../../Scripts/service/ServiceRobotInfo.js");
    </script>
    <style type="text/css">
        .OP{margin-left: 200px; margin-top:20px; width:1000px;}
        .valueIn{
            width:400px;
        }
        .key{width:100px;text-align:right;}
        #ModifyPici{display:none;position:absolute;width:100%;height:100%;background:rgba(0,0,0,0.7);left:0;top:0;
                    padding-top:200px;
        }
        #ModifyPici a{display:block;background:#ddd;font-size:18px;text-align:center;margin-top:20px;}
        .OpModifyPici{width:800px; background:rgb(255,255,255);padding-bottom:20px}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="OP">
        <h2 style="padding: 20px; text-align: center">机器人信息修改</h2>
        <table class="table" style="width:600px;margin:0 auto;">
            <tr>
                <td class="key">机器人ID:</td>
                <td class="valueIn"><input type="text" class="form-control" id="txtRobotId" placeholder="" name="txtRobotId" /></td>
                <td style="width:300px;">机器人ID范围:10099001--10099200</td>
            </tr>
            <tr>
                <td class="key">昵称:</td>
                <td><input type="text" class="form-control" id="txtNickName" placeholder="" name="txtNickName" /></td>
                <td><input type="button" class="btn btn-default" id="btnNickName" data="txtNickName" value="提交修改" /></td>
            </tr>
            <tr>
                <td class="key">VIP等级:</td>
                <td><input type="text" class="form-control" id="txtVipLevel" placeholder="" name="txtVipLevel" /></td>
                <td><input type="button" class="btn btn-default" id="btnVipLevel" data="txtVipLevel" value="提交修改" /></td>
            </tr>
            <tr>
                <td class="key">头像:</td>
                <td><input type="text" class="form-control" id="txtHead" placeholder="" name="txtHead" /></td>
                <td><input type="button" class="btn btn-default" id="btnHead" data="txtHead" value="提交修改"/></td>
            </tr>
            <tr>
                <td class="key">头像框:</td>
                <td><input type="text" class="form-control" id="txtFrameId" placeholder="" name="txtFrameId" /></td>
                <td><input type="button" class="btn btn-default" id="btnFrameId" data="txtFrameId" value="提交修改" /></td>
            </tr>
            <tr>
                <td class="key">金币:</td>
                <td><input type="text" class="form-control" id="txtGold" placeholder="" name="txtGold" /></td>
                <td><input type="button" class="btn btn-default" id="btnGold" data="txtGold" value="提交修改" /></td>
            </tr>
            <tr>
                <td class="key">青铜鱼雷:</td>
                <td><input type="text" class="form-control" id="txtItem24" placeholder="" name="txtItem24" /></td>
                <td><input type="button" class="btn btn-default" id="btnItem24" data="txtItem24" value="提交修改" /></td>
            </tr>
            <tr>
                <td class="key">白银鱼雷:</td>
                <td><input type="text" class="form-control" id="txtItem25" placeholder="" name="txtItem25" /></td>
                <td><input type="button" class="btn btn-default" id="btnItem25" data="txtItem25" value="提交修改" /></td>
            </tr>
            <tr>
                <td class="key">黄金鱼雷:</td>
                <td><input type="text" class="form-control" id="txtItem26" placeholder="" name="txtItem26" /></td>
                <td><input type="button" class="btn btn-default" id="btnItem26" data="txtItem26" value="提交修改" /></td>
            </tr>
            <tr>
                <td class="key">钻石鱼雷:</td>
                <td><input type="text" class="form-control" id="txtItem27" placeholder="" name="txtItem27" /></td>
                <td><input type="button" class="btn btn-default" id="btnItem27" data="txtItem27" value="提交修改" /></td>
            </tr>
        </table>

         <div class="opPanel" style="width:300px;margin:0 auto;height:auto;">
            <input type="button" value="查询" class="btn btn-primary" data="3"/>
            <input type="button" value="增加机器人数据" class="btn btn-primary" data="0"/>
            <span id="m_res" style="font-size:medium;color:red"></span>
        </div>
    </div>
      
</asp:Content>
