<%@ Page 
    Language="C#" 
    MasterPageFile="~/Core.master" 
    AutoEventWireup="True" 
    Inherits="Gaia.WebWidgets.Samples.Extensions.TreeView.MediaLibrary.Default"
    Title="Ajax TreeView Sample" Codebehind="Default.aspx.cs" %>

<%@ Register 
    Src="TreeViewResults.ascx" 
    TagName="TreeViewResults" 
    TagPrefix="uc1" %>

<asp:Content 
    ID="Content1" 
    ContentPlaceHolderID="ContentPlaceHolder1" 
    Runat="Server">
    
    <h1>Gaia Ajax TreeView</h1>
    <p>
        This is an example of our <em>Ajax TreeView Control</em>. The Gaia Ajax TreeView Control is very flexible and easy to use. 
        You can use static items and dynamic items. Static items are rendered as HTML output on the page and dynamic items are retrieved 
        <b>on-demand</b>. In this sample we have created various combinations of TreeViewItems to demonstrate different usage patterns.
        <br />
        Try to expand folder items and click and/or drag and drop any of the green file items which becomes visible.
    </p>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:ExtendedPanel 
        ID="pnlWrapper"
        runat="server"
        Caption="Ajax TreeView" 
        IconCssClass="windowIcon" 
        AnimationDuration="300"
        style="width:195px;float:left;" 
        Draggable="false">
        <div style="height:399px;width:100%;">

            <gaia:TreeView 
                runat="server"  
                ID="tree" 
                AnimateToggling="true"
                AllowSelections="false"
                style="overflow:auto; height: 100%; width: 100%;">

                <gaia:TreeViewItem runat="server" Collapsed="false" ID="cool_stuff" IconCssClass="folder">
                    Cool Stuff
                    <gaia:TreeViewItem runat="server" ID="images" Collapsed="false" IconCssClass="folder">
                        Images
                        <gaia:TreeViewItem runat="server" ID="beautiful" OnGetChildrenControls="RetrieveTreeViewItems" IconCssClass="folder">
                            Beautiful
                        </gaia:TreeViewItem>
                        <gaia:TreeViewItem runat="server" ID="people" OnGetChildrenControls="RetrieveTreeViewItems" IconCssClass="folder">
                            People
                        </gaia:TreeViewItem>
                        <gaia:TreeViewItem runat="server" ID="food" OnGetChildrenControls="RetrieveTreeViewItems" IconCssClass="folder">
                            Food
                        </gaia:TreeViewItem>
                    </gaia:TreeViewItem>
                    <gaia:TreeViewItem runat="server" ID="videos" IconCssClass="folder">
                        Videos
                        <gaia:TreeViewItem runat="server" ID="jeff_dunham" OnGetChildrenControls="RetrieveTreeViewItems" IconCssClass="folder">
                            Jeff Dunham
                        </gaia:TreeViewItem>
                    </gaia:TreeViewItem>
                    <gaia:TreeViewItem runat="server" ID="music" IconCssClass="folder">
                        Music
                        <gaia:TreeViewItem runat="server" ID="jazz_music" OnGetChildrenControls="RetrieveTreeViewItems" IconCssClass="folder">
                            Jazz
                        </gaia:TreeViewItem>
                        <gaia:TreeViewItem runat="server" ID="classic" OnGetChildrenControls="RetrieveTreeViewItems" IconCssClass="folder">
                            Classic
                        </gaia:TreeViewItem>
                    </gaia:TreeViewItem>
                    <gaia:TreeViewItem runat="server" ID="advanced" OnGetChildrenControls="RetrieveAdvancedItems" IconCssClass="folder">
                        Advanced
                    </gaia:TreeViewItem>
                </gaia:TreeViewItem>

            </gaia:TreeView>
        </div>
    <gaia:Panel 
        runat="server" 
        ID="updateProgressControl" 
        style="position:absolute;top:0px;right:15px;display:none;">
        <img runat="server" style="float:right;" alt="Update Control" src="~/media/contentImages/ajax-loader.gif" />
    </gaia:Panel>
    </gaia:ExtendedPanel>

    <div style="float:left;margin-left:5px;">
        <gaia:ExtendedPanel 
            runat="server" 
             
            Caption="Results" 
            AnimationDuration="300"
            ID="resultsPanel"
            style="width:350px;">
            <uc1:TreeViewResults 
                ID="viewer" 
                runat="server" />
        </gaia:ExtendedPanel>
    </div>

    <br style="clear:both;" />

</asp:Content>

