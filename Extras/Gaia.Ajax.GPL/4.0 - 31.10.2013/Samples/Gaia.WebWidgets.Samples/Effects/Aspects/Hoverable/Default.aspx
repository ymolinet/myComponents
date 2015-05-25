<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.Aspects.Hoverable.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>AspectHoverable MouseOver/MouseOut Effects</h1>
<p>
The Image below has attached custom effects to MouseOver and MouseOut.
The effects are run clientside and doesn't require a call to the server unless
you explicitly listen to the Events on the Aspect itself. 
</p>
<p><b>Note: AspectHoverable must be added to the widget for correct behaviour</b></p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:Image 
    ID="zImage" 
    runat="server" 
    ImageUrl="digital_dreams.jpg" />

</asp:Content>
