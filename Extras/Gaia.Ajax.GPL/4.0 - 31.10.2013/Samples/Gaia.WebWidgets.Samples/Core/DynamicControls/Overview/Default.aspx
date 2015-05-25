<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Core.DynamicControls.Overview.Default"
    Title="Gaia Ajax: Using Dynamic Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        Gaia Ajax: Using Dynamic Controls</h1>
    <p>
        <strong>Dynamic Controls are a feature of ASP.NET, which means to
            work with controls that are not added statically/declarative
            in an ASPX page). Dynamic Controls are typically added in the
            Page_Load event. The need to master Dynamic Controls becomes
            more visible when dealing with highly interactive ajaxified
            websites. That's why learning about Dynamic Controls is very
            useful when using Gaia Ajax.</strong></p>
    <br />
    <p>
        This sample shows a generic approach with adding controls dynamically
        by adding a Gaia Ajax Timer dynamically in an Ajax callback</p>
    <p>
        The timer that gets rendered didn't exist at all on the page
        or in the lifecycle, before it gets added during the button
        callback. When it's added the first time, it must be added on
        every subsequent callback and that's the purpose of this code
        to teach you how to do that.
    </p>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    
<gaia:Button 
    ID="zCreateTimer" 
    runat="server" 
    Text="Add Timer" 
    OnClick="createTimer_Click" />

<gaia:Label 
    ID="zMessage" 
    runat="server"></gaia:Label>

<gaia:Panel 
    ID="zContainer" 
    runat="server">
</gaia:Panel>

</asp:Content>
