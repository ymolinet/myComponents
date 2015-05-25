<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.Toolbar.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Extensions.Toolbar" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<div>
<gaia:Toolbar runat="server" ID="toolbar" Width="500px" >
    <gaia:ToolbarItem ID="ToolbarItem1" runat="server" Animation="Blind">
        Edit
        <gaia:ToolbarItem ID="ToolbarItem2" runat="server"  IconCssClass="icon-house">
            <gaia:LinkButton 
                runat="server" 
                ID="copyBtn" 
                OnClick="ItemCommand"
                Text="Copy" />
        </gaia:ToolbarItem>
        <gaia:ToolbarItem ID="ToolbarItem3" runat="server"  IconCssClass="icon-computer">
            <gaia:LinkButton 
                runat="server" 
                ID="cutBtn" 
                OnClick="ItemCommand"
                Text="Cut" />
        </gaia:ToolbarItem>
        <gaia:ToolbarItem ID="ToolbarItem4" runat="server"  IconCssClass="icon-nofolder">
            <gaia:LinkButton 
                runat="server" 
                ID="pasteBtn" 
                OnClick="ItemCommand"
                Text="Paste" />
        </gaia:ToolbarItem>
        <gaia:ToolbarItem ID="ToolbarItem5" runat="server"  IconCssClass="icon-musicfolder">
            <gaia:LinkButton 
                runat="server" 
                ID="previewBtn" 
                OnClick="ItemCommand"
                Text="Preview" />
        </gaia:ToolbarItem>
    </gaia:ToolbarItem>
    <gaia:ToolbarItem ID="ToolbarItem6" runat="server"  Animation="Blind">
        Format
        <gaia:ToolbarItem ID="ToolbarItem7" runat="server"  IconCssClass="icon-folder">
            <gaia:LinkButton 
                runat="server" 
                ID="hyperlinkBtn" 
                OnClick="ItemCommand"
                Text="Hyperlink" />
        </gaia:ToolbarItem>
        <gaia:ToolbarItem ID="ToolbarItem8" runat="server"  IconCssClass="icon-trashcan" Animation="Blind">
            Text Formatting &gt;
            <gaia:ToolbarItem ID="ToolbarItem9" runat="server"  IconCssClass="icon-writer">
                <gaia:LinkButton 
                    runat="server" 
                    ID="boldBtn" 
                    OnClick="ItemCommand"
                    style="font-weight:bold;"
                    Text="Bold" />
            </gaia:ToolbarItem>
            <gaia:ToolbarItem ID="ToolbarItem10" runat="server"  IconCssClass="icon-refresh">
                <gaia:LinkButton 
                    runat="server" 
                    ID="italicsBtn" 
                    style="font-style:italic;"
                    OnClick="ItemCommand"
                    Text="Italics" />
            </gaia:ToolbarItem>
            <gaia:ToolbarItem ID="ToolbarItem11" runat="server"  IconCssClass="icon-drivers">
                <gaia:LinkButton 
                    runat="server" 
                    ID="underlineBtn" 
                    style="text-decoration:underline;"
                    OnClick="ItemCommand"
                    Text="Underline" />
            </gaia:ToolbarItem>
            <gaia:ToolbarItem ID="ToolbarItem12" runat="server"  IconCssClass="icon-package">
                <gaia:LinkButton 
                    runat="server" 
                    ID="strikeBtn" 
                    style="text-decoration:line-through;"
                    OnClick="ItemCommand"
                    Text="Strike-through" />
            </gaia:ToolbarItem>
        </gaia:ToolbarItem>
        <gaia:ToolbarItem ID="ToolbarItem13" runat="server"  IconCssClass="icon-mouse" Animation="Blind">
            Select Font &gt;
            <gaia:ToolbarItem ID="ToolbarItem14" runat="server"  IconCssClass="icon-writer">
                <gaia:LinkButton 
                    runat="server" 
                    ID="arial" 
                    style="font-family:Arial;"
                    OnClick="ItemCommand"
                    Text="Arial" />
            </gaia:ToolbarItem>
            <gaia:ToolbarItem ID="ToolbarItem15" runat="server"  IconCssClass="icon-cd">
                <gaia:LinkButton 
                    runat="server" 
                    ID="verdana" 
                    style="font-family:Verdana;"
                    OnClick="ItemCommand"
                    Text="Verdana" />
            </gaia:ToolbarItem>
            <gaia:ToolbarItem ID="ToolbarItem16" runat="server"  IconCssClass="icon-flower">
                <gaia:LinkButton 
                    runat="server" 
                    ID="times" 
                    style="font-family:Time New Roman;"
                    OnClick="ItemCommand"
                    Text="Times New Roman" />
            </gaia:ToolbarItem>
            <gaia:ToolbarItem ID="ToolbarItem17" runat="server"  IconCssClass="icon-warning">
                <gaia:LinkButton 
                    runat="server" 
                    ID="microsoft" 
                    style="font-family:Microsoft Sans Serif;"
                    OnClick="ItemCommand"
                    Text="MS Sans Serif" />
            </gaia:ToolbarItem>
        </gaia:ToolbarItem>
    </gaia:ToolbarItem>
</gaia:Toolbar>
<br />
<h2>Ajax Menu</h2>
<h3>Let's play around with a couple of ideas for this control together.</h3>
<p><b>Would it be possible to add up an Autocompleter inside one of the submenus?</b> </p>
<p>- Of course. That's actually too easy. </p>
<p><b>Another Question: Can you make the items themselves draggable? </b></p>
<p>- No problem! You can even support drag and drop between toolbars</p>
<p><b>Ok, third question: Is it possible to dynamically populate the Toolbar on-demand? </b></p>
<p>- ahh ... Yes. I am getting sleepy here. 
Do you have any more questions for me? And btw, all Gaia controls yields the same answers. It's because
it's about flexibility and versatility. I rest my case!
</p>
</div>

<gaia:Label ID="zMessage" runat="server" />

</asp:Content>
