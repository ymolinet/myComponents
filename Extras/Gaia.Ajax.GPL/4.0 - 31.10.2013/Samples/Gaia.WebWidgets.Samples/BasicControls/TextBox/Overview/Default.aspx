<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.TextBox.Overview.Default"
    Title="Gaia Ajax: TextBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.TextBox" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:TextBox ID="zTextBox1" runat="server" Text="Here is the Gaia Ajax TextBox"
        Width="250px" />
    <br />
    <br />
    <div>
        <gaia:CheckBox ID="zBoldness" runat="server" AutoPostBack="True"
            OnCheckedChanged="zBoldness_CheckedChanged" Text="Bold?" />
        <br />
        <gaia:CheckBox ID="zEnabled" runat="server" AutoPostBack="True"
            Checked="true" OnCheckedChanged="zEnabled_CheckedChanged"
            Text="Enabled?" />
        <br />
        <gaia:CheckBox ID="zVisible" runat="server" AutoPostBack="True"
            Checked="true" OnCheckedChanged="zVisible_CheckedChanged"
            Text="Visible?" />
        <br />
        <gaia:CheckBox ID="zToggleCssClass" runat="server" AutoPostBack="True"
            OnCheckedChanged="zToggleCssClass_CheckedChanged" Text="Toggle CssClass" />
        <br />
        <gaia:Button ID="zSelectAll" runat="server" 
            Text="Select All Text" OnClick="zSelectAll_Click" />
    </div>
</asp:Content>
