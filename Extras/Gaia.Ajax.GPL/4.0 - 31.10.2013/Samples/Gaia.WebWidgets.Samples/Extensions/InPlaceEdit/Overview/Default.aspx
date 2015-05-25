<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.InPlaceEdit.Overview.Default"
    Title="Gaia Ajax: InPlaceEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Extensions.InPlaceEdit" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="container-inplaceedit">
        <div>
            <b>Default:</b><br />
            <gaia:InPlaceEdit ID="zInPlaceEdit1" runat="server" CssClass="control-inplaceedit">Edit me</gaia:InPlaceEdit></div>
        <div>
            <b>HasButtons="True"</b><br />
            <gaia:InPlaceEdit ID="zInPlaceEdit2" runat="server" CssClass="control-inplaceedit"
                HasButtons="True">Edit me</gaia:InPlaceEdit></div>
        <div>
            <b>MouseClicksToTriggerEditMode="DoubleClick"</b><br />
            <gaia:InPlaceEdit ID="zInPlaceEdit3" runat="server" CssClass="control-inplaceedit"
                MouseClicksToTriggerEditMode="DoubleClick">Edit me</gaia:InPlaceEdit></div>
        <div>
            <b>Subscribing to OnTextChanged event and AutoPostBack = True</b><br />
            <gaia:InPlaceEdit ID="zInPlaceEdit4" runat="server" CssClass="control-inplaceedit"
                OnTextChanged="zInPlaceEdit4_OnTextChanged">Edit me</gaia:InPlaceEdit>
            <br /><gaia:Label ID="zResultTextChanged" runat="server" /></div>
    </div>
    <br />
    <br />
</asp:Content>
