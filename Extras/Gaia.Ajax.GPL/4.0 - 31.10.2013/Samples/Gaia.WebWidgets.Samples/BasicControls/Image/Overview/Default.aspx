<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.Image.Overview.Default"
    Title="Gaia Ajax: Image" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Image" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:Image ID="zImage1" runat="server" />This
    is text beside the Gaia Ajax Image. We will see how it behaves
    when we change the alignment of the image.<br />
    <hr />
    <br />
    <div class="controlPanel">
        ImageAlign<br />
        <gaia:DropDownList ID="zDropDownListAlign" runat="server" AutoPostBack="true"
            OnSelectedIndexChanged="zDropDownListAlign_SelectedIndexChanged">
            <asp:ListItem>AbsBottom</asp:ListItem>
            <asp:ListItem>AbsMiddle</asp:ListItem>
            <asp:ListItem>Baseline</asp:ListItem>
            <asp:ListItem>Bottom</asp:ListItem>
            <asp:ListItem>Left</asp:ListItem>
            <asp:ListItem>Middle</asp:ListItem>
            <asp:ListItem Selected="true">NotSet</asp:ListItem>
            <asp:ListItem>Right</asp:ListItem>
            <asp:ListItem>TextTop</asp:ListItem>
            <asp:ListItem>Top</asp:ListItem>
        </gaia:DropDownList>
    </div>
    <div class="controlPanel">
        Switch Image<br />
        <gaia:DropDownList ID="zDropDownListImage" runat="server" AutoPostBack="true"
            OnSelectedIndexChanged="zDropDownListImage_SelectedIndexChanged"
            DataValueField="ImageUrl" DataTextField="Text" />
    </div>
</asp:Content>
