<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemDetail.aspx.cs"
    Inherits="FinalProject.SystemAdmin.Warehouse.ItemDetail" %>

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
                        target="_blank"
                        style="text-decoration: none">帳號管理</a>
                </div>
                <div style="width: 25%; text-align: center">
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
            </div>
            <div
                id="divMiddleContent"
                style="width: 70%; display: flex; flex-flow: column; align-items: center; text-align: center; justify-content: center; float: right;">
                <h2 style="margin-top: 5rem">庫存商品資訊</h2>
                <table style="margin: auto; border-spacing: 3rem">
                    <tr>
                        <th>商品編號:</th>
                        <td align="left">
                            <asp:Label ID="lblItemNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>商品名稱:</th>
                        <td align="left">
                            <asp:TextBox ID="txtItemName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>商品分類:</th>
                        <td align="left">
                            <asp:DropDownList ID="ddlCategory" runat="server">
                                <asp:ListItem Value="0" Text="肉類"></asp:ListItem>
                                <asp:ListItem Value="1" Text="海鮮類"></asp:ListItem>
                                <asp:ListItem Value="2" Text="蔬果類"></asp:ListItem>
                                <asp:ListItem Value="3" Text="火鍋料類"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>庫存數量:</th>
                        <td align="left">
                            <asp:TextBox
                                ID="txtStockCount"
                                runat="server"
                                TextMode="Number"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>單價:</th>
                        <td align="left">
                            <asp:TextBox
                                ID="txtItemPrice"
                                runat="server"
                                TextMode="Number"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button
                                ID="btnSave"
                                runat="server"
                                Text="儲存"
                                OnClick="btnSave_Click"
                                Font-Size="Medium" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Literal runat="server" ID="ltMsg"></asp:Literal>
                        </td>
                    </tr>
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
