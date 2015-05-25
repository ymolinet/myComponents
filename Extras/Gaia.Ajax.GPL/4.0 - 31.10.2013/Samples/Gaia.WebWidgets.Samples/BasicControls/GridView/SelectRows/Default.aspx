<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.GridView.SelectRows.Default"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>Gaia Ajax: GridView - Select Multiple Rows</h1>
    <p>
        Selecting all or just one and one row in a Gaia GridView is fairly simple. Here
        is some simple logic for having a Gaia CheckBox in the header, which will select
        or deselect all the checkboxes in the rows. If you select one by one and all are
        finally selected, then the checkbox in the header will be checked as well.</p>
    <p>
        The sample also show how you how to add selection capabilities to the actual row
        of the GridView. The selection will toggle when you hold Ctrl down when clicking
        the row. To combine selection for both checkboxes and GridViewRow we have chosen
        to listen to the Clicked event of AspectClickable for both, preventing bubbling
        issues.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:GridView ID="zGrid" runat="server" AutoGenerateColumns="false" OnRowCreated="RowCreated">
    <Columns>
        <gaia:TemplateField>
            <HeaderTemplate>
                <gaia:CheckBox 
                    runat="server" 
                    ID="cbxHeader"
                    AutoPostBack="true" 
                    OnCheckedChanged="cbxHeader_OnCheckedChanged" />
            </HeaderTemplate>
         <ItemTemplate>
            <gaia:CheckBox 
                runat="server" 
                ID="cbx" />
            </ItemTemplate>
        </gaia:TemplateField>
        <gaia:BoundField DataField="ContactPerson" HeaderText="Person" />
        <gaia:BoundField DataField="ActivityName" HeaderText="Activity Name" />
    </Columns>
</gaia:GridView>
<gaia:Label 
    runat="server" 
    ID="zStatus" />
<p>Tips: Hold Ctrl down when clicking anywhere at a row, and it will toggle.</p>
</asp:Content>

