<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectResizable.Overview.Default"
    Title="Gaia Ajax: Aspect Resizable" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.AspectResizable" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:Label runat="server" ID="lblMsg" Text="Try to resize the panel" /><br />
    <gaia:Panel runat="server" ID="pnlResizable" BorderColor="Blue"
        BorderStyle="Dotted" Width="100px" Height="75px" BorderWidth="2px">
    </gaia:Panel>
    
</asp:Content>
