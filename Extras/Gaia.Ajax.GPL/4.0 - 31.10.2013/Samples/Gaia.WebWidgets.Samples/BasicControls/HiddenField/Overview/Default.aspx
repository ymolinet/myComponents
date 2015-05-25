<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.HiddenField.Overview.Default"
    Title="Gaia Ajax: HiddenField" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.HiddenField" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:Button runat="server" ID="zButton" Text="Get value of HiddenField"
        OnClick="zButton_Click" />
    <gaia:Label ID="zLabel" runat="server" />
    <gaia:HiddenField runat="server" ID="zHiddenField" Value="I am hidden, so you can't see me" />
</asp:Content>
