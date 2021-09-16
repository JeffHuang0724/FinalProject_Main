﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs"
    Inherits="FinalProject.SystemAdmin.Sales.OrderDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>毛豆端火鍋訂單出貨系統</title>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Header -->
        <div
            id="divHeader"
            style="width: 100%; height: 30%; display: flex; flex-flow: row; align-items: center; justify-content: center; text-align: center;">
            <div id="divLogo" style="width: 10%; float: left">
                <a href="/SystemAdmin/UserDefault.aspx" target="_self">
                    <img
                        alt="毛豆"
                        src="/Images/Logo.png"
                        width="80"
                        height="80"
                        style="border-radius: 50%" />
                </a>
            </div>
            <div id="divTitle" style="width: 70%; text-align: center">
                <h1>毛豆端火鍋訂單出貨系統</h1>
            </div>
            <!-- 首頁右方功能列表 -->
            <div
                id="divList"
                style="width: 20%; float: right; text-align: right; display: flex; flex-flow: row;">
                <div style="width: 30%; text-align: center">
                    <a
                        href="/SystemAdmin/UserDefault.aspx"
                        target="_self"
                        style="text-decoration: none">訂單管理</a>
                </div>
                <div style="width: 30%; text-align: center">
                    <a
                        href="/SystemAdmin/User/UserInfo.aspx"
                        target="_blank"
                        style="text-decoration: none">帳號管理</a>
                </div>
                <div style="width: 30%; text-align: center">
                    <asp:Button
                        runat="server"
                        ID="btnLogout"
                        Text="登出"
                        OnClick="btnLogout_Click"
                        Style="border: none; background: none; color: #0000ff; margin: 0; padding: 0; font-size: 1rem;" />
                </div>
            </div>
        </div>
        <!-- Header -->
        <!-- content -->
        <div
            id="divMiddle"
            style="width: 100%; height: 70%; display: flex; flex-flow: row; align-items: center; justify-content: center; text-align: center;">
            <div
                id="divMiddleLeft"
                style="width: 10%; height: 100%; display: flex; flex-flow: column; align-items: center; justify-content: center; text-align: center;">
                <a
                    href="/SystemAdmin/Sales/Order.aspx"
                    target="_self"
                    style="text-decoration: none; position: absolute; top: 12rem; left: 3rem;">處理訂單</a>
                <a
                    href="/SystemAdmin/Sales/HistoryOrder.aspx"
                    target="_self"
                    style="text-decoration: none; position: absolute; top: 16rem; left: 3rem;">歷史訂單</a>
            </div>
            <div
                id="divMiddleContent"
                style="width: 70%; display: flex; flex-flow: column; align-items: center; text-align: center; justify-content: center; float: right;">
                <h2>銷售訂單資訊</h2>
                <asp:PlaceHolder ID="plcNoData" runat="server" Visible="false">
                    <div style="margin-top: 1rem">
                        <p style="color: red; background-color: yellow; font-size: large">
                            查無此訂單
                        </p>
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="plcHasData" runat="server" Visible="true">
                    <table border="1" cellpadding="20" style="margin-top: 1rem;">
                        <tr>
                            <th>訂單編號</th>
                            <td>
                                <asp:Literal runat="server" ID="ltlOrderNo"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>銷售單號</th>
                            <td>
                                <asp:Literal runat="server" ID="ltlSNo"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>下單人員</th>
                            <td>
                                <asp:Literal
                                    runat="server"
                                    ID="ltlOrderEmployeeID"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>下單分店</th>
                            <td>
                                <asp:Literal runat="server" ID="ltlOrderBranch"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>下單時間</th>
                            <td>
                                <asp:Literal runat="server" ID="ltlOrderDate"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>預計發貨日</th>
                            <td>
                                <asp:Literal
                                    runat="server"
                                    ID="ltlPreShipmentDate"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>分店訂單備註</th>
                            <td>
                                <asp:Literal runat="server" ID="ltlRemark"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>下單總額</th>
                            <td>
                                <asp:Label runat="server" ID="lblOrderAmount"></asp:Label>
                                <asp:Literal
                                    runat="server"
                                    ID="ltlOrderItemList"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>銷售處理人員</th>
                            <td>
                                <asp:Literal runat="server" ID="ltlSalesNo"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>備註說明(內部)</th>
                            <td>
                                <asp:PlaceHolder
                                    ID="plcReadInnerRemark"
                                    runat="server"
                                    Visible="true">
                                    <asp:Label
                                        runat="server"
                                        ID="lblInnerRemark"
                                        TextMode="MultiLine"></asp:Label>
                                </asp:PlaceHolder>
                                <br />
                                <asp:PlaceHolder
                                    ID="plcEditInnerRemark"
                                    runat="server"
                                    Visible="true">
                                    <asp:TextBox
                                        runat="server"
                                        ID="txtInnerRemark"
                                        TextMode="MultiLine"></asp:TextBox>
                                </asp:PlaceHolder>
                            </td>
                        </tr>
                        <asp:PlaceHolder
                            ID="plcWriteCustomerRemark"
                            runat="server"
                            Visible="false">
                            <tr>
                                <th>給分店的訊息</th>
                                <td>
                                    <asp:TextBox
                                        runat="server"
                                        ID="txtCustomerRemark"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder
                            ID="plcProcessBtn"
                            runat="server"
                            Visible="false">
                            <tr>
                                <td colspan="2">
                                    <asp:Button
                                        runat="server"
                                        ID="btnApprove"
                                        Text="核可"
                                        Font-Size="Medium"
                                        OnClick="btnApprove_Click" />
                                    <asp:Button
                                        runat="server"
                                        ID="btnReject"
                                        Text="退回"
                                        Font-Size="Medium"
                                        OnClick="btnReject_Click"
                                        Style="margin-left: 5rem" />
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                    </table>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="plcErrMsg" runat="server" Visible="false">
                    <div style="margin-top: 1rem">
                        <p style="color: red; background-color: yellow; font-size: large">
                            <asp:Literal runat="server" ID="ltlErrMsg"></asp:Literal>
                        </p>
                    </div>
                </asp:PlaceHolder>
            </div>
            <div
                id="divMiddleRight"
                style="width: 20%; height: 100%; float: right; text-align: right; display: flex; flex-flow: row;">
                <asp:Label
                    ID="lblNowDate"
                    runat="server"
                    Style="position: fixed; bottom: 1rem; right: 1rem;"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
