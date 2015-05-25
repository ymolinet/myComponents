<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Step1.ascx.cs"
    Inherits="Gaia.WebWidgets.Samples.Core.BrowserHistory.DynamicUserControls.Step1" %>
<h1>
    Step 1</h1>
<p>
    This is the first dynamically loaded user control. Notice how
    the location url has been changed to indicate #step1. Now click
    the button to advance to Step2.
</p>
<gaia:Button ID="zNextStep" runat="server" Text="Next ..." OnClick="zNextStep_Click" />