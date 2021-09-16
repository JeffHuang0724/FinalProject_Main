<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPager.ascx.cs" Inherits="FinalProject.UserControls.ucPager" %>
<div>
    <asp:Literal runat="server" ID="ltPager"></asp:Literal>
    <a runat="server" ID="aLinkFirst" href="#">First</a>
    <a runat="server" ID="aLink1" href="#">1</a>
    <a runat="server" ID="aLink2" href="#">2</a>
    <asp:Literal runat="server" ID="ltlCurrentPage"></asp:Literal>
    <a runat="server" ID="aLink4" href="#">4</a>
    <a runat="server" ID="aLink5" href="#">5</a>
    <a runat="server" ID="aLinkLast" href="#">Last</a>
</div>