<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.Window.OnClosing.Default"
    Title="Gaia Ajax: Window - OnClosing Event" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: Window - OnClosing Event</h1>
    <p>
        With the OnClosing event on the Gaia Ajax Window you can put
        validation and logic into natural areas of your web application.
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:LinkButton ID="zReOpenWindow" runat="server" Text="Open Window"
        OnClick="zReOpenWindow_OnClick" />
    <gaia:Window ID="zWindow" runat="server" Height="250px" Width="300px"
        Maximizable="false" Minimizable="false" Caption="Force user action"
        Modal="true" OnClosing="zWindow_OnClosing">
        <div class="window-container">
            <strong>Today's Challenge:</strong> click each of the three
            squares before you are allowed to close the window.<br />
            Hint: try to close the window before all are clicked.
            <br />
            <br />
            <div id="squares-container">
                <gaia:Label ID="zl1" runat="server" OnClick="Label_Clicked"></gaia:Label>
                <gaia:Label ID="zl2" runat="server" OnClick="Label_Clicked"></gaia:Label>
                <gaia:Label ID="zl3" runat="server" OnClick="Label_Clicked"></gaia:Label>
            </div>
        </div>
    </gaia:Window>
</asp:Content>
