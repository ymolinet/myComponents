<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" Codebehind="Default.aspx.cs"
    Inherits="Gaia.WebWidgets.Samples.Effects.Various.ShowMethod.Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Effect.Show()</h1>
<p>This demonstrates the usage of Effect.Show() which allows you to apply Effects to DOM
nodes based on their ID. 
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
   
   <div style="height:350px;">
    <gaia:Button 
        ID="zFireEffectButton" 
        Text="Drop Out!" 
        runat="server"
        OnClick="zFireEffectButton_Click" />
    <div id="div" 
        style="position: absolute; width: 300px; height: 300px; border: 1px ridge black; background-color: #666;">
        <p style="color:white;">This is a normal DOM element</p>
    </div>
   </div> 
   
</asp:Content>
