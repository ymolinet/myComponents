<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.TreeView.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Extensions.TreeView" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
 <div style="height:300px;">
 <gaia:TreeView runat="server" ID="tree" 
    AnimateToggling="false" 
    Style="width: 100%; height: 100%; overflow: auto;">
        <gaia:TreeViewItem runat="server" Collapsed="false" ID="cool_stuff"
            IconCssClass="folder">
            Cool Stuff
            <gaia:TreeViewItem runat="server"  ID="images" Collapsed="false"
                IconCssClass="folder">
                Images
                <gaia:TreeViewItem runat="server" IsLeaf="true"  ID="beautiful" 
                    IconCssClass="file">
                    Beautiful
                </gaia:TreeViewItem>
                <gaia:TreeViewItem runat="server" IsLeaf="true"  ID="people" 
                    IconCssClass="file">
                    People
                </gaia:TreeViewItem>
                <gaia:TreeViewItem runat="server" IsLeaf="true"  ID="food" 
                    IconCssClass="file">
                    Food
                </gaia:TreeViewItem>
            </gaia:TreeViewItem>
            <gaia:TreeViewItem runat="server"  ID="videos" IconCssClass="folder">
                Videos
                <gaia:TreeViewItem runat="server" IsLeaf="true"  ID="jeff_dunham" 
                    IconCssClass="file">
                    Jeff Dunham
                </gaia:TreeViewItem>
            </gaia:TreeViewItem>
            <gaia:TreeViewItem runat="server"  ID="music" IconCssClass="folder">
                Music
                <gaia:TreeViewItem runat="server" IsLeaf="true"  ID="jazz_music" 
                    IconCssClass="file">
                    Jazz
                </gaia:TreeViewItem>
                <gaia:TreeViewItem runat="server"  ID="classic" IconCssClass="folder">
                    Classic
                    <gaia:TreeViewItem IsLeaf="true" runat="server"  ID="TreeViewItem1"
                        IconCssClass="file">
                        Classic Piece 1
                    </gaia:TreeViewItem>
                    <gaia:TreeViewItem IsLeaf="true" runat="server"  ID="TreeViewItem2"
                        IconCssClass="file">
                        Classic Piece 2
                    </gaia:TreeViewItem>
                    <gaia:TreeViewItem IsLeaf="true" runat="server"  ID="TreeViewItem3"
                        IconCssClass="file">
                        Classic Piece 3
                    </gaia:TreeViewItem>
                </gaia:TreeViewItem>
            </gaia:TreeViewItem>
            <gaia:TreeViewItem runat="server" IsLeaf="true"  ID="advanced"
                IconCssClass="file">
                Advanced
            </gaia:TreeViewItem>
        </gaia:TreeViewItem>
    </gaia:TreeView>
</div>
</asp:Content>
