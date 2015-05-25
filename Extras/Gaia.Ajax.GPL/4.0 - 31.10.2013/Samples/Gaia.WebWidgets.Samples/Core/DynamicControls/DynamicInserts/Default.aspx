<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Core.DynamicControls.DynamicInserts.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
 <h1>Gaia Ajax:  Dynamic Inserts vs Partial Rendering</h1>
 <p> Which rendering strategy would you prefer</p>
    <gaia:RadioButtonList ID="zRenderOption" runat="server">
        <Items>
            <asp:ListItem Value="0" Text="Partial Rendering (ie. ForceAnUpdate)"></asp:ListItem>
            <asp:ListItem Selected="True" Value="1" Text="Dynamic Inserts"></asp:ListItem>
        </Items>
    </gaia:RadioButtonList>
<gaia:LinkButton ID="zButtonAddManually" OnClick="zButtonManualAdd_Click" runat="server" Text="Add Flag (manually)"></gaia:LinkButton>
<br />
<gaia:LinkButton ID="zButtonStart" OnClick="zButtonStartStop_Click" runat="server" Text="Add Flags with Timer (automatic)"></gaia:LinkButton>
<gaia:LinkButton ID="zButtonStop" OnClick="zButtonStartStop_Click" runat="server" Visible="false" Text="Stop timer"></gaia:LinkButton>
<br />
<gaia:LinkButton ID="zButtonClear" OnClick="zButtonClear_Click" runat="server" Text="Clear Flags"></gaia:LinkButton>
<br />
<gaia:LinkButton ID="zButtonRenderAll" OnClick="zButtonRenderAll_Click" runat="server" Text="Add All Flags"></gaia:LinkButton>
<div style="position: relative;">
    <gaia:Panel ID="zContainer" runat="server" CssClass="flag-container"></gaia:Panel>
</div>
<gaia:Timer ID="zTimer" runat="server" OnTick="zTimer_Tick" Enabled="false"></gaia:Timer>

</asp:Content>
