<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="mod_companies.ascx.cs"
    Inherits="myComponents.ActivationWebService.admin.mods.mod_companies" %>
<gaia:Label runat="server" ID="lblNew" Text="Nom du client" /><gaia:TextBox runat="server"
    ID="tbNewCustomer" /><gaia:Button runat="server" ID="btnNewCustomer" Text="Ajouter"
        OnClick="btnNewCustomer_OnClick" />
<br />
<gaia:Label runat="server" ID="lblInfo" />
<br />
<br />
<gaia:Panel runat="server" ID="pnlCustomer">
    <gaia:GridView runat="server" ID="customerGridview" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="ID" DataSourceID="mySqlDataSource"
        EnableModelValidation="True" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                SortExpression="ID" />
            <asp:BoundField DataField="NOM" HeaderText="NOM" SortExpression="NOM" />
            <asp:CheckBoxField DataField="ACTIF" HeaderText="ACTIF" SortExpression="ACTIF" />
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    </gaia:GridView>
    <asp:SqlDataSource ID="mySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:myComponents.ActivationWebService.Properties.Settings.BDD %>"
        ProviderName="<%$ ConnectionStrings:myComponents.ActivationWebService.Properties.Settings.BDD.ProviderName %>"
        SelectCommand="SELECT ID, NOM, ACTIF FROM customers"></asp:SqlDataSource>
</gaia:Panel>
