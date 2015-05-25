<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.TextBox.Event_OnTextChanged.Default"
    Title="Gaia Ajax: TextBox - Event - OnTextChanged" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        TextBox Event: OnTextChanged</h1>
    <p>
        The Gaia Ajax TextBox is more than a usual text box. Among other
        features it gives you the ability to fire events when the user
        enter something in it. This enables the Gaia TextBox to be used
        e.g. as a filter and search input.</p>
    <p>
        To enable these features, follow these steps:</p>
    <ol>
        <li>Set AutoPostBack = true</li>
        <li>Enable the OnTextChanged event</li>
        <li>Under the Misc section in the Properties, set KeyChangeEvents
            = true</li>
        <li>Optional: Set KeyChangeEventsInterval (default:500ms), which
            is the interval in milliseconds the TextBox will raise the TextChanged
            event to the server</li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <b>Filter the list of tasks below:</b><br />
    <gaia:TextBox ID="zFilter" runat="server" AutoPostBack="True"
        KeyChangeEvents="True" OnTextChanged="zFilter_TextChanged"></gaia:TextBox>
        <br /><br />
    <gaia:Panel runat="server">
        <asp:Repeater ID="zRepeater" runat="server">    
            <ItemTemplate>
               <gaia:Label runat="server" Text='<%# Eval("ActivityName") + "|" %>' ></gaia:Label>
            </ItemTemplate>
        </asp:Repeater>
    </gaia:Panel>
</asp:Content>
