<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gaia.WebWidgets.Samples.Core.Manager.PageMethods.Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1>Invoking PageMethods</h1>
<p>There are very few cases where you need to write javascript, but if you really have to 
for some reason you can utilize the PageMethods to access the server and invoke functions
on the Page. Just decorate the function with the [Method] attribute and you can use the 
syntax outlined in the code to invoke these server side functions. 
</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">

 <script type="text/javascript">

    function afterwards(t){
      jQuery('#div1').html('Return value from method was: ' + t);
    }

    function callMethod(){
      Gaia.Control.callPageMethod('PageAjaxMethod', 
      ['ClientValue', jQuery('#number1').val(), true], afterwards);
    }

</script>

<gaia:Label 
    ID="zLabel" 
    runat="server">
    This is an example of the Control.callMethod to invoke any method on the Web Page.
</gaia:Label>

<div id="div1">
    Check the text of this changing when pressing the button
</div>

<input type="text" value="Type in something here" id="number1" />
<input value="Push me to invoke Page method" id="inputButton" type="button" onclick="callMethod();" />

</asp:Content>
