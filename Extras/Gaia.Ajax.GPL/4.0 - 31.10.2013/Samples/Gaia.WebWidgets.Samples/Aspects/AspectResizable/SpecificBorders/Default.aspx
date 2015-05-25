<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectResizable.SpecificBorders.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>AspectResizable with Specific Borders</h1>
<p>This AspectResizable is configured to only allow resizing from the bottom and right side
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

    <gaia:Panel 
        ID="zPanel" 
        runat="Server" 
        CssClass="panel"> 
    </gaia:Panel>

</asp:Content>
