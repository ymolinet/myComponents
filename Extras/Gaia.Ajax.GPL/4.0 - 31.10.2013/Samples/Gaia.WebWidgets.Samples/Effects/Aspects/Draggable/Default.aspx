<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.Aspects.Draggable.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>AspectDraggable Effects</h1>
<p>This sample demonstrates how you can add custom effects to AspectDraggable. Just try to move the three panels below
to see various nice effects added to StartDragging, EndDragging and Revert. 
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<div style="height: 400px;position: relative;">

    <gaia:Panel 
        ID="zPanel1" 
        runat="server" 
        Style="top: 0;height: 100px;"
        BackColor="#99ffcc"
        CssClass="panel">
            <p class="fx-info">No effects</p>
        </gaia:Panel>
    
    <gaia:Panel 
        ID="zPanel2" 
        runat="server"
        Style="top: 100px;height: 100px;" 
        BackColor="#ffcccc" 
        CssClass="panel">
            <p class="fx-info">EffectMove</p>
        </gaia:Panel>

    <gaia:Panel 
        ID="zPanel3" 
        runat="server" 
        Style="top: 200px;height: 100px;"
        BackColor="#ffffcc" 
        CssClass="panel">
        <p class="fx-info">EffectMorph & EffectScale</p>
        </gaia:Panel>
    
    <div class="info-box">
        <p><b>Try to move the panels</b></p>
        <p>1) The first Panel has only AspectDraggable attached with no effects</p>
        <p>2) The second Panel has added EffectMove() to the Revert event</p>
        <p>3) The third Panel has attached EffectMorph and EffectScale to the StartDrag and EndDrag events</p>
    </div>    
    
</div>
</asp:Content>
