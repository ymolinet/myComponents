<%@ Page 
    Language="C#" 
    MasterPageFile="~/Core.master" 
    AutoEventWireup="true" 
    Title="Google Maps for ASP.NET" 
    Inherits="Gaia.WebWidgets.Samples.CommunityExtensions.GMap.Overview.Default" 
    Codebehind="Default.aspx.cs" %>

<asp:Content 
    ID="Content1" 
    ContentPlaceHolderID="ContentPlaceHolder1" 
    Runat="Server">

    <h1>Google Maps for ASP.NET</h1>
    <p>
        This is <em>Google Maps for ASP.NET</em>. It is a Gaia Ajax extension control that wraps the Google Maps
        (GMaps) service! You can write C# or VB.NET and experience a full integration with Google Maps without
        use of any JavaScript.
    </p>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:Label 
        runat="server" 
        ID="err" 
        ForeColor="Red" />
    <div class="container-gmap">
        <gaia:GMap 
            runat="server" 
            ID="GMap1"
            Height="350px"
            Width="430px" 
            OnMapTypeChanged="GMap1_MapTypeChanged" 
            OnClick="GMap1_Click" 
            OnZoom="GMap1_Zoom" 
            OnMoveEnd="GMap1_MoveEnd"
            OnMoveStart="GMap1_MoveStart" />
    </div>
    <div class="container-controlpanel">
        <gaia:Panel runat="server" ID="zPnlStatus" GroupingText="Status" CssClass="container-status">
            <gaia:Label 
                runat="server" 
                ID="msg" />
        </gaia:Panel>
        <table>
            <tr>
                <td valign="top">
                    SetCenter</td>
                <td>
                    Latitude<br />
                    <gaia:TextBox 
                        runat="server" 
                        BackColor="#ddddff"
                        ID="txtLatitude" 
                        Text="59.19857" Width="60px" /><br />
                    Longitude<br />
                    <gaia:TextBox 
                        runat="server" 
                        BackColor="#ddddff"
                        ID="txtLongitude" 
                        Text="9.613282" Width="60px" /><br />    
                    <gaia:Button 
                        runat="server" 
                        ID="btnGoTo" 
                        Text="Go to"
                        OnClick="btnGoTo_Click" />
                    <gaia:Button 
                        ID="btnPanTo" 
                        runat="server" 
                        OnClick="btnPanTo_Click" 
                        Text="Pan to" />
                </td>
            </tr>
            <tr>
                <td>
                    SetZoomLevel</td>
                <td>
                    <gaia:DropDownList 
                        runat="server"
                        ID="ddlZoom"
                        AutoPostBack="true" 
                        OnSelectedIndexChanged="ddlZoom_SelectedIndexChanged">
                        <items>
                            <asp:ListItem Value="0" />
                            <asp:ListItem Value="1" />
                            <asp:ListItem Value="2" />
                            <asp:ListItem Value="3" />
                            <asp:ListItem Value="4" />
                            <asp:ListItem Value="5" Selected="True" />
                            <asp:ListItem Value="6" />
                            <asp:ListItem Value="7" />
                            <asp:ListItem Value="8" />
                            <asp:ListItem Value="9" />
                            <asp:ListItem Value="10" />
                            <asp:ListItem Value="11" />
                            <asp:ListItem Value="12" />
                            <asp:ListItem Value="13" />
                            <asp:ListItem Value="14" />
                            <asp:ListItem Value="15" />
                            <asp:ListItem Value="16" />
                            <asp:ListItem Value="17" />
                            <asp:ListItem Value="18" />
                            <asp:ListItem Value="19" />
                        </items>
                    </gaia:DropDownList></td>
            </tr>
            <tr>
                <td>
                    Coordinates</td>
                <td>
                    <gaia:Button 
                        runat="server" 
                        ID="btnGetCenterLatLng" 
                        Text="Get coordinates"
                        OnClick="btnGetCenterLatLng_Click" /></td>
            </tr>
            <tr>
                <td>
                    Configuration</td>
                <td>
                    <gaia:Button 
                        runat="server" 
                        ID="btnToggleDragging" 
                        Text="Toggle dragging"
                        OnClick="btnToggleDragging_Click" />
                    <br />
                    <gaia:Button 
                        runat="server" 
                        ID="btnToggleZoomControls" 
                        Text="Toggle zoom"
                        OnClick="btnToggleZoomControls_Click" /></td>
            </tr>
            <tr>
                <td>
                    InfoWindow</td>
                <td>
                    <gaia:Button 
                        runat="server" 
                        ID="btnAddInfoWindow" 
                        Text="Add InfoWindow"
                        OnClick="btnAddInfoWindow_Click" />
                    <br />
                    <gaia:Button 
                        runat="server" 
                        ID="btnCloseInfoWindow" 
                        Text="Close InfoWindow"
                        OnClick="btnCloseInfoWindow_Click" /></td>
            </tr>
            <tr>
                <td>
                    MapType</td>
                <td>
                    <gaia:DropDownList
                        runat="server"
                        ID="ddlMapType"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlMapType_SelectedIndexChanged">
                        <Items>
                            <asp:ListItem Value="Normal" />
                            <asp:ListItem Value="Satellite" />
                            <asp:ListItem Value="Hybrid" />
                            <asp:ListItem Value="Physical" />
                        </Items>
                    </gaia:DropDownList></td>
            </tr>
        </table>
    </div>
    <br style="clear:both;" />
</asp:Content>

