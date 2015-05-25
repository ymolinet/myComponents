<%@ Page Language="C#" Title="Gaia Ajax: AspectDraggable with Ghosting" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectDraggable.Ghosting.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Gaia Ajax: AspectDraggable with Ghosting</h1>
<p>Various Ghosting configurations of AspectDraggable.
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<p><b>This draggable Image will be ghosted with a special CssClass and is dragged at the document 
level, circumventing overflow and stacking contexts. When <gaia:Label ID="zGhostLabel1" Text="Dropped" runat="server"></gaia:Label>, it will be
reverted back to its original location and the ghost is removed.  
</b></p>
<gaia:Image ID="zGhostImage1" runat="server" />

<br />
<hr />
<p><b>This draggable Image is ghosted with shallow copy and a special CssClass with some
opacity applied. The Aspect is attached to a surrounding panel to properly copy only the 
root element. The ghost is immediately reverted on <gaia:Label ID="zGhostLabel2" Text="drop" runat="server"></gaia:Label>.
</b></p>
<gaia:Panel 
    ID="zGhostPanel" runat="server" Width="115" Height="86">
    <gaia:Image ID="zGhostImage2" runat="server" />
</gaia:Panel>
<br />
<br />
<br />
<hr />
<p><b>This draggable is ghosted during drag, but will not revert after drop. If <gaia:Label ID="zGhostLabel3" Text="dropped" runat="server"></gaia:Label> outside 
it's container, the image will be hidden because of it's parent overflow. This is something
you need to take into consideration when designing your applications. 
</b></p>
 <gaia:Image ID="zGhostImage3" runat="server" />


</asp:Content>
