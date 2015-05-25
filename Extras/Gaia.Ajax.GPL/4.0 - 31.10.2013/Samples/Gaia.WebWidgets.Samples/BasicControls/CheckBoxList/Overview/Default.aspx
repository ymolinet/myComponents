<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.CheckBoxList.Overview.Default"
    Title="Gaia Ajax: CheckBoxList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.CheckBoxList" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div style="margin-left: 150px; position: absolute;">
        <gaia:Label runat="server" ID="zLabel"></gaia:Label>
    </div>
    <gaia:CheckBoxList ID="zCheckBoxList" runat="server" AutoPostBack="true"
        OnSelectedIndexChanged="zCheckBoxList_OnSelectedIndexChanged">
        <asp:ListItem Enabled="True" Selected="True" Text="First" />
        <asp:ListItem Enabled="True" Selected="False" Text="Second" />
        <asp:ListItem Enabled="False" Selected="True" Text="Third (disabled)" />
    </gaia:CheckBoxList>
</asp:Content>
