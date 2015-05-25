<%@ Page Language="C#" MasterPageFile="~/Core.master"
    AutoEventWireup="true" Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.DateTimePicker.Overview.Default"
    Title="Gaia Ajax: DateTimePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Extensions.DateTimePicker" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:DateTimePicker ID="zDateTimePicker1" runat="server" Width="150px" Format="MM/dd/yyyy HH:mm"
    FirstDayOfWeek="Monday" HasTimePart="true" OnSelectedDateChanged="zDateTimePicker1_SelectedDateChanged" />
<gaia:Label ID="zResult" runat="server"></gaia:Label><br />
<br />
<b>Set FirstDayOfWeek</b><br />
<gaia:DropDownList ID="zFirstDayOfWeek" runat="server" OnInit="zFirstDayOfWeek_OnInit"
    AutoPostBack="True" OnSelectedIndexChanged="zFirstDayOfWeek_SelectedIndexChanged">
    <asp:ListItem>Sunday</asp:ListItem>
    <asp:ListItem>Monday</asp:ListItem>
    <asp:ListItem>Tuesday</asp:ListItem>
    <asp:ListItem>Wednesday</asp:ListItem>
    <asp:ListItem>Thursday</asp:ListItem>
    <asp:ListItem>Friday</asp:ListItem>
    <asp:ListItem>Saturday</asp:ListItem>
</gaia:DropDownList>
<br />
<b>Set Format</b><br />
<gaia:TextBox ID="zFormat" runat="server" Width="150px" AutoPostBack="true"
    KeyChangeEvents="true" OnTextChanged="zFormat_OnTextChanged">MM/dd/yyyy HH:mm</gaia:TextBox>
<gaia:Label ID="zFormatErrorMsg" runat="server" />
<br />
<b>Select Time by setting HasTimePart to true</b><br />
<gaia:CheckBox ID="zHasTimePart" runat="server" Checked="true"
    OnCheckedChanged="zHasTimePart_Checked" Text="Has TimePart?">
</gaia:CheckBox>
</asp:Content>
