<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectClickable.RelativeCoordinates.Default"
    Title="Gaia Ajax: AspectClickable - Using Relative Coordinates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Using Relative Coordinates</h1>
<p>By setting UseRelativeCoordinates to true the x,y coordinates retrieved from the 
ClickEventArgs will be relative to the AspectContainer. Just try it by clicking in the
Red Panel below. Another Box will be added at that exact position. 
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:panel 
    id="zPanel" 
    style="position: relative;"
    runat="Server" 
    CssClass="panel">     
</gaia:panel>

</asp:Content>
