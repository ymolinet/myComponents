<%@ Control Language="C#" AutoEventWireup="true" Codebehind="OrderLineGridView.ascx.cs"
    Inherits="Gaia.WebWidgets.Samples.BasicControls.GridView.NestedGridView.OrderLineGridView" %>
<gaia:GridView ID="zGvCC" runat="server" AutoGenerateColumns="false"
    ShowHeader="false" ShowFooter="true" Width="100%" OnRowDataBound="zGvCC_RowDataBound">
    <Columns>
        <gaia:TemplateField>
            <ItemStyle CssClass="product-cell" />
            <ItemTemplate>
                <gaia:Label runat="server" ID="name" Text='<%#XPath("ProductName")%>' />
            </ItemTemplate>
            <FooterTemplate>
                 <gaia:Label runat="server" ID="footer" Font-Bold="true" Text="Sum" />
            </FooterTemplate>
        </gaia:TemplateField>
        <gaia:TemplateField>
            <ItemStyle CssClass="right-aligned quantity-cell" />
            <ItemTemplate>
                <gaia:Label runat="server" ID="quantity" Text='<%#XPath("Quantity")%>' />
            </ItemTemplate>
        </gaia:TemplateField>
        <gaia:TemplateField>
            <ItemStyle CssClass="right-aligned price-cell" />
            <ItemTemplate>
                <gaia:Label runat="server" ID="price" Text='<%#XPath("Price")%>' />
            </ItemTemplate>
        </gaia:TemplateField>
        <gaia:TemplateField>
            <ItemStyle CssClass="right-aligned price-cell" />
            <ItemTemplate>
                <gaia:Label runat="server" ID="linetotal" />
            </ItemTemplate>
            <FooterStyle CssClass="right-aligned" />
            <FooterTemplate>
                <gaia:Label runat="server" ID="footer" Font-Bold="true" />
            </FooterTemplate>
        </gaia:TemplateField>
    </Columns>
</gaia:GridView>