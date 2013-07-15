using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace myComponents.Utilities
{
    public static class DBTools
    {
        public static void CompactSQL(String ConnectionString, String BaseFileName, String LogFileName)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(string.Format("DBCC SHRINKDATABASE({0});", connection.Database));
            command.CommandTimeout = 0;
            command.Connection = connection;
            command.ExecuteNonQuery();
            string cmdText = "DBCC SHRINKFILE(" + BaseFileName + ");";
            command = new SqlCommand(cmdText);
            command.CommandTimeout = 0;
            command.Connection = connection;
            command.ExecuteNonQuery();
            cmdText = "DBCC SHRINKFILE(" + LogFileName + ");";
            command = new SqlCommand(cmdText);
            command.CommandTimeout = 0;
            command.Connection = connection;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void CompressFile(string FileToZip, string ZipFileName)
        {
            if (!File.Exists(FileToZip))
            {
                throw new FileNotFoundException("Fichier " + FileToZip + " non trouv\x00e9 !");
            }
            if (File.Exists(ZipFileName))
            {
                File.Delete(ZipFileName);
            }
            Ionic.Zip.ZipFile file = new Ionic.Zip.ZipFile(ZipFileName);
            file.AddFile(FileToZip);
            file.Save();
        }

        public static void BackupSQL(String ConnectionString, String BackupFileName)
        {
            if (string.IsNullOrEmpty(BackupFileName))
            {
                throw new Exception("La sauvegarde n'a pas aboutie");
            }
            if (File.Exists(BackupFileName))
            {
                File.Delete(BackupFileName);
            }
            SqlConnection connection = new SqlConnection(ConnectionString);
            string cmdText = string.Format("BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH NOFORMAT, NOINIT,  NAME = N'ContactHotel', SKIP, NOREWIND, NOUNLOAD,  STATS = 10", connection.Database, BackupFileName);
            connection.Open();
            SqlCommand command = new SqlCommand(cmdText);
            command.CommandTimeout = 0;
            command.Connection = connection;
            command.ExecuteNonQuery();
            connection.Close();

        }
    }
}
