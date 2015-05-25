<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Effects.EffectScrollTo.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Effects.EffectScrollTo" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:LinkButton 
    ID="zButtonDemo" 
    runat="server"
    OnClick="zButtonDemo_Click" 
    Text="Click here for demo">
    </gaia:LinkButton>

    <div style="width: 200px; color: #777;">

        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum consequat suscipit
            nisl, non dignissim justo venenatis a. Nulla tempus ornare arcu vitae blandit. In
            egestas eros in ipsum malesuada fringilla. Donec ut felis quis nunc consectetur
            consectetur cursus vel est. Morbi rhoncus tincidunt lorem, eu pellentesque lacus
            bibendum vel. Fusce sed tincidunt augue. Mauris arcu eros, commodo ac viverra sed,
            tempus vestibulum nulla. Sed vel elit ut purus interdum cursus. Suspendisse vel
            tristique justo. In pretium eleifend eros, a adipiscing purus varius nec. Vestibulum
            ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Fusce
            id tortor leo, quis adipiscing dui. Phasellus venenatis fringilla orci eu luctus.
            Cras vel arcu magna. Nam luctus ultrices justo, ac tempus erat fringilla at. Proin
            mattis libero at nulla scelerisque ullamcorper. Suspendisse placerat ipsum sed nisl
            dictum tempor sollicitudin magna suscipit.</p>
        <p>Morbi tellus enim, viverra nec molestie eget, placerat eget risus. Lorem ipsum dolor
            sit amet, consectetur adipiscing elit. Quisque dictum tortor pellentesque metus
            iaculis pharetra. Morbi tempus interdum lectus, vitae imperdiet ipsum accumsan dapibus.
            Mauris lacinia turpis a nisl dignissim nec vestibulum risus vehicula. Aenean eleifend
            tincidunt arcu, nec adipiscing sem lacinia at. Nulla facilisi. Fusce elit purus,
            sollicitudin sed imperdiet vel, vulputate sit amet sem. Aenean fringilla lobortis
            auctor. Quisque sed lacus est. Aenean sollicitudin semper nulla viverra lacinia.
            Morbi iaculis magna nec purus convallis sollicitudin mollis mi egestas. Suspendisse
            odio risus, dignissim tempus ullamcorper non, rutrum vel quam. Proin sagittis sodales
            ante ut bibendum. Cum sociis natoque penatibus et magnis dis parturient montes,
            nascetur ridiculus mus. Sed ac ultrices erat. Fusce eu euismod sem.</p>
        <p>Integer magna augue, tincidunt non sodales in, mollis vel orci. Mauris quis diam
            lorem, at egestas justo. Nullam pulvinar convallis quam a dapibus. Aliquam erat
            volutpat. Aliquam a elit in augue feugiat bibendum. Integer erat sapien, congue
            et commodo eu, pellentesque sed sapien. Integer mattis enim sit amet velit malesuada
            iaculis scelerisque lectus elementum. Duis porta posuere tellus, in euismod ligula
            consequat a. Maecenas sit amet eros sed arcu rhoncus ornare nec a lorem. Pellentesque
            orci sapien, interdum id blandit ac, fermentum nec nibh. Ut tristique posuere ligula
            et tempor. Quisque tempus enim quis tortor pulvinar ut dapibus nisl sagittis. Duis
            imperdiet, ipsum commodo pretium suscipit, nulla lectus elementum nunc, vitae imperdiet
            lorem ante eleifend mauris. Aenean pulvinar tellus vitae odio scelerisque dictum.
            Cras suscipit neque vitae est eleifend lacinia. Vivamus massa dolor, bibendum ut
            commodo ut, sodales vitae elit. Etiam eget elit eget nisl interdum placerat.
        </p>
    </div>

<gaia:LinkButton 
    ID="zButtonDest" 
    runat="server"
    OnClick="zButtonDest_Click" 
    Text="You've reached the end of this article. Click here to go back.">
    </gaia:LinkButton>

</asp:Content>
