<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatActivityTouchFish.aspx.cs" Inherits="WebManager.appaspx.stat.StatActivityTouchFish" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal" style="width:80%">
        <h2 style="padding:20px;text-align:center">一起来摸鱼</h2>
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">统计项目:</label>
            <div class="col-sm-2">
                <asp:DropDownList ID="m_optional" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <label for="account" class="col-sm-1 control-label">赛季(202101):</label>
            <div class="col-sm-5">
               <asp:TextBox ID="m_season" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>    

        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">时间：</label>
            <div class="col-sm-8">
                <asp:TextBox ID="m_time" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        
        <asp:Button ID="Button1" runat="server" Text="确定" onclick="onQuery" CssClass="btn btn-primary" style="width:30%;margin-left:30%"/>
        <br/>
        <br/>
     </div>
    <div class="table-responsive" style="margin-top: 10px; text-align: center">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;width:95%;margin:0 auto"></asp:Table>
    </div>
    <div class="table-responsive" style="margin-top: 10px; text-align: center">
        <asp:Table ID="m_result1" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;width:95%;margin:0 auto"></asp:Table>
    </div>
    <div class="table-responsive" style="margin-top: 10px; text-align: center">
        <asp:Table ID="m_result2" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;width:95%;margin:0 auto"></asp:Table>
    </div>
    <div class="table-responsive" style="margin-top: 10px; text-align: center">
        <asp:Table ID="m_result3" runat="server" CssClass="table table-hover table-bordered" style="text-align:center;width:95%;margin:0 auto"></asp:Table>
    </div>
    <span id="m_page" style="text-align:center;display:block" runat="server"></span>
    <br />
    <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
</asp:Content>
