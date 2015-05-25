<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/Core.master"
    Title="Gaia Ajax Samples" Inherits="Gaia.WebWidgets.Samples.Default"
    Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">
    <div>
        <ul class="proudest">
            <li>
                <a href="BasicControls/GridView/Overview/">
                    <img src="media/landingpage/gridview.gif" alt="Gaia Ajax GridView" />
                    <h3>Gaia Ajax GridView</h3>
                    Same API as ASP.NET GridView, just Ajax lightning fast!
                </a>
            </li>
            <li>
                <a href="Extensions/TreeView/Overview/">
                    <img src="media/landingpage/treeview.gif" alt="Gaia Ajax TreeView" />
                    <h3>Gaia Ajax TreeView</h3>
                    Finally a flexible and versatile TreeView. Any Gaia control can be a child element!
                </a>
            </li>
            <li>
                <a href="Extensions/Scheduler/Overview/">
                    <img src="media/landingpage/scheduler.gif" alt="Gaia Ajax Scheduler" />
                    <h3>Gaia Ajax Scheduler</h3> A powerful and easy-to-code scheduler with
                    DayView or MonthView. Still no JavaScript!
                </a>
            </li>
            <li>
                <a href="Combinations/WebApps/SimpleCalendarApplication/">
                    <img src="media/landingpage/scheduler-monthview.gif" alt="Gaia Ajax Scheduler MonthView" />
                    <h3>Scheduler MonthView + GridView + Window</h3>
                    A minimalistic
                    calendar showing the power of Gaia Ajax.
                </a>
            </li>
            <li>
                <a href="Combinations/WebApps/DashboardWithWebParts/">
                    <img src="media/landingpage/dashboard.gif" alt="Gaia Ajax TreeView" />
                    <h3>Dashboard with WebParts</h3>
                    Create your own drag'n'drop dashboard with C# or VB.NET!
                </a>
            </li>
            <li>
                <a href="Extensions/AutoCompleter/Overview/">
                    <img src="media/landingpage/autocompleter.gif" alt="Gaia Ajax AutoCompleter" />
                    <h3>Gaia Ajax AutoCompleter</h3>
                    The AutoCompleter makes it easy to make searching and auto suggestion features.
                </a>
            </li>
        </ul>
        <h2>What's new in Gaia Ajax 4.0?</h2>
        <ul class="whatsnew">
            <li>
                <h3>Gaia is now using jQuery</h3>
                The JavaScript in all controls is ported to jQuery. <del>Prototype.js</del> <del>Third party conflicts.</del>
            </li>
            <li>
                <h3>Upgraded effects</h3>
                All previous effects are replaced with jQuery UI Effects.
            </li>
            <li>
                <h3>Performance gain</h3>
                Due to the upgrade to jQuery, Gaia will benefit from several performance improvements.
            </li>
            <li>
                <h3>Bug fixes</h3>
                With the release of 4.0, Gaia Ajax is even more stable.
            </li>
            <li>
                <h3>New Scheduler control</h3>
                The <a href="Extensions/Scheduler/Overview/">Scheduler control</a> provides
                two main views:
                <a href="Extensions/Scheduler/Overview/">DayView</a> and 
                <a href="Extensions/Scheduler/MonthView/">MonthView</a>. It is
                easy to add, move or select calendar items. It has a simple, yet powerful API, and
                supports templating making it simple to customize your own needs.
            </li>
        </ul>
    </div>
</asp:Content>
