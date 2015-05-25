<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.EffectSlideDown.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Effects.EffectSlideDown" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<div style="height: 300px; width: 150px;">

<gaia:LinkButton 
    ID="zButtonDemo" 
    runat="server"
    OnClick="zButtonDemo_Click" 
    Text="Run Demo!">
    </gaia:LinkButton>
    
<gaia:LinkButton 
    ID="zButtonReset" 
    runat="server"
    OnClick="zButtonReset_Click" 
    Text="Reset">
    </gaia:LinkButton>

<gaia:Panel 
    ID="zPanel" 
    runat="server"
    BorderStyle="Solid"
    BorderWidth="1"
    Style="position: absolute; display: none;"
    BackColor="lightblue"
    Width="150px">
    
        <p>Nunc scelerisque sollicitudin leo. 
        Fusce id sapien. Ut
        faucibus. Aliquam erat volutpat. Vestibulum arcu libero, 
        iaculis id, pretium eu, posuere gravida, felis. Aliquam 
        erat volutpat.</p>
    
    </gaia:Panel>

</div>

</asp:Content>
