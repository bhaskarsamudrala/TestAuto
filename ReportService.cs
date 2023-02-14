using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using Company.Platform.automation.App.web.Utils;

namespace Company.Platform.automation.App.web.ReportsHelper
{
    public class ReportService
    {
        private static readonly Lazy<ExtentReports> Lazy = new Lazy<ExtentReports>(() => new ExtentReports());

        static ReportService()
        {
            var projectPath = DirUtils.GetProjectReportsDirectory();
            var configPath = DirUtils.GetProjectDirectory();
            if (!Directory.Exists(projectPath))
            {
                Directory.CreateDirectory(projectPath);
            }
            var reportPath = projectPath + "\\" + "index.html";
            var configFile = configPath + "report-config.xml";
            var reporter = new ExtentHtmlReporter(reportPath)
            {
                Config =
                {
                    Theme = Theme.Standard
                }
            };
            Instance.AttachReporter(reporter);
            reporter.LoadConfig(configFile);
        }

        private ReportService()
        {
        }

        public static ExtentReports Instance => Lazy.Value;
    }
}