<%@ Page Title="" Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.GridView.DragGridViewRow.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Gaia Ajax: GridView - Adding AspectDraggable to a GridViewRow</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

    <gaia:GridView 
        runat="server" 
        ID="zGv" 
        CssClass="gridview" 
        AutoGenerateColumns="false" 
        Width="600px" 
        RowStyle-CssClass="row"
        OnRowCreated="RowCreated" >
        <Columns>
            <gaia:BoundField HeaderText="Coming?" DataField="IsChecked" />
            <gaia:BoundField HeaderText="Name" DataField="ContactPerson" />
            <gaia:BoundField HeaderText="Date" DataField="ActivityDate" />
        </Columns>
        <EmptyDataTemplate>
            <gaia:Label runat="server" ID="zEmpty" Text="No items found" />
        </EmptyDataTemplate>
    </gaia:GridView>

    <gaia:Panel runat="server" ID="zDropArea" CssClass="container-drop">
        <gaia:Label runat="server" ID="zLbl" />
    </gaia:Panel>
</asp:Content>
