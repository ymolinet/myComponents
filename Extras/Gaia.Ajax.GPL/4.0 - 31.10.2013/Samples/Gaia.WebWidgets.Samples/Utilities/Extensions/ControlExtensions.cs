namespace Gaia.WebWidgets.Samples.Utilities.Extensions
{
    using System.Web.UI;
    using System.Collections.Generic;

    public static class ControlExtensions
    {
        public static IEnumerable<Control> All(this ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control.HasControls())
                {
                    foreach (var grandchild in control.Controls.All())
                        yield return grandchild;
                }

                yield return control;
            }
        }
    }
}