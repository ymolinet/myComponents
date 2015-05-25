<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" Codebehind="Default.aspx.cs"
    Inherits="Gaia.WebWidgets.Samples.Aspects.AspectMouseMove.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.AspectMouseMove" />
    <p>In the example below the polling time is 100 milliseconds on localhost or 500 milliseconds. 
    You can configure this interval as low or high as you need depending on things like server saturation or
    network latency. 
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:Panel 
        ID="zPanel" 
        runat="Server" 
        CssClass="panel">
        
        <gaia:Label 
            ID="zMessage" 
            runat="server"></gaia:Label>
    
    </gaia:Panel>
</asp:Content>
