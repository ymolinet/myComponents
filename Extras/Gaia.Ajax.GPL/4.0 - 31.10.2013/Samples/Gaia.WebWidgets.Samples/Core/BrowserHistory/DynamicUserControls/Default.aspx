<%@ Page Language="C#" StylesheetTheme="gaiax" MasterPageFile="~/Core.master"
    AutoEventWireup="true" Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Core.BrowserHistory.DynamicUserControls.Default"
    Title="Gaia Ajax: Browser History with Dynamic User Controls" %>

<%@ Register 
    Assembly="Gaia.WebWidgets.Samples" 
    TagPrefix="samples"
    Namespace="Gaia.WebWidgets.Samples.Core.BrowserHistory.DynamicUserControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Gaia Ajax: BrowserHistory with Dynamic User Controls</h1>
<p>This example turns out to be quite useful in application development. All the nitty
gritty details of loading dynamic usercontrols, keeping state for the back/forward
buttons in the browser is abstracted away in the DynamicUserControl classes. 
</p>
<br />
<p>As an extra bonus you also get a clean API for navigating between the different
usercontrols. 
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<samples:DynamicUserControl ID="zHost" runat="server" SelectedEntryIndex="0">
  <DynamicUserControls>
      <samples:DynamicUserControlEntry Token="step1" UserControlPath="~/Core/BrowserHistory/DynamicUserControls/Step1.ascx" />
      <samples:DynamicUserControlEntry Token="step2" UserControlPath="~/Core/BrowserHistory/DynamicUserControls/Step2.ascx" />
      <samples:DynamicUserControlEntry Token="step3" UserControlPath="~/Core/BrowserHistory/DynamicUserControls/Step3.ascx" />
      <samples:DynamicUserControlEntry Token="step4" UserControlPath="~/Core/BrowserHistory/DynamicUserControls/Step4.ascx" />
  </DynamicUserControls>
</samples:DynamicUserControl>
</asp:Content>
