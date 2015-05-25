<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.TreeView.DragAndDrop.Default"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: TreeView - With Drag and Drop</h1>
    <p>
        It's easy to enable drag and drop in a Gaia Ajax TreeView. When
        you combine the fact that: 1) you can insert any Gaia control
        as a child control into each TreeViewItem and 2) you can make
        any Gaia control draggable, only your imagination will be the
        limit.
    </p>
    <p>
        In this sample we have made a Gaia panel draggable, and set
        the Gaia TreeView at the right as the drop target.
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

        <div class="container-left">
            <gaia:TreeView 
                runat="server" 
                ID="zTreeLeft">

                <gaia:TreeViewItem 
                    runat="server"
                    Collapsed="false" 
                    ID="zLeftRoot" 
                    OnGetChildrenControls="getTreeItemsLeft"
                    IconCssClass="folder">
                        Source
                    </gaia:TreeViewItem>
            
            </gaia:TreeView>
        </div>
        <div class="container-right">
            <gaia:TreeView 
                runat="server" 
                ID="zTreeRight">

                <gaia:TreeViewItem 
                    runat="server"
                    Collapsed="false" 
                    ID="zRightRoot"
                    OnGetChildrenControls="getTreeItemsRight"
                    IconCssClass="folder">
                        Drag items here
                    </gaia:TreeViewItem>
            
            </gaia:TreeView>
        </div>
        <br style="clear: both" />
</asp:Content>
