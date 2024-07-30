<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationPlayerOp.aspx.cs" Inherits="WebManager.appaspx.operation.OperationPlayerOp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Scripts/operation/OperationPlayerOp.js?ver=1" type="text/javascript"></script>
    <style type="text/css">
        .cOp1 table {
            border-collapse: collapse;
        }

        .cOp1 td {
            padding: 10px;
            width: 200px;
            border: 1px solid black;
            text-align: center;
            font-size: 14px;
            background: #ccf;
            color: #000;
            font-weight: bold;
        }

        .cOp1 input[type=text] {
            padding: 1px;
            width: 90%;
            height: 20px;
        }

        .cOp1 input[type=button] {
            padding: 6px;
            width: 80px;
            height: 40px;
            line-height: 10px;
        }

        .cOp1 input.SingleOp {
            width: 200px;
        }

        #logFish li {
            list-style: none;
            font-size: 14px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align: center; padding: 20px;">玩家相关操作</h2>
        <div class="cOp1 cDiv1">
            <table style="margin: 0 auto">
                <tr>
                    <td>操作内容</td>
                    <td>玩家ID</td>
                    <td>数值</td>
                    <td></td>
                </tr>
                
                <tr>
                    <td>玩家经验</td>
                    <td>
                        <input type="text" id="playerXP" /></td>
                    <td>
                        <input type="text" id="valueXP" /></td>
                    <td>
                        <input type="button" id="btnAddXP" value="增加" player="playerXP" val="valueXP" optype="add" playerprop="xp" class="SingleOp" />
                    </td>
                </tr>
                <tr>
                    <td>VIP经验</td>
                    <td>
                        <input type="text" id="playerExp" /></td>
                    <td>
                        <input type="text" id="valueExp" /></td>
                    <td>
                        <input type="button" id="btnAddExp" value="增加" player="playerExp" val="valueExp" optype="add" playerprop="vip" class="SingleOp" />
                    </td>
                </tr>
                <tr>
                    <td>金币</td>
                    <td>
                        <input type="text" id="playerGold" /></td>
                    <td>
                        <input type="text" id="valueGold" /></td>
                    <td>
                        <input type="button" id="btnAddGold" value="增加" player="playerGold" val="valueGold" optype="add" playerprop="gold" />
                        <input type="button" id="btnDecGold" value="减少" player="playerGold" val="valueGold" optype="dec" playerprop="gold" />
                    </td>
                </tr>

                <tr>
                    <td>钻石</td>
                    <td>
                        <input type="text" id="playerGem" /></td>
                    <td>
                        <input type="text" id="valueGem" /></td>
                    <td>
                        <input type="button" id="Button3" value="增加" player="playerGem" val="valueGem" optype="add" playerprop="gem" />
                        <input type="button" id="Button4" value="减少" player="playerGem" val="valueGem" optype="dec" playerprop="gem" />
                    </td>
                </tr>

                <tr>
                    <td>碎片</td>
                    <td>
                        <input type="text" id="playerChip" /></td>
                    <td>
                        <input type="text" id="valueChip" /></td>
                    <td>
                        <input type="button" id="Button2" value="增加" player="playerChip" val="valueChip" optype="add" playerprop="chip" />
                        <input type="button" id="Button5" value="减少" player="playerChip" val="valueChip" optype="dec" playerprop="chip" />
                    </td>
                </tr>

                <tr>
                    <td>魔石</td>
                    <td>
                        <input type="text" id="playerMoshi" /></td>
                    <td>
                        <input type="text" id="valueMoshi" /></td>
                    <td>
                        <input type="button" id="Button6" value="增加" player="playerMoshi" val="valueMoshi" optype="add" playerprop="moshi" />
                        <input type="button" id="Button8" value="减少" player="playerMoshi" val="valueMoshi" optype="dec" playerprop="moshi" />
                    </td>
                </tr>

                <tr>
                    <td>踢出</td>
                    <td>
                        <input type="text" id="playerKick" /></td>
                    <td></td>
                    <td>
                        <input type="button" id="Button7" value="踢出玩家" player="playerKick" optype="kick" playerprop="" />
                    </td>
                </tr>
                <tr>
                    <td>引导礼包</td>
                    <td></td>
                    <td></td>
                    <td>
                        <input type="button" id="Button1" value="重置所有玩家引导礼包状态"
                            class="SingleOp" optype="resetGiftGuideFlag" playerprop="" />
                    </td>
                </tr>

                <tr>
                    <td>修改昵称</td>
                    <td>
                        <input type="text" id="playerNickName" /></td>
                    <td>
                        <input type="text" id="valueNickName" /></td>
                    <td>
                        <input type="button" id="Button9" value="修改" player="playerNickName" val="valueNickName" optype="modify" playerprop="nickName" />
                    </td>
                </tr>
                <tr>
                    <td>修改贡献值</td>
                    <td>
                        <input type="text" id="playerContr" /></td>
                    <td>
                        <input type="text" id="valueContr" /></td>
                     <td>
                        <input type="button" id="Button10" value="增加" player="playerContr" val="valueContr" optype="add" playerprop="rechargeContr" />
                        <input type="button" id="Button11" value="减少" player="playerContr" val="valueContr" optype="dec" playerprop="rechargeContr" />
                    </td>
                </tr>
            </table>
        </div>

    </div>
</asp:Content>
