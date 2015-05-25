<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Step4.ascx.cs" Inherits="Gaia.WebWidgets.Samples.Core.BrowserHistory.DynamicUserControls.Step4" %>
<h1>Step 4</h1>
<p>Congratulations! You have finished the Mini-Wizard. All made easy with Gaia Ajax</p>

<gaia:Button 
    ID="zStartOver" 
    runat="server" 
    Text="Click here to start over ... " 
    OnClick="zStartOver_Click" />