<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.ExtendedPanel.Overview.Default"
    Title="Gaia Ajax: ExtendedPanel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Extensions.ExtendedPanel" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:ExtendedPanel ID="zExtendedPanel1" runat="server" Height="100px"
        Width="640px">
        This is the Gaia ExtendedPanel with 5 buttons added dynamically
        in the Page_Load.<br />
        <br />
    </gaia:ExtendedPanel>
    <div>
        <gaia:Button ID="zToggle" runat="server" Text="Toggle ExtendedPanel"
            OnClick="zToggle_Click" />
        
        <gaia:CheckBox 
            ID="zCanBeToggled" 
            runat="server" 
            Checked="True"
            AutoPostBack="true" 
            Text="Show Toggle Icon" 
            OnCheckedChanged="zCanBeToggled_OnCheckedChanged" />
    </div>
</asp:Content>
