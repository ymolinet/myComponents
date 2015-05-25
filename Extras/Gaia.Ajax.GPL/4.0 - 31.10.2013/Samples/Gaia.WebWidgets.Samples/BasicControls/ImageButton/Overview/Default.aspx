<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.ImageButton.Overview.Default"
    Title="Gaia Ajax: ImageButton" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.ImageButton" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<div class="play-buttons-container">
    <gaia:ImageButton ID="zPrev" runat="server" ImageUrl="~/media/icons/prev_gray.png"
        OnClick="zPrev_Click" />
    <gaia:ImageButton ID="zPlay" runat="server" ImageUrl="~/media/icons/play_gray.png"
        OnClick="zPlay_Click" />
    <gaia:ImageButton ID="zPause" runat="server" ImageUrl="~/media/icons/pause_gray_disabled.png"
        OnClick="zPause_Click" Enabled="false" />
    <gaia:ImageButton ID="zNext" runat="server" ImageUrl="~/media/icons/next_gray.png"
        OnClick="zNext_Click" />
</div>
<gaia:Label ID="zState" runat="server" />
</asp:Content>
