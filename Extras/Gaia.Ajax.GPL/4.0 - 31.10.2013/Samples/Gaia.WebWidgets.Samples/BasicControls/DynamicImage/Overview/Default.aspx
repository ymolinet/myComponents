<%@ Page 
    Language="C#" 
    AutoEventWireup="True" 
    MasterPageFile="~/Core.master" 
    Title="Gaia Ajax: DynamicImage" 
    Inherits="Gaia.WebWidgets.Samples.BasicControls.DynamicImage.Overview.Default" 
    Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.DynamicImage" />
</asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <br style="clear:both;" />
    <div style="float:left; width: 65%; height: 50px;">
        Select Image: <br />
        <gaia:DropDownList 
            Width="240"
            runat="server" 
            ID="ddl" 
            AutoPostBack="true" 
            OnSelectedIndexChanged="DdlSelectedIndexChanged">
        </gaia:DropDownList>
        <gaia:Button ID="uploadImageButton" runat="server" Text="Upload Image" OnClick="UploadImageButtonClick" /> 
    </div>
    <div style="float:left; width: 30%; height: 50px;">
        Zoom (<gaia:Label ID="lblResult" runat="server" Text="50.0 %" />)
        <br/>
        <gaia:Slider 
            runat="server" 
            ID="slider" 
            Width="200px"
            OnValueChanged="SliderValueChanged" />
    </div>
    <br style="clear:both;" />
    <gaia:Panel 
        runat="server" 
        ID="mapWrapper" 
        Width="200px"
        Height="200px"
        style="float:left; border:solid 5px #777; overflow:hidden; position:relative;">
        <gaia:DynamicImage 
            runat="server" 
            ID="map" 
            AltText="" 
            style="cursor:move;-moz-user-select:none;"
            OnRetrieveImage="MapRetrieveImage" />
    </gaia:Panel>

    <gaia:Image 
        runat="server" 
        ID="descriptionImage" 
        style="float:left;"
        ImageUrl="resizeMe.png" 
        AlternateText="Description" />
    <br style="clear:both;" />
    <gaia:Label 
        runat="server" 
        ID="mapCoords" />
        
    <gaia:Window 
        ID="winFileUpload" 
        runat="server" 
        Caption="Upload Image" 
        OpacityWhenMoved="1"
        Closable="true"
        Visible="false"
        Maximizable="false"
        Minimizable="false"
        Resizable="false"
        Draggable="false"
        Height="300px"
        Width="300px"
        ScrollBars="Auto"
        CenterInForm="true">
    
        <gaia:FileUpload 
            id="fileUpload" 
            runat="server"
            Width="100%"
            OnFileUploaded="fileUpload_OnFileUploaded"
            OnUploadFinished="fileUpload_OnUploadFinished" 
            ImgLoadingSrc="" 
            MakeCallback="true" 
            MaxFiles="1">
            
        </gaia:FileUpload>
        
    </gaia:Window>
        
  
</asp:Content>

