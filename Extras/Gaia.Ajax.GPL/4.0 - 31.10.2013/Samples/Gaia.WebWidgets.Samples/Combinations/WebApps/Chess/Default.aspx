<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Combinations.WebApps.Chess.Default"
    Title="A Simple Chess Game for ASP.NET" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>A Simple Chess Game for ASP.NET</h1>
<p>Creating a Chess game is fairly simple with Gaia Ajax. Three different custom server
controls where created to simulate the Board, Square and Piece. The object model was developed
separately and is presentation layer independent. 
 </p>
 <p><i>Note: This chess game is not complete. It was only created to serve as an example of
 how to use Gaia Ajax to created sophisticated UIs. Certain moves are not yet implemented
 and the game currently halts when the king is checked. Feel free to complete the game and
 publish it on CodeProject if you want.  
 </i></p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

    <div class="current-player">
        <p>Who's turn</p>
        <gaia:Image ID="zWhosNext" runat="server" ImageUrl="img/white_pawn.png" />
    </div>

    <gaia:Panel
        runat="server"
        ID="c">
    </gaia:Panel>

</asp:Content>
