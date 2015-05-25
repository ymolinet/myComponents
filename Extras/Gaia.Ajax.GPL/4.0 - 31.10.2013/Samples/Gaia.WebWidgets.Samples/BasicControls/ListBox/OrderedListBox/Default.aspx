<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.ListBox.OrderedListBox.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Reordering Items in a Listbox</h1>
<p>This example demonstrates howto reorder listitems with Gaia Ajax. 
Now let's see if you can move the greatest company to the top of the list? 
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

    <gaia:Button 
        ID="zUp" 
        OnClick="zUp_Click" 
        runat="server" 
        Text="Move Up" 
        Width="100px" />
        
    <br />

    <gaia:ListBox ID="zList" runat="server" Height="100px" Width="100px">
        <Items>
            <asp:ListItem Text="Microsoft"></asp:ListItem>
            <asp:ListItem Text="Google"></asp:ListItem>
            <asp:ListItem Text="Apple"></asp:ListItem>
            <asp:ListItem Text="Oracle"></asp:ListItem>
            <asp:ListItem Text="Gaiaware"></asp:ListItem>
        </Items>
    </gaia:ListBox>
    
    <br />
    
    <gaia:Button 
        ID="Button1" 
        OnClick="zDown_Click" 
        runat="server" 
        Text="Move Down" 
        Width="100px" />
        
    <gaia:Window 
        ID="zWindow" 
        CenterInForm="true"
        Caption="Congratulations!"
        runat="server"
        Width="480"
        Height="170"
        Modal="true"
        Visible="false"
        Draggable="false"
        Resizable="false"
        Maximizable="false"
        Minimizable="false">
        <h1>You win!</h1>
        <p>Gaiaware is the correct choice!</p><br />
        <p>And because all Gaia listcontrols use the ListControl StateManager you can easily
        swap out the ListBox with any of the other ones (ie. DropDownList) if you please :-)
        </p>
        </gaia:Window>
    
    
</asp:Content>
