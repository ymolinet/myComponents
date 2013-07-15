using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace myComponents.Utilities
{
    public class FTPclient
    {
        // Fields
        private string _currentDirectory;
        private string _hostname;
        private string _lastDirectory;
        private string _password;
        private string _username;

        // Methods
        public FTPclient()
        {
            this._lastDirectory = "";
            this._currentDirectory = "/";
        }

        public FTPclient(string Hostname)
        {
            this._lastDirectory = "";
            this._currentDirectory = "/";
            this._hostname = Hostname;
        }

        public FTPclient(string Hostname, string Username, string Password)
        {
            this._lastDirectory = "";
            this._currentDirectory = "/";
            this._hostname = Hostname;
            this._username = Username;
            this._password = Password;
        }

        private string AdjustDir(string path)
        {
            return ((path.StartsWith("/") ? "" : "/").ToString() + path);
        }

        public bool Download(FTPfileInfo file, FileInfo localFI, bool PermitOverwrite)
        {
            return this.Download(file.FullName, localFI, PermitOverwrite);
        }

        public bool Download(FTPfileInfo file, string localFilename, bool PermitOverwrite)
        {
            return this.Download(file.FullName, localFilename, PermitOverwrite);
        }

        public bool Download(string sourceFilename, FileInfo targetFI, bool PermitOverwrite)
        {
            string str;
            if (!(!targetFI.Exists || PermitOverwrite))
            {
                throw new ApplicationException("Target file already exists");
            }
            if (sourceFilename.Trim() == "")
            {
                throw new ApplicationException("File not specified");
            }
            if (sourceFilename.Contains("/"))
            {
                str = this.AdjustDir(sourceFilename);
            }
            else
            {
                str = this.CurrentDirectory + sourceFilename;
            }
            string uRI = this.Hostname + str;
            FtpWebRequest request = this.GetRequest(uRI);
            request.Method = "RETR";
            request.UseBinary = true;
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    FileStream stream2 = targetFI.OpenWrite();
                    try
                    {
                        byte[] buffer = new byte[0x800];
                        int count = 0;
                        do
                        {
                            count = stream.Read(buffer, 0, buffer.Length);
                            stream2.Write(buffer, 0, count);
                        }
                        while (count != 0);
                        stream.Close();
                        stream2.Flush();
                        stream2.Close();
                    }
                    catch (Exception)
                    {
                        stream2.Close();
                        targetFI.Delete();
                        throw;
                    }
                    finally
                    {
                        if (stream2 != null)
                        {
                            stream2.Dispose();
                        }
                    }
                    stream.Close();
                }
                response.Close();
            }
            return true;
        }

        public bool Download(string sourceFilename, string localFilename, bool PermitOverwrite)
        {
            FileInfo targetFI = new FileInfo(localFilename);
            return this.Download(sourceFilename, targetFI, PermitOverwrite);
        }

        public bool FtpCreateDirectory(string dirpath)
        {
            string uRI = this.Hostname + this.AdjustDir(dirpath);
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "MKD";
            try
            {
                string stringResponse = this.GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool FtpDelete(string filename)
        {
            string uRI = this.Hostname + this.GetFullPath(filename);
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "DELE";
            try
            {
                string stringResponse = this.GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool FtpDeleteDirectory(string dirpath)
        {
            string uRI = this.Hostname + this.AdjustDir(dirpath);
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "RMD";
            try
            {
                string stringResponse = this.GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool FtpFileExists(string filename)
        {
            bool flag;
            try
            {
                long fileSize = this.GetFileSize(filename);
                flag = true;
            }
            catch (Exception exception)
            {
                if (!(exception is WebException))
                {
                    throw;
                }
                if (!exception.Message.Contains("550"))
                {
                    throw;
                }
                return false;
            }
            return flag;
        }

        public bool FtpRename(string sourceFilename, string newName)
        {
            string fullPath = this.GetFullPath(sourceFilename);
            if (!this.FtpFileExists(fullPath))
            {
                throw new FileNotFoundException("File " + fullPath + " not found");
            }
            string filename = this.GetFullPath(newName);
            if (filename == fullPath)
            {
                throw new ApplicationException("Source and target are the same");
            }
            if (this.FtpFileExists(filename))
            {
                throw new ApplicationException("Target file " + filename + " already exists");
            }
            string uRI = this.Hostname + fullPath;
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "RENAME";
            ftp.RenameTo = filename;
            try
            {
                string stringResponse = this.GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private ICredentials GetCredentials()
        {
            return new NetworkCredential(this.Username, this.Password);
        }

        private string GetDirectory(string directory)
        {
            string str;
            if (directory == "")
            {
                str = this.Hostname + this.CurrentDirectory;
                this._lastDirectory = this.CurrentDirectory;
                return str;
            }
            if (!directory.StartsWith("/"))
            {
                throw new ApplicationException("Directory should start with /");
            }
            str = this.Hostname + directory;
            this._lastDirectory = directory;
            return str;
        }

        public long GetFileSize(string filename)
        {
            string str;
            if (filename.Contains("/"))
            {
                str = this.AdjustDir(filename);
            }
            else
            {
                str = this.CurrentDirectory + filename;
            }
            string uRI = this.Hostname + str;
            FtpWebRequest ftp = this.GetRequest(uRI);
            ftp.Method = "SIZE";
            string stringResponse = this.GetStringResponse(ftp);
            return this.GetSize(ftp);
        }

        private string GetFullPath(string file)
        {
            if (file.Contains("/"))
            {
                return this.AdjustDir(file);
            }
            return (this.CurrentDirectory + file);
        }

        private FtpWebRequest GetRequest(string URI)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(URI);
            request.Credentials = this.GetCredentials();
            request.KeepAlive = false;
            return request;
        }

        private long GetSize(FtpWebRequest ftp)
        {
            long contentLength;
            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                contentLength = response.ContentLength;
                response.Close();
            }
            return contentLength;
        }

        private string GetStringResponse(FtpWebRequest ftp)
        {
            string str = "";
            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                long contentLength = response.ContentLength;
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        str = reader.ReadToEnd();
                        reader.Close();
                    }
                    stream.Close();
                }
                response.Close();
            }
            return str;
        }

        public List<string> ListDirectory(string directory)
        {
            FtpWebRequest ftp = this.GetRequest(this.GetDirectory(directory));
            ftp.Method = "NLST";
            string str = this.GetStringResponse(ftp).Replace("\r\n", "\r").TrimEnd(new char[] { '\r' });
            List<string> list = new List<string>();
            list.AddRange(str.Split(new char[] { '\r' }));
            return list;
        }

        public FTPdirectory ListDirectoryDetail(string directory)
        {
            FtpWebRequest ftp = this.GetRequest(this.GetDirectory(directory));
            ftp.Method = "LIST";
            return new FTPdirectory(this.GetStringResponse(ftp).Replace("\r\n", "\r").TrimEnd(new char[] { '\r' }), this._lastDirectory);
        }

        public bool Upload(FileInfo fi, string targetFilename)
        {
            string str;
            if (targetFilename.Trim() == "")
            {
                str = this.CurrentDirectory + fi.Name;
            }
            else if (targetFilename.Contains("/"))
            {
                str = this.AdjustDir(targetFilename);
            }
            else
            {
                str = this.CurrentDirectory + targetFilename;
            }
            string uRI = this.Hostname + str;
            FtpWebRequest request = this.GetRequest(uRI);
            request.Method = "STOR";
            request.UseBinary = true;
            request.ContentLength = fi.Length;
            byte[] buffer = new byte[0x800];
            using (FileStream stream = fi.OpenRead())
            {
                try
                {
                    using (Stream stream2 = request.GetRequestStream())
                    {
                        int num;
                        do
                        {
                            num = stream.Read(buffer, 0, 0x800);
                            stream2.Write(buffer, 0, num);
                        }
                        while (num >= 0x800);
                        stream2.Close();
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    stream.Close();
                }
            }
            request = null;
            return true;
        }

        public bool Upload(string localFilename, string targetFilename)
        {
            if (!File.Exists(localFilename))
            {
                throw new ApplicationException("File " + localFilename + " not found");
            }
            FileInfo fi = new FileInfo(localFilename);
            return this.Upload(fi, targetFilename);
        }

        // Properties
        public string CurrentDirectory
        {
            get
            {
                return (this._currentDirectory + (this._currentDirectory.EndsWith("/") ? "" : "/").ToString());
            }
            set
            {
                if (!value.StartsWith("/"))
                {
                    throw new ApplicationException("Directory should start with /");
                }
                this._currentDirectory = value;
            }
        }

        public string Hostname
        {
            get
            {
                if (this._hostname.StartsWith("ftp://"))
                {
                    return this._hostname;
                }
                return ("ftp://" + this._hostname);
            }
            set
            {
                this._hostname = value;
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        public string Username
        {
            get
            {
                return ((this._username == "") ? "anonymous" : this._username);
            }
            set
            {
                this._username = value;
            }
        }
    }

    public class FTPdirectory : List<FTPfileInfo>
    {
        // Fields
        private const char slash = '/';

        // Methods
        public FTPdirectory()
        {
        }

        public FTPdirectory(string dir, string path)
        {
            foreach (string str in dir.Replace("\n", "").Split(new char[] { Convert.ToChar('\r') }))
            {
                if (str != "")
                {
                    base.Add(new FTPfileInfo(str, path));
                }
            }
        }

        public bool FileExists(string filename)
        {
            foreach (FTPfileInfo info in this)
            {
                if (info.Filename == filename)
                {
                    return true;
                }
            }
            return false;
        }

        public FTPdirectory GetDirectories()
        {
            return this.GetFileOrDir(FTPfileInfo.DirectoryEntryTypes.Directory, "");
        }

        private FTPdirectory GetFileOrDir(FTPfileInfo.DirectoryEntryTypes type, string ext)
        {
            FTPdirectory pdirectory = new FTPdirectory();
            foreach (FTPfileInfo info in this)
            {
                if (info.FileType == type)
                {
                    if (ext == "")
                    {
                        pdirectory.Add(info);
                    }
                    else if (ext == info.Extension)
                    {
                        pdirectory.Add(info);
                    }
                }
            }
            return pdirectory;
        }

        public FTPdirectory GetFiles(string ext)
        {
            return this.GetFileOrDir(FTPfileInfo.DirectoryEntryTypes.File, ext);
        }

        public static string GetParentDirectory(string dir)
        {
            string str = dir.TrimEnd(new char[] { '/' });
            int num = str.LastIndexOf('/');
            if (num <= 0)
            {
                throw new ApplicationException("No parent for root");
            }
            return str.Substring(0, num - 1);
        }
    }

    public class FTPfileInfo
    {
        // Fields
        private DateTime _fileDateTime;
        private string _filename;
        private DirectoryEntryTypes _fileType;
        private static string[] _ParseFormats = new string[] { @"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\w+\s+\w+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{4})\s+(?<name>.+)", @"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{4})\s+(?<name>.+)", @"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{1,2}:\d{2})\s+(?<name>.+)", @"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\w+\s+\w+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{1,2}:\d{2})\s+(?<name>.+)", @"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})(\s+)(?<size>(\d+))(\s+)(?<ctbit>(\w+\s\w+))(\s+)(?<size2>(\d+))\s+(?<timestamp>\w+\s+\d+\s+\d{2}:\d{2})\s+(?<name>.+)", @"(?<timestamp>\d{2}\-\d{2}\-\d{2}\s+\d{2}:\d{2}[Aa|Pp][mM])\s+(?<dir>\<\w+\>){0,1}(?<size>\d+){0,1}\s+(?<name>.+)" };
        private string _path;
        private string _permission;
        private long _size;

        // Methods
        public FTPfileInfo(string line, string path)
        {
            Match matchingRegex = this.GetMatchingRegex(line);
            if (matchingRegex == null)
            {
                throw new ApplicationException("Unable to parse line: " + line);
            }
            this._filename = matchingRegex.Groups["name"].Value;
            this._path = path;
            long.TryParse(matchingRegex.Groups["size"].Value, out this._size);
            this._permission = matchingRegex.Groups["permission"].Value;
            string str = matchingRegex.Groups["dir"].Value;
            if ((str != "") && (str != "-"))
            {
                this._fileType = DirectoryEntryTypes.Directory;
            }
            else
            {
                this._fileType = DirectoryEntryTypes.File;
            }
            try
            {
                this._fileDateTime = DateTime.Parse(matchingRegex.Groups["timestamp"].Value);
            }
            catch (Exception)
            {
                this._fileDateTime = Convert.ToDateTime((string)null);
            }
        }

        private Match GetMatchingRegex(string line)
        {
            for (int i = 0; i <= (_ParseFormats.Length - 1); i++)
            {
                Match match = new Regex(_ParseFormats[i]).Match(line);
                if (match.Success)
                {
                    return match;
                }
            }
            return null;
        }

        // Properties
        public string Extension
        {
            get
            {
                int num = this.Filename.LastIndexOf(".");
                if ((num >= 0) && (num < (this.Filename.Length - 1)))
                {
                    return this.Filename.Substring(num + 1);
                }
                return "";
            }
        }

        public DateTime FileDateTime
        {
            get
            {
                return this._fileDateTime;
            }
        }

        public string Filename
        {
            get
            {
                return this._filename;
            }
        }

        public DirectoryEntryTypes FileType
        {
            get
            {
                return this._fileType;
            }
        }

        public string FullName
        {
            get
            {
                return (this.Path + this.Filename);
            }
        }

        public string NameOnly
        {
            get
            {
                int length = this.Filename.LastIndexOf(".");
                if (length > 0)
                {
                    return this.Filename.Substring(0, length);
                }
                return this.Filename;
            }
        }

        public string Path
        {
            get
            {
                return this._path;
            }
        }

        public string Permission
        {
            get
            {
                return this._permission;
            }
        }

        public long Size
        {
            get
            {
                return this._size;
            }
        }

        // Nested Types
        public enum DirectoryEntryTypes
        {
            File,
            Directory
        }
    }

    public class FTPTools
    {
        public static Boolean SendFTPFile(String FTPServer, String FTPUsername, String FTPPassword, String Filename)
        {
            string filename = Filename.Substring(Filename.LastIndexOf(@"\") + 1);
            FTPclient pclient = new FTPclient(FTPServer, FTPUsername, FTPPassword);
            if (pclient.FtpFileExists(filename))
            {
                pclient.FtpDelete(filename);
            }
            return pclient.Upload(Filename, filename);

        }
    }
}
