<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="True"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.Button.ButtonInRepeater.Default"
    Title="Gaia Ajax Button" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax Button in Repeater</h1>
    <p>
        This sample shows dynamic creation of Gaia Buttons in a repeater</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:DropDownList ID="ddl" runat="server" OnSelectedIndexChanged="ddl_OnSelectedIndexChanged"
        AutoPostBack="True">
        <asp:ListItem>1</asp:ListItem>
        <asp:ListItem Selected="True">10</asp:ListItem>
        <asp:ListItem>25</asp:ListItem>
        <asp:ListItem>50</asp:ListItem>
        <asp:ListItem>100</asp:ListItem>
    </gaia:DropDownList>
    <gaia:Label runat="server" ID="lblMsg" />
    <gaia:Panel runat="server" ID="pnl">
        <asp:Repeater ID="rep" runat="server" OnItemCommand="rep_OnItemCommand">
            <ItemTemplate>
                <gaia:Button runat="server" Text='<%# Eval("Name") %>' CommandArgument='<%# Eval("Name")%>' />
            </ItemTemplate>
        </asp:Repeater>
    </gaia:Panel>
</asp:Content>
