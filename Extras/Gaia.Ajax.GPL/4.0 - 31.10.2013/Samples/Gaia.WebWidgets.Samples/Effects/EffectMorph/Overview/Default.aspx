<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.EffectMorph.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Effects.EffectMorph" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:LinkButton 
    OnClick="zMorph_Click"
    ID="zMorph" 
    runat="server" 
    Text="Morph!">
</gaia:LinkButton>

<div style="position: relative;height: 200px;">

<gaia:Panel 
    id="zMorphPanel" 
    runat="server"
    Width="200"
    Height="200"
    BorderStyle="Dashed"
    BorderWidth="0"
    Style="top: 0px; left: 0px;background-color: #ccc;position: absolute;border-color: #fff;">
</gaia:Panel>

</div>
    

</asp:Content>
