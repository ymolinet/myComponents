<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectResizable.MaxHeightWidth.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>AspectResizable with MaxHeight and MaxWidth, also bound in a rectangle</h1>
<p>This AspectResizable has been configured with a MaxWidth and MaxHeight of 250 pixels.
This is a useful feature to constrain the area in which items can be resized;
This sample also demonstrates using snapping functionality with AspectResizable and using BoundingRectangle.
</p>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    
    <gaia:Panel 
        ID="zPanel" 
        runat="Server" 
        CssClass="panel">
    
    </gaia:Panel>
    
    <br />
    
    <gaia:Panel 
        ID="zPanelSnap" 
        runat="Server" 
        CssClass="panel">
    
    </gaia:Panel>
    
    <br />
    
    <gaia:Panel runat="server" ID="zContainer" CssClass="panelContainer">
        <gaia:Panel 
            ID="zPanelBound" 
            runat="Server" 
            CssClass="panelBound">
        
        </gaia:Panel>
    </gaia:Panel>

</asp:Content>
