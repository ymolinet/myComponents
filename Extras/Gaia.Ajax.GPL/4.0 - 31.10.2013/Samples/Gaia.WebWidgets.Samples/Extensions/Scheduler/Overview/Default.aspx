<%@ Page 
    Title="Gaia Ajax: Scheduler Overview" 
    Language="C#" 
    MasterPageFile="~/Core.master" 
    AutoEventWireup="true" 
    CodeBehind="Default.aspx.cs" 
    Inherits="Gaia.WebWidgets.Samples.Extensions.Scheduler.Overview.Default" %>
<%@ Import Namespace="Gaia.WebWidgets.Samples.Extensions.Scheduler.Overview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <h1>Gaia Ajax: Scheduler Overview</h1>
   <p>The scheduler allows you to display multiple tasks pr/day that can be <strong>resized</strong> and <strong>moved</strong> around. It also features a MonthView or DayView. In the DayView you can 
    select how many days you want and you can also specify how many minutes each Resize "tick" represents and how many minutes each Cell represents. 
    <p>
    Other useful features include the ability to set StartHour, EndHour and allow TimeRanges to be selected by just selecting the cells with your mouse. 
    This raises the TimeRangeSelected event where you can do whatever you want, for example create new tasks. Open Dialogs, etc.
    </p>
    <p><strong>Oh, and the Scheduler itself is written in pure C# just based on the Gaia Ajax components. Yes, not a single line of JavaScript for this component. IOHO - That rocks!
    </strong></p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    
    <gaia:LinkButton 
        ID="zClear" 
        runat="server" 
        OnClick="ClearTasks"
        Text="Clear Tasks">
    </gaia:LinkButton>
    -
    <gaia:LinkButton 
        ID="zReset" 
        runat="server" 
        Text="Load Sample Data"
        OnClick="ReloadSampleData">
    </gaia:LinkButton>
    
    <asp:Panel runat="server" ID="zResources" />
    
    <gaia:Scheduler 
        runat="server" 
        ID="zScheduler"
        Width="100%"
        View="DayView"
        OnTaskModified="OnTaskModified"
        OnTimeRangeSelected="OnTimeRangeSelected"
        CssClass="scheduler">        
        
    <Workday 
        StartHour="7" 
        EndHour="18" />
    
    <DayView 
        ResizeMinutes="30" 
        NumberOfDays="3">

        <Styles>
            <DefaultCellStyle CssClass="scheduler-defaultcellstyle" />
            <HeaderCellStyle CssClass="scheduler-headercellstyle" />
            
            <TaskPanelStyle 
                CssClass="scheduler-taskpanelstyle" 
                BorderWidth="2px" 
                CssClassDrag="scheduler-taskpanelstyle-drag" />
            
            <TaskCellStyle 
                CssClass="scheduler-taskcellstyle" 
                BorderWidth="1px" 
                Height="15px" />

            <TaskContainerStyle CssClass="scheduler-taskcontainerstyle" />
            <TaskContainerSelectedStyle CssClass="scheduler-taskcontainerselectedstyle" />
            <TimeCellStyle CssClass="scheduler-timecellstyle" />
        </Styles>
       
        <Timeline Granularity="30" />
        
        <Templates>
            <TaskTemplate>
                 <samples:DataItemContainerPanel 
                        runat="server" 
                        ID="c" 
                        CssClass="<%# GetTaskCssClass(Container.DataItem) %>">
                     
                    <gaia:LinkButton 
                        ID="zDeleteTask" 
                        runat="server" 
                        EnableBubbling="false"
                        Text="&nbsp;" 
                        CssClass="task-delete"
                        OnCommand="DeleteTask"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                        CommandName="Delete">
                    </gaia:LinkButton>

                    <gaia:Label 
                            runat="server" 
                            ID="f" 
                            Text='<%# DataBinder.Eval(Container.DataItem, "StartTime", "{0:HH:mm}") %>' />
                        - 
                        <gaia:Label 
                            runat="server" 
                            ID="t" 
                            Text='<%# DataBinder.Eval(Container.DataItem, "EndTime", "{0:HH:mm}") %>' />
                 
                   <gaia:InPlaceEdit
                        ID="zTaskTitle"
                        HasButtons="false"
                        Blink="false"
                        runat="server"
                        CssClass="task-title"
                        OnTextChanged="TaskTitleChanged"
                        Text='<%# ((Task)Container.DataItem).Title %>'
                         />

                 </samples:DataItemContainerPanel>    
            </TaskTemplate>
        </Templates>
    </DayView>
    
    </gaia:Scheduler>

    <gaia:Label runat="server" ID="msg" ForeColor="Red" Font-Bold="True"></gaia:Label>

</asp:Content>
