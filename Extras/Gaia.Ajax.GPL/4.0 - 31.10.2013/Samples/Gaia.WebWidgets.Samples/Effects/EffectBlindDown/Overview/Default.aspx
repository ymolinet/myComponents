<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.EffectBlindDown.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Effects.EffectBlindDown" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">


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
    Style="display:none;"
    BackColor="lightblue"
    Width="150px">
    
    <p>Nunc scelerisque sollicitudin leo. 
    Fusce id sapien. Ut
    faucibus. Aliquam erat volutpat. Vestibulum arcu libero, 
    iaculis id, pretium eu, posuere gravida, felis. Aliquam 
    erat volutpat.</p>
    
</gaia:Panel>

</asp:Content>
