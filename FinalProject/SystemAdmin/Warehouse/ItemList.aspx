<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemList.aspx.cs" Inherits="FinalProject.SystemAdmin.Warehouse.ItemList" %>

<%@ Register Src="~/UserControls/ucPager.ascx" TagPrefix="uc1" TagName="ucPager" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>毛豆端火鍋訂單出貨系統</title>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Header -->
        <div id="divHeader" style="width: 100%; height: 30%; display: flex; flex-flow: row; align-items: center; justify-content: center; text-align: center">
            <div id="divLogo" style="width: 10%; float: left;">
                <a href="/SystemAdmin/UserDefault.aspx" target="_self">
                    <img alt="毛豆" src="/Images/Logo.png" width="80" height="80" style="border-radius: 50%;" />
                </a>
            </div>
            <div id="divTitle" style="width: 70%; text-align: center;">
                <h1>毛豆端火鍋訂單出貨系統</h1>
            </div>
            <!-- 首頁右方功能列表(依照部門不同顯示內容作調整) -->
            <div id="divList" style="width: 20%; float: right; text-align: right; display: flex; flex-flow: row;">
                <div style="width: 25%; text-align: center;">
                    <a href="/SystemAdmin/UserDefault.aspx" target="_self" style="text-decoration: none;">訂單管理</a>
                </div>
                <div style="width: 25%; text-align: center;">
                    <a href="#" target="_self" style="text-decoration: none;">品項管理</a>
                </div>
                <div style="width: 25%; text-align: center;">
                    <a href="/SystemAdmin/User/UserInfo.aspx" target="_blank" style="text-decoration: none;">帳號管理</a>
                </div>
                <div style="width: 25%; text-align: center;">
                    <asp:Button runat="server" ID="btnLogout" Text="登出" OnClick="btnLogout_Click" Style="border: none; background: none; color: #0000ff; margin: 0; padding: 0; font-size: 1rem;" />
                </div>
            </div>
        </div>
        <!-- Header -->
        <!-- content -->
        <div id="divMiddle" style="width: 100%; height: 70%; display: flex; flex-flow: row; align-items: center; justify-content: center; text-align: center;">
            <div id="divMiddleLeft" style="width: 10%; height: 100%; display: flex; flex-flow: column; align-items: center; justify-content: center; text-align: center;">
            </div>
            <div id="divMiddleContent" style="width: 70%; display: flex; flex-flow: column; align-items: center; text-align: center; justify-content: center; float: right;">
                <h2>庫存商品列表</h2>
                <asp:GridView ID="gvWarehouseItemList" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvWarehouseItemList_RowDataBound" Style="margin-top: 1rem" CellPadding="10" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="ItemNo" HeaderText="商品編號" />
                        <asp:TemplateField HeaderText="商品種類">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlCategory"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemName" HeaderText="商品名稱" />
                        <asp:TemplateField HeaderText="庫存數量">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStockCount"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemPrice" HeaderText="商品單價" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a href="/SystemAdmin/Warehouse/ItemDetail.aspx?ItemNo=<%# Eval("ItemNo") %>">編輯品項</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" Text="新增品項" Font-Size="Medium" Style="margin-top: 1rem;" />
                <div style="margin-top: 1rem;">
                    <uc1:ucPager runat="server" ID="ucPager" PageSize="10" CurrentPage="1" TotalSize="10" Url="/SystemAdmin/Warehouse/ItemList.aspx" />
                    <asp:PlaceHolder ID="plcNoData" runat="server" Visible="false">
                        <p style="color: red; background-color: yellow; font-size: large;">
                            庫存無商品，請再次確認!
                        </p>
                    </asp:PlaceHolder>
                </div>
            </div>
            <div id="divMiddleRight" style="width: 20%; height: 100%; float: right; text-align: right; display: flex; flex-flow: row;">
                <asp:Label ID="lblNowDate" runat="server" Style="position: fixed; bottom: 1rem; right: 1rem;"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
