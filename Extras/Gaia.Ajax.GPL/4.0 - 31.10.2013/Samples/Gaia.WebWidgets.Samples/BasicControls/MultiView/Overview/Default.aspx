<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.MultiView.Overview.Default"
    Title="Gaia Ajax: MultiView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.MultiView" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:Button ID="zButton1" runat="server" OnClick="zButton1_Click"
        Text="View One" />
    <gaia:Button ID="zButton2" runat="server" OnClick="zButton2_Click"
        Text="View Two" />
    <gaia:Button ID="zButton3" runat="server" OnClick="zButton3_Click"
        Text="View Three" />
    <gaia:MultiView ID="zMultiView1" runat="server" ActiveViewIndex="0">
        <asp:View runat="server" ID="zView1">
            <gaia:Label runat="server" ID="zLbl1" Text="Content (View 1)" />
        </asp:View>
        <asp:View runat="server" ID="zView2">
            <gaia:Label runat="server" ID="zLbl2" Text="Content (View 2)" />
        </asp:View>
        <asp:View runat="server" ID="zView3">
            <gaia:Label runat="server" ID="zLbl3" Text="Content (View 3)" />
        </asp:View>
    </gaia:MultiView>
</asp:Content>
