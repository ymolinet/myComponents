<%@ Page
    Language="C#"
    MasterPageFile="~/Core.master"
    AutoEventWireup="True"
    Inherits="Gaia.WebWidgets.Samples.Aspects.AspectScrollable.Overview.Default"
    Title="Aspect Scrollable" Codebehind="Default.aspx.cs" %>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.AspectScrollable" />
    <br/>
    <p>
        This sample shows how you can attach events to the vertical
        scrollbar of a Gaia Panel. By adding AspectScrollable you can
        easily create a widget that is dynamically being fed with items
        from the server as the user scrolls further down on. This sample
        throws an event when you scroll to the bottom of the vertical
        scrollbar.
    </p>
    <p>
        Note when we are appending controls to the ControlsCollection
        of the Panel, they are automatically sent to our page. Also note
        that the image is clickable and opens a Gaia Window.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

    <gaia:ExtendedPanel 
        runat="server" 
        ID="container"
        ScrollBars="None"
        Caption="Live Ajax Scrolling"
        Height="300px">
        
         <gaia:Panel
            ID="panel"
            Style="height:300px;position:relative;"
            runat="server" />
        
    </gaia:ExtendedPanel>
    <br /><br />
    
    <hr />
    <div class="features">
        <h2>Features</h2>
        <ul>
            <li><strong>Low bandwidth usage</strong> - Easily load contents on demand</li>
            <li><strong>Flexible</strong> - Inject any Gaia Ajax Control into the LiveScroll</li>
            <li><strong>XHTML compliant</strong> - Of course</li>
            <li><strong>Small footprint</strong> - Close to zero amount of JavaScript for the live scrolling feature</li>
            <li><strong>"Just works"</strong> - Use keys, mouse or any other scrolling mechanism to initiate scrolling</li>
            <li><strong>Easy to use</strong> - All coding is done server side in C# or VB.NET</li>
        </ul>
    </div>

    <gaia:Window 
        ID="bioWindow" 
        runat="server" 
        Caption="Biography" 
        Draggable="true" 
        Resizable="true" 
        Minimizable="false" 
        Maximizable="false" 
        Closable="true"
        Height="450px" 
        Width="500px" 
        Visible="false"
        CssClass="gaiax">
        <gaia:Label runat="server" ID="bioLbl" />
    </gaia:Window>

</asp:Content>
