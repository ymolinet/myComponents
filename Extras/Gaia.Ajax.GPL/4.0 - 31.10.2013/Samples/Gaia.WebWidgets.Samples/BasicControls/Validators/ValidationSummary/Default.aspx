<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.Validators.ValidationSummary.Default"
    Title="Gaia Ajax: ValidationSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: ValidationSummary</h1>
    <p>
        The Gaia Ajax ValidationSummary displays a summary of all validation
        errors inline on a Web page, in a message box, or both. It inherits
        from the <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.validationsummary.aspx">
            ASP.NET ValidationSummary</a>, and has built-in Ajax behaviour.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="validation-row">
        <div>
            <gaia:ValidationSummary ID="ValidationSummary1" runat="server"
                DisplayMode="List" HeaderText="Please enter a value in the following fields:" />
        </div>
    </div>
    <div class="validation-row">
        <div class="validation-col-first">
            Card Type:
            <gaia:RequiredFieldValidator ID="zRequiredFieldValidator1" ControlToValidate="zCardType"
                ErrorMessage="Card Type" Display="Dynamic" InitialValue=""
                runat="server">*</gaia:RequiredFieldValidator>
        </div>
        <div class="validation-col-second">
            <gaia:RadioButtonList ID="zCardType" RepeatLayout="Flow" runat="server">
                <asp:ListItem>MasterCard</asp:ListItem>
                <asp:ListItem>Visa</asp:ListItem>
            </gaia:RadioButtonList>
        </div>
    </div>
    <div class="validation-row">
        <div class="validation-col-first">
            Card Number:
            <gaia:RequiredFieldValidator ID="zRequiredFieldValidator2" runat="server"
                ControlToValidate="zCardNumber" ErrorMessage="Card Number">*</gaia:RequiredFieldValidator>
        </div>
        <div class="validation-col-second">
            <gaia:TextBox ID="zCardNumber" runat="server" />
        </div>
    </div>
    <div class="validation-row">
        <div class="validation-col-first">
            &nbsp;
        </div>
        <div class="validation-col-second">
            <gaia:Button ID="zSubmit" runat="server" Text="Submit" />
        </div>
    </div>
</asp:Content>
