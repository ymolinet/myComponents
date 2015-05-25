<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectUpdateControl.Multiple.Default"
    Title="Gaia Ajax: Multiple Update Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: Multiple Update Controls</h1>
    <p>
        In the sample below you can click each word in the quote to
        colorize that word. The ajax callback will take 1 second due
        to a Sleep on the server and meanwhile the UpdateControl kicks
        in for that particular control.
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<h2>Click on any of the words in the quote below</h2>
<gaia:Panel id="zPanel" runat="server"></gaia:Panel>
</asp:Content>
