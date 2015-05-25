namespace Gaia.WebWidgets.Samples.Aspects.AspectKey.Overview
{
    using System;
    using System.Collections.Generic;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                txt.Focus();

            BindKeys();
        }

        private void BindKeys()
        {
            var keyCodes = new Gaia.WebWidgets.AspectKey.KeyCode[]
                                               {
                                                   Gaia.WebWidgets.AspectKey.KeyCode.ESC,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.TAB,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.SPACE,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.RETURN,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.BACKSPACE,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.SCROLL,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.SCROLL,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.CAPSLOCK,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.NUMLOCK,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.PAUSE,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.INSERT,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.HOME,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.DELETE,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.END,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.PAGEUP,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.PAGEDOWN,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.LEFT,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.UP,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.RIGHT,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.DOWN,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F1,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F2,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F3,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F4,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F5,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F6,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F7,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F8,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F9,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F10,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F11,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F12,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.D0,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.D1,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.D2,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.D3,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.D4,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.D5,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.D6,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.D7,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.D8,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.D9,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.A,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.B,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.C,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.D,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.E,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.F,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.G,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.H,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.I,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.J,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.K,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.L,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.M,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.N,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.O,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.P,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.Q,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.R,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.S,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.T,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.U,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.V,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.W,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.X,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.Y,
                                                   Gaia.WebWidgets.AspectKey.KeyCode.Z
                                               };

            txt.Aspects.Add(AddKeyBindingToControl(keyCodes));
        }

        private IAspect AddKeyBindingToControl(IEnumerable<Gaia.WebWidgets.AspectKey.KeyCode> keyCodes)
        {
            var aspectKey = new Gaia.WebWidgets.AspectKey();
            foreach (Gaia.WebWidgets.AspectKey.KeyCode code in keyCodes)
                aspectKey.AddFilter(new Gaia.WebWidgets.AspectKey.KeyFilter(code, false, false, false, code == WebWidgets.AspectKey.KeyCode.ESC));

            aspectKey.KeyDown += aspectKey_KeyDown;

            return aspectKey;
        }

        void aspectKey_KeyDown(object sender, Gaia.WebWidgets.AspectKey.KeyEventArgs e)
        {
            var content = (System.Web.UI.WebControls.ContentPlaceHolder)Master.FindControl("p");
            //Get control for the KeyCode specified
            var control = content.FindControl(e.Key.KeyCode.ToString()) as Panel ??
                            content.FindControl("Panel" + e.Key.KeyCode) as Panel;

            if (control == null)
                return;

            control.Effects.Add(new Gaia.WebWidgets.Effects.EffectHighlight(WebUtility.GetRandomColor()));
        }

        protected void zFocusBack_Click(object sender, EventArgs e)
        {
            txt.Focus(); 
        }
    }
}