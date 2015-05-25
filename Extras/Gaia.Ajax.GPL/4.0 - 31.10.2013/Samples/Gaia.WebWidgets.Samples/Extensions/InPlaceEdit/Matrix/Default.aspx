<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.InPlaceEdit.Matrix.Default"
    Title="Gaia Ajax: InPlaceEdit - Matrix" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Gaia Ajax: InPlaceEdit - Matrix</h1>
<p>Make anything editable by using InPlaceEdit</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<h2>Click a cell and type in a number between 0 and 99</h2>
<p>Select Matrix Layout

<gaia:RadioButtonList 
    ID="zMatrixSize" 
    runat="server"
    RepeatDirection="Horizontal"
    RepeatLayout="Flow"
    AutoPostBack="true" 
    OnSelectedIndexChanged="ZMatrixSizeSelectedIndexChanged">
        <Items>
            <asp:ListItem Selected="true" Value="4" Text="4x4"></asp:ListItem>
            <asp:ListItem Value="8" Text="8x8"></asp:ListItem>
            <asp:ListItem Value="12" Text="12x12"></asp:ListItem>
            <asp:ListItem Value="16" Text="16x16"></asp:ListItem>
        </Items>
    </gaia:RadioButtonList>

</p>

<gaia:Panel 
    ID="zWrap" 
    runat="server" />
    
<gaia:Label 
    ID="zResult"
    ForeColor="Green"
    Font-Size="Large"
    runat="server" />

</asp:Content>
