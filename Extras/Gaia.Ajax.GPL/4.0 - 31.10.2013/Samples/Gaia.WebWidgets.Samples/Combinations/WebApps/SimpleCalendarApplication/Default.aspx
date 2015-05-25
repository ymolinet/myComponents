<%@ Page Language="C#" AutoEventWireup="True"  
    MasterPageFile="~/Core.master" 
    Title="Simple Calendar Application" 
    CodeBehind="Default.aspx.cs" 
    Inherits="Gaia.WebWidgets.Samples.Combinations.WebApps.SimpleCalendarApplication.Default" %>

<%@ Register Src="CalendarApplication.ascx" TagName="CalendarApplication" TagPrefix="calendar" %>

<asp:Content 
    ID="Content1" 
    ContentPlaceHolderID="ContentPlaceHolder1" 
    Runat="Server">
    <h1>Simple Calendar Application</h1>
    <p>
        Gaia Ajax is an Ajax Library for ASP.NET. It uses the ASP.NET WebControl model
        which means most controls are 100% compatible with existing controls and you can leverage all of your existing knowledge.
        Gaia is very efficient in regards to bandwidth, much easier to use and your code will be a lot easier to maintain since it's
        all in managed code. This has a lot of benefits, but one of the most important ones is that <strong>you don't have
        to use JavaScript at all</strong> to develop your web applications.
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:ExtendedButton 
    ID="zBtnStartApplication"
    runat="server" 
    Text="Open Application" 
    Width="150px" 
    Toggled="false"  
    OnClick="zBtnStartApplication_Click" />
    
<gaia:Window 
    ID="zWindow" 
    runat="server"
    Width="950px" 
    Height="525px"
    Visible="false" 
    Draggable="true" 
    Minimizable="false"
    OpacityWhenMoved="1" 
    Resizable="false" 
    Maximizable="false"
    Caption="Calendar Application with Scheduler and GridView inside a Window"
    Closable="true"
    OnClosing="zWindow_Closing" >
        <calendar:CalendarApplication runat="server" ID="calendar" />
</gaia:Window>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder2">
    <h2>This sample demonstrates:</h2>
    <ul>
        <li><b>Loading resources (HTML, JavaScript) on demand</b><br />
            The <a href="../../../Extensions/Window/Overview/">Window</a>, 
            <a href="../../../Extensions/Scheduler/Overview/">Scheduler</a> and 
            <a href="../../../BasicControls/GridView/Overview/">GridView</a> is not loaded until you click the button.
            This means that the initial load of a page is fast and small. When needed,
            you can load more heavy and complex UI after the user requests it.
        </li>
        <li><b>Using Scheduler combined with 
            <a href="../../../Aspects/AspectClickable/Overview/">AspectClickable</a> and 
            <a href="../../../Aspects/AspectDroppable/Overview/">AspectDroppable</a></b><br />
            Each day cell in Scheduler is clickable and will open a window where you can add
            a new appointment. Also each cell is droppable, meaning it is a target for a draggable
            item.
        </li>
        <li><b>Dragging a GridView Row into the Scheduler</b><br />
            It only requires a few lines of C# code to make a row draggable. This is used to
            reschedule an appointment to a different day.
        </li>
        <li><b>Content of Window is a UserControl</b><br />
            Gaia has no problem of working together with UserControls due to the fact that it
            is fully built on the ASP.NET WebForms architecture.
        </li>
    </ul>
</asp:Content>
