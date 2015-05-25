<%@ Page Language="C#" Title="Gaia Ajax: AspectDraggable with Snapping" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectDraggable.Snapping.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Gaia Ajax: AspectDraggable with Snapping</h1>
<p>Various Snapping configurations of AspectDraggable.
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<script type="text/javascript">
    /* Snap by 15 function */
    function snapper(x, y, draggable) {
        return [Math.round(x / 15) * 15, Math.round(y / 15) * 15];
    }
</script>

<div style="height: 550px;">

<p><b>This draggable Image will be snapped by fixed deltas both horizontally and vertically.
Image will automatically revert to its initial position.
</b></p>
<gaia:Image ID="zSnapImage1" runat="server" />
<br />
<hr />
<p><b>This draggable Image will be snapped by a client-side function.</b></p>
<gaia:Image ID="zSnapImage2" runat="server" />
<br />
<br />
<br />
<hr />
<p><b>This draggable is snapped in a constrainted region.</b></p>
<gaia:Panel runat="server" ID="zImageContainer">
    <gaia:Image ID="zSnapImage3" runat="server" />
</gaia:Panel>

</div>

</asp:Content>
