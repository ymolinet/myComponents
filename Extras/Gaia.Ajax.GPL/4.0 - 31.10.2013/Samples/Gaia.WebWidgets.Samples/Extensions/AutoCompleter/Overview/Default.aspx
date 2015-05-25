<%@ Page 
    Language="C#" 
    AutoEventWireup="true" 
    MasterPageFile="~/Core.master" 
    Title="Ajax AutoCompleter Sample"
    Inherits="Gaia.WebWidgets.Samples.Extensions.AutoCompleter.Overview.Default" Codebehind="Default.aspx.cs" %>

<asp:Content 
    ID="Content1" 
    ContentPlaceHolderID="ContentPlaceHolder1" 
    Runat="Server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Extensions.AutoCompleter" />
    <p>In this example a user can select a country just by typing in the first few letters
    of the country and a list of matched countries are displayed with the flag, name and the
    searched portion is also highlighted. 
    </p>
    <p>These features were not built into the Autocompleter, but because the control is so
    flexible it was quite easy to build this functionality with a few simple building blocks</p>
    
    </asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<div class="container-status">
    <gaia:Label 
        ID="Label1"
        Font-Bold="true"
        runat="server" /> 
    <gaia:Panel 
        runat="server" 
        ID="zPanelResult" 
        Visible="false">
            <h3>Results</h3>
            <p>You selected <gaia:Image id="zImageResult" runat="server"/>
            <gaia:Label ID="zCountryResult" runat="server" /> 
            </p>
    </gaia:Panel>
</div>

<asp:Label 
    runat="server" 
    ID="AutoCompleterLabelHelper"
    Font-Bold="true"
    Text="Which country do you live in ? " />
<br />   

<gaia:AutoCompleter 
    ID="AutoCompleter1" 
    runat="server" 
    Width="350px" 
    Height="225px"
    KeyChangeEventsInterval="200"
    OnGetAutoCompleterItems="AutoCompleter1_GetAutoCompleteData" 
    OnSelectionChanged="AutoCompleter1_SelectionChanged" />
<br />
<br />

</asp:Content>


