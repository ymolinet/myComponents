<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.Panel.Overview.Default"
    Title="Gaia Ajax: Panel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.Panel" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:Panel ID="zPanel" runat="server" Height="100px" Width="200px"
        BorderStyle="Dotted" BorderWidth="1">
        Panel content
    </gaia:Panel>
    <br />
    <div>
        <div class="property-container">
            <div class="property-name">
                Border Style</div>
            <div class="property-controlpanel">
                <gaia:DropDownList ID="zBorderStyle" runat="server" OnInit="zBorderStyle_OnInit"
                    AutoPostBack="True" OnSelectedIndexChanged="zBorderStyle_SelectedIndexChanged">
                </gaia:DropDownList>
            </div>
        </div>
        <div class="property-container">
            <div class="property-name">
                Border Width</div>
            <div class="property-controlpanel">
                <gaia:DropDownList ID="zBorderWidth" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="zBorderWidth_SelectedIndexChanged">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                </gaia:DropDownList>
            </div>
        </div>
        <div class="property-container">
            <div class="property-name">
                Visibility</div>
            <div class="property-controlpanel">
                <gaia:CheckBox ID="zVisibility" runat="server" AutoPostBack="true"
                    Checked="true" OnCheckedChanged="zVisibility_OnCheckedChanged" />
            </div>
        </div>
        <div class="property-container">
            <div class="property-name">
                Back Color:</div>
            <div class="property-controlpanel">
                <div>
                    <span class="span-red">Red (<gaia:Label ID="zLabelRed" runat="server"
                        Text="255" />)</span>
                    <gaia:Slider ID="zRed" runat="server" Width="150px" Value="100"
                        OnValueChanged="zRed_ValueChanged" />
                </div>
                <div>
                    <span class="span-green">Green (<gaia:Label ID="zLabelGreen"
                        runat="server" Text="255" />):</span>
                    <gaia:Slider ID="zGreen" runat="server" Width="150px" Value="100"
                        OnValueChanged="zGreen_ValueChanged" />
                </div>
                <div>
                    <span class="span-blue">Blue (<gaia:Label ID="zLabelBlue" runat="server"
                        Text="255" />):</span>
                    <gaia:Slider ID="zBlue" runat="server" Width="150px" Value="100"
                        OnValueChanged="zBlue_ValueChanged" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
