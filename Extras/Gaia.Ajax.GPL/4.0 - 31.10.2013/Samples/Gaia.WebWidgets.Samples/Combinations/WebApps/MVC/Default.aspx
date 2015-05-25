<%@ Page 
    Language="C#" 
    MasterPageFile="~/Core.master" 
    AutoEventWireup="true" 
    CodeBehind="Default.aspx.cs" 
    Inherits="Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Default" 
    Title="MVC on ASP.NET with Gaia Ajax" %>

<%@ Register Src="~/UI/CodeViewerControl.ascx" TagName="CodeViewerControl"
    TagPrefix="uc2" %>
    
<%@ Register 
    Src="ActivityView.ascx" 
    TagName="ActivityView" 
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>MVC on ASP.NET with Gaia Ajax</h1>
<p>The MVC pattern demonstrated with 4 different views using the 
WebForms model on top of ASP.NET with Gaia Ajax. </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

    <h3>Select View</h3>
    <gaia:DropDownList 
        ID="zAvailableViews" 
        AutoPostBack="true" 
        OnSelectedIndexChanged="zAvailableViews_SelectedIndexChanged" 
        runat="server">
    </gaia:DropDownList>

    <gaia:Button 
        ID="zRunTests" 
        runat="server" 
        Text="Click here to run Tests" 
        OnClick="zRunTests_Click" />

    <uc1:ActivityView id="zActs" runat="server" />

</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder2">
    <gaia:Panel ID="zInfo" runat="server" style="border: 1px solid #aaa; padding: 10px; " >
<h2>MVC - Model View Controller</h2>
<p>Just select one of the available views from the dropdownlist and see how that part of the application
is swapped out. The controller has no concept of the different View implementations, just the model on which it operates.
Here's the controller which is used for this demo - 
<uc2:CodeViewerControl 
    id="zControllerCode" 
    Format="csharp"
    CodeFileName="Controller Code"
    CodeFile="Controllers/ActivityController.cs" 
    runat="server">
</uc2:CodeViewerControl>
</p>
<br />
<p>
 Because of this distinct separation between UI and business layer, it's easy to construct a suite of unit tests
 which uses various mock objects to represent the UI. Demonstrated by looking at the -
 <uc2:CodeViewerControl 
    id="zTestsCode" 
    Format="csharp"
    CodeFileName="Unit Tests"
    CodeFile="Tests/ActivityTests.cs" 
    runat="server">
</uc2:CodeViewerControl>
</p>
<br />
<p><b>So how would the model(s) look like?</b>
<br />Basically the models are just interfaces and you can/should model them as 
you find natural for your specific needs. In this example I've created an 
<uc2:CodeViewerControl 
    id="zCodeActivityModel" 
    Format="csharp"
    CodeFileName="IActivityModel"
    CodeFile="Models/IActivityModel.cs" 
    runat="server">
</uc2:CodeViewerControl> which is a Composite model leveraging another two models, one for filtering and one for displaying the results
</p>
<br />
<uc2:CodeViewerControl 
    id="CodeViewerControl1" 
    Format="csharp"
    CodeFileName="IActivityFilter"
    CodeFile="Models/IActivityFilter.cs" 
    runat="server">
</uc2:CodeViewerControl>
<uc2:CodeViewerControl 
    id="CodeViewerControl2" 
    Format="csharp"
    CodeFileName="IActivityList"
    CodeFile="Models/IActivityList.cs" 
    runat="server">
</uc2:CodeViewerControl>
<br /><br />
<p><b>So how would you define a "view", for example a Web View? </b><br />
Easy! - Just implement the model and pass the (this) instance to the controller. This can be abstracted away in 
various base classes. For this demo we've created a couple of base controllers for convenience. 
Here's the code for some of these views</p> 
<uc2:CodeViewerControl 
    id="zGridViewMarkup" 
    Format="aspx"
    CodeFileName="GridView View (markup)"
    CodeFile="Views/ActivityListGridView.ascx" 
    runat="server">
</uc2:CodeViewerControl>
<br />
<uc2:CodeViewerControl 
    id="CodeViewerControl3" 
    Format="csharp"
    CodeFileName="GridView View (codebehind)"
    CodeFile="Views/ActivityListGridView.ascx.cs" 
    runat="server">
</uc2:CodeViewerControl>
<br />
<uc2:CodeViewerControl 
    id="CodeViewerControl4" 
    Format="aspx"
    CodeFileName="Repeater View (markup)"
    CodeFile="Views/ActivityListRepeater.ascx" 
    runat="server">
</uc2:CodeViewerControl>
<br />
<uc2:CodeViewerControl 
    id="CodeViewerControl5" 
    Format="csharp"
    CodeFileName="Repeater View (codebehind)"
    CodeFile="Views/ActivityListRepeater.ascx.cs" 
    runat="server">
</uc2:CodeViewerControl>
<br /><br />
<p><b>That's nice, but how are the views loaded and displayed? </b>
<br />The views can be loaded in many different ways, either statically or dynamically. In this demo we've 
created a little utility which enumerates over all the IActivityList views in the Views folder and display these in 
a dropdownlist. Then we automatically load the view and pass it to the controller. In this demo, not even the composite
model knows about it's child implementation(s) so you basically can "swap" out any part of the UI whenever you want.  
</p>
<p><i>Idea: If you want you can add some "routing" capabilities based on url/urlrewriting. Some very good projects exist
out there today to rewrite the url into whatever you want and then you can basically load the views based on this info</i></p>
<br />
<p><b><i>You don't need ASP.NET MVC to get MVC on ASP.NET</i></b></p>
</gaia:Panel>
</asp:Content>
