<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ServiceMail.aspx.cs" Inherits="WebManager.appaspx.service.ServiceMail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_logOutTime').daterangepicker();
        });
	</script>
    <script src="../../Scripts/service/ServiceMail.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="text-align: center;">发邮件</h2>
        <p class="col-sm-offset-2 col-sm-10">目标玩家ID，可以输入多个，以空格相隔</p>
        <p class="col-sm-offset-2 col-sm-10">道具列表输入格式: 道具ID + 空格 + 道具数量。若需要输入多个，以;号相隔。如 10001 1;10002 2;10003 3</p>
        <%--  发放方式:&nbsp;&nbsp;<asp:DropDownList ID="m_target" runat="server" style="width:200px;height:30px"></asp:DropDownList>
    &nbsp;&nbsp;<asp:CheckBox ID="m_chk" runat="server" Text="缓存邮件，以待检查" style="width:200px;height:30px"></asp:CheckBox>
    <br /><br />
    标题：<asp:TextBox ID="m_title" runat="server" style="width:300px;height:20px"></asp:TextBox>
    <br />
    发送者：<asp:TextBox ID="m_sender" runat="server" style="width:300px;height:20px;margin-top:10px"></asp:TextBox>
    <br />
    邮件内容：<asp:TextBox ID="m_content" runat="server" style="width:300px;height:20px;margin-top:10px"></asp:TextBox>
    <br />
    目标玩家ID：<asp:TextBox ID="m_toPlayer" runat="server" style="width:800px;height:20px;margin-top:10px"></asp:TextBox>
    <br />
    道具列表：<asp:TextBox ID="m_itemList" runat="server" style="width:800px;height:20px;margin-top:10px"></asp:TextBox>
    <br />
    有效时间(天，默认7天)：<asp:TextBox ID="m_validDay" runat="server" style="width:300px;height:20px;margin-top:10px"></asp:TextBox>
    <br />

    <div id="divFullCond">
        全服发放条件--下线时间区间：<asp:TextBox ID="m_logOutTime" runat="server" style="width:350px;height:20px;margin-top:10px"></asp:TextBox>
        <br />
        全服发放条件--VIP等级区间(如3 10)：<asp:TextBox ID="m_vipLevel" runat="server" style="width:300px;height:20px;margin-top:10px"></asp:TextBox> 
    </div>
    <br />
    备注(可填本次操作原因)：<asp:TextBox ID="m_comment" runat="server" style="width:800px;height:20px;margin-top:10px"></asp:TextBox>
    <br />
    <asp:Button ID="btnSend" runat="server" onclick="onSendMail" Text="发送邮件" style="width:133px;height:25px;margin-top:10px" />
    <br /> --%>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:CheckBox ID="m_chk" runat="server" Text="缓存邮件，以待检查" CssClass="form-control"></asp:CheckBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">发放方式:</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="m_target" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">标题:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_title" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">发送者:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_sender" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">邮件内容:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_content" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">目标玩家ID:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_toPlayer" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group" id="itemList" runat="server">
            <label for="account" class="col-sm-2 control-label">道具列表:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_itemList" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">有效时间(天，默认7天):</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_validDay" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div id="divFullCond">
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">全服发放条件--下线时间区间:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_logOutTime" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">全服发放条件--VIP等级区间(如3 10):</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_vipLevel" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">全服发放条件--渠道号:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_channelId" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">备注(可填本次操作原因):</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_comment" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="Button2" runat="server" Text="发送邮件" CssClass="btn btn-primary form-control"
                    OnClick="onSendMail" ClientIDMode="Static" />
            </div>
        </div>
        <div class="col-sm-offset-2 col-sm-10">
            <span id="m_res" style="font-size: medium; color: red" runat="server"></span>
        </div>
    </div>
</asp:Content>
