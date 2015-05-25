<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CodeViewerControl.ascx.cs" Inherits="Gaia.WebWidgets.Samples.UI.CodeViewerControl" %>
<gaia:LinkButton ID="zExpandCode" runat="server" Text="Expand Code" OnClick="zExpandCode_Click" />
<gaia:Window 
    ID="zWindow" 
    runat="server"
    OpacityWhenMoved="1" 
    Minimized="false"
    Minimizable="false"
    Maximizable="false"
    Closable="true"
    Resizable="true"
    Draggable="true"
    CenterInForm="false"
    Width="720px"
    Visible="false"
    Height="400px">
   
    <gaia:Panel ID="zCode" runat="server" style="padding: 15px;background-color: White; border: 1px solid #aaa;">
    </gaia:Panel>
   
</gaia:Window>