<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Core.Events.MouseOverOut.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Event System: MouseOver MouseOut</h1>
<p>Who thought you could listen to OnMouseOver and OnMouseOut in ASP.NET? Well, with Gaia Ajax you can and it's
as easy as classic Windows Programming. The panel below will listen to these events and it's all programmed
from the server in .NET managed code. 
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:Panel
    id="zPanel"
    runat="server"
    OnMouseOut="zPanel_MouseOut"
    OnMouseOver="zPanel_MouseOver"
    BackColor="lightblue"
    Width="600px"
    Height="175px">
    <h2>Flexible Events for ALL Gaia Ajax Controls</h2>
    <p>Things starts to get real interesting when you look at the 
    stuff going over the wire. Even though we change the background
    color of the Panel in the ajax callback, the only thing that get
    serialized is the style for the Panel and the text for the Label
    </p>
    
    <br />
    <gaia:Label 
        Font-Bold="true" 
        Font-Size="Larger" 
        ID="zPanelOutput" 
        runat="server"></gaia:Label>
    
    </gaia:Panel>

</asp:Content>
