<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.Accordion.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Extensions.Accordion" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:Label 
    id="zMessage" 
    ForeColor="navy" 
    Font-Bold="true" 
    runat="server" 
    Text="This accordion listens to the Toggled event">
</gaia:Label>
    
    <gaia:Accordion ID="Accordion1" OnToggled="Accordion1_Toggled" runat="server">
        <gaia:ExtendedPanel
            runat="server"
            Caption="Accordion 1"
            ID="extPnl1">
            
             <p style="padding: 10px; line-height:2em;">
                <gaia:Image 
                    ID="zImg"
                    style="float: left;"
                    runat="server" 
                    ImageUrl="man.png" 
                    AlternateText="Man" />
            Nunc scelerisque sollicitudin leo. 
            Fusce id sapien. Ut
            faucibus. Aliquam erat volutpat. Vestibulum arcu libero, 
            iaculis id, pretium eu, posuere gravida, felis. Aliquam 
            erat volutpat. Fusce a diam. Morbi sit amet ligula.
            Class aptent taciti sociosqu ad litora torquent per 
            conubia nostra, per inceptos
            himenaeos. Curabitur vehicula, turpis nec scelerisque 
            pulvinar, massa massa imperdiet eros, non fermentum sem 
            quam vel nulla.</p>
            
        </gaia:ExtendedPanel>
        <gaia:ExtendedPanel
            runat="server"
            Caption="Accordion 2"
            ID="extPnl2">
            
            <p style="padding: 10px;">
            
             <gaia:Image 
                ID="zImg2"
                style="float: left;"
                runat="server" 
                ImageUrl="woman.png" 
                AlternateText="Woman" />
            
            Lorem ipsum dolor sit amet, 
            consectetuer adipiscing elit. 
            Fusce quis erat. Ut congue erat. Nullam dignissim. 
            Suspendisse nisl pede, auctor vel, accumsan tempor, convallis
            vel, sapien. Curabitur quis dui. Nunc et velit. In porttitor, 
            leo eu adipiscing suscipit, est nisi cursus lorem, in mattis 
            libero libero sit amet tellus. Suspendisse</p>
            
        </gaia:ExtendedPanel>
        <gaia:ExtendedPanel
            runat="server"
            Caption="Accordion 3"
            ID="extPnl3">
            
             <p style="padding: 10px;">Vel, sapien. Curabitur quis dui. 
            Nunc et velit. In porttitor, 
            leo eu adipiscing suscipit, est nisi cursus lorem, in mattis 
            libero libero sit amet tellus. Suspendisse mollis consequat 
            lectus. Nunc scelerisque sollicitudin leo. Fusce id sapien. Ut
            faucibus. Aliquam erat volutpat. Vestibulum arcu libero, 
            iaculis id, pretium eu, posuere gravida, felis. Aliquam erat volutpat. 
            Fusce a diam. Morbi sit amet ligula. Class aptent taciti sociosqu
            ad litora torquent per conubia nostra, per inceptos</p>
            
        </gaia:ExtendedPanel>
    </gaia:Accordion>
</asp:Content>
