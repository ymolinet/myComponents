<%@ Page 
    Language="C#" 
    MasterPageFile="~/Core.master" 
    AutoEventWireup="True" 
    Inherits="Gaia.WebWidgets.Samples.Combinations.WebApps.DashboardWithWebParts.Default" 
    Title="Dashboard with Web Parts" Codebehind="Default.aspx.cs" %>

<asp:Content 
    ID="Content1" 
    ContentPlaceHolderID="ContentPlaceHolder1" 
    Runat="Server">
    
    <asp:PlaceHolder runat="server" ID="windowHolder"></asp:PlaceHolder>
    
    <h1>Gaia Ajax: Dashboard with Web Parts</h1>
    <p>
        Gaia was created to be an <em>expressive</em> Ajax Library for ASP.NET WebForms and this sample was created to be a minimalistic
        <em>dashboard example</em>. Note that the sample is by no means an exhaustive implementation, but it might serve
        as a start point for your own idea. By creating an Ajax Portal with Gaia you get to use the whole sleeve
        of ASP.NET runtime features like for instance UserControls, WebControls and so on in addition to that
        you can create the entire portal in C# or VB.NET which will significantly reduce the complexity of
        your portal.
    </p>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
  
    <gaia:Panel runat="server" ID="w">
        <gaia:Panel 
            runat="server" 
            ID="Left" 
            CssClass="dashboard-column">
        </gaia:Panel>

        <gaia:Panel 
            runat="server" 
            ID="Center" 
            CssClass="dashboard-column">
        </gaia:Panel>

        <gaia:Panel 
            runat="server" 
            ID="Right" 
            CssClass="dashboard-column">
        </gaia:Panel>
    </gaia:Panel>
    <br style="clear:both;" />

</asp:Content>

