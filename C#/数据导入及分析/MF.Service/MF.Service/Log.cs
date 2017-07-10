using System;
using System.IO;
using System.Configuration;
using System.Text;

namespace MF.Service
{
    public class Log
    {

        private static object Obj = new object();
        private static FileSystemWatcher WATCHER;

        static Log()
        {
            WATCHER = new FileSystemWatcher(Folder);
            WATCHER.EnableRaisingEvents = true;
            WATCHER.Created += (sender, e)=> {
                lock (Obj)
                {
                    var obj = sender as FileSystemWatcher;
                    var date = File.GetCreationTime(e.FullPath);
                    var ndate = date.Date.AddDays(-30);
                    DirectoryInfo directory = new DirectoryInfo(obj.Path);
                    var files = directory.GetFiles();
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i].CreationTime <= ndate)
                        {
                            files[i].Delete();
                        }
                    }
                }
            };
        }

        private static string Folder
        {
            get
            {
                var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log\\");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        private static void Write(string log,string value)
        {
            var path = Path.Combine(Folder, string.Format("{0}{1}", DateTime.Now.ToString("yyyy-MM-dd"), log));

            using (var writer = File.AppendText(path))
            {
                writer.WriteLine(string.Format("{0}：{1}", DateTime.Now.ToString("HH:mm:ss "), value));
            }
        }

        private static void Write(string log, params object[] arry)
        {
            var sb = new StringBuilder();
            foreach (var message in arry)
            {
                sb.Append(message);
            }
            Write(log, sb.ToString());
        }

        public static void WriteError(params object[] arry)
        {
            Write(".error", arry);
        }

        public static void WriteError(string message,Exception ex)
        {
            var log = string.Format(message," ","Exception Message：",ex.Message, "StackTrace：", ex.StackTrace);
            WriteError(log, string.Empty);
        }

        public static void WriteError(Exception ex)
        {
            var log = string.Format("Exception Message：", ex.Message, "StackTrace：", ex.StackTrace);
            WriteError(log, string.Empty);
        }

        public static void WriteLog(params string[] arry)
        {
            Write(".log", arry);
        }
    }
}
