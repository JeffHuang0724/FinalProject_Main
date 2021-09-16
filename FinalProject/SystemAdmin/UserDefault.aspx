<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDefault.aspx.cs"
    Inherits="FinalProject.SystemAdmin.UserDefault" %>

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
            <!-- 首頁右方功能列表(依照部門不同顯示內容作調整) -->
            <div
                id="divList"
                style="width: 20%; float: right; text-align: right; display: flex; flex-flow: row;">
                <asp:PlaceHolder runat="server" ID="phSalesManu" Visible="false">
                    <div style="width: 30%; text-align: center">
                        <a
                            href="/SystemAdmin/UserDefault.aspx"
                            target="_self"
                            style="text-decoration: none">訂單管理</a>
                    </div>
                    <div style="width: 30%; text-align: center">
                        <a
                            href="/SystemAdmin/User/UserInfo.aspx"
                            target="_self"
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
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phWarehouseManu" Visible="false">
                    <div style="width: 25%; text-align: center">
                        <a
                            href="/SystemAdmin/UserDefault.aspx"
                            target="_self"
                            style="text-decoration: none">訂單管理</a>
                    </div>
                    <div style="width: 25%; text-align: center">
                        <a
                            href="/SystemAdmin/Warehouse/ItemList.aspx"
                            target="_self"
                            style="text-decoration: none">品項管理</a>
                    </div>
                    <div style="width: 25%; text-align: center">
                        <a
                            href="/SystemAdmin/User/UserInfo.aspx"
                            target="_self"
                            style="text-decoration: none">帳號管理</a>
                    </div>
                    <div style="width: 25%; text-align: center">
                        <asp:Button
                            runat="server"
                            ID="Button1"
                            Text="登出"
                            OnClick="btnLogout_Click"
                            Style="border: none; background: none; color: #0000ff; margin: 0; padding: 0; font-size: 1rem;" />
                    </div>
                </asp:PlaceHolder>
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
                <asp:PlaceHolder
                    runat="server"
                    ID="phSalesOrderManagement"
                    Visible="false">
                    <a
                        href="/SystemAdmin/Sales/Order.aspx"
                        target="_self"
                        style="text-decoration: none; position: absolute; top: 12rem; left: 3rem;">處理訂單</a>
                    <a
                        href="/SystemAdmin/Sales/historyOrder.aspx"
                        target="_self"
                        style="text-decoration: none; position: absolute; top: 16rem; left: 3rem;">歷史訂單</a>
                </asp:PlaceHolder>
                <asp:PlaceHolder
                    runat="server"
                    ID="phWarehouseOrderManagement"
                    Visible="false">
                    <a
                        href="/SystemAdmin/Warehouse/Order.aspx"
                        target="_self"
                        style="text-decoration: none; position: absolute; top: 12rem; left: 3rem;">處理訂單</a>
                    <a
                        href="/SystemAdmin/Warehouse/historyOrder.aspx"
                        target="_self"
                        style="text-decoration: none; position: absolute; top: 16rem; left: 3rem;">歷史訂單</a>
                </asp:PlaceHolder>
            </div>
            <div
                id="divMiddleContent"
                style="width: 70%; display: flex; flex-flow: column; align-items: center; text-align: left; justify-content: flex-start; float: right;">
                <table border="1" cellpadding="20" style="margin-top:3rem;">
                    <tr>
                        <th colspan="2" style="text-align: center">個人數據</th>
                    </tr>
                    <tr>
                        <th>尚未處理訂單數:</th>
                        <td>
                            <asp:Label
                                ID="lblUnFinishedOrderCount"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>已處理訂單數:</th>
                        <td>
                            <asp:Label
                                ID="lblFinishedOrderCount"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>總訂單數:</th>
                        <td>
                            <asp:Label ID="lblTotalOrderCount" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <asp:PlaceHolder runat="server" ID="phManager" Visible="false">
                        <tr>
                            <th colspan="2" style="text-align: center">部門數據</th>
                        </tr>
                        <tr>
                            <th>部門尚未處理訂單數:</th>
                            <td>
                                <asp:Label
                                    ID="lblSecUnFinishedOrderCount"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>部門已處理訂單數:</th>
                            <td>
                                <asp:Label
                                    ID="lblSecFinishedOrderCount"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>部門總訂單數:</th>
                            <td>
                                <asp:Label
                                    ID="lblSecTotalOrderCount"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>

                    </asp:PlaceHolder>
                </table>
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
