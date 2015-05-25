namespace Gaia.WebWidgets.Samples.Utilities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class SampleSearchController
    {
        // workaround for bug described here: http://tracker.gaiaware.net/Default.aspx?p=1&i=511
        private const string GreaterThanSeparator = " > ";
        private const string DefaultPageName = "Default.aspx";
        private const string SampleSearchApplicationKeyName = "sampleSearch";

        /// <summary>
        /// Return absolute path and title
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, string>> Find(string query)
        {
            return Find(query, int.MaxValue);
        }

        /// <summary>
        /// Return absolute path and title
        /// </summary>
        /// <param name="query"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, string>> Find(string query, int maxResults)
        {
            query = query.ToLower();

            var results = (from pair in FindByTitleOrFileName(query)
                           select new KeyValuePair<string, string>(pair.Key.AbsolutePath, pair.Value)).Take(maxResults).
                ToList();

            if (results.Count() < maxResults)
                results.AddRange(FindBySampleFilesContent(query).Take(maxResults));

            return results.Distinct().Take(maxResults);
        }

        private static IEnumerable<KeyValuePair<SampleConfiguration, string>> All()
        {
            //TODO refactor SampleConfiguration, so it inherits from a navigation object
            //make it contain all files, also default.aspx / aspx.cs
            //make it possible to fully navigate it so it eliminates the SampleMenuItemBase as well.

            var cached = HttpContext.Current.Application[SampleSearchApplicationKeyName];

            if (cached == null)
            {

                var results = (from section in SampleConfiguration.GetCachedChildrenMenuItems("~/", true)
                               from subSection in section.Children
                               from sample in subSection.Children
                               let title =
                                   section.Title + GreaterThanSeparator + subSection.Title + GreaterThanSeparator + 
                                   sample.Title
                               select
                                   new KeyValuePair<SampleConfiguration, string>(
                                   new SampleConfiguration(sample.OriginalAbsolutePath + DefaultPageName), title)).ToList();

                HttpContext.Current.Application[SampleSearchApplicationKeyName] = results;
            }

            return HttpContext.Current.Application[SampleSearchApplicationKeyName] as List<KeyValuePair<SampleConfiguration, string>>; 
        }

        private static IEnumerable<KeyValuePair<SampleConfiguration, string>> FindByTitleOrFileName(string query)
        {
            return from s in All()
                    where s.Value.ToLower().Contains(query) || s.Key.FileName.ToLower().Contains(query)
                    select new KeyValuePair<SampleConfiguration, string>(s.Key, s.Value);
        }

        private static IEnumerable<KeyValuePair<string, string>> FindBySampleFilesContent(string query)
        {
            return from pair in All().OrderBy(o => o.Value.ToLower().Contains(query)).ThenBy(o => o.Key.ParentMenuItem.Parent.Index)
                   where pair.Key.SampleFiles != null
                   from file in pair.Key.SampleFiles
                   where file.FileContent.ToLower().IndexOf(query) > -1
                   select
                       new KeyValuePair<string, string>(pair.Key.AbsolutePath, pair.Value);
        }
    }
}