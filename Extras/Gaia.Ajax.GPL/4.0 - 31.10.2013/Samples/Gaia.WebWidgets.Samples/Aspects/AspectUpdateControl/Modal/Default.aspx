<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectUpdateControl.Modal.Default"
    Title="Gaia Ajax: Update Control with Modality" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: Update Control with Modality</h1>
    <p>
        Use the update control combined with modality if you have a
        operation that you know takes a little while on the server and
        it is important that the user doesn't click again or start any
        other events.</p>
    <p>
        Note: you will need to handle the construction of modality yourself
        with CSS. Take a look at the attached styles.css and you will
        see how to do it.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    Trigger Thread.Sleep(2000)<br />
    <gaia:Button ID="zButton1" runat="server" Text="Trigger Modal Update Control"
        OnClick="zButton1_Click" />
    <gaia:Panel ID="zModalUpdateControl" runat="server" Style="display: none;"
        CssClass="ajax-loader-modal">
        <div class="ajax-loader-modal-content">
            Waiting for result...</div>
    </gaia:Panel>
</asp:Content>
