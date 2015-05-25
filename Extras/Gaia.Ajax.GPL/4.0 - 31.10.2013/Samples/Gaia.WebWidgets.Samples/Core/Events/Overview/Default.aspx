<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Core.Events.Overview.Default"
    Title="Gaia Ajax: Event System - TextBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Gaia Ajax: Event System - TextBox</h1>
<p>At the heart of Gaia lies the flexible event system which dispatches familiar events on the server like
Click, IndexChanged, etc. Additionally you get events which are familiar to you from Windows programming like
MouseOver, MouseOut, Blur, Focus, Closing, etc. This is made possible by the powerful ajax machinery built into 
all Gaia Ajax controls.</p>
<p>The textbox in this example listens to</p>
<ul style="list-style-type:circle; padding-left: 20px;">
    <li>Blur</li>
    <li>Focus</li>
    <li>DoubleClick</li>
    <li>TextChanged</li>
    <li>TextSelected</li>
</ul>
<p><b>Just try it and look at the code to see how easy you can develop this yourself</b></p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

    <gaia:TextBox 
        ID="zInput" 
        AutoPostBack="true"
        KeyChangeEvents="true"
        KeyChangeEventsInterval="250"
        runat="server"
        OnBlur="zInput_Blur"
        OnFocused="zInput_Focus"
        OnDoubleClick="zInput_DoubleClick"
        OnTextChanged="zInput_TextChanged"
        OnSelect="zInput_TextSelected" />
    
    <gaia:Label 
        ID="zInputInfo" 
        ForeColor="Green" 
        runat="server" />

</asp:Content>
