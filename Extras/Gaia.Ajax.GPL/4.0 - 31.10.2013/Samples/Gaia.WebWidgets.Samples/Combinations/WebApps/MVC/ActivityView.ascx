<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ActivityView.ascx.cs" 
    Inherits="Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.ActivityView" %>

<%@ Register 
    Src="Views/ActivityFilterView.ascx" 
    TagName="ActivityFilterView" 
    TagPrefix="filter" %>

<filter:ActivityFilterView 
    ID="zFilter" 
    runat="server" 
    OnFilterChanged="ViewChanged" />

<gaia:Panel 
    ID="zView" 
    runat="server"></gaia:Panel>


