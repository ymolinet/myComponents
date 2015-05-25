<%@ Control Language="C#" AutoEventWireup="true" Codebehind="OrderGridView.ascx.cs"
    Inherits="Gaia.WebWidgets.Samples.BasicControls.GridView.NestedGridView.OrderGridView" %>
<%@ Register Src="OrderLineGridView.ascx" TagName="OrderLineGridView"
    TagPrefix="uc1" %>

<gaia:LinkButton runat="server" ID="zToggleViewOrders" Text="Show Orders" OnClick="zToggleViewOrders_Click"/>

<gaia:GridView ID="zGvC" runat="server" Visible="false"
    AutoGenerateColumns="false" ShowHeader="false" Width="100%">
    <Columns>
        <gaia:TemplateField>
            <ItemStyle CssClass="order-cell" />
            <ItemTemplate>
            Order No.: <asp:Label runat="server" ID="oId" Text='<%#XPath("OrderId")%>' />
        </ItemTemplate>
        </gaia:TemplateField>
        <gaia:TemplateField>
            <ItemTemplate>
            <uc1:OrderLineGridView id="olgv" runat="server" GridDataSource='<%# XPathSelect("OrderLine") %>' />
        </ItemTemplate>
        </gaia:TemplateField>
    </Columns>
</gaia:GridView>