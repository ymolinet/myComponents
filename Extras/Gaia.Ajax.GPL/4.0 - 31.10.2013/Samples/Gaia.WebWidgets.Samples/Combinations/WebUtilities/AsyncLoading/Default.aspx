<%@ Page Language="C#" MasterPageFile="~/Core.master" Title="Polling Data from Asynchronous Background Tasks" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Combinations.WebUtilities.AsyncLoading.Default" %>
<%@ Import Namespace="Gaia.WebWidgets.Samples.Utilities"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Polling Data from Asynchronous Background Tasks</h1>
This sample demonstrates one of the most requested featured of a modern responsive Web Application. The idea is to serve an initial page and
then populate the Data continuously as it becomes available. This data is often the result of time consuming operations and you don't want to
leave the UI unresponsive in the meantime. <br />
<gaia:LinkButton ID="zViewResponse" OnClick="zViewResponse_Click" runat="server"></gaia:LinkButton>
<br />
<gaia:Panel ID="zCodeResponse" runat="server" style="display: none; border: 1px solid #aaa; padding: 10px; width: 0px; height:0px; overflow: hidden; " >
<h2>Made possible with Gaia Ajax DRIMR</h2>
With a little help from Gaia Ajax we are able to create such highly responsive and sophisticated UIs. New items are created on a background
thread and we use the Timer_Tick event to poll for changes. Thanks to DRIMR we get an automatic diff of the previous vs. new state and only 
serialize the changes back to the client.<br /><br />
<h2>Server Response</h2>
<div class="csharpcode">
    Gaia.CRP('<span class="str">m_p_zGrid_ctl04</span>','<span class="kwrd">&lt;</span><span class="html">tr</span> <span class="attr">id</span><span class="kwrd">="m_p_zGrid_ctl04"</span> <span class="attr">class</span><span class="kwrd">="itemEven"</span><span class="kwrd">&gt;</span>\r\n    <span class="kwrd">&lt;</span><span class="html">td</span> <span class="attr">id</span><span class="kwrd">="m_p_zGrid_ctl04_ctl00"</span><span class="kwrd">&gt;</span>04:42<span class="kwrd">&lt;/</span><span class="html">td</span><span class="kwrd">&gt;&lt;</span><span class="html">td</span> <span class="attr">id</span><span class="kwrd">="m_p_zGrid_ctl04_ctl01"</span> <span class="attr">style</span><span class="kwrd">="width:50%;"</span><span class="kwrd">&gt;</span>Interview<span class="kwrd">&lt;/</span><span class="html">td</span><span class="kwrd">&gt;&lt;</span><span class="html">td</span> <span class="attr">id</span><span class="kwrd">="m_p_zGrid_ctl04_ctl02"</span> <span class="attr">style</span><span class="kwrd">="width:25%;"</span><span class="kwrd">&gt;</span>Myron Meyers<span class="kwrd">&lt;/</span><span class="html">td</span><span class="kwrd">&gt;</span>\r\n<span class="kwrd">&lt;/</span><span class="html">tr</span><span class="kwrd">&gt;</span>');
    <br /><br />
<pre class="alt">$RC(<span class="kwrd">new</span> Gaia.TableCell(<span class="str">'m_p_zGrid_ctl04_ctl00'</span>));</pre>
<pre>$RC(<span class="kwrd">new</span> Gaia.TableCell(<span class="str">'m_p_zGrid_ctl04_ctl01'</span>));</pre>
<pre class="alt">$RC(<span class="kwrd">new</span> Gaia.TableCell(<span class="str">'m_p_zGrid_ctl04_ctl02'</span>));</pre>
<pre>$RC(<span class="kwrd">new</span> Gaia.GridViewRow(<span class="str">'m_p_zGrid_ctl04'</span>));</pre><br />
    Gaia.IA('<span class="str">m_p_zGrid_ctl04</span>','<span class="kwrd">&lt;</span><span class="html">tr</span> <span class="attr">id</span><span class="kwrd">="m_p_zGrid_ctl05"</span> <span class="attr">class</span><span class="kwrd">="itemOdd"</span><span class="kwrd">&gt;</span>\r\n    <span class="kwrd">&lt;</span><span class="html">td</span> <span class="attr">id</span><span class="kwrd">="m_p_zGrid_ctl05_ctl00"</span><span class="kwrd">&gt;</span>08:07<span class="kwrd">&lt;/</span><span class="html">td</span><span class="kwrd">&gt;&lt;</span><span class="html">td</span> <span class="attr">id</span><span class="kwrd">="m_p_zGrid_ctl05_ctl01"</span> <span class="attr">style</span><span class="kwrd">="width:50%;"</span><span class="kwrd">&gt;</span>Morning meeting<span class="kwrd">&lt;/</span><span class="html">td</span><span class="kwrd">&gt;&lt;</span><span class="html">td</span> <span class="attr">id</span><span class="kwrd">="m_p_zGrid_ctl05_ctl02"</span> <span class="attr">style</span><span class="kwrd">="width:25%;"</span><span class="kwrd">&gt;</span>Myron Meyers<span class="kwrd">&lt;/</span><span class="html">td</span><span class="kwrd">&gt;</span>\r\n<span class="kwrd">&lt;/</span><span class="html">tr</span><span class="kwrd">&gt;</span>');
    </div><br />

<h2>Nonblocking UI</h2>
<p>Also notice that while the server feeds the GridView dynamically with processed data, the UI is still working and you can collapse/expand
TreeView nodes, navigate the Code Tabs and all other things. This is made possible because in Gaia Ajax - only 1 ajax request is fired at a time
and therefore everything will always be in sync. Here's a little home lesson for you: Expand the sample to provide update/delete capabilities to
the Grid while it's being updated from the server. :-) 
</p>
</gaia:Panel>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:Button 
    ID="zButton" 
    runat="server" 
    Text="Start Async Operation" 
    OnClick="zButton_Click" />
    
<gaia:Image 
    ID="zImageLoader" 
    ImageUrl="ajax-loader.gif" 
    runat="server" 
    Visible="false" />
    
<gaia:GridView 
    runat="server" 
    ID="zGrid" 
    Width="100%"
    AutoGenerateColumns="false"
    CssClass="async-grid">
        <RowStyle CssClass="itemEven" />
        <AlternatingRowStyle CssClass="itemOdd" />
        <Columns>
            <gaia:BoundField 
                HeaderText="Time" 
                DataField="ActivityDate" 
                DataFormatString="{0:HH:mm}" />
            
            <gaia:BoundField 
                HeaderText="Name" 
                ItemStyle-Width="50%"
                DataField="ActivityName" />
            
            <gaia:BoundField 
                HeaderText="Contact" 
                ItemStyle-Width="25%"
                DataField="ContactPerson" />
        </Columns>
</gaia:GridView>

<gaia:Timer 
    ID="zTimer" 
    runat="server" 
    Milliseconds="1000"
    OnTick="zTimer_Tick" 
    Enabled="false" >
</gaia:Timer>

</asp:Content>
