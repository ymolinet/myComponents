<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.Window.Nested.Default"
    Title="Gaia Ajax: Nested Windows" %>

<asp:Content 
    ID="Content1" 
    ContentPlaceHolderID="ContentPlaceHolder1" 
    runat="server">
    <h1>
        Gaia Ajax: Nested Windows
    </h1>
    <p>
        This example demonstrates the powerful capabilities of the Window
        control and the Gaia code model. Notice that with just a few
        lines of code in pure .NET managed code you are able to deliver
        a nested set of dynamically configured Windows to the client.
    </p>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

    How many Windows would you like to recursively open?
    <gaia:DropDownList ID="zNumberOfWindows" runat="server">
        <Items>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
        </Items>
    </gaia:DropDownList>
    
    <br /><br />
    
    <gaia:CheckBox 
        ID="zModal" 
        runat="server" 
        Text="Would you like the Windows to be Modal ? " 
        TextAlign="left" />
    
    <br /><br />
    
    <gaia:Button 
        id="zOpenFirstWindow" 
        runat="server" 
        Text="Click here to start opening the Windows" />

</asp:Content>
