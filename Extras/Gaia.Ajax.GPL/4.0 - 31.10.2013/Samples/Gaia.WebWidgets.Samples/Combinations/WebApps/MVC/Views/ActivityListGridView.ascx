<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ActivityListGridView.ascx.cs" 
    Inherits="Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Views.ActivityListGridView" %>

<gaia:GridView 
    ID="zGrid" 
    runat="server" 
    AutoGenerateColumns="false">
    <Columns>
        <gaia:BoundField 
            DataField="ActivityDate" 
            HeaderText="Date" 
            DataFormatString="{0:ddd dd. MMM}" 
            ShowHeader="true">
        </gaia:BoundField>
        
        <gaia:BoundField 
            DataField="ContactPerson" 
            HeaderText="Contact Person" 
            ShowHeader="true">
        </gaia:BoundField>
        
        <gaia:BoundField 
            DataField="ActivityName" 
            ShowHeader="true" 
            HeaderText="Text">
        </gaia:BoundField>
    </Columns>
</gaia:GridView>
