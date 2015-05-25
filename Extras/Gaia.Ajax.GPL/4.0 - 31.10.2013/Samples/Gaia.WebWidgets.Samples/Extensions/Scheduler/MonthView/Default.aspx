<%@ Page 
    Title="Gaia Ajax Scheduler MonthView" 
    Language="C#" 
    MasterPageFile="~/Core.master" 
    AutoEventWireup="true" 
    CodeBehind="Default.aspx.cs" 
    Inherits="Gaia.WebWidgets.Samples.Extensions.Scheduler.MonthView.Default" %>

<%@ Import Namespace="Gaia.WebWidgets.Samples.Extensions.Scheduler.MonthView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Gaia Ajax : Scheduler MonthView</h1>
    <p>The Scheduler MonthView is fully "templatable" and can be DataBound to any collection of items</p>
    <p>In this sample we allow you to assign multiple resource's availability to a selected range of days in two simple steps. 
    The sample also demonstrates how you can add the ControlCollector control manually to your code to allow clientside selection of Gaia Controls.
    </p>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">    
    
    <h3 class="header-step">Step 1: Select Resources</h3>
    <asp:PlaceHolder runat="server" ID="zResourcesPlaceholder" />
    
    <h3 class="header-step">Step 2: Select Day(s)</h3>
    
    <gaia:ControlCollector 
        ID="monthCellCollector"
        CssClassCollected="month-cell-collected"
        Filter=".scheduler-monthcell"
        FilterEventSource=".scheduler-container"
        OnCollected="MonthCellsCollected"
        runat="server" />

    <gaia:Scheduler 
        runat="server" 
        ID="zScheduler" 
        View="MonthView" 
        Width="100%"
        CssClass="scheduler">
  
      <MonthView>
        <Styles>
            <DefaultCellStyle CssClass="scheduler-defaultcellstyle" />
            <HeaderCellStyle CssClass="scheduler-headercellstyle scheduler-headercell" />
            <CellStyle CssClass="scheduler-monthcell" />
            <CellOutOfRangeStyle CssClass="scheduler-outofrangecell" />
            <ContainerStyle CssClass="scheduler-container" />
        </Styles>
    
        <Templates>
            <ReportTemplate>
                <asp:Repeater runat="server" ID="rep" DataSource='<%# Container.Items %>'>
                    <ItemTemplate>
            
                        <gaia:HiddenField 
                            runat="server"
                            ID="zTaskId"
                            Visible="false"
                            Value='<%# ((AssignedResource)Container.DataItem).ID %>' />

                        <gaia:LinkButton
                            ID="zAssigned"
                            runat="server"
                            CssClass='<%# ((AssignedResource)Container.DataItem).CssClass + " ico"%>'
                            ToolTip='<%# ((AssignedResource)Container.DataItem).Name %>'
                            Text="&nbsp;"
                            EnableBubbling="false"
                            OnClick="ClearResourcesForSelectedResource" />

                    </ItemTemplate>
                </asp:Repeater>

                <gaia:LinkButton
                            id="zClear"
                            runat="server"
                            CssClass="clear-resources"
                            Text="&nbsp;"
                            Visible='<%# MakeClearButtonVisible(Container) %>'
                            CommandArgument='<%# Container.Date %>'
                            EnableBubbling="false"
                            OnClick="ClearResourcesForSelectedDay" />

            </ReportTemplate>
        </Templates>

      </MonthView>
    </gaia:Scheduler>

<br/>

<gaia:LinkButton
    id="zClearAll"
    runat="server"
    Text='Clear All'
    OnClick="ClearAllAssignedResources" />

</asp:Content>
