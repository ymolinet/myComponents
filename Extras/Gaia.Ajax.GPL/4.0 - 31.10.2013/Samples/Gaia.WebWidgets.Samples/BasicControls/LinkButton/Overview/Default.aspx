<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.LinkButton.Overview.Default"
    Title="Gaia Ajax: LinkButton" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.LinkButton" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:LinkButton ID="zLinkButton1" runat="server" OnClick="zLinkButton1_Click">This is your LinkButton</gaia:LinkButton><br />
    <br />
    <gaia:CheckBox ID="zBold" runat="server" AutoPostBack="True"
        OnCheckedChanged="zBold_CheckedChanged" Text="Toggle boldness" />
    <gaia:CheckBox ID="zCssClass" runat="server" AutoPostBack="True"
        OnCheckedChanged="zCssClass_CheckedChanged" Text="Add/remove CssClass" />
    <gaia:CheckBox ID="zToggleEnabling" runat="server" AutoPostBack="True"
        OnCheckedChanged="zToggleEnabling_CheckedChanged" Text="Disabled" />
</asp:Content>
