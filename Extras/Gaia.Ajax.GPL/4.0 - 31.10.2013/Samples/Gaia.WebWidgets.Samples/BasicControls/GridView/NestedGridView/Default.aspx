<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" StylesheetTheme="Gaiax"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.GridView.NestedGridView.Default"
    Title="Gaia Ajax: Nested GridViews" %>

<%@ Register Src="OrderGridView.ascx" TagName="OrderGridView"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>Gaia Ajax: Nested GridViews</h1>
    <p>
        Here we are showing how easily you can build nested gridviews.
        Though this functionality is quite advanced and highly efficient,
        there is not much coding required. Also note that we are using
        the XmlDataSource as the source for this example, using XPath
        in each of the nested level.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<gaia:GridView 
    ID="zGvP" 
    runat="server" 
    AutoGenerateColumns="false" 
    DataSourceID="XmlDataSource1" 
    Width="100%">
    <Columns>
        <gaia:BoundField 
            HeaderText="Customer" 
            DataField="Name" 
            ItemStyle-CssClass="customer-cell" />
        <gaia:TemplateField>
            <ItemTemplate>
                <uc1:OrderGridView 
                    ID="zUcC" 
                    runat="server" 
                    GridDataSource='<%# XPathSelect("Order") %>' />
            </ItemTemplate>
        </gaia:TemplateField>
    </Columns>
</gaia:GridView>
    
<asp:XmlDataSource 
    ID="XmlDataSource1" 
    runat="server" 
    DataFile="~/App_Data/customers-orderheaders-orderlines.xml">
</asp:XmlDataSource>
</asp:Content>
