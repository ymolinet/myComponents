<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" Codebehind="Default.aspx.cs"
    Inherits="Gaia.WebWidgets.Samples.Core.Manager.IncludeFiles.Default" Title="Gaia Ajax: Include JavaScript Files in Ajax response" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        Manager: Include JavaScript file in Ajax response</h1>
    <p>
        If you want to include your own JavaScript in an Ajax response, Gaia provides a nice
        way to achieve this. You can get the file from the file system or from the resource
        library in your application.</p>
    <p>
        The sample also show how you evaluate a JavaScript function from the server code
        using the method AddScriptForClientSideEval(..)
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:LinkButton 
        ID="lnkBtn" 
        runat="server" 
        OnClick="lnkBtn_Click">
        Include CustomJavaScript.js in Ajax request
    </gaia:LinkButton>
    <br />
    <gaia:Label 
        ID="Label1" 
        runat="server" />
    <br />
    <gaia:LinkButton 
        ID="lnkBtnInvokeJsFunction" 
        runat="server" 
        OnClick="lnkBtnInvokeJsFunction_Click"
        Visible="False">
        Run JavaScript function in included file
    </gaia:LinkButton>
</asp:Content>
