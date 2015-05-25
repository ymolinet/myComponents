<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.Validators.CompareValidator.Default"
    Title="Gaia Ajax: Compare Validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <h1>
        Gaia Ajax: Compare Validator</h1>
    <p>
        The Gaia Ajax Compare Validator compares the value entered by
        the user in an input control with the value entered in another
        input control, or with a constant value. It inherits from the
        <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.comparevalidator.aspx">
            ASP.NET CompareValidator</a>, and has built-in Ajax behaviour.</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    Enter a value in the two textboxes. Select a comparison operator
    and a data type. Click "Validate" to compare values.<br />
    <br />
    <div>
        <div class="verticalStack">
            String 1:<br />
            <gaia:TextBox ID="zCompare1" runat="server" CssClass="input-text" />
        </div>
        <div class="verticalStack">
            Comparison Operator:<br />
            <gaia:DropDownList ID="zOperator" runat="server" OnSelectedIndexChanged="zOperator_SelectedIndexChanged">
                <asp:ListItem Selected="true" Value="Equal">Equal</asp:ListItem>
                <asp:ListItem Value="NotEqual">NotEqual</asp:ListItem>
                <asp:ListItem Value="GreaterThan">GreaterThan</asp:ListItem>
                <asp:ListItem Value="GreaterThanEqual">GreaterThanEqual</asp:ListItem>
                <asp:ListItem Value="LessThan">LessThan</asp:ListItem>
                <asp:ListItem Value="LessThanEqual">LessThanEqual</asp:ListItem>
                <asp:ListItem Value="DataTypeCheck">DataTypeCheck</asp:ListItem>
            </gaia:DropDownList>
        </div>
        <div class="verticalStack">
            String 2:<br />
            <gaia:TextBox ID="zCompare2" runat="server" CssClass="input-text" />
        </div>
        <div class="verticalStack">
            Data Type:<br />
            <gaia:DropDownList ID="zDataType" runat="server" OnSelectedIndexChanged="zDataType_SelectedIndexChanged">
                <asp:ListItem Selected="true" Value="String">String</asp:ListItem>
                <asp:ListItem Value="Integer">Integer</asp:ListItem>
                <asp:ListItem Value="Double">Double</asp:ListItem>
                <asp:ListItem Value="Date">Date</asp:ListItem>
                <asp:ListItem Value="Currency">Currency</asp:ListItem>
            </gaia:DropDownList>
        </div>
        <div class="verticalStack">
            <gaia:Button ID="zValidate" runat="server" Text="--- Validate ---"
                OnClick="zValidate_Click" CssClass="input-submit" />
        </div>
        <div class="verticalStack">
            <gaia:Label ID="zResult" runat="server" CssClass="resultSpan" />
        </div>
    </div>
    <gaia:CompareValidator ID="zCompareValidator1" runat="server"
        ControlToCompare="zCompare2" ControlToValidate="zCompare1"
        EnableClientScript="False" Type="String" />
</asp:Content>
