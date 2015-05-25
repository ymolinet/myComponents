<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.DynamicImage.Captcha.Default"
    Title="Gaia Ajax: How to create a simple Captcha" %>

<%@ Register Src="Captcha.ascx" TagName="Captcha" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: How to create a simple Captcha</h1>
    <p>
        A <a href="http://en.wikipedia.org/wiki/CAPTCHA">Captcha</a>&nbsp;
        is a type of challenge-response test used in computing to ensure
        that the response is not generated by a computer. This example
        shows how easy it is to build a simple Captcha control using
        Gaia Ajax. The features are not complete, and we have left it
        to you make it complete. The core of the sample is the <a href="../Overview/">
            Gaia Ajax DynamicImage</a> control, which you will find
        in the user control.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <table>
        <tr>
            <td>
                First Name:
            </td>
            <td>
                <gaia:TextBox ID="zFirstName" runat="server" />
                <gaia:RequiredFieldValidator 
                    ID="zRequiredFieldValidator1" 
                    runat="server"
                    ControlToValidate="zFirstName">*</gaia:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Last Name:
            </td>
            <td>
                <gaia:TextBox ID="zLastName" runat="server" />
                <gaia:RequiredFieldValidator 
                    ID="zRequiredFieldValidator2" 
                    runat="server"
                    ControlToValidate="zLastName">*</gaia:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                Retype the text<br />
                on the image:</td>
            <td>
                <uc1:Captcha 
                    ID="zCaptcha" 
                    runat="server" 
                    CaptchaLength="5" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <gaia:Button ID="zSubmit" runat="server" OnClick="zSubmit_Click"
                    Text="Submit Form" /></td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <gaia:Label ID="zResult" runat="server"></gaia:Label></td>
        </tr>
    </table>
</asp:Content>
