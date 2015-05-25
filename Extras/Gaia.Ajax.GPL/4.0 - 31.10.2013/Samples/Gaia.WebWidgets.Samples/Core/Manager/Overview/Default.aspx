<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" Codebehind="Default.aspx.cs"
    Inherits="Gaia.WebWidgets.Samples.Core.Manager.Overview.Default" Title="Gaia Ajax: Manager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        Gaia Ajax: Manager</h1>
    <p>
        You will find the Manager in Gaia.WebWidgets.Manager.Instance as a Singleton instance.
        This is the main entry point for getting and setting information about the Ajax
        request in Gaia.
    </p>
    <p>
        In this example we simply want to determine if it is a Gaia Ajax request/callback.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:Button ID="zButton" runat="server" Text="Click this Gaia:Button" />
    <gaia:Label ID="zLabel" runat="server" />
</asp:Content>
