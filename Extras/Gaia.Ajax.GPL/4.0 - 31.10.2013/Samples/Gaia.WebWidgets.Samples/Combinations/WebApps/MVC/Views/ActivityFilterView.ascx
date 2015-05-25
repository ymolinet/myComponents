<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivityFilterView.ascx.cs" Inherits="Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Views.ActivityFilterView" %>
<div style="margin-bottom: 5px;">
    <h3>Select Person</h3>
    <gaia:DropDownList 
        ID="zPersons" 
        runat="server" 
        AutoPostBack="true" 
        OnSelectedIndexChanged="ViewChanged">
    </gaia:DropDownList>
</div>
<div style="margin-bottom: 5px;">
    <h3>Select Date</h3>
    <gaia:DateTimePicker 
        ID="zWhen" 
        runat="server" 
        HasDropDownButton="false" 
        HasTimePart="false" 
        Width="50px"
        Format="MM/dd"
        OnSelectedDateChanged="ViewChanged" />
</div>
