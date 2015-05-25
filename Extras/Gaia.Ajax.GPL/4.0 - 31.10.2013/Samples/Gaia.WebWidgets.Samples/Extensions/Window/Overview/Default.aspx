<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.Window.Overview.Default"
    Title="Gaia Ajax: Window" %>
<%@ Import Namespace="Gaia.WebWidgets.Samples.Utilities"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Extensions.Window" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<p><b>Type in your desired Window Caption:</b> </p>
<gaia:TextBox 
    ID="zCaption"
    Width="300px"
    Text="Configure the Window"
    KeyChangeEvents="true"
    OnTextChanged="zCaption_TextChanged"
    runat="server">
    </gaia:TextBox>
<br /><br />

<p><b>Tick off some desired functionality:</b></p>
<gaia:CheckBox 
    ID="zMinimizable" 
    AutoPostBack="true"
    OnCheckedChanged="zMinimizable_CheckedChanged"
    Text="Minimizable" 
    runat="server" />

<gaia:CheckBox 
    ID="zClosable" 
    AutoPostBack="true"
    OnCheckedChanged="zClosable_CheckedChanged"
    Text="Closable" 
    runat="server" />
    
<gaia:CheckBox 
    ID="zMaximizable" 
    AutoPostBack="true"
    OnCheckedChanged="zMaximizable_CheckedChanged"
    Text="Maximizable" 
    runat="server" />

<br /><br />
<p>Click <gaia:LinkButton 
    ID="zMaximize" 
    runat="server"
    OnClick="zMaximize_Click"
    Text="here">
</gaia:LinkButton> to programmatically Maximize the Window</p>

<gaia:Button
    ID="zShowWindow"
    runat="server"
    Enabled="false"
    Text="Open Window again"
    OnClick="zShowWindow_Click"
/>

   <gaia:Window 
        ID="zWindow" 
        Caption="Configure the Window"
        OnClosing="zWindowSuccess_Closing"
        runat="server"
        OpacityWhenMoved="1" 
        Minimizable="false"
        Maximizable="false"
        Closable="false"
        Draggable="true"
        Resizable="false"
        CenterInForm="false"
        Width="400px"
        Visible="true"
        Height="200px">
       
        <div style="padding: 15px;">
            <h1>Configure the Window in Ajax callbacks</h1>
            <p>Notice how easy you can modify the Window in Ajax callbacks by ticking off
            the functionality you want. 
            </p>
        </div>
       
    </gaia:Window>

</asp:Content>
