<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Extensions.TabControl.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Extensions.TabControl" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:Panel runat="server" ID="zPnl" CssClass="tabexample-container">

    <gaia:TabControl 
        ID="zTabControl1" 
        runat="server"
        ForceDynamicRendering="true">
    
        <gaia:TabView ID="zTabView1" runat="server" Caption="Tab 1">
        
            <p>Nunc scelerisque sollicitudin leo. 
            Fusce id sapien. Ut
            faucibus. Aliquam erat volutpat. Vestibulum arcu libero, 
            iaculis id, pretium eu, posuere gravida, felis. Aliquam 
            erat volutpat. Fusce a diam. Morbi sit amet ligula.
            Class aptent taciti sociosqu ad litora torquent per 
            conubia nostra, per inceptos
            himenaeos. Curabitur vehicula, turpis nec scelerisque 
            pulvinar, massa massa imperdiet eros, non fermentum sem 
            quam vel nulla.</p>
               
        </gaia:TabView>
        <gaia:TabView ID="zTabView2" runat="server" Caption="Tab 2">
            
            <p>Lorem ipsum dolor sit amet, 
            consectetuer adipiscing elit. 
            Fusce quis erat. Ut congue erat. Nullam dignissim. 
            Suspendisse nisl pede, auctor vel, accumsan tempor, convallis
            vel, sapien. Curabitur quis dui. Nunc et velit. In porttitor, 
            leo eu adipiscing suscipit, est nisi cursus lorem, in mattis 
            libero libero sit amet tellus. Suspendisse</p>
            
        </gaia:TabView>
        
        <gaia:TabView ID="zTabView3" runat="server" Caption="Tab 3">
        
            <p>Vel, sapien. Curabitur quis dui. 
            Nunc et velit. In porttitor, 
            leo eu adipiscing suscipit, est nisi cursus lorem, in mattis 
            libero libero sit amet tellus. Suspendisse mollis consequat 
            lectus. Nunc scelerisque sollicitudin leo. Fusce id sapien. Ut
            faucibus. Aliquam erat 
            <gaia:LinkButton 
                ID="zHiddenGem" 
                runat="server" 
                Text="Gaia"
                Style="color: Black; text-decoration: none;"
                OnClick="zHiddenGem_Click"></gaia:LinkButton>
            volutpat. Vestibulum arcu libero, 
            iaculis id, pretium eu, posuere gravida, felis. Aliquam erat volutpat. 
            Fusce a diam. Morbi sit amet ligula. Class aptent taciti sociosqu
            ad litora torquent per conubia nostra, per inceptos</p>
           
        </gaia:TabView>
        
        <gaia:TabView 
            ID="zTabView4" 
            runat="server" 
            Caption="Disabled Tab 4" 
            Enabled="false">
               
        </gaia:TabView>
        
    </gaia:TabControl>
</gaia:Panel>
<p>Here you can add more TabViews dynamically.</p>
<gaia:Button runat="server" ID="zAddTabView" Text="Add TabView" OnClick="zAddTabView_Click" />

 <gaia:Window 
    ID="zWindowSuccess" 
    Caption="Congratulations!"
    runat="server" 
    Modal="true" 
    Minimizable="false"
    Maximizable="false"
    Draggable="false"
    Resizable="false"
    CenterInForm="true"
    Width="400px"
    Visible="false"
    Height="200px">
   
    <div style="padding: 15px;">
        <h1>You did it!</h1>
        <p>The hidden word was disguised as a LinkButton with a OnClick handler
        that made this Window visible. The best part is that the Window was not
        made visible to the client (aka. Rendered to the Browser) before the 
        click handler on the server executed. 
        </p>
    </div>
   
    </gaia:Window>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<p>One of the Lorem Ipsum words are clickable in one of the Tabs. If you can find 
it a "hidden" secret will be revealed for you :-) 
<b>*Don't cheat by looking at the source*</b>
</p><br />
</asp:Content>