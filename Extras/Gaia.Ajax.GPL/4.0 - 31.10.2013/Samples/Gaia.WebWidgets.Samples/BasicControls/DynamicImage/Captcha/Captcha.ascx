<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Captcha.ascx.cs"
    Inherits="Gaia.WebWidgets.Samples.BasicControls.DynamicImage.Captcha.Captcha" %>
<gaia:Panel runat="server" ID="zCaptchaContainer">
    <gaia:DynamicImage runat="server" ID="zDynImg" OnRetrieveImage="zDynImg_RetrieveImage" /><br />
    <gaia:TextBox ID="zText" runat="server" />
</gaia:Panel>
