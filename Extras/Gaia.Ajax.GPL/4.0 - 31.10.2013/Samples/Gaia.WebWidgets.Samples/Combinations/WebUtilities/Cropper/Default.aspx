<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Combinations.WebUtilities.Cropper.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Image Cropper Tool</h1>
<p>This example demonstrates how easily you can create an Image cropper tool with only
approx 50 lines of code in the codebehind + markup.
</p>
<br />
<p>Just move or resize the Panel and click the Crop button to have only that "slice" 
cut out and presented. The output Image will be opened in a separate Window, but you could
easily modify the original if you want that instead. 
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<gaia:Button 
        ID="zButtonCrop" 
        runat="server" 
        OnClick="zButtonCrop_Click"
        Text="Crop the selected part of the Image" />

<div id="outer">
    <div id="inner">
    <gaia:Image 
        ID="zImage"
        runat="server"
        Width="600px"
        Height="400px"
        style="position: absolute;"
        ImageUrl="crop.jpg"
        AlternateText="Crop"/>
    
    <gaia:Panel 
        ID="zPanelCrop" 
        runat="server" 
        CssClass="crop-tool"
        Width="200px"
        Height="200px"
        style="left: 0px;top: 0px;">
        <gaia:Panel 
            ID="zPanelCropHandle" 
            Width="100%"
            Height="10px"
            CssClass="crop-handle"
            runat="server" />
            
        </gaia:Panel>

    </div>       
</div>

<gaia:Window 
    ID="zWindowResult" 
    Caption="Cropped Image"
    runat="server" 
    Modal="true" 
    Minimizable="false"
    Maximizable="false"
    Draggable="true"
    Resizable="false"
    CenterInForm="true"
    OpacityWhenMoved="1"
    Visible="false">
       <gaia:DynamicImage
            id="zCroppedImage"
            runat="server"
            OnRetrieveImage="zCroppedImage_RetrieveImage" />
    </gaia:Window>

</asp:Content>
