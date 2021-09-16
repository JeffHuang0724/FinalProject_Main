<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderItemList.aspx.cs"
    Inherits="FinalProject.SystemAdmin.Warehouse.OrderItemList" %>

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
                <div style="width: 25%; text-align: center">
                    <a
                        href="/SystemAdmin/UserDefault.aspx"
                        target="_self"
                        style="text-decoration: none">訂單管理</a>
                </div>
                <div style="width: 25%; text-align: center">
                    <a
                        href="/SystemAdmin/Warehouse/ItemList.aspx"
                        target="_blank"
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
                <a
                    href="/SystemAdmin/Warehouse/Order.aspx"
                    target="_self"
                    style="text-decoration: none; position: absolute; top: 12rem; left: 3rem;">處理訂單</a>
                <a
                    href="/SystemAdmin/Warehouse/HistoryOrder.aspx"
                    target="_self"
                    style="text-decoration: none; position: absolute; top: 16rem; left: 3rem;">歷史訂單</a>
            </div>
            <div
                id="divMiddleContent"
                style="width: 70%; display: flex; flex-flow: column; align-items: center; text-align: center; justify-content: center; float: right;">
                <h2>訂單商品列表</h2>
                <asp:PlaceHolder ID="plcNoData" runat="server" Visible="false">
                    <p style="color: red; background-color: yellow; font-size: large">
                        查無此訂單之訂購品項資料
                    </p>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="plcHasData" runat="server" Visible="true">
                    <table>
                        <tr>
                            <td>訂單編號：<asp:Literal
                                runat="server"
                                ID="ltlOrderNo"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView
                                    ID="gvWarehouseOrderItemList"
                                    runat="server"
                                    AutoGenerateColumns="False"
                                    OnRowDataBound="gvWarehouseOrderItemList_RowDataBound"
                                    OnRowCommand="gvWarehouseOrderItemList_RowCommand"
                                    Style="margin-top: 1rem"
                                    CellPadding="4"
                                    ForeColor="#333333"
                                    GridLines="Both">
                                    <Columns>
                                        <asp:BoundField
                                            DataField="ItemNo"
                                            HeaderText="產品編號" />
                                        <asp:BoundField
                                            DataField="ItemName"
                                            HeaderText="產品名稱" />
                                        <asp:BoundField
                                            DataField="ItemCount"
                                            HeaderText="訂單數量" />
                                        <asp:BoundField
                                            DataField="Remark"
                                            HeaderText="客戶備註" />
                                        <asp:TemplateField HeaderText="是否有貨">
                                            <ItemTemplate>
                                                <asp:Label
                                                    ID="lblName"
                                                    runat="server"
                                                    Text='<%# Bind("WetherStock") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="貨源確認">
                                            <ItemTemplate>
                                                <asp:Button
                                                    ID="btnEnough"
                                                    Text="貨源足夠"
                                                    runat="server"
                                                    CommandName="Enough"
                                                    CommandArgument="<%# Container.DataItemIndex %>" />
                                                <asp:Button
                                                    ID="btnNotEnough"
                                                    Text="貨源不足"
                                                    runat="server"
                                                    CommandName="NotEnough"
                                                    CommandArgument="<%# Container.DataItemIndex %>"
                                                    Style="margin-left: 1.5rem" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle
                                        BackColor="#507CD1"
                                        Font-Bold="True"
                                        ForeColor="White" />
                                    <HeaderStyle
                                        BackColor="#507CD1"
                                        Font-Bold="True"
                                        ForeColor="White" />
                                    <PagerStyle
                                        BackColor="#2461BF"
                                        ForeColor="White"
                                        HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle
                                        BackColor="#D1DDF1"
                                        Font-Bold="True"
                                        ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>內部備註：<asp:Literal
                                runat="server"
                                ID="ltlInnerRemark"></asp:Literal>
                            </td>
                        </tr>
                        <asp:PlaceHolder runat="server" ID="plcEdit" Visible="false">
                            <tr>
                                <td>內部備註：<asp:TextBox
                                    runat="server"
                                    ID="txtInnerRemark"
                                    TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td>
                                <asp:Button
                                    runat="server"
                                    ID="btnOrderSubmit"
                                    Text="訂單確認送出"
                                    Font-Size="Medium"
                                    OnClick="btnOrderSubmit_Click"
                                    Style="margin-top: 1rem" />
                                <asp:PlaceHolder
                                    runat="server"
                                    ID="plcManager"
                                    Visible="false">
                                    <asp:Button
                                        runat="server"
                                        ID="btnReject"
                                        Text="退回"
                                        Font-Size="Medium"
                                        OnClick="btnReject_Click"
                                        Style="margin-top: 1rem; margin-left: 2rem;" />
                                </asp:PlaceHolder>
                            </td>
                        </tr>
                    </table>
                    <asp:PlaceHolder ID="plcErrMsg" runat="server" Visible="false">
                        <div style="margin-top: 1rem">
                            <p
                                style="color: red; background-color: yellow; font-size: large">
                                <asp:Literal runat="server" ID="ltlErrMsg"></asp:Literal>
                            </p>
                        </div>
                    </asp:PlaceHolder>
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
