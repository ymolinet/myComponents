<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.Aspects.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Attaching Effects to Events</h1>
<p>Gaia Aspects also offers hooks to attach Effects to clientside events. These can be native
DOM events like click and doubleclick or customized versions like for example MouseOver/MouseOut
as handled by AspectHoverable or Resized which is handled by AspectResizable. 
</p>
<p><i>Bubbling is not supported on EffectEvents. Imagine what a wonderful mess it would be :-)</i></p>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:Panel 
    ID="zPanel2" 
    runat="server"
    BorderStyle="Ridge"
    Style="cursor: pointer;padding: 10px; text-align: center;"
    BorderWidth="2"
    BackColor="#ccffcc" 
    Height="60px"
    Width="150px">
   
    DoubleClick me and I will become very angry
</gaia:Panel>
    <br />
<gaia:Panel 
    ID="zPanel" 
    runat="server"
    BorderStyle="Dashed"
    BorderWidth="2"
    Style="padding: 10px; text-align: center"
    BackColor="lightblue" 
    Height="150px"
    Width="150px">

    If you resize me I will never come back :-)

</gaia:Panel>


</asp:Content>
