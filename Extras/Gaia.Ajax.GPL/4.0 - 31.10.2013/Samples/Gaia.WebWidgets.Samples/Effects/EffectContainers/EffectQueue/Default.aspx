<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.EffectContainers.EffectQueue.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Effects.EffectQueue" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<p><gaia:Button ID="zButtonFire" runat="server" Text="Click here" OnClick="zButtonFire_Click" /> 
to run the effect</p>

<gaia:Window 
        ID="zWindow" 
        Caption="EffectQueue"
        runat="server" 
        Minimizable="false"
        Maximizable="false"
        Draggable="true"
        Closable="false"
        OpacityWhenMoved="1"
        Resizable="false"
        CenterInForm="true"
        Width="480"
        Visible="true"
        Height="250">
    <h1>Create Queues of Effects</h1>
    <p>
        EffectQueue takes an arbitrary amount of effects that will be fired 
        "one at a time".
    </p>
    <gaia:LinkButton
        ID="zFireEffect"
        runat="server"
        Text="Click here to run the effect"
        OnClick="zFireEffect_Click" />
    <p>
        With EffectQueue you can easily create animations with storylines and
        other similar interactive experiences. 
    </p>

</gaia:Window>

</asp:Content>
