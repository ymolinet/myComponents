<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.EffectContainers.EffectParallel.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Effects.EffectParallel" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<p><gaia:Button 
    ID="zButtonFire" 
    runat="server" 
    Text="Click here to run the effect" 
    OnClick="zButtonFire_Click" /></p>

 <gaia:Window 
        ID="zWindow" 
        Caption="EffectParallel"
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
        Height="250"
        >
       <h1>Flexible Effects in Parallel</h1>
     <p>
         EffectParallel takes an arbitrary amount of effects that 
        will be fired simultaneously.
        <gaia:LinkButton ID="zFireEffect" runat="server"
            Text="Click here to run the effect"
            OnClick="zFireEffect_Click" />
     </p>
     <p>
         All made possible with just one line of code to add the 
        EffectParallel to the Effects collection of the Window. 
     </p>
        
        </gaia:Window>
        

</asp:Content>
