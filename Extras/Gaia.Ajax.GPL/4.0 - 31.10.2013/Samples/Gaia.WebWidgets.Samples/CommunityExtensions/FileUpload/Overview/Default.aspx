<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.CommunityExtensions.FileUpload.Overview.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Gaia Ajax: Multiple FileUpload Control</h1>
<p>The FileUpload control allows you to upload multiple files with ease in ajax style way.</p>
<p>The control is highly customizable with Custom Error Messages, Localizable, Custom Image Loader
Icon and events for when each file is uploaded and when they are all uploaded
</p>
<br />
<p><span style="color:red">Note: Because the Image is uploaded in the background using an IFrame 
you don't have access to the Page i the FileUploaded event handler. However when all Images are 
fully uploaded, the UploadFinished event is fired and here you can access the page and your Gaia
controls the way you are used to. 
</span></p>
<br />
<p>Notice how we populate a temporary storage (in this case the session) 
to fully know which images were uploaded in the last go. It would be easy to use another mechanism 
to render the images. For example by looking at the control collection and only add images which 
were not you added to the page. If you need to continuously display the images you could have a
Gaia Timer poll the server for new changes and perform the control insertions. 
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

<h2>Please select the Images you want to upload (maximum 5 at a time) and then click Save File(s)</h2>
<br />
 <gaia:FileUpload 
        id="fileUpload" 
        runat="server"
        CssClass="gaiax"
        Width="100%"
        OnFileUploaded="fileUpload_OnFileUploaded"
        OnUploadFinished="fileUpload_OnUploadFinished" 
        ImgLoadingSrc="file-loader.gif" 
        MakeCallback="true" 
        UploadOnce="false"
        MaxFiles="5" 
        TextBtnUpload="Save File(s)" />


<gaia:Panel ID="zResults" runat="server" CssClass="results">

</gaia:Panel>

</asp:Content>
