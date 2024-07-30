<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationSpeaker.aspx.cs" Inherits="WebManager.appaspx.operation.OperationSpeaker" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="text-align:center;padding:20px;">通告消息</h2>
        <div class="col-sm-offset-2 col-sm-10" style="margin-bottom:20px;">
            这里的通告消息，将出现在如图所示的通告栏
            <img alt="" src="../../image/notify.png" width="400" height="100"/>
        </div>

    <%--    <div>
            显示内容:<asp:TextBox ID="txtContent" runat="server" CssClass="cTextBox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ErrorMessage="不可为空" ForeColor="Red" ControlToValidate="txtContent" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        
        <div>
            发送时间:<asp:TextBox ID="txtSendTime" runat="server" CssClass="cTextBox"></asp:TextBox>
           
            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                ErrorMessage="时间格式非法" ForeColor="Red" ControlToValidate="txtSendTime" Display="Dynamic"
                ValidationExpression="^\s*(\d{4})/(\d{1,2})/(\d{1,2})\s+(\d{1,2}):(\d{1,2})$">
            </asp:RegularExpressionValidator>

             可预设什么时候发到通告栏。不填立即发送。完整格式:2015/12/29 12:30
        </div>

        <div>
            重复次数:<asp:TextBox ID="txtRepCount" runat="server" CssClass="cTextBox"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                ErrorMessage="只能输入数字" ForeColor="Red" ControlToValidate="txtRepCount" Display="Dynamic" 
                ValidationExpression="\d*"></asp:RegularExpressionValidator>
            默认为1
        </div>--%>

        <%--  <div>
           重复间隔:<asp:TextBox ID="txtInterval" runat="server" CssClass="cTextBox"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                ErrorMessage="只能输入数字" ForeColor="Red" ControlToValidate="txtInterval" Display="Dynamic" 
                ValidationExpression="\d*"></asp:RegularExpressionValidator>
            秒, 默认为1
        </div> --%>

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">显示内容:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="txtContent" runat="server" CssClass="form-control" ></asp:TextBox>
            </div>
            <div class="col-sm-10">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="不可为空" ForeColor="Red" ControlToValidate="txtContent" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
        </div> 
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">重复次数(默认为1):</label>
            <div class="col-sm-10">
                <asp:TextBox ID="txtRepCount" runat="server" CssClass="form-control" ></asp:TextBox>
            </div>
            <div class="col-sm-offset-2 col-sm-10">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                ErrorMessage="只能输入数字" ForeColor="Red" ControlToValidate="txtRepCount" Display="Dynamic" 
                ValidationExpression="\d*"></asp:RegularExpressionValidator>
            </div>
        </div> 
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">发送时间:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="txtSendTime" runat="server" CssClass="form-control" ></asp:TextBox>
            </div>
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                    ErrorMessage="时间格式非法" ForeColor="Red" ControlToValidate="txtSendTime" Display="Dynamic"
                    ValidationExpression="^\s*(\d{4})/(\d{1,2})/(\d{1,2})\s+(\d{1,2}):(\d{1,2})$">
                 </asp:RegularExpressionValidator>
            </div>
            <label class="col-sm-offset-2 col-sm-10">可预设什么时候发到通告栏。不填立即发送。完整格式:2015/12/29 12:30</label>
        </div>
        <div class="col-sm-offset-2 col-sm-10">
            <asp:Button ID="btnSend" runat="server" Text="发送" CssClass="btn btn-primary form-control" OnClick="btnSend_Click" />
        </div>
        <div class="col-sm-offset-2 col-sm-10">
            <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        </div>
    </div>
</asp:Content>
