<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Core.master"
    Title="Gaia Ajax: Cascading DropDown" Inherits="Gaia.WebWidgets.Samples.BasicControls.DropDownList.CascadingDropDown.Default"
    Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">
    <h1>
        Gaia Ajax: Cascading DropDownList</h1>
    <p>
        This is our <em>cascading Ajax DropDownList sample</em>. In
        ASP.NET AJAX Toolkit Extensions there's an example of how to
        create a "related DropDownList/ComboBox widget". In Gaia Ajax
        we get this wanted behaviour by using the standard DropDownList control
        without any modifications.
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    Select a genre:
    <gaia:DropDownList ID="ddlGamesGenres" runat="server" AutoPostBack="True"
        OnSelectedIndexChanged="ddlGamesGenres_SelectedIndexChanged" Width="180px" />
    <br />
    <br />
    Select a game:
    <gaia:DropDownList ID="ddlGames" runat="server" AutoPostBack="true"
        OnSelectedIndexChanged="ddlGames_SelectedIndexChanged" Enabled="false" Width="180px" />
    <br />
    <br />
    <gaia:Label ID="lblSelectedGame" runat="server" Text="" />
</asp:Content>
