<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.Validators.RangeValidator.Default"
    Title="Gaia Ajax: RangeValidator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: RangeValidator</h1>
    <p>
        The Gaia Ajax RangeValidator checks whether the value of an
        input control is within a specified range of values. It inherits
        from the <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.rangevalidator.aspx">
            ASP.NET RangeValidator</a>, and has built-in Ajax behaviour.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    Enter a number from 1 to 10<br />
    <gaia:TextBox ID="zInput" runat="server"></gaia:TextBox>
    <gaia:Button ID="zSubmit" runat="server" OnClick="zSubmit_Click"
        Text="Submit" />
    <gaia:RangeValidator ID="zRangeValidator1" runat="server" ControlToValidate="zInput"
        EnableClientScript="False" MaximumValue="10" MinimumValue="1"
        SetFocusOnError="True" Type="Integer">The value must be from 1 to 10!</gaia:RangeValidator>
    <br />
    <gaia:Label ID="zResult" runat="server" />
</asp:Content>
