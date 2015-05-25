<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="True"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.CheckBox.Overview.Default"
    Title="Gaia Ajax: CheckBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.CheckBox" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    Home Phone
    <gaia:TextBox ID="txtHome" runat="server"></gaia:TextBox>
    <gaia:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
        ControlToValidate="txtHome">*</gaia:RequiredFieldValidator><br />
    Work Phone
    <gaia:TextBox ID="txtWork" runat="server"></gaia:TextBox>
    <gaia:CheckBox ID="cbxSame" runat="server" AutoPostBack="True"
        OnCheckedChanged="cbxSame_CheckedChanged" Text=" Same as Home Phone" />
</asp:Content>
