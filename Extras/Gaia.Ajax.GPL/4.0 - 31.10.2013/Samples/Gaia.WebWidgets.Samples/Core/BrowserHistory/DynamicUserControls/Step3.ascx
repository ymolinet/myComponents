<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Step3.ascx.cs" Inherits="Gaia.WebWidgets.Samples.Core.BrowserHistory.DynamicUserControls.Step3" %>
<h1>Step 3</h1>
<p>This is the third Step. Another "clue" was added to the browser history so please try
the browser back button if you want. Notice how you easily can get started with creating
functionality for your customers with this navigation/loading framework easily accessible</p>
<p>Here we only offer a button to go to the last step</p>
<gaia:Button 
    ID="zNextStep" 
    runat="server" 
    Text="Go to the final step" 
    OnClick="zNextStep_Click" />