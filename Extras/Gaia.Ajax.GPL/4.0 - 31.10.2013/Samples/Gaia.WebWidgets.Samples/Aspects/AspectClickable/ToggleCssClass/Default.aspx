<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectClickable.ToggleCssClass.Default"
    Title="Gaia Ajax: AspectClickable - Toggle CssClass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Gaia Ajax: AspectClickable - Toggle CssClass</h1>
    <p>
        Easily Toggle the CssClass property by adding AspectClickable
        and listen to the Clicked event.
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:panel 
    id="zPanel" 
    runat="Server" 
    CssClass="panel">     
</gaia:panel>

</asp:Content>
