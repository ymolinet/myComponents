<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.RadioButton.Overview.Default"
    Title="Gaia Ajax: RadioButton" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.RadioButton" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    Select which continent you live on:<br />
    <gaia:RadioButton ID="zRadioButton1" runat="server" GroupName="RadioGroup1"
        Text="America" />
    <gaia:RadioButton ID="zRadioButton2" runat="server" GroupName="RadioGroup1"
        Text="Africa" />
    <gaia:RadioButton ID="zRadioButton3" runat="server" GroupName="RadioGroup1"
        Text="Europe" />
    <gaia:RadioButton ID="zRadioButton4" runat="server" GroupName="RadioGroup1"
        Text="Asia" />
    <gaia:RadioButton ID="zRadioButton5" runat="server" GroupName="RadioGroup1"
        Text="Australia" /><br />
    &nbsp;<gaia:Button ID="zButtonConfirm" runat="server" Text="I confirm that I live on this continent" OnClick="zButtonConfirm_Click" />
    <gaia:Label ID="zLabelContinent" runat="server"></gaia:Label>
</asp:Content>
