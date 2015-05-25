<%@ Control 
    Language="C#" 
    AutoEventWireup="True" 
    Inherits="Gaia.WebWidgets.Samples.Combinations.WebApps.SimpleCalendarApplication.CalendarApplication" 
    Codebehind="CalendarApplication.ascx.cs" %>
<%@ Register TagPrefix="gaia" Namespace="Gaia.WebWidgets.Samples.Combinations.WebApps.SimpleCalendarApplication" Assembly="Gaia.WebWidgets.Samples" %>
<div class="left" style="width: 50%;">
    <div class="top-nav">
        <gaia:LinkButton 
            runat="server"
            ID="newApp"
            CssClass="right btn"
            OnClick="newApp_click"
            Text="New..." />
        <gaia:LinkButton 
            runat="server"
            ID="zToday"
            CssClass="left btn"
            OnClick="zToday_Clicked"
            Text="Today" />
        <gaia:LinkButton 
            runat="server"
            ID="previous"
            CssClass="left btn"
            OnClick="previous_click"
            Text="<<" />
        <gaia:LinkButton  
            runat="server" 
            ID="next" 
            CssClass="left btn"
            OnClick="next_click"
            Text=">>" />
 
        <gaia:Label runat="server" 
            ID="zLblCurrentMonth" 
            CssClass="left" />
    </div>
    <gaia:Scheduler 
        runat="server" 
        ID="zScheduler" 
        View="MonthView" 
        CssClass="scheduler"
        Width="100%"
        DateStartPropertyName="ActivityDate"
        DateEndPropertyName="ActivityDate">
        <MonthView>
            <Styles>
                <HeaderCellStyle CssClass="scheduler-headercell" />
                <CellStyle CssClass="scheduler-monthcell" />
                <CellOutOfRangeStyle CssClass="scheduler-outofrangecell" />
                <ContainerStyle CssClass="scheduler-container" />
            </Styles>
            <Templates>
                <HeaderTemplate>
                    <gaia:Label
                        runat="server" 
                        Text='<%# Container.Value.ToString("ddd.") %>' />
                </HeaderTemplate>
                <DayTemplate>
                    <gaia:Label 
                        runat="server" 
                        CssClass='<%# GetDayCssClass(Container.Value) %>'
                        Text='<%# Container.Value.ToString("dd") %>' />
                </DayTemplate>
                <ReportTemplate>
                    <gaia:ReportTemplateWrapperPanel 
                        runat="server"
                        CssClass='<%# GetDayTemplateCssClass(Container) %>'
                        OnLoad="LoadClickablePanel" >
                        <gaia:Label
                            id="zSelectDay"
                            runat="server"
                            CssClass="app-count"
                            Text='<%# GetAppointmentsCountForDay(Container.Date) %>'
                            Visible='<%# GetAppointmentsCountForDay(Container.Date) > 0 %>' />
                    </gaia:ReportTemplateWrapperPanel>
                </ReportTemplate>
            </Templates>
        </MonthView>
    </gaia:Scheduler>

</div>
           
<div class="right" style="width: 45%; padding: 0 10px;">
    <div class="top-nav">
        <gaia:Label runat="server" ID="zGvHeader" CssClass="left" />
    </div>
    <gaia:GridView runat="server" ID="zGridView" 
        AutoGenerateColumns="false"
        DataKeyNames="Id" 
        OnRowDeleting="zGridView_OnRowDeleting" BorderWidth="0"
        OnRowCreated="GridRowCreated"
        CssClass="gv-calendar">
        <RowStyle CssClass="itemEven" />
        <AlternatingRowStyle CssClass="itemOdd" />
        <Columns>
            <gaia:BoundField HeaderText="Time" DataField="ActivityDate" DataFormatString="{0:HH:mm}" />
            <gaia:BoundField HeaderText="Name" DataField="ActivityName" />
            <gaia:BoundField HeaderText="Contact" DataField="ContactPerson" />
            <gaia:CommandField ShowDeleteButton="true" />
        </Columns>
    </gaia:GridView>
</div>

<gaia:Window 
    runat="server" 
    ID="zNewAppWindow"
    Caption="Create new activity..."
    Resizable="false" 
    Closable="true"
    Maximizable="false" 
    Minimizable="false" 
    Visible="false"
    Height="290" 
    Width="400">

    <table>
        <tr>
            <td>Date: </td>
            <td class="td-dtp">
                <gaia:DateTimePicker 
                    runat="server" 
                    HasDropDownButton="true" 
                    HasTimePart="true" 
                    ID="zDateAppointment" 
                    Width="120px"
                    Format="yyyy-MM-dd HH:mm"  />
                <gaia:RequiredFieldValidator 
                    runat="server" 
                    ID="zReqDtp" 
                    ControlToValidate="zDateAppointment"
                    ErrorMessage="*" />
            </td>
        </tr>
        <tr>
            <td>Task name:</td>
            <td>
                <gaia:TextBox 
                    runat="server" 
                    Width="220px"
                    ID="zTxtTaskName" />
                <gaia:RequiredFieldValidator 
                    runat="server" 
                    ID="zReqHeader" 
                    ControlToValidate="zTxtTaskName" 
                    ErrorMessage="*" />
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top;">Person: </td>
            <td>
                <gaia:TextBox 
                    runat="server" 
                    ID="zTxtPerson"
                    Width="220px" />
                <gaia:RequiredFieldValidator 
                    runat="server" 
                    ID="zReqBody" 
                    ControlToValidate="zTxtPerson"
                    ErrorMessage="*" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <gaia:ExtendedButton 
                    runat="server" 
                    style="float: left;"
                    OnClick="zBtnOk_Click"
                    Width="100px"
                    ID="zBtnOk" 
                    Text="Ok" />
                <gaia:ExtendedButton 
                    runat="server" 
                    style="float: left;"
                    OnClick="zBtnCancel_Click"
                    Width="100px"
                    ID="zBtnCancel" 
                    CausesValidation="False"
                    Text="Cancel" />
            </td>
        </tr>
    </table>
</gaia:Window>

<div class="note-box">
    <h4>Things to note:</h4>
    <ul>
        <li>On the left is the Gaia Ajax Scheduler</li>
        <li>On the right is the Gaia Ajax GridView</li>
        <li>Add a new appointment with a single click in the Scheduler</li>
        <li>Drag a row into Scheduler to move an appointment</li>
        <li>Single click a cell to list appointments</li>
        <li>Double click a cell to add a new appointment</li>
    </ul>
</div>
