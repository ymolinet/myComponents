<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectHoverable.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.AspectHoverable" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

  <gaia:panel 
    id="zPanel" 
    runat="Server" 
    CssClass="panel">
         
         <gaia:Label 
            ID="zMessage" 
            runat="server">
         </gaia:Label>
   
   </gaia:panel>

</asp:Content>
