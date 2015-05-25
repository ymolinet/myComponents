<%@ Control 
    Language="C#" 
    AutoEventWireup="True" 
    Inherits="Gaia.WebWidgets.Samples.Extensions.TreeView.MediaLibrary.ViewStuffFromTreeView" Codebehind="TreeViewResults.ascx.cs" %>

<gaia:Label runat="server" ID="label" Style="padding:10px;">
  As you navigate our Ajax TreeView Control you will notice that this part of the page
  will contain "navigation results". Very often when a component vendor demonstrates
  a complex Ajax Control like an Ajax TreeView there will be no logic behind the
  selection of TreeViewNodes. As you can see, this is not the case for our Gaia Ajax
  TreeView Control Sample ;)
</gaia:Label>
<gaia:Panel runat="server" ID="pnl" Width="100%"></gaia:Panel>