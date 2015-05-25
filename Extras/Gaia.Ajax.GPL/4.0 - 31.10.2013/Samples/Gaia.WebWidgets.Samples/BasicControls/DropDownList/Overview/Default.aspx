<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.DropDownList.Overview.Default"
    Title="Gaia Ajax: DropDownList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.DropDownList" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:DropDownList ID="zDropDownList" runat="server" AutoPostBack="true"
        OnSelectedIndexChanged="zDropDownList_SelectedIndexChanged">
        <asp:ListItem>First Item</asp:ListItem>
        <asp:ListItem Selected="True">Second Item</asp:ListItem>
        <asp:ListItem>Third item</asp:ListItem>
        <asp:ListItem>Fourth Item</asp:ListItem>
    </gaia:DropDownList>
    <gaia:Label ID="zLabel" runat="server"></gaia:Label>
</asp:Content>
