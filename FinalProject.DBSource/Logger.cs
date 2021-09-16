using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DBSource
{
    public class Logger
    {
        public static void WriteLog(Exception ex)
        {
            string msg = $@" {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
            msg += $@" {ex.ToString()}";
            msg += "\r\n--------------------------------------------------------\r\n";


            string folderPath = System.Configuration.ConfigurationManager.AppSettings["logFolderPath"];
            string logPath = $"{folderPath}\\Logs.log";

            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            if (!System.IO.File.Exists(logPath))
                System.IO.File.Create(logPath);

            System.IO.File.AppendAllText(logPath, msg);
        }
    }
}
