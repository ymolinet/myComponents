<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.EffectAppear.Overview.Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Effects.EffectAppear" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:LinkButton 
    ID="zButtonDemo" 
    runat="server"
    OnClick="zButtonDemo_Click" 
    Text="Run Demo!">
    </gaia:LinkButton>
    
<gaia:LinkButton 
    ID="zButtonReset" 
    runat="server"
    OnClick="zButtonReset_Click" 
    Text="Reset">
    </gaia:LinkButton>

<gaia:Panel 
    ID="zPanel" 
    runat="server"
    BorderStyle="Solid"
    BorderWidth="1"
    Style="display:none; padding: 10px; text-align: center"
    BackColor="lightblue" 
    Height="150px"
    Width="150px">
</gaia:Panel>

</asp:Content>
