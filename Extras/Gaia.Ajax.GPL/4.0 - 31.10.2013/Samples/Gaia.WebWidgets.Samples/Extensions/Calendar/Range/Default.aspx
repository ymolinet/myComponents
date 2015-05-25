<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.Calendar.Range.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Date Ranges</h1>
<p>A minor example demonstrating how you can have the user select date ranges by using
two calendars on the page. They could easily have been switched with <a href="../../DateTimePicker/Overview/">DateTimePickers</a> 
if needed.
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="left">
        <b>StartDate</b>
        <gaia:Calendar 
                ID="zCalendarStart" 
                runat="server"
                OnSelectedDateChanged="Calendars_Changed" 
                FirstDayOfWeek="Monday"
                />
    </div>
    <div class="left">
        <b>EndDate</b>
        <gaia:Calendar 
                ID="zCalendarEnd" 
                runat="server" 
                OnSelectedDateChanged="Calendars_Changed"
                FirstDayOfWeek="Monday" />
    </div>
    <div class="clear">
        <gaia:Label 
            Font-Size="Large"
            ForeColor="Blue" 
            ID="zTimeSpan" 
            runat="server" />
    </div>
</asp:Content>
