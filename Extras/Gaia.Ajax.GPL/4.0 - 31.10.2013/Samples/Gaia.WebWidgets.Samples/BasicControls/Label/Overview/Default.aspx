<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.Label.Overview.Default"
    Title="Gaia Ajax: Label" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Label" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:Label ID="zLabel1" runat="server">This is your label</gaia:Label><br />
    <br />
    <gaia:CheckBox ID="zBold" runat="server" AutoPostBack="True"
        OnCheckedChanged="zBold_CheckedChanged" Text="Toggle boldness" />
    <gaia:CheckBox ID="zCssClass" runat="server" AutoPostBack="True"
        OnCheckedChanged="zCssClass_CheckedChanged" Text="Add/remove CssClass" />
</asp:Content>
