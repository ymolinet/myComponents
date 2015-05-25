<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Core.StateManagers.Serialization.Default"
    Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>State Managers: Serialization</h1>
<p>Gaia Ajax detects property changes during ajax callbacks and serializes changes back to the client.
In this example 40 nested labels are rendered and the backColor style is serialized for that element on the
OnClick event. The key point behind this sample is to demonstrate that basic ajax re-rendering is flawfull in that 
it would generate too much response for the simple change of a property. 
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <p> Which rendering strategy would you prefer</p>
    <gaia:RadioButtonList ID="zRenderOption" runat="server">
        <Items>
            <asp:ListItem Value="0" Text="Partial Rendering"></asp:ListItem>
            <asp:ListItem Selected="True" Value="1" Text="Gaia StateManagers"></asp:ListItem>
        </Items>
    </gaia:RadioButtonList>
    <h2>Click on any of the Boxes</h2>
    <gaia:Panel id="zRoot" runat="server" CssClass="root-panel"></gaia:Panel>
    
    <p style="color: #a05;">With 40 nested labels, the partial rendering strategy delivers around
    <b>8.5KB of data</b>, while the simple serialization of the property takes around 512 bytes. Partial rendering
    then occupies approx <b><u>16X</u></b> more bandwidth in this simple case. Imagine a more complex page?  
    </p>
    <br />
    <i>Don't believe it? Download <a href="http://getfirebug.com/" title="FireBug">Firebug</a> or <a href="http://www.fiddler2.com/fiddler2/" title="Fiddler">Fiddler</a>  and see for yourself</i>
</asp:Content>
