<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectUpdateControl.Overview.Default"
    Title="Gaia Ajax: AspectUpdateControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.AspectUpdateControl" />
    <p>
        Note: you need to set the style of the control you will use
        as Update Control to display:none;.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="content-action">
        <gaia:Button ID="zButtonNotLengthy" runat="server" OnClick="zButtonNotLengthy_Click"
            Text="Not Lengthy" />
        <gaia:Button ID="zButtonLengthy" runat="server" OnClick="zButtonLengthy_Click"
            Text="Lengthy" />
        <gaia:Panel ID="zUpdateControlButton" runat="server" CssClass="ajax-half-circle-blue"
            Style="display: none;">
        </gaia:Panel>
    </div>
    <div class="content-action">
        <gaia:CheckBox ID="zCheckBoxNotLengthy" runat="server" AutoPostBack="True"
            OnCheckedChanged="zCheckBoxNotLengthy_CheckedChanged" Text="Not Lengthy" />
        <gaia:CheckBox ID="zCheckBoxLengthy" runat="server" AutoPostBack="True"
            OnCheckedChanged="zCheckBoxLengthy_CheckedChanged" Text="Lengthy" />
        <gaia:Panel ID="zUpdateControlCheckBox" runat="server" CssClass="ajax-tiny-circle-red"
            Style="display: none;">
        </gaia:Panel>
    </div>
    <div class="content-action">
        <gaia:LinkButton ID="zLinkButtonNotLengthy" runat="server" OnClick="zLinkButtonNotLengthy_Click">Not Lengthy</gaia:LinkButton>
        <gaia:LinkButton ID="zLinkButtonLengthy" runat="server" OnClick="zLinkButtonLengthy_Click">Lengthy</gaia:LinkButton>
        <gaia:Panel ID="zUpdateControlLinkButton" runat="server" CssClass="ajax-flowerish-blue"
            Style="display: none;">
        </gaia:Panel>
    </div>
    <br />
    How long should lengthy be?<br />
    <gaia:DropDownList ID="zDelay" runat="server">
        <asp:ListItem Selected="True" Value="500">500ms</asp:ListItem>
        <asp:ListItem Value="1000">1sec</asp:ListItem>
        <asp:ListItem Value="2000">2sec</asp:ListItem>
        <asp:ListItem Value="3000">3sec</asp:ListItem>
    </gaia:DropDownList>
</asp:Content>
