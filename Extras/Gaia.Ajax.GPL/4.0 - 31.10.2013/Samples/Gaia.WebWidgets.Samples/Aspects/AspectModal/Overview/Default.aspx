<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Aspects.AspectModal.Overview.Default"
    Title="Gaia Ajax: Aspect Modal" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.AspectModal" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:LinkButton 
    ID="zOpenWindow1" 
    runat="server" 
    Text="Click here to open the Window with Modal property set to true"
    OnClick="zOpenWindow1_Click">
    </gaia:LinkButton>
    
<br />

<gaia:LinkButton 
    ID="zOpenWindow2" 
    runat="server" 
    Text="Click here to open the Window with AspectModal added programmatically"
    OnClick="zOpenWindow2_Click">
    </gaia:LinkButton>

<gaia:Window 
    ID="zWindow1" 
    Visible="false"
    runat="server" 
    Caption="Modal Window"
    Width="480"
    Height="200"
    Draggable="false"
    Resizable="false"
    Minimizable="false"
    Maximizable="false"
    Modal="true">
        This Window is Modal when visible. Just set the Modal property to True on the Window
        to make it so. For all other Gaia Controls you can make them modal by adding AspectModal()
        directly yourself.
    </gaia:Window>
    
<gaia:Window 
    ID="zWindow2" 
    Visible="false"
    runat="server" 
    Caption="Modal Window with AspectModal added"
    Width="480"
    Height="200"
    Draggable="false"
    Resizable="false"
    Minimizable="false"
    Maximizable="false">
        This Window is Modal when visible. This Window was made Modal by adding AspectModal
        to the Aspects collection of the Gaia Control. Most Gaia Controls have this Aspects collection
        and thus can be made Modal.
        <br />
        Notice that when you add AspectModal yourself you can customize the Color and
        Opacity of the Modality. 
    </gaia:Window>

</asp:Content>
