<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectDraggable.Overview.Default"
    Title="Gaia Ajax: AspectDraggable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.AspectDraggable" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<div style="position:relative; height: 200px;">

    <gaia:panel 
        id="zDraggable" 
        runat="Server" 
        CssClass="panel-drag"></gaia:panel>

</div>

</asp:Content>
