<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatFishlordBaojinBasicData.aspx.cs" Inherits="WebManager.appaspx.stat.StatFishlordBaojinBasicData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>

    <script type="text/javascript" src="../../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MainContent_m_time').daterangepicker();
            $('#MainContent_m_time4').daterangepicker();
            $('#MainContent_m_time5').daterangepicker();
        });
        seajs.use("../../Scripts/stat/FishlordBaojinBasicData.js");
	</script>
    <style type="text/css">
        .OpModify{width:550px; background:rgb(255,255,255);padding-top:20px;text-align:center}

        .OP{margin-top:50px;}
        .OP>div{display:none;}
        #ModifyPici{display:none;position:absolute;width:100%;height:100%;background:rgba(0,0,0,0.7);left:0;top:0;
                    padding-top:200px;
        }
        #ModifyPici a{display:block;background:#ddd;font-size:18px;text-align:center;margin-top:20px;}
        .OpModifyPici{width:800px; background:rgb(255,255,255);padding:15px}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="clearfix" style="width:1300px;margin:0 auto;padding-top:20px;">
        <ul class="SelCard">
            <li class="Active" data="op1" style="width:150px;">爆金比赛场统计</li>
            <li data="op2" style="width:150px;">爆金比赛场当前日排行</li>
            <li data="op3" style="width:150px;">爆金比赛场当前周排行</li>
            <li data="op4" style="width:150px;">爆金比赛场历史日排行</li>
            <li data="op5" style="width:150px;">爆金比赛场历史周排行</li>
            <li data="op6" style="width:150px;">爆金比赛场参数调整</li>
            <li data="op7" style="width:150px;">竞技场得分修改</li>
        </ul>
    </div>
    <div class="OP container form-horizontal" style="text-align:center">
        <div id="op1" class="container">
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">时间：</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_time" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                     <input type="button" id="btnQueryStat" value="统计" class="btn btn-primary form-control"/>
                </div>
            </div>
            <div class="table-responsive">
                <asp:Table ID="m_result1" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
            </div>
        </div>
        <div id="op2" class="container">
            <asp:Table ID="m_result2" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        </div>
        <div id="op3" class="container">
            <asp:Table ID="m_result3" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        </div>
        <div id="op4" class="container">
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">时间：</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_time4" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                     <input type="button" id="btnQuery4" value="统计" class="btn btn-primary form-control"/>
                </div>
            </div>
            <div class="table-responsive">
                <asp:Table ID="m_result4" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
            </div>
        </div>
        <div id="op5" class="container">
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">时间：</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_time5" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                     <input type="button" id="btnQuery5" value="统计" class="btn btn-primary form-control"/>
                </div>
            </div>
            <div class="table-responsive">
                <asp:Table ID="m_result5" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
            </div>
        </div>
        <div id="op6" role="form" class="container" style="display:block;padding:5px;">
            <asp:Table ID="m_result6" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        </div>
        <div id="op7" class="container">
            <h2 style="height:40px;line-height:40px;background-color:#ccc;padding:0 50px;">
                修改竞技场得分提示：
                修改得分时，需要先下线。等待超过7分钟再上线，才能确保服务器的数据不会覆盖修改后的数据。
            </h2>
            <br />
            <br />
            <div class="form-group">
                <label for="account" class="col-sm-2 control-label">玩家ID：</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="m_playerId" runat="server" CssClass="form-control" style="height:30px;"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                     <input type="button" id="btnStat7"  value="查询" class="btn btn-primary form-control"/>
                </div>
            </div>
            <asp:Table ID="m_result7" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
            
            <div id="alterScore" style="text-align:center;display:none;">
                <span style="font-weight:bold;">修改玩家竞技场得分：</span>
                <input id="m_param" type="text" style="width:200px;height:31px;"/>
                <input id="btnConfirm" type="button" class="btn btn-primary form-control" value="确定" style="width:125px;"/>
                <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
            </div>
        </div>
    </div>
    <span id="m_page" style="text-align:center;display:block" runat="server"></span>
            <br />
            <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
    <div id="divModifyNewParam" class="PopDialogBg">
        <div class="container form-horizontal OpModify">
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <h2 id="paramTips"></h2>
                </div>
            </div>
            <br />
            <div class="form-group" style="display:inline-block">
                <div class="col-sm-offset-2 col-sm-10">
                     <input type="button" value="取消" style="width:100px;" class="btn btn-default form-control" id="btnCancel" />
                </div>
            </div>
            <div class="form-group" style="display:inline-block">
                <div class="col-sm-offset-2 col-sm-10">
                     <input type="button" value="确定" style="width:100px;" class="btn btn-primary form-control" id="btnModifyParam" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
