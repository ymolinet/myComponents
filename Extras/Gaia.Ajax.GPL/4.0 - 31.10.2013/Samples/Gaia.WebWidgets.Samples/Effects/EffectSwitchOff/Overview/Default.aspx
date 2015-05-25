<%@ Page Language="C#" StylesheetTheme="gaiax" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.EffectSwitchOff.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Effects.EffectSwitchOff" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
  <div>
  
    <gaia:Label 
        ID="zMessage" 
        runat="server" 
        Text="Click the TV to turn it off"></gaia:Label>
    
    <br />
    
    <gaia:ImageButton 
        ID="imgButtonTv" 
        runat="server" 
        ImageUrl="tv.jpg"
        Height="200px"
        OnClick="imgButtonTv_Click" />
        
    <gaia:Timer 
        ID="timer" 
        runat="server" 
        Milliseconds="4000" 
        Enabled="false">
    </gaia:Timer>
        
    </div>
</asp:Content>
