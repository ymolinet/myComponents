<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectScrollable.Another.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

  <gaia:Panel
            ID="zPanel"
            Style="width:550px;height:300px;position:relative"
            runat="server"></gaia:Panel>
     
  <gaia:Window 
    ID="zWindow" 
    Caption="Scrolled"
    runat="server" 
    Modal="true"
    Width="400px"
    Height="200px" 
    Minimizable="false"
    Maximizable="false"
    Draggable="false"
    Resizable="false"
    CenterInForm="true"
    OpacityWhenMoved="1"
    Visible="false">
    <h1>Scrolled Event triggered</h1>
    <p>You scrolled to the end of the list and this Window was made visible
    You can do whatever you want in the server callback, including populating
    the list with further data if required. It's your choice! 
    </p>
    <br />
    <i>Have a look at the Overview example to see how additional data can be 
    retrieved on scrolling. 
    </i>
    </gaia:Window>

</asp:Content>
