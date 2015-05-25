<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.CommunityExtensions.FishEye.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Gaia Ajax: FishEye</h1>
    <p>
        BlowFish, FishEye, Mac OS X Menu etc. Loved child has many names. Here is our version
        of the <b>Ajax FishEye Menu</b> made popular among the early innovators of Ajax and DHTML.
        <br />
        To use it just add up any widget (or HTML elements) which create "img" elements and set your properties.
        Just click the image to Display it inside a Window in full size.
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

    <div style="position:relative;width:600px;height:100px;clear:both;">
        <gaia:FishEyeMenu 
            id="zFishEyeMenu" 
            runat="server" 
            style="width:100%;" 
            Threshold="150"
            EndSize="96" 
            StartSize="48">

            <asp:Repeater ID="zRepeater1" runat="server">
                <ItemTemplate>
                    <gaia:Image
                        ID="dynamicImage" 
                        Height="48px"
                        OnClick="dynamicImage_Click"
                        Width="48px"
                        style="cursor: pointer;"
                        runat="server" 
                        AlternateText='<%# Bind("Text") %>'
                        ImageUrl='<%# Bind("ThumbUrl") %>' />
                </ItemTemplate>
            </asp:Repeater>

        </gaia:FishEyeMenu>
    </div>
    
           
    <gaia:Window 
        id="zWinImage" 
        Visible="false" 
        Draggable="false" 
        Minimizable="false"
        CenterInForm="true"
        Modal="true"
        OpacityWhenMoved="1" 
        Resizable="false" 
        Maximizable="false"
        Closable="true"
        OnClosing="zWinImage_Closing"
        runat="server">
        
             <gaia:Image
                ID="zImage" 
                runat="server" />
        
        </gaia:Window>

</asp:Content>
