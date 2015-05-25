<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.ExtendedButton.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Extensions.ExtendedButton" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:ExtendedButton 
    ID="zButton1"
    Width="200px"
    Toggled="false" 
    runat="server"
    Text="Initial Button"
    OnClick="zButton1_Click"
     />

<p class="spacer"></p>

<gaia:ExtendedButton 
    ID="zButton2"
    Toggled="false"
    Width="250px" 
    runat="server"
    Visible="false"
    Text="Visible when Button1 is Toggled"
    OnClick="zButton2_Click"
     />
 
 <p class="spacer"></p>
     
<gaia:ExtendedButton 
    ID="zButton3"
    Width="150px"
    Toggled="false" 
    runat="server"
    Visible="false"
    Text="Click to disable"
    OnClick="zButton3_Click"
     />
 
 <p class="spacer"></p>
 
 <gaia:ExtendedButton 
    ID="zButton4"
    Toggled="false" 
    runat="server"
    Width="300px"
    ImageCssClass="extbutton-icon"
    Visible="false"
    Text="This button is configured with an Icon"
    OnClick="zButton4_Click"
     />  

</asp:Content>
