<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.Calendar.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Extensions.Calendar" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:Calendar 
    ID="zCalendar" 
    runat="server" 
    FirstDayOfWeek="Monday"
    OnActiveDateViewChanged="zCalendar_ActiveDateViewChanged"
    OnCalendarDayClicked="zCalendar_CalendarDayClicked"
    OnTodayButtonClicked="zCalendar_TodayButtonClicked" />

<br />
    
<gaia:Label 
   ForeColor="DarkGreen" 
   Font-Size="Larger"
   ID="zInfo" 
   runat="server"></gaia:Label>

<br /><br />

<gaia:Button 
    ID="zClearDate"
    runat="server"
    Enabled="false"
    OnClick="zClearDate_Click"
    Text="Click here to clear the Selected Date"
    />   

</asp:Content>
