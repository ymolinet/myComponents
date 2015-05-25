<%@ Page 
    Language="C#"
    MasterPageFile="~/Core.master" 
    AutoEventWireup="True" 
    StylesheetTheme="Gaiax"
    Inherits="Gaia.WebWidgets.Samples.BasicControls.GridView.RichInlineEditing.Default"
    Title="Gaia Ajax: GridView - Rich Inline Editing" 
    Codebehind="Default.aspx.cs" %>

<asp:Content 
    ID="Content1" 
    ContentPlaceHolderID="ContentPlaceHolder1" 
    Runat="Server">
    
    <h1>Gaia Ajax: GridView - Rich Inline Editing</h1>
    <p>
        To have an immediate editing experience and avoid extra steps
        when updating a grid, you can take advantage of putting controls
        into a TemplateField. In this sample we are using <a href="../../../Extensions/InPlaceEdit/Overview/">
            InPlaceEdit</a>, <a href="../../../Extensions/DateTimePicker/Overview/">
                DateTimePicker</a>, <a href="../../RadioButtonList/Overview/">RadioButtonList</a>
        and <a href="../../CheckBox/Overview/">CheckBox</a> inside TemplateFields
        to update instantly update our data source.
    </p>
    <p>
        This samples also shows easy it is to use connect an ObjectDataSource
        to your GridView. Here we only use SelectMethod="ListAsDataTable".
        The other updates are done in the code behind.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<div class="container-feedback">
    <div class="container-feedback-area">
        <img 
            runat="server"
            alt=" " 
            class="container-feedback-area-tag"
            src="~/media/contentImages/tag.png" />
        <gaia:Label
            runat="server" 
            ID="status"
            CssClass="container-feedback-area-status"
            Text="Edit the grid to see its status change here" />
    </div>
</div>
<gaia:GridView 
    ID="zGrid" 
    runat="server" 
    Width="100%"
    DataSourceID="zObjectDataSource"
    AutoGenerateColumns="False">
    <Columns>
        <gaia:TemplateField HeaderText="Activity Name">
            <ItemStyle Width="175px" />
            <ItemTemplate>
                <gaia:InPlaceEdit 
                    SingleLine="true" 
                    HasButtons="false"
                    MouseClicksToTriggerEditMode="SingleClick"
                    runat="server"
                    CssClass="inplaceedit"
                    OnTextChanged="ActivityNameTextChanged"
                    Text='<%# Eval("ActivityName")%>' />
            </ItemTemplate>
        </gaia:TemplateField>
        <gaia:TemplateField HeaderText="Date" ItemStyle-CssClass="itemCenterDate">
            <ItemTemplate>
                <gaia:DateTimePicker 
                    runat="server" 
                    Width="100px"
                    Value='<%# Eval("ActivityDate")%>'
                    CalendarWidth="200px" 
                    Format="dd.MMM yy HH:mm" 
                    OnSelectedDateChanged="DateChanged"
                    HasDropDownButton="false"
                    HasTimePart="true" />
            </ItemTemplate>
        </gaia:TemplateField>
        <gaia:TemplateField HeaderText="Person Name">
            <ItemStyle Width="175px" />
            <ItemTemplate>
                <gaia:InPlaceEdit 
                    SingleLine="true" 
                    HasButtons="false"
                    MouseClicksToTriggerEditMode="SingleClick"
                    runat="server"
                    OnTextChanged="ContactPersonTextChanged"
                    CssClass="inplaceedit"
                    Text='<%# Eval("ContactPerson")%>' />
            </ItemTemplate>
        </gaia:TemplateField>
        <gaia:TemplateField HeaderText="Status" ItemStyle-CssClass="itemCenter">
            <ItemTemplate>
                <gaia:RadioButtonList 
                    runat="server" 
                    AutoPostBack="true" 
                    OnSelectedIndexChanged="StatusChanged"
                    RepeatDirection="Horizontal" 
                    RepeatLayout="Flow" 
                    SelectedValue='<%# Eval("Status") %>'>
                    <Items>
                        <asp:ListItem Value="1" Text="Yes" />
                        <asp:ListItem Value="2" Text="No" />
                    </Items>
                </gaia:RadioButtonList>
            </ItemTemplate>
        </gaia:TemplateField>
        <gaia:TemplateField HeaderText="Confirmed" ItemStyle-CssClass="itemCenter">
            <ItemTemplate>
                <gaia:CheckBox 
                    runat="server" 
                    AutoPostBack="true" 
                    OnCheckedChanged="IsCheckedChanged"
                    Checked='<%# Eval("IsChecked") %>' />
            </ItemTemplate>
        </gaia:TemplateField>
    </Columns>
</gaia:GridView>
Hint: Click the text to edit directly.
<gaia:Panel 
    runat="server" 
    ID="updateControl"
    CssClass="container-updatecontrol">
        <img runat="server" style="float:right;" alt="Update Control" src="~/media/contentImages/ajax-loader.gif" />
</gaia:Panel>
<asp:ObjectDataSource
    ID="zObjectDataSource"
    TypeName="Gaia.WebWidgets.Samples.BasicControls.GridView.RichInlineEditing.Default"
    runat="server" 
    SelectMethod="ListAsDataTable">
</asp:ObjectDataSource>
</asp:Content>
