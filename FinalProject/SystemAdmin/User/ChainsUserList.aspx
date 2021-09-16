<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ChainsUserList.aspx.cs"
    Inherits="FinalProject.SystemAdmin.User.ChainsUserList" %>

<%@ Register
    Src="~/UserControls/ucPager.ascx" TagPrefix="uc1" TagName="ucPager" %>

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
                <a
                    href="/SystemAdmin/User/UserList.aspx"
                    target="_self"
                    style="text-decoration: none; position: absolute; top: 12rem; left: 3rem;">中央廚房員工通訊錄</a>
                <a
                    href="/SystemAdmin/User/ChainsUserList.aspx"
                    target="_self"
                    style="text-decoration: none; position: absolute; top: 16rem; left: 3rem;">分店員工通訊錄</a>
                <asp:PlaceHolder runat="server" ID="phManager" Visible="false">
                    <a
                        href="/SystemAdmin/User/UserDetail.aspx"
                        target="_self"
                        style="text-decoration: none; position: absolute; top: 20rem; left: 3rem;">新增部門員工</a>
                </asp:PlaceHolder>
            </div>
            <div
                id="divMiddleContent"
                style="width: 70%; display: flex; flex-flow: column; align-items: center; text-align: left; justify-content: center; float: right;">
                <h2>分店員工通訊錄</h2>
                <asp:GridView
                    ID="gvChainsEmployeeList"
                    runat="server"
                    AutoGenerateColumns="False"
                    Style="margin-top: 1rem"
                    CellPadding="10"
                    ForeColor="#333333"
                    GridLines="Both">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="EmployeeID" HeaderText="員工編號" />
                        <asp:BoundField DataField="EmployeeName" HeaderText="員工姓名" />
                        <asp:BoundField DataField="EmployeePhone" HeaderText="員工電話" />
                        <asp:BoundField DataField="BranchNo" HeaderText="所在分店" />
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
                <div style="margin-top: 1rem">
                    <uc1:ucPager
                        runat="server"
                        ID="ucPager"
                        PageSize="10"
                        CurrentPage="1"
                        TotalSize="10"
                        Url="/SystemAdmin/User/ChainsUserList.aspx" />
                    <asp:PlaceHolder ID="plcNoData" runat="server" Visible="false">
                        <p style="color: red; background-color: yellow; font-size: large">
                            查無分店員工資料
                        </p>
                    </asp:PlaceHolder>
                </div>
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
