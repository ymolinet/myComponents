<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.Validators.RequiredFieldValidator.Default"
    Title="Gaia Ajax: Required Field Validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: Required Field Validator</h1>
    The Gaia Ajax Required Field validator makes the associated
    input control a required field. It inherits
        from the <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.requiredfieldvalidator.aspx">
            ASP.NET RequiredFieldValidator</a>, and has built-in Ajax behaviour.<br />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:ValidationSummary ID="ValidationSummary1" runat="server" />
First Name:
<gaia:TextBox ID="txtFirstName" runat="server"></gaia:TextBox>
<gaia:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
    ControlToValidate="txtFirstName" ErrorMessage="Please enter your first name">*
</gaia:RequiredFieldValidator>
<br />
Last Name:
<gaia:TextBox ID="txtLastName" runat="server"></gaia:TextBox>
<gaia:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
    ControlToValidate="txtLastName" ErrorMessage="Please enter your last name">*</gaia:RequiredFieldValidator>
<br />
<gaia:Button ID="btnSubmit" runat="server" Text="Submit" />
</asp:Content>
