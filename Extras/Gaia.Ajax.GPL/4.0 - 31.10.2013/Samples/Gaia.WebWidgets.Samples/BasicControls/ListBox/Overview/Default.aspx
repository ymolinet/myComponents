<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.ListBox.Overview.Default"
    Title="Gaia Ajax: ListBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.ListBox" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:ListBox ID="zListBox" runat="server" AutoPostBack="True"
        OnSelectedIndexChanged="zListBox_SelectedIndexChanged" Rows="3">
        <asp:ListItem>First</asp:ListItem>
        <asp:ListItem>Second</asp:ListItem>
        <asp:ListItem>Third</asp:ListItem>
    </gaia:ListBox>
    <gaia:Label ID="zLabel" runat="server" /><br/>
    <gaia:Button ID="zButtonClearSelection" runat="server" 
                 Text="Clear selection" Enabled="False"
        OnClick="zButtonClearSelection_Click"/>
</asp:Content>
