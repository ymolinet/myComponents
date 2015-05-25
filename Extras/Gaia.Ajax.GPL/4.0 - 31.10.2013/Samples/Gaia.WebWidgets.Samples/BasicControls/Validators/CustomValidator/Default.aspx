<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.Validators.CustomValidator.Default"
    Title="Gaia Ajax: CustomValidator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: CustomValidator</h1>
    <p>
        The Gaia Ajax CustomValidator performs user-defined validation
        on an input control. It inherits from the <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.customvalidator.aspx">
            ASP.NET CustomValidator</a>, and has built-in Ajax behaviour.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    Enter an even number:<br />
    <gaia:TextBox ID="zInput" runat="server" />
    <gaia:Button ID="zSubmit" runat="server" OnClick="zSubmit_Click"
        Text="Validate" />
    <gaia:CustomValidator ID="zCustomValidator1" runat="server" OnServerValidate="zCustomValidator1_ServerValidate"
        ControlToValidate="zInput" Display="Dynamic" ErrorMessage="Not an even number!" />
    <br />
    <gaia:Label ID="zResult" runat="server"></gaia:Label>
</asp:Content>
