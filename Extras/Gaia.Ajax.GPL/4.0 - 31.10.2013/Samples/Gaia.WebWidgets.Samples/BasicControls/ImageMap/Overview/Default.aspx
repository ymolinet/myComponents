<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.BasicControls.ImageMap.Overview.Default"
    Title="Gaia Ajax: ImageMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.ImageMap" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
<div class="container-sample">
    <div class="container-imagemap">
        <gaia:ImageMap ID="zScenery" runat="server" ImageUrl="~/media/collections/images/scenery/summer_lake.jpg"
            HotSpotMode="PostBack" OnClick="zScenery_Click">
            <asp:RectangleHotSpot Bottom="160" Right="350" AlternateText="Sky"
                PostBackValue="sky" />
            <asp:RectangleHotSpot Bottom="180" Right="350" AlternateText="Forest"
                Top="160" PostBackValue="forest" />
            <asp:RectangleHotSpot AlternateText="Water" Bottom="263" Right="350"
                Top="180" PostBackValue="water" />
        </gaia:ImageMap>
    </div>
    <div class="container-todays-task">
        <b>
            Today's task: Roughly click the sky, the forest and the water on the image</b>
        <gaia:Panel ID="zSky" runat="server" CssClass="answer-box answer-box-negative">
            Sky
        </gaia:Panel>
        <gaia:Panel ID="zForest" runat="server" CssClass="answer-box answer-box-negative">
            Forest
        </gaia:Panel>
        <gaia:Panel ID="zWater" runat="server" CssClass="answer-box answer-box-negative">
            Water
        </gaia:Panel>
    </div>
</div>
</asp:Content>
