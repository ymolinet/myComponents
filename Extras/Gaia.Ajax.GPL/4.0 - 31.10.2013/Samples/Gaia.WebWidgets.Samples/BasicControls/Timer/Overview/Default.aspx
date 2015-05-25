<%@ Page 
    Language="C#" 
    AutoEventWireup="true" 
    MasterPageFile="~/Core.master" 
    Title="Ajax Timer Sample" 
    Inherits="Gaia.WebWidgets.Samples.BasicControls.Timer.Overview.Default" 
    Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Timer" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <p>Server CPU Usage <small>(updates every 2 sec)</small>:</p>

    <div style="float:left;">
        <gaia:Label 
            ID="Label1" 
            runat="server"
            Font-Size="100"
            Text = "-" />
    </div>
    <div style="float:right;">
        <gaia:Slider 
            runat="server" 
            Enabled="false"
            DisplayDirection="Vertical"
            Height="200px"
            ID="slider"
            Value="100"
            CssClass="gaiax" />
    </div>
    
    <br style="clear: both;" />
    
    <gaia:ExtendedButton
        ID="Button1" 
        runat="server" 
        CssClass="gaiax"
        style="width: 100px;"
        OnClick="Button1_Click" 
        Text="Pause" />

    <gaia:Timer 
        ID="Timer1" 
        Milliseconds="2000"
        runat="server"
        OnTick="Timer1_Tick" />

    <p>
        <b style="color:Red;"> The CPU Usage information is only based on random data </b>
        Try to click the Button to disable and enable the Timer. Use the Timer if you need to periodically check 
        the server for changes you need to display back to the user. The Ajax Timer is often used in e.g. Chat Controls
        like our own <a href="../../../Combinations/WebApps/ChatControl/">Ajax Chat Control</a> for polling the server to check for new messages.
    </p>

</asp:Content>

