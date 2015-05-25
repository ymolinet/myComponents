<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.Validators.RegularExpressionValidator.Default"
    Title="Gaia Ajax: Regular Expression Validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: Regular Expression Validator</h1>
    <p>
        The Gaia Ajax Regular Expression Validator validates whether
        the value of an associated input control matches the pattern
        specified by a regular expression. It inherits from the <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.regularexpressionvalidator.aspx">
            ASP.NET RegularExpressionValidator</a>, and has built-in
        Ajax behaviour.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    Enter your email address:<br />
    <gaia:TextBox ID="zEmail" runat="server" />
    <gaia:RegularExpressionValidator ID="zRegularExpressionValidator1"
        runat="server" ErrorMessage="Please provide a correct e-mail address"
        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
        ControlToValidate="zEmail" Display="Dynamic"></gaia:RegularExpressionValidator>
    <gaia:RequiredFieldValidator ID="zRequiredFieldValidator1" runat="server"
        ControlToValidate="zEmail" Display="Dynamic">Please enter something in the textbox</gaia:RequiredFieldValidator><br />
    <gaia:Button ID="zSubmit" runat="server" OnClick="btnSubmit_Click"
        Text="Submit Form" />
    <gaia:Label ID="zResult" runat="server" />
</asp:Content>
