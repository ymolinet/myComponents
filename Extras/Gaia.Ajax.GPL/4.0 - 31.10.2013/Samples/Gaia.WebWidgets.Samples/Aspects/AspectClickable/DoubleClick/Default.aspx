<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectClickable.DoubleClick.Default"
    Title="Gaia Ajax: AspectClickable - DoubleClick Event" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Gaia Ajax: AspectClickable - DoubleClick Event</h1>
<p>This demonstrates how to add AspectClickable to trap DoubleClick events for ANY Gaia
Ajax control. 
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:panel 
    id="zPanel" 
    runat="Server" 
    CssClass="panel">     
</gaia:panel>

</asp:Content>
