<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SampleSearch.ascx.cs" Inherits="Gaia.WebWidgets.Samples.UI.SampleSearch" %>
<span class="title">Search</span>
<div class="search-input">
    <gaia:AutoCompleter
        ID="ac"
        runat="server"
        OnGetAutoCompleterItems="AcGetAutoCompleterItems"
        OnSelectionChanged="AcSelectionChanged"
        OnClosing="AcClosing" />
</div>
<gaia:Label
    runat="server"
    ID="l"
    CssClass="count">&nbsp;</gaia:Label>
