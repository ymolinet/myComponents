<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Core.Manager.GlobalUpdateControl.Default"
    Title="Gaia Ajax: Global UpdateControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: Global UpdateControl</h1>
    <p>
        When you have operations that take some time on the server and
        you would like to keep the user informed that something is processing,
        an UpdateControl is a very useful feature.</p>
    <p>
        You have two options for this in Gaia Ajax:</p>
    <ol>
        <li>Attach a Global UpdateControl via the Gaia Manager, which this
            sample shows. It will display an UpdateControl for all events
            triggered from the controls at the page.</li>
        <li>Add an AspectUpdateControl to a specific Gaia control that you
            want to display an UpdateControl. <a href="../../../Aspects/AspectUpdateControl/Overview/">
                Samples for AspectUpdateControl showing simple</a> and <a href="../../../Aspects/AspectUpdateControl/Modal/">
                    modal UpdateControls.</a></li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:Panel runat="server" ID="updateControl" CssClass="container-updatecontrol">
        <img id="Img1" runat="server" alt="Update Control"
            src="~/media/contentImages/ajax-loader.gif" />
    </gaia:Panel>
    <gaia:Button ID="zButton" runat="server" Text="Standard Button" OnClick="zButton_Click" /><br /><br />
    <gaia:LinkButton ID="zLinkButton" runat="server" OnClick="zLinkButton_Click">LinkButton</gaia:LinkButton><br /><br />
    <gaia:DropDownList ID="zDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="zDropDownList_SelectedIndexChanged">
        <asp:ListItem Selected="True">DropDownList</asp:ListItem>
        <asp:ListItem>Value 1</asp:ListItem>
        <asp:ListItem>Value 2</asp:ListItem>
        <asp:ListItem>Value 3</asp:ListItem>
    </gaia:DropDownList>
</asp:Content>
