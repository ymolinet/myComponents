<%@ Page 
    Language="C#" 
    MasterPageFile="~/Core.master" 
    AutoEventWireup="true" 
    Title="Effect Highlight" 
    Inherits="Gaia.WebWidgets.Samples.Effects.EffectHighlight.Overview.Overview" 
    Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Effects.EffectHighlight" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
        <asp:Table CssClass="table" runat="server" ID="table"></asp:Table>
</asp:Content>