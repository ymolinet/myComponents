<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Step2.ascx.cs" Inherits="Gaia.WebWidgets.Samples.Core.BrowserHistory.DynamicUserControls.Step2" %>
<h1>Step 2</h1>
<p>You've now reached the Second Step defined in UserControl named Step2.ascx.
We've also modified the location url so now it's safe to click the back button. The
BrowserHistory control allows you to bookmark pages as you perform Ajax callbacks and
thereby allowing you to restore the state based on these clues easily. The DynamicUserControl 
entry code also make it easy to <gaia:LinkButton ID="zGoBack" runat="server" Text="programmatically go back" OnClick="zGoBack_Click"></gaia:LinkButton> 
Or click the button below to go to the next Step

</p>

<gaia:Button 
    ID="zNextStep" 
    runat="server" 
    Text="Go to Step 3" 
    OnClick="zNextStep_Click" />