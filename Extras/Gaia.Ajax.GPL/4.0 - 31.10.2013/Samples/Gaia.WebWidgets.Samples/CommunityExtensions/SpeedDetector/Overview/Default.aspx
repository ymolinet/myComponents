<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.CommunityExtensions.SpeedDetector.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Gaia Ajax: Speed Detector</h1>
<h3>Meassure the speed of your customers broadband connection</h3>
<p>This simple CommunityExtension will send a lot of "gibberish" to the client and perform some
simple calculations to meassure the available bandwidth the client has. This approach may 
yield wrong results, but it's a simple way enable/disable features based on network latency.
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:Button 
    ID="zButtonStart" 
    runat="server" 
    Text="Start Test"
    Enabled="true"
    OnClick="zButtonStart_Click" />

<gaia:SpeedDetector
    id="zSpeedDetector"
    runat="server"
    DownloadSize="1048576"
    OnSpeedDetectionComplete="zSpeedDetector_SpeedDetectionComplete">
    
    </gaia:SpeedDetector>

<gaia:Label ID="zResult" runat="server"></gaia:Label>

</asp:Content>
