<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="myComponents.ActivationWebService.admin._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagName="Companies" TagPrefix="my" Src="~/admin/mods/mod_companies.ascx" %>
<%@ Register TagName="Softwares" TagPrefix="my" Src="~/admin/mods/mod_softwares.ascx" %>
<%@ Register TagName="Licences" TagPrefix="my" Src="~/admin/mods/mod_licences.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <gaia:TabControl runat="server" ID="tabCtrl" ForceDynamicRendering="true">
            <gaia:TabView runat="server" ID="tabCompanies" Caption="Liste des sociétés">
                <my:Companies runat="server" ID="modCompanies" />
            </gaia:TabView>
            <gaia:TabView runat="server" ID="tabSoftwares" Caption="Liste des logiciels">
                <my:Softwares runat="server" ID="modSoftwares" />
            </gaia:TabView>
            <gaia:TabView runat="server" ID="tabLicences" Caption="Liste des licences">
                <my:Licences runat="server" ID="modLicences" />
            </gaia:TabView>
        </gaia:TabControl>
    </div>
    </form>
</body>
</html>
