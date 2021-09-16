<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePwd.aspx.cs"
Inherits="FinalProject.SystemAdmin.User.ChangePwd" %>

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
        style="
          width: 100%;
          height: 30%;
          display: flex;
          flex-flow: row;
          align-items: center;
          justify-content: center;
          text-align: center;
        "
      >
        <div id="divLogo" style="width: 10%; float: left">
          <a href="/SystemAdmin/UserDefault.aspx" target="_self">
            <img
              alt="毛豆"
              src="/Images/Logo.png"
              width="80"
              height="80"
              style="border-radius: 50%"
            />
          </a>
        </div>
        <div id="divTitle" style="width: 70%; text-align: center">
          <h1>毛豆端火鍋訂單出貨系統</h1>
        </div>
        <!-- 首頁右方功能列表(依照部門不同顯示內容作調整) -->
        <div
          id="divList"
          style="
            width: 20%;
            float: right;
            text-align: right;
            display: flex;
            flex-flow: row;
          "
        >
          <asp:PlaceHolder runat="server" ID="phSalesManu" Visible="false">
            <div style="width: 30%; text-align: center">
              <a
                href="/SystemAdmin/UserDefault.aspx"
                target="_self"
                style="text-decoration: none"
                >訂單管理</a
              >
            </div>
            <div style="width: 30%; text-align: center">
              <a
                href="/SystemAdmin/User/UserInfo.aspx"
                target="_self"
                style="text-decoration: none"
                >帳號管理</a
              >
            </div>
            <div style="width: 30%; text-align: center">
              <asp:Button
                runat="server"
                ID="btnLogout"
                Text="登出"
                OnClick="btnLogout_Click"
                Style="border: none; background: none; color: #0000ff; margin: 0; padding: 0; font-size: 1rem;"
              />
            </div>
          </asp:PlaceHolder>
          <asp:PlaceHolder runat="server" ID="phWarehouseManu" Visible="false">
            <div style="width: 25%; text-align: center">
              <a
                href="/SystemAdmin/UserDefault.aspx"
                target="_self"
                style="text-decoration: none"
                >訂單管理</a
              >
            </div>
            <div style="width: 25%; text-align: center">
              <a
                href="/SystemAdmin/Warehouse/ItemList.aspx"
                target="_self"
                style="text-decoration: none"
                >品項管理</a
              >
            </div>
            <div style="width: 25%; text-align: center">
              <a
                href="/SystemAdmin/User/UserInfo.aspx"
                target="_self"
                style="text-decoration: none"
                >帳號管理</a
              >
            </div>
            <div style="width: 25%; text-align: center">
              <asp:Button
                runat="server"
                ID="Button1"
                Text="登出"
                OnClick="btnLogout_Click"
                Style="border: none; background: none; color: #0000ff; margin: 0; padding: 0; font-size: 1rem;"
              />
            </div>
          </asp:PlaceHolder>
        </div>
      </div>
      <!-- Header -->
      <!-- content -->
      <div
        id="divMiddle"
        style="
          width: 100%;
          height: 70%;
          display: flex;
          flex-flow: row;
          align-items: center;
          justify-content: center;
          text-align: center;
        "
      >
        <div
          id="divMiddleLeft"
          style="
            width: 10%;
            height: 100%;
            display: flex;
            flex-flow: column;
            align-items: center;
            justify-content: center;
            text-align: center;
          "
        >
          <a
            href="/SystemAdmin/User/UserList.aspx"
            target="_self"
            style="
              text-decoration: none;
              position: absolute;
              top: 12rem;
              left: 3rem;
            "
            >中央廚房員工通訊錄</a
          >
          <a
            href="/SystemAdmin/User/ChainsUserList.aspx"
            target="_self"
            style="
              text-decoration: none;
              position: absolute;
              top: 16rem;
              left: 3rem;
            "
            >分店員工通訊錄</a
          >
          <asp:PlaceHolder runat="server" ID="phManager" Visible="false">
            <a
              href="/SystemAdmin/User/UserDetail.aspx"
              target="_self"
              style="
                text-decoration: none;
                position: absolute;
                top: 20rem;
                left: 3rem;
              "
              >新增部門員工</a
            >
          </asp:PlaceHolder>
        </div>
        <div
          id="divMiddleContent"
          style="
            width: 70%;
            display: flex;
            flex-flow: column;
            align-items: center;
            text-align: left;
            justify-content: center;
            float: right;
          "
        >
          <h2 style="margin-top: 5rem">會員管理</h2>
          <table style="margin: auto; border-spacing: 3rem">
            <tr>
              <th>現在密碼:</th>
              <td>
                <asp:TextBox
                  ID="txtOldPwd"
                  runat="server"
                  TextMode="Password"
                ></asp:TextBox>
              </td>
            </tr>
            <tr>
              <th>再次確認:</th>
              <td>
                <asp:TextBox
                  ID="txtChkOldPwd"
                  runat="server"
                  TextMode="Password"
                ></asp:TextBox>
              </td>
            </tr>
            <tr>
              <th>新密碼:</th>
              <td>
                <asp:TextBox
                  ID="txtNewPwd"
                  runat="server"
                  TextMode="Password"
                ></asp:TextBox>
              </td>
            </tr>
            <tr>
              <td align="center" colspan="2">
                <asp:Button
                  ID="btnSave"
                  runat="server"
                  Text="確認送出"
                  OnClick="btnSave_Click"
                  Font-Size="Medium"
                />
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
          style="
            width: 20%;
            height: 100%;
            float: right;
            text-align: right;
            display: flex;
            flex-flow: row;
          "
        >
          <asp:Label
            ID="lblNowDate"
            runat="server"
            Style="position: fixed; bottom: 1rem; right: 1rem;"
          ></asp:Label>
        </div>
      </div>
    </form>
  </body>
</html>
