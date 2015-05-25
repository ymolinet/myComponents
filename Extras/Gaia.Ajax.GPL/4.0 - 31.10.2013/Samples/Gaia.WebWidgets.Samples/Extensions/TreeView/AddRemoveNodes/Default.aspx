<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.TreeView.AddRemoveNodes.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Adding and Clearing Nodes</h1>
<p>Demonstrates adding nodes dynamically to a TreeViewItem. The sample is configured
to allow adding 10 nodes. After that a modal Window will appear. Then you can clear the
list by clicking the Clear All Nodes button and start over :-)
</p>

<p>Each TreeNode also has a custom Icon defined</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
 <div>
    <gaia:Button 
        ID="zButtonAdd" 
        runat="server"
        OnClick="zButtonAdd_Click" 
        Text="Add Node" />
        
     <gaia:Button 
        ID="zButtonClear" 
        runat="server"
        OnClick="zButtonClear_Click" 
        Text="Clear All Nodes" />
    
    <br /><br />
     <div style="height:200px;width:100%;">
        <gaia:TreeView 
            runat="server" 
            ID="tree"
            Style="width: 100%; height: 100%; overflow: auto;">
            <gaia:TreeViewItem 
                runat="server"
                CssClass="gaiax"
                IsLeaf="false"
                Collapsed="false"
                ID="root" 
                IconCssClass="folder">
                Root
            </gaia:TreeViewItem>
        </gaia:TreeView>
    </div>
    <gaia:Window 
        ID="window" 
        CenterInForm="true"
        Caption="Great stuff!"
        runat="server"
        Width="480"
        Height="100"
        Modal="true"
        Visible="false"
        Draggable="false"
        Resizable="false"
        Maximizable="false"
        Minimizable="false">
        <h1>That was easy wasn't it?</h1>
    </gaia:Window>
    
</div>
</asp:Content>
