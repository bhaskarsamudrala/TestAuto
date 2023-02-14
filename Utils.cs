using System;
using System.IO;
using System.Reflection;

namespace Company.Platform.automation.App.web.Utils
{
    public class DirUtils
    {
        public static string GetProjectDirectory()
        {
            var path = Assembly.GetCallingAssembly().CodeBase;
            var actualPath = path.Substring(0, path.LastIndexOf("bin"));
            var projectPath = new Uri(actualPath).LocalPath;
            return projectPath;
        }
        public static string GetProjectReportsDirectory()
        {
            var path = Assembly.GetCallingAssembly().CodeBase;
            var actualPath = path.Substring(0, path.LastIndexOf("bin"));
            var projectPath = new Uri(actualPath).LocalPath;
            return projectPath + "Reports";
        }

        public static void CleanDirectory(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    var fileNames = Directory.GetFiles(directoryPath);
                    foreach (var fileName in fileNames) File.Delete(fileName);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Exception:" + exception);
            }
        }
    }
}