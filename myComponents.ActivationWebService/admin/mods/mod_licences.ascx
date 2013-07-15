<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="mod_licences.ascx.cs"
    Inherits="myComponents.ActivationWebService.admin.mods.mod_licences" %>
<gaia:Label runat="server" ID="lblCustomer" Text="Client: " />
<gaia:DropDownList runat="server" ID="ddlClients" DataSourceID="customersSqlDataSource"
    DataTextField="NOM" DataValueField="ID" />
<gaia:Label runat="server" ID="lblSoftware" Text="Logiciel: " />
<gaia:DropDownList runat="server" ID="ddlSoftware" DataSourceID="SoftwaresSqlDataSource"
    DataTextField="NAME" DataValueField="ID" />
<gaia:Label runat="server" ID="lblQuantite" Text="Nombre de licence autorisée: " />
<gaia:TextBox runat="server" ID="tbQuantite" />
<gaia:Button runat="server" ID="btnNewLicence" Text="Ajouter" OnClick="btnNewLicence_OnClick" />
<br />
<asp:SqlDataSource ID="SoftwaresSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:myComponents.ActivationWebService.Properties.Settings.BDD %>"
    ProviderName="<%$ ConnectionStrings:myComponents.ActivationWebService.Properties.Settings.BDD.ProviderName %>"
    SelectCommand="SELECT NAME, ID FROM softwares"></asp:SqlDataSource>
<asp:SqlDataSource ID="customersSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:myComponents.ActivationWebService.Properties.Settings.BDD %>"
    ProviderName="<%$ ConnectionStrings:myComponents.ActivationWebService.Properties.Settings.BDD.ProviderName %>"
    SelectCommand="SELECT ID, NOM FROM customers WHERE (ACTIF &lt;&gt; 0)"></asp:SqlDataSource>
<br />
<gaia:Label runat="server" ID="lblInfo" />
<br />
<br />
<gaia:Panel runat="server" ID="pnlLicences">
<gaia:GridView runat="server" ID="licencesGridview" AllowPaging="True" 
        AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
        DataSourceID="licencesSqlDataSource" EnableModelValidation="True" 
        ForeColor="#333333" GridLines="None" >
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:CommandField ShowSelectButton="True" />
        <asp:BoundField DataField="NOM" HeaderText="NOM" SortExpression="NOM" />
        <asp:BoundField DataField="NAME" HeaderText="NAME" SortExpression="NAME" />
        <asp:BoundField DataField="LICENCES_NB" HeaderText="LICENCES_NB" 
            SortExpression="LICENCES_NB" />
        <asp:BoundField DataField="USED" HeaderText="USED" SortExpression="USED" />
    </Columns>
    <EditRowStyle BackColor="#2461BF" />
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#EFF3FB" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    </gaia:GridView>
    <asp:SqlDataSource ID="licencesSqlDataSource" runat="server" 
        ConnectionString="<%$ ConnectionStrings:myComponents.ActivationWebService.Properties.Settings.BDD %>" 
        ProviderName="<%$ ConnectionStrings:myComponents.ActivationWebService.Properties.Settings.BDD.ProviderName %>" 
        
        SelectCommand="SELECT customers.NOM, softwares.NAME, licences.LICENCES_NB, COUNT(activation_keys.ACTIVATION_KEY) AS USED FROM licences INNER JOIN softwares ON licences.SOFTWARE_ID = softwares.ID INNER JOIN customers ON licences.CUSTOMER_ID = customers.ID LEFT OUTER JOIN activation_keys ON activation_keys.COMPANY_ID = customers.ID AND activation_keys.SOFTWARE_ID = softwares.ID">
    </asp:SqlDataSource>
</gaia:Panel>