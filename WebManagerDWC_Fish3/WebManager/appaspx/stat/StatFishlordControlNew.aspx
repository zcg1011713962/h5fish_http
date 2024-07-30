<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatFishlordControlNew.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordControlNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .OpModify {
            width: 800px;
            background: rgb(255,255,255);
            padding-bottom: 20px;
        }
    </style>
    <script src="../../Scripts/module/sea.js"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../../Scripts/datepicker/daterangepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#m_resetMidTime').daterangepicker();
            $('#m_resetHighTime').daterangepicker();
        });
        seajs.use("../../Scripts/stat/FishControlNew.js?ver=5");
    </script>
    <style>
        #m_resetMidTime {
            margin-bottom: 10px;
        }

        .col-sm-2 {
            width: 200px;
        }

        .col-sm-10 {
            width: 500px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h2 style="text-align: center; margin-bottom: 20px;">大水池参数调整</h2>
        <asp:Table ID="m_expRateTable" runat="server" CssClass="table table-hover table-bordered" Style="text-align: center">
        </asp:Table>

        <asp:Button ID="Button1" runat="server" Text="重置" OnClick="onReset" Style="width: 125px; height: 25px" />
        <span id="m_res" style="font-size: medium; color: red" runat="server"></span>
    </div>

    <div id="divModifyNewParam" class="PopDialogBg">
        <div class="container form-horizontal OpModify">
            <h2 style="text-align: center; padding: 10px; margin-bottom: 10px; background: #ccc; cursor: pointer;">关闭</h2>
            <h3 style="text-align: center; padding: 10px; margin-bottom: 10px;"></h3>
           
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">抽水率:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_bigPoolPumpRate" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:300px; display:inline-block"></asp:TextBox>
                    <span>百分比</span>
                </div>
            </div>

            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">期望盈利率:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_bigPoolExpectedEarnRate" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:300px; display:inline-block"></asp:TextBox>
                    <span>百分比</span>
                </div>
            </div>
             <%--///////////////////////////////////////////////////////////////////--%>
            <div id="roomId_9">
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">朱雀转化玄武系数:</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="m_mythicalScoreTurnRate" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:300px; display:inline-block"></asp:TextBox>
                        <span>百分比,默认90</span>
                    </div>
                </div>
                
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">玄武累分系数:</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="m_mythicalFishRate" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:300px; display:inline-block"></asp:TextBox>
                        <span>百分比,默认95</span>
                    </div>
                </div>
            </div>
            <%--///////////////////////////////////////////////////////////////////--%>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">码量控制值:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_incomeThreshold" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:300px; display:inline-block"></asp:TextBox>
                    <span>区间[0，+∞]整数</span>
                </div>
            </div>

            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">机率控制:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_rateCtr" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:300px; display:inline-block"></asp:TextBox>
                    <span>区间[0.1，2.0]小数</span>
                </div>
            </div>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">刷新周期(1-300秒):</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_checkRate" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:300px; display:inline-block"></asp:TextBox>
                    <span>区间[1，300]整数</span>
                </div>
            </div>
            <div class="form-group" id="roomId_2">
                <label for="account" class="col-sm-2 control-label">玩法误差值:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_trickDeviationFix" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:300px; display:inline-block"></asp:TextBox>
                    <span>区间[0，10]小数</span>
                </div>
            </div>
            <%--算法调整--%>
            <div  id="roomId_2_add">
            <hr />
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">捕获次数（一档）:</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="m_colorFishCtrCount1" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:300px; display:inline-block"></asp:TextBox>
                    </div>
             </div>
             <div class="form-group">
                <label for="account" class="col-sm-2 control-label">花色鱼命中系数（一档）:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_colorFishCtrRate1" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:300px; display:inline-block"></asp:TextBox>
                    <span>区间[0，100]整数</span>
                </div>
            </div>
            <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">捕获次数（二档）:</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="m_colorFishCtrCount2" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:300px; display:inline-block"></asp:TextBox>
                    </div>
            </div>
                <div class="form-group">
                    <label for="account" class="col-sm-2 control-label">花色鱼命中系数（二档）:</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="m_colorFishCtrRate2" runat="server" CssClass="form-control" ClientIDMode="Static" style="width:300px; display:inline-block"></asp:TextBox>
                        <span>区间[0，100]整数</span>
                    </div>
                </div>
            <hr />
            </div>
            <%-- END --%>
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label"></label>
                <div class="col-sm-10">
                    <input type="button" value="提交修改" class="btn btn-primary form-control" id="btnModifyParam" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <span id="opRes" style="color: red"></span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
