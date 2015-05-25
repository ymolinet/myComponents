<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Core.XhtmlTagFactory.Overview.Default"
    Title="Gaia Ajax: XhtmlTagFactory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <samples:GDoc runat="server" Member="T:Gaia.WebWidgets.HtmlFormatting.XhtmlTagFactory" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:Button ID="btn" runat="server" Text="Get valid XHTML link" OnClick="btn_OnClick" />
<br />
<gaia:Label ID="lbl" runat="server" />
</asp:Content>
