<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectClickable.ShortHandSyntax.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Initializing Aspects with the ShortHand Syntax</h1>
<p>Most Aspects have overloaded constructors where you can pass construct the 
entire Aspect initially. Then it can be added directly to the Aspects collection
so that it all can be done with 1 line.
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:panel 
    id="zPanel" 
    runat="Server" 
    CssClass="panel">     
</gaia:panel>

</asp:Content>
