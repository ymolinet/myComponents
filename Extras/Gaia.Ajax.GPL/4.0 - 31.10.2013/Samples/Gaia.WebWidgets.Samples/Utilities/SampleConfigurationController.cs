namespace Gaia.WebWidgets.Samples.Utilities
{
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class SampleConfigurationController
    {
        public static readonly Dictionary<string, Literal> SourceFiles = new Dictionary<string, Literal>();

        public static readonly Dictionary<string, SampleConfiguration> SampleConfigurationPerPage =
            new Dictionary<string, SampleConfiguration>();

        public static SampleConfiguration GetSampleConfigurationForPath(string appRelativePath)
        {
            return SampleConfigurationPerPage.ContainsKey(appRelativePath)
                       ? SampleConfigurationPerPage[appRelativePath]
                       : new SampleConfiguration(appRelativePath);
        }
    }
}
