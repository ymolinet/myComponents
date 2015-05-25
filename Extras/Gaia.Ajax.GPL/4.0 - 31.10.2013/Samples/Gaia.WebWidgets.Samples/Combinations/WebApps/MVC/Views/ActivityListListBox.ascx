<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ActivityListListBox.ascx.cs" 
    Inherits="Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Views.ActivityListListBox" %>
  
<gaia:ListBox 
    ID="zList" 
    AutoPostBack="true" 
    runat="server" 
    OnSelectedIndexChanged="zList_SelectedIndexChanged">
</gaia:ListBox>

<gaia:Window 
    ID="zWindow" 
    runat="server"
    OpacityWhenMoved="1" 
    Minimizable="false"
    Maximizable="false"
    Closable="true"
    Draggable="true"
    Modal="true"
    Resizable="false"
    CenterInForm="false"
    Width="400px"
    Visible="false"
    Height="200px">
   
    <div style="padding: 15px;">
        <h1>Separation of concerns</h1>
        <p>Because of the flexibility of the MVC pattern, we can easily provide "custom" UI logic in each view if we need to. 
        This should mostly be UI specific things like effects, coloring, etc and not business logic which should be kept in the
        controller
        </p>
    </div>
   
</gaia:Window>