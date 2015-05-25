<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectClickable.Overview.Default"
    Title="Gaia Ajax: AspectClickable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.AspectClickable" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:Panel ID="zPanelClickable" runat="server" BorderStyle="Dotted"
        BorderWidth="1px" Height="150px" Width="300px">
        This area is now clickable.<br />
        <gaia:Label ID="zCoordinates" runat="server" Font-Bold="True"></gaia:Label></gaia:Panel>
</asp:Content>
