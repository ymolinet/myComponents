<%@ Page 
    Language="C#" 
    MasterPageFile="~/Core.master" 
    AutoEventWireup="true" 
    StylesheetTheme="Gaiax"
    CodeBehind="Default.aspx.cs" 
    Inherits="Gaia.WebWidgets.Samples.BasicControls.GridView.Overview.Default" 
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.GridView" />
    <p>
        In this sample we are showing the power of the Gaia Ajax GridView
        just using markup. Edit, Delete, Select, Paging and Sorting
        is all ready using the flexible ObjectDataSource.</p>
    <p>The benefits are:</p>
        <ul>
            <li>Either selecting, editing, deleting, paging or sorting, no full page PostBack - only <b>small Ajax callbacks</b></li>
            <li><b>No code needed</b> for simple gridviews like this</li>
            <li>High quality of application - <b>no JavaScript written</b></li>
        </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <p>Want more complex GridView sample? Try <a href="../NestedGridView/">nested</a>, 
        <a href="../RichInlineEditing/">inline editing</a>, 
        <a href="../DragGridViewRow/">drag and drop rows</a> or
        <a href="../SelectRows/">row selection</a>
    </p>
    <p>Tired of GridViews? Try <a href="../../../Combinations/WebApps/DashboardWithWebParts/">dashboard with web parts</a>, 
        <a href="../../../Combinations/WebApps/Chess/">chess game</a> or 
        <a href="../../../Combinations/WebApps/SimpleCalendarApplication/">calendar application!</a>
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:GridView 
    ID="zGridView" 
    runat="server" 
    Width="100%"
    AllowPaging="True" 
    AllowSorting="True" 
    DataKeyNames="Id"
    DataSourceID="zObjectDataSource" 
    AutoGenerateColumns="False"
    EmptyDataText="No elements returned..." >
    <Columns>
        <gaia:CommandField
            ShowEditButton="true" 
            ShowDeleteButton="true" 
            ShowSelectButton="true" />
        <gaia:BoundField 
            DataField="Id" 
            HeaderText="Id" 
            SortExpression="Id" 
            ReadOnly="true" />
        <gaia:BoundField 
            DataField="Status" 
            HeaderText="Status" 
            SortExpression="Status" 
            ReadOnly="true" />
        <gaia:BoundField 
            DataField="ContactPerson" 
            HeaderText="Contact Person"
            SortExpression="ContactPerson"
            ControlStyle-Width="100px" />
        <gaia:BoundField 
            DataField="ActivityName" 
            HeaderText="Activity Name"
            SortExpression="ActivityName"
            ControlStyle-Width="100px" />
        <gaia:BoundField 
            DataField="ActivityDate" 
            HeaderText="Activity Date"
            SortExpression="ActivityDate" 
            ReadOnly="true" />
        <gaia:CheckBoxField 
            DataField="IsChecked" 
            HeaderText="Is Checked"
            SortExpression="IsChecked" 
            ItemStyle-HorizontalAlign="Center" />
    </Columns>
</gaia:GridView>
<asp:ObjectDataSource 
    EnablePaging="False" 
    TypeName="Gaia.WebWidgets.Samples.Utilities.CalendarController"
    ID="zObjectDataSource" 
    runat="server"  
    SelectMethod="ListAsDataTable"
    UpdateMethod="Update"
    DeleteMethod="Delete">
</asp:ObjectDataSource>
</asp:Content>