<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Core.Manager.CustomErrors.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Custom Error Handling ClientSide</h1>
<p>If you don't like the built-in Exception handling to Gaia Ajax you can override 
it with a custom function. Just follow the code outlined in this example to provide
a custom clientside function to handle the error. In this example we just use a few
alerts to display the errors
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

 <script type="text/javascript">
        function errorHandler(status, statusText, fullTrace){
          alert(status);
          alert(statusText);
          alert(fullTrace);
        }
    </script>

 <gaia:button 
        ID="zButton" 
        runat="server" 
        OnClick="Button_Click" 
        Text="Intentionally throw an exception to see custom Error Handling" />

</asp:Content>
