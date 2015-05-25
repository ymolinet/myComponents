namespace Gaia.WebWidgets.Samples.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class FileUtility
    {
        public static FileInfo[] GetFiles(DirectoryInfo directoryInfo, string searchPatterns)
        {
            return GetFiles(directoryInfo, searchPatterns, null);
        }

        /// <summary>
        /// Get all files withing a specified directory. Filter using an array of search patters with a defined separator
        /// FileUtility.GetFiles(new DirectoryInfo(path), "*.cs,*.vb,*.css,*.js", new char[] {','})
        /// </summary>
        /// <param name="directoryInfo">Directory to search in</param>
        /// <param name="searchPatterns">Search pattern e.g.  "*.cs,*.vb,*.css,*.js"</param>
        /// <param name="searchExcludePatterns">Exclude pattern e.g. "*.designer.cs"</param>
        /// <returns></returns>
        public static FileInfo[] GetFiles(DirectoryInfo directoryInfo, string searchPatterns, string searchExcludePatterns)
        {
            var files = new List<FileInfo>();
            string[] includePatterns = searchPatterns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string includePattern in includePatterns)
                files.AddRange(directoryInfo.GetFiles(includePattern));

            //check if we should exclude some patterns
            if (!string.IsNullOrEmpty(searchExcludePatterns))
            {
                string[] excludePatterns = searchExcludePatterns.Split(new char[] {','},
                                                                       StringSplitOptions.RemoveEmptyEntries);
                foreach (string excludePattern in excludePatterns)
                    files.RemoveAll(f => f.Name.ToUpper().Contains(excludePattern.ToUpper()));
            }

            return files.ToArray();
        }

     
    }
}