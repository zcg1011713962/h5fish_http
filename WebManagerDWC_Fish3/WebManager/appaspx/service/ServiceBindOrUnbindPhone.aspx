<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServiceBindOrUnbindPhone.aspx.cs" Inherits="WebManager.appaspx.service.ServiceBindOrUnbindPhone" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
            selectOpType();
        });

        seajs.use("../../Scripts/service/ServiceBindUnbindPhone.js");

        function selectOpType()
        {
            var type=$("#MainContent_m_type").prop("selectedIndex");
            if (type == 0) {
                $("#phoneNum").show();
            } else
            {
                $("#phoneNum").hide();
            }
        }
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="container form-horizontal">
        <h2 style="padding:20px;text-align:center">绑定解绑玩家手机</h2>
         <div class="form-group">
             <label for="account" class="col-sm-2 control-label">类型：</label>
             <div class="col-sm-10">
                 <asp:DropDownList ID="m_type" runat="server" CssClass="form-control" onchange="selectOpType()"></asp:DropDownList>
             </div>
         </div>        
         <div class="form-group">
            <label for="account" class="col-sm-2 control-label">玩家ID：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_playerId" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
            </div>
        </div>
        <div class="form-group" id="phoneNum" >
            <label for="account" class="col-sm-2 control-label">手机号码：</label>
            <div class="col-sm-10">
                <asp:TextBox ID="m_phone" runat="server" CssClass="form-control" style="height:30px;width:65%;display:inline-block"></asp:TextBox>                 
                 &ensp;&ensp;
                <asp:Button ID="btn_vertify" runat="server" Text="验证手机" style="width:16%;height:30px;line-height:15px;display:inline-block"
                     CssClass="btn btn-primary form-control" OnClick="onVertify"/>

                <asp:Button ID="btn_query" runat="server" Text="查询验证码" style="width:16%;height:30px;line-height:15px;display:inline-block"
                     CssClass="btn btn-primary form-control" OnClick="btn_query_Click"/>

            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="btn_click" runat="server" Text="确定" CssClass="btn btn-primary form-control" OnClick="onClick"/>
            </div>
        </div>
         <div style="text-align:center">
            <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        </div>
        <div class="table-responsive" style="margin-top:10px;text-align:center">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        </div>
    </div>
</asp:Content>
