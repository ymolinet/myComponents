<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.DateTimePicker.Localization.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Gaia Ajax: Calendar and DateTimePicker Localization</h1>
<p>In this example the DateTimePicker is localized into the Norwegian language by 
setting the Culture property. Resources are added to the App_GlobalResources folder can easily
be localized into any culture you want. 
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:DateTimePicker ID="zPicker" runat="server" Culture="nb-NO" Format="yyyy-MM-dd" />
</asp:Content>
