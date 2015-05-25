<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.TreeView.DataBinding.Default"
    Title="Gaia Ajax: TreeView - Data Binding" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: TreeView - Data Binding
    </h1>
    <p>
        This sample shows how you can fill your treeview with items
        from a datasource. Note that the Gaia Ajax TreeView doesn't
        have a DataSource property. This is due to the fact that the
        Gaia Ajax TreeView is an on demand loading TreeView, so it doesn't
        make sense to prefill it with data.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<div class="container-tree">
    <gaia:TreeView runat="server" ID="zTree">
        <gaia:TreeViewItem runat="server" ID="zTreeRoot" CssClass="gaiax"
            IsLeaf="false" Collapsed="true" IconCssClass="folder" OnGetChildrenControls="zTreeRoot_OnGetChildrenControls">
            My Calendar
        </gaia:TreeViewItem>
    </gaia:TreeView>
    </div>
    <div class="container-selected-item">
        <gaia:Label ID="zActivityDate" runat="server" Font-Underline="true" /><br />
        <gaia:Label ID="zActivityName" runat="server" Font-Size="Large" /><br />
        <gaia:Label ID="zMoreInfo" runat="server" />
    </div>
    <br style="clear: both" />
</asp:Content>
