<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.Button.Overview.Default"
    Title="Gaia Ajax: Button" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Button" />
    <p>This sample demonstrates how to get the current server time and displays it in a
        <a href="../../Label/Overview/">Gaia Label.</a> Use your favorite network traffic inspector (FireBug, Chrome Developer
        Tools, Fiddler etc) to see how fast and small the response from the server is.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:Button ID="btn" runat="server" Text="Get Server Time" OnClick="btn_Click" />
<gaia:Label ID="lbl" runat="server" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder2">
    <p>
        Even though this is a very simple operation, a lot of things are going on behind
        the scenes. The core purpose of Gaia Ajax is that you don't have to think much
        about that. Just enjoy a postback free user experience, and continue using .NET
        for web development. It just works!
    </p>
</asp:Content>
