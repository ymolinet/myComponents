namespace Gaia.WebWidgets.Samples.Core.Manager.IncludeFiles
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkBtn_Click(object sender, EventArgs e)
        {
            var javaScriptUrl = Page.ResolveUrl("~/Core/Manager/IncludeFiles/CustomJavaScript.js");

            Gaia.WebWidgets.Manager.Instance.AddInclusionOfFile(
                javaScriptUrl,
                typeof (Default), "CustomJavaScript.js",
                "myCustomClass.loaded");

            Label1.Text = "CustomJavaScript.js was loaded and ready for use";

            lnkBtnInvokeJsFunction.Visible = true;
        }

        protected void lnkBtnInvokeJsFunction_Click(object sender, EventArgs e)
        {
            Gaia.WebWidgets.Manager.Instance.AddScriptForClientSideEval(
                "myCustomClass.displayAlertBox('Fired from server event. Server time: " + DateTime.Now + "');");
        }
    }
}