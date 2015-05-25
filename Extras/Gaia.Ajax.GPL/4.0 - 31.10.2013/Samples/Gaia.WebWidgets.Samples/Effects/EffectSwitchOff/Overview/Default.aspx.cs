namespace Gaia.WebWidgets.Samples.Effects.EffectSwitchOff.Overview
{
    using System;
    using System.Web.UI;
    using Gaia.WebWidgets.Samples.UI;


    public partial class Default : FxSamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            timer.Tick += delegate
                              {
                                  timer.Enabled = false;
                                  imgButtonTv.Effects.Add(new Gaia.WebWidgets.Effects.EffectAppear(0.35M));
                                  zMessage.Text = "Click the TV to turn it off";
                              };
        }

        protected void imgButtonTv_Click(object sender, ImageClickEventArgs e)
        {
             //todo : switchoff can be simulated with an easing ... 
            var switchOff = new Gaia.WebWidgets.Effects.EffectSwitchOff
                                {
                                    Delay = 0.3M,
                                    Duration = 1.5M,
                                  //  Transition = Gaia.WebWidgets.Effects.ScriptaculousTransitions.Sinoidal
                                };

            imgButtonTv.Effects.Add(switchOff);
            timer.Enabled = true;
            zMessage.Text = "Time to go to sleep";
        } 
        #endregion
    }
}