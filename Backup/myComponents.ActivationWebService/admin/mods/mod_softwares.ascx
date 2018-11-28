<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="mod_softwares.ascx.cs"
    Inherits="myComponents.ActivationWebService.admin.mods.mod_software" %>
<gaia:Label runat="server" ID="lblNew" Text="Nom du logiciel" /><gaia:TextBox runat="server"
    ID="tbNewSoftware" /><gaia:Label runat="server" ID="lblIdentifier" Text="Identifier" /><gaia:TextBox
        runat="server" ID="tbIdentifier" /><gaia:Button runat="server" ID="btnNewSoftwares"
            Text="Ajouter" OnClick="btnNewSoftwares_OnClick" />
<br />
<gaia:Label runat="server" ID="lblInfo" />
<br />
<br />
<gaia:Panel runat="server" ID="pnlSoftwares">
    <gaia:GridView runat="server" ID="softwaresGridview" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="ID" DataSourceID="mysqlSqlDataSource"
        EnableModelValidation="True" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                SortExpression="ID" />
            <asp:BoundField DataField="NAME" HeaderText="NAME" SortExpression="NAME" />
            <asp:BoundField DataField="IDENTIFIER" HeaderText="IDENTIFIER" SortExpression="IDENTIFIER" />
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    </gaia:GridView>
    <asp:SqlDataSource ID="mysqlSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:myComponents.ActivationWebService.Properties.Settings.BDD %>"
        ProviderName="<%$ ConnectionStrings:myComponents.ActivationWebService.Properties.Settings.BDD.ProviderName %>"
        SelectCommand="SELECT ID, NAME, IDENTIFIER FROM softwares"></asp:SqlDataSource>
</gaia:Panel>
