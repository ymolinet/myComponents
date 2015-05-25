<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.EffectMove.PuzzleGame.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Effects.EffectMove" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

    What Word would you like to challenge yourself with ? 
    <gaia:DropDownList 
        ID="zChallenge" 
        runat="server" 
        AutoPostBack="true" 
        OnSelectedIndexChanged="zChallenge_SelectedIndexChanged">
        <Items>
            <asp:ListItem>YES</asp:ListItem>
            <asp:ListItem>GAIA</asp:ListItem>
            <asp:ListItem>GAIA AJAX</asp:ListItem>
        </Items>
    </gaia:DropDownList>
    
    <br /><br />
    
    What kind of transition would you like on the Move Effect? 
    <gaia:DropDownList ID="zDesiredEffect" runat="server">
        <Items>
            <asp:ListItem>EaseInElastic</asp:ListItem>
            <asp:ListItem>EaseInQuart</asp:ListItem>
            <asp:ListItem>EaseInQuad</asp:ListItem>
        </Items>
    </gaia:DropDownList>
    
    <br /><br />

 <gaia:Button 
        ID="zButtonMove" 
        runat="server" 
        OnClick="zButtonMove_Click" 
        Text="Move Windows" />
    
    <gaia:Label ID="zCount" runat="server"></gaia:Label>
    <gaia:Panel ID="zContentPanel" runat="server" Height="220"></gaia:Panel>
    
    <gaia:Window 
        ID="zWindowSuccess" 
        Caption="You Win!"
        OnClosing="zWindowSuccess_Closing"
        runat="server" 
        Modal="true" 
        Minimizable="false"
        Maximizable="false"
        Draggable="false"
        Resizable="false"
        CenterInForm="true"
        Width="400px"
        Visible="false"
        Height="75px">
        <h1>Congratulations! You win!</h1>
        </gaia:Window>


</asp:Content>
