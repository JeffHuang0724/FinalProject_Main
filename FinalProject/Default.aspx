<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FinalProject.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>毛豆端火鍋訂單出貨系統</title>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Header -->
        <div id="divHeader" style="width: 100%; display: flex; flex-flow: row; align-items: center; justify-content: center; text-align: center">
            <!-- <div style="width: 90%; float: right;display: flex; flex-flow:row;"> -->
            <img alt="LogoImages" src="Images/Logo.png" width="80" height="80" style="margin-right: 1rem; border-radius:50%;" />
            <h1>毛豆端火鍋訂單出貨系統</h1>
            <!-- </div> -->
        </div>
        <!-- Content -->
        <div id="divContent" style="display: flex; flex-flow: column; align-items: center; text-align: center; justify-content: center;">
            <asp:PlaceHolder ID="plcLogin" runat="server" Visible="true">
                <table style="width: 20%; margin: auto; border-spacing: 1.5rem;">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblNowDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>帳號: </th>
                        <td>
                            <asp:TextBox ID="txtAccount" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>密碼: </th>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnLogin" runat="server" Text="登入" Font-Size="Medium" OnClick="btnLogin_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Literal ID="ltlMsg" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>
        </div>
    </form>
</body>
</html>
