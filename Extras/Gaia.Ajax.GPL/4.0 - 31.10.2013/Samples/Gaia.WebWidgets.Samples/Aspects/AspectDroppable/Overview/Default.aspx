<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectDroppable.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.AspectDroppable" />
<h2>Passing a Custom ID in Drag and Drop Scenarios</h2>
<p>By setting the IdToPass property you can send custom IDs from each draggable
into the dropcontainer. This makes it easy and flexible to retrieve items based on a custom
defined string. 
</p>
<p>You always have access to the ControlID, but it's a bit more work to find the control
again.
</p>
<p><b>Note: </b>A Custom HoverClass has also been defined to signalize that this is a 
drop container and you are allowed to drop the Draggable Control here.
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<div style="position:relative; height: 200px;">

<gaia:panel 
    id="zDraggable" 
    runat="Server" 
    CssClass="panel-drag"></gaia:panel>
    
<gaia:panel 
    id="zDroppable" 
    runat="Server" 
    CssClass="panel-drop">
    <gaia:Label id="zLabel" ForeColor="WhiteSmoke" runat="server"></gaia:Label>
    </gaia:panel>

</div>


</asp:Content>
