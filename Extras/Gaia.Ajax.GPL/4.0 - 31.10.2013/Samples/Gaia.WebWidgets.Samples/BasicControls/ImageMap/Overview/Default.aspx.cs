namespace Gaia.WebWidgets.Samples.BasicControls.ImageMap.Overview
{
    using UI;
    using System;
    using ASP = System.Web.UI.WebControls;
    

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void zScenery_Click(object sender, ASP.ImageMapEventArgs e)
        {
            ASP.WebControl answerControl = null;
            switch (e.PostBackValue)
            {
                case "sky":
                    answerControl = zSky;
                    break;
                case "forest":
                    answerControl = zForest;
                    break;
                case "water":
                    answerControl = zWater;
                    break;
            }

            if (answerControl != null && !answerControl.CssClass.Contains("answer-box-positive"))
                answerControl.CssClass += " answer-box-positive";
        }
    }
}
