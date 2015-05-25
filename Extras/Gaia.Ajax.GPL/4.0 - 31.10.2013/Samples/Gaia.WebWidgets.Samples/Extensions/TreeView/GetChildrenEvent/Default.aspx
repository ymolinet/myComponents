<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.TreeView.GetChildrenEvent.Default"
    Title="Gaia Ajax: TreeView - GetChildrenControls Event" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: TreeView - GetChildrenControls Event</h1>
    <p>
        It's easy to add new TreeViewItems dynamically to the TreeView
        by using the GetChildrenControls event. This event is fired
        if the TreeViewItem subscribes to it and allows you to dynamically
        populate a TreeView "on-the-fly" and only construct the nodes
        needed. Dynamic controls must be re-created on each callback,
        but this is taken care of automatically by this event handler.
    </p>
    <p>
        In this example the same event handler renders more childnodes
        and each reuse the same event handler resulting in a never ending
        container of children. Go ahead and test how much your browser
        can take!
    </p>

    <p>
        You can select how many Nodes will be dynamically added by making
        a selection in the DropDownList. Note that after you have retrieved
        the first batch of nodes that the list is disabled so that the Count
        is fixed. If not we would have to keep memory of how many nodes were
        available at each folder.

        <strong>A practical maximum limit of 2000 nodes has been defined in the 
                codebehind. 
        </strong>
    </p>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

    <p>How many TreeViewItems would you like to dynamically expand?
    <gaia:DropDownList ID="zTreeItemcount" runat="server">
        <Items>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>100</asp:ListItem>
            <asp:ListItem>500</asp:ListItem>
            <asp:ListItem>1000</asp:ListItem>
        </Items>
    </gaia:DropDownList>
    </p>

    <br />

    <gaia:TreeView 
        AnimateToggling="false" 
        AllowSelections="false" 
        runat="server" 
        ID="tree" 
        Style="width: 100%; height: 100%;overflow: auto;">
        
        <gaia:TreeViewItem 
            runat="server" 
            IsLeaf="false"
            Collapsed="true" 
            OnGetChildrenControls="TreeViewGetChildrenControls"
            ID="root" 
            IconCssClass="folder">
            Root
        </gaia:TreeViewItem>
    
    </gaia:TreeView>


    <gaia:Window 
        ID="zWindow" 
        Caption="Limit Reached"
        runat="server"
        OpacityWhenMoved="1" 
        Minimizable="false"
        Maximizable="false"
        Closable="true"
        Draggable="false"
        Resizable="false"
        CenterInForm="true"
        Modal="true"
        Width="400px"
        Visible="false"
        Height="200px">
       
        <div style="padding: 15px;">
            <h1>Maximum Number of Items Allowed reached</h1>
            <p>To set a practical limit for this Sample we have defined a constant of (2000) items in the CodeBehind
            which defines how many nodes can be dynamically added.
            </p>
        </div>
       
    </gaia:Window>

</asp:Content>
