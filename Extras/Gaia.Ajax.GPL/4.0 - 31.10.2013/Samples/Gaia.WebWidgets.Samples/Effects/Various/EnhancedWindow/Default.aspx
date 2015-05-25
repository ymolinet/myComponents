<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.Various.EnhancedWindow.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Enhanced Window</h1>
<p>Effects can be added to various events for some of the advanced Widgets. 
In this demo the Gaia Window introduces custom Close, Appear, Minimize and Restore effects
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

    <gaia:Button 
        id="buttonOpen" 
        runat="server" 
        Text="Open Enhanced Window" 
        OnClick="buttonOpen_Click" />

    <gaia:Window 
        ID="window" 
        runat="server"
        OpacityWhenMoved="1" 
        Caption="Gaia Ajax Window" 
        Width="320px" 
        Height="200px"
        Visible="false"
        Draggable="true">
            <div></div>
        </gaia:Window>

</asp:Content>
