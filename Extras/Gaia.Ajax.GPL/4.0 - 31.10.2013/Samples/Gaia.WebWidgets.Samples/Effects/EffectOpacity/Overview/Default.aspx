<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.EffectOpacity.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Effects.EffectOpacity" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<div style="height: 300px;">
<p>Select Opacity: 
    <gaia:RadioButtonList 
    ID="zOpacity" 
    runat="server"
    RepeatDirection="Horizontal"
    AutoPostBack="true"  RepeatLayout="Flow"
    OnSelectedIndexChanged="zOpacity_SelectedIndexChanged">
        <Items>
            <asp:ListItem Selected="true" Value="1">100%</asp:ListItem>
            <asp:ListItem Value="0.75">75%</asp:ListItem>
            <asp:ListItem Value="0.50">50%</asp:ListItem>
            <asp:ListItem Value="0.25">25%</asp:ListItem>
        </Items>
</gaia:RadioButtonList></p>
    
<gaia:Panel 
    ID="zPanel" 
    runat="server"
    BorderStyle="Solid"
    BorderWidth="1"
    BackColor="lightblue"
    Width="150px">
    
    <p>Nunc scelerisque sollicitudin leo. 
    Fusce id sapien. Ut
    faucibus. Aliquam erat volutpat. Vestibulum arcu libero, 
    iaculis id, pretium eu, posuere gravida, felis. Aliquam 
    erat volutpat.</p>
    
</gaia:Panel>

</div>

</asp:Content>
