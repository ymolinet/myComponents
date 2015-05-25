<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Core.BrowserHistory.Overview.Default"
    Title="Gaia Ajax: BrowserHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.BrowserHistory" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:BrowserHistory 
    id="zHistory" 
    runat="server" 
    OnNavigated="zHistory_Navigated">
</gaia:BrowserHistory>
 
 <p>Please select your favorite Comedian</p>
 <gaia:DropDownList 
        ID="zComedian" 
        runat="server" 
        AutoPostBack="true" 
        OnSelectedIndexChanged="zComedian_SelectedIndexChanged">
        <Items>
            <asp:ListItem Value="jerry">Jerry Seinfeld</asp:ListItem>
            <asp:ListItem Value="rowan">Rowan Atkinson</asp:ListItem>
            <asp:ListItem Value="dave">Dave Chappelle</asp:ListItem>
            <asp:ListItem Value="sacha">Sacha Baron Cohen</asp:ListItem>
            <asp:ListItem Value="al">"Weird Al" Yankovic</asp:ListItem>
            <asp:ListItem Value="chong">Cheech and Chong</asp:ListItem>
        </Items>
    </gaia:DropDownList>
    
    <br /><br />
 <gaia:Label Visible="false" ID="zInfo" runat="server">
 Wonderful! You have changed the selection in the dropdownlist, now try 
 the back button in the browser to restore to the previous state. 
 </gaia:Label>

</asp:Content>
