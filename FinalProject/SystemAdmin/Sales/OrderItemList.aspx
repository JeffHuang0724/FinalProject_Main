<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderItemList.aspx.cs"
    Inherits="FinalProject.SystemAdmin.Sales.OrderItemList" %>

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
                <h2>訂單詳細品項列表</h2>
                <asp:PlaceHolder ID="plcNoData" runat="server" Visible="false">
                    <div style="margin-top: 1rem">
                        <p style="color: red; background-color: yellow; font-size: large">
                            查無此訂單內容
                        </p>
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="plcHasData" runat="server" Visible="true">
                    <asp:GridView
                        ID="gvSalesOrderItemList"
                        runat="server"
                        AutoGenerateColumns="False"
                        Style="margin-top: 1rem"
                        CellPadding="4"
                        ForeColor="#333333"
                        GridLines="Both">
                        <Columns>
                            <asp:BoundField DataField="ItemNo" HeaderText="產品編號" />
                            <asp:BoundField DataField="ItemName" HeaderText="產品名稱" />
                            <asp:BoundField DataField="ItemCount" HeaderText="訂單數量" />
                            <asp:BoundField
                                DataField="ItemPrice"
                                HeaderText="貨品單價"
                                DataFormatString="{0:N0}" />
                            <asp:BoundField
                                DataField="TotalAmount"
                                HeaderText="品項總額"
                                DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Remark" HeaderText="客戶備註" />
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
