using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace myComponents.Utilities
{
    public class EventManager
    {
        // Fields
        private StreamWriter _aFile;
        private bool _autoflush = false;
        private bool _consoleOutput = true;
        private bool _FileIsClose = true;
        private FileMode _mode;
        private string _title = string.Empty;
        private string _txtfile = string.Empty;
        private bool _txtfileOutput = false;

        // Methods
        public EventManager(bool OutToConsole, bool OutToTXTFile, bool AutoFlush)
        {
            this._consoleOutput = OutToConsole;
            this._txtfileOutput = OutToTXTFile;
            this._autoflush = AutoFlush;
        }

        public void CloseFile()
        {
            this._aFile.Flush();
            this._aFile.Close();
            this._aFile.Dispose();
            this._aFile = null;
            this._FileIsClose = true;
        }

        public void setFile(string pathtofile, FileMode mode)
        {
            if (((mode != FileMode.Append) && (mode != FileMode.Create)) && (mode != FileMode.CreateNew))
            {
                throw new Exception("Ce type System.IO.FileMode (" + mode.ToString() + ") n'est pas pris en charge!");
            }
            this._txtfile = pathtofile;
            this._mode = mode;
            if (File.Exists(this._txtfile) && (this._mode == FileMode.Create))
            {
                File.Delete(this._txtfile);
            }
            StreamWriter writer = null;
            if (this._mode == FileMode.Create)
            {
                writer = new StreamWriter(this._txtfile, false);
            }
            if (this._mode == FileMode.Append)
            {
                writer = new StreamWriter(this._txtfile, true);
            }
            if (this._mode == FileMode.CreateNew)
            {
                if (File.Exists(this._txtfile))
                {
                    File.Delete(this._txtfile);
                }
                writer = new StreamWriter(this._txtfile);
            }
            if (writer == null)
            {
                throw new Exception("Une erreur s'est produite lors de la cr\x00e9ation du fichier de logs!");
            }
            this._aFile = writer;
            this._FileIsClose = false;
        }

        public void Write(string txttowrite)
        {
            this.Write(txttowrite, false);
        }

        public void Write(string txttowrite, bool nouvelLigne)
        {
            if (this._consoleOutput)
            {
                Console.Title = this._title;
                if (nouvelLigne)
                {
                    Console.WriteLine(txttowrite);
                }
                else
                {
                    Console.Write(txttowrite);
                }
            }
            if (this._txtfileOutput && !this._FileIsClose)
            {
                if (string.IsNullOrEmpty(this._txtfile))
                {
                    throw new Exception("Vous devez sp\x00e9cifier un fichier texte \x00e0 \x00e9crire !");
                }
                if (nouvelLigne)
                {
                    this._aFile.WriteLine(txttowrite);
                }
                else
                {
                    this._aFile.Write(txttowrite);
                }
                if (this._autoflush)
                {
                    this._aFile.Flush();
                }
            }
        }

        public void WriteLine(string txttowrite)
        {
            this.Write(txttowrite, true);
        }

        // Properties
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }
    }




}
