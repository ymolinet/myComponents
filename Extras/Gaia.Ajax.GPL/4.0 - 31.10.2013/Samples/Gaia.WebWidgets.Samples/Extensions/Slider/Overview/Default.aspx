<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.Slider.Overview.Default"
    Title="Gaia Ajax: Slider" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Extensions.Slider" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    Horizontal Slider (default)
    <gaia:Slider ID="zSliderHorizontal" 
        runat="server" 
        Width="200px"
        OnValueChanged="slider_OnValueChanged" 
        OnInit="slider_OnInit" />
    <gaia:Label 
        ID="zResult" 
        runat="server" 
        Font-Bold="true" />
    <br />
    Vertical Slider
    <gaia:Slider 
        ID="zSliderVertical" 
        runat="server" 
        DisplayDirection="Vertical"
        Height="100px" 
        OnValueChanged="slider_OnValueChanged" 
        OnInit="slider_OnInit" />
</asp:Content>
