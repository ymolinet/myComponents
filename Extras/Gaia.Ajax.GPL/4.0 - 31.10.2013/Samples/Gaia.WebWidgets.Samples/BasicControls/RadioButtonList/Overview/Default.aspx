<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.RadioButtonList.Overview.Default"
    Title="Gaia Ajax: RadioButtonList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.RadioButtonList" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:RadioButtonList ID="zRadioButtonList1" runat="server" AutoPostBack="true"
        OnSelectedIndexChanged="zRadioButtonList1_OnSelectedIndexChanged">
        <asp:ListItem>Item 1</asp:ListItem>
        <asp:ListItem>Item 2</asp:ListItem>
        <asp:ListItem>Item 3</asp:ListItem>
        <asp:ListItem>Item 4</asp:ListItem>
        <asp:ListItem>Item 5</asp:ListItem>
        <asp:ListItem>Item 6</asp:ListItem>
    </gaia:RadioButtonList>
    <br />
    <gaia:Label ID="zLabel1" runat="server" />
    <br />
    <br />
    <div>
        <gaia:CheckBox ID="zRepeatDirection" runat="server" Text="Display Horizontally"
            AutoPostBack="true" OnCheckedChanged="zRepeatDirection_CheckedChanged" />
    </div>
    <div>
        <gaia:CheckBox ID="zRepeatLayout" runat="server" Text="Display Table Layout"
            Checked="true" AutoPostBack="true" OnCheckedChanged="zRepeatLayout_OnCheckedChanged" />
    </div>
</asp:Content>
