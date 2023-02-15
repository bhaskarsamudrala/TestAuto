using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using Company.Platform.automation.App.web.Utils;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using static Company.Platform.automation.App.web.ReportsHelper.ReportService;
using static Company.Platform.automation.App.web.ReportsHelper.ReportLog;
using Company.Platform.automation.App.web.Common;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Company.Platform.automation.App.web.ReportsHelper;

namespace Company.Platform.automation.App.web.Tests
{
    public class AppStarUp
    {
        public TestContext TestContext { get; set; }

        //protected PowerApp PwrApp { get; set; }

        protected SeleniumActions SeleniumActions { get; set; }
        public static int ThinkTime => TestSettings.Options.DefaultThinkTime;
        public static Random Random = new Random();
        public static AppUrl PowerAppSelected()
        {
            return _PowerAppSelected;
        }
        private static string TestCaseName { get; set; }
        private static string TestUserId { get; set; }
        private static List<string> SecurityRoles { get; set; }
        private static Uri LoginUrl { get; set; }

        private static AppUrl _PowerAppSelected { get; set; }

        protected static ExtentReports ExtentReports;
        protected static ExtentTest TestParent;
        protected static ExtentTest ExtentTest;

        private WebClient Client;


        [TestInitialize]
        public async Task SetupAsync()
        {
            try
            {
                Client = new WebClient(TestSettings.Options);
                //PwrApp = new PowerApp(Client);
                SeleniumActions = new SeleniumActions(Client.Browser.Driver);

                TestCaseName = TestContext.TestName.Substring(4, 5);
                TestUserId = TestSettings.TestUserId;
                LoginUrl = TestSettings.PowerAppUrl;

                //Change this assignment for the different environments, after providing a URI in the switch statement
                _PowerAppSelected = AppUrl.Qa;

                switch (_PowerAppSelected)
                {
                    case AppUrl.Qa:
                        LoginUrl = new Uri("");
                        break;

                    case AppUrl.Dev:
                        LoginUrl = new Uri(" ");
                        break;

                    case AppUrl.Uat:
                        LoginUrl = new Uri(" ");
                        break;
                }

                // Go ahead and run tests cases against the chosen environment.
                //pwrApp.OnlineLogin.Login(_loginUrl);
                PwrApp.OnlineLogin.Login(LoginUrl,  TestSettings.PowerAppAdmin.UName,  TestSettings.PowerAppAdmin.PWord);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        [AssemblyInitialize]
        public static void TestAssemblyInit(TestContext testContext)
        {
            DirUtils.CleanDirectory(DirUtils.GetProjectReportsDirectory());
        }

        [AssemblyCleanup]
        public static void TestAssemblyClean()
        {
            //report flush
            Instance.Flush();
        }


        [TestCleanup]
        public virtual void TestCleanUp()
        {
            try
            {
                switch (TestContext.CurrentTestOutcome)
                {
                    case UnitTestOutcome.Error:
                        ReportLog.Log(Status.Error, "Failed - Error");
                        AddScreenShot(Client, "Failed");
                        break;
                    case UnitTestOutcome.Failed:
                        ReportLog.Log(Status.Error, "Failed");
                        AddScreenShot(Client, "Failed");
                        break;
                    case UnitTestOutcome.Passed:
                        ReportLog.Log(Status.Pass, " Passed");
                        break;
                    case UnitTestOutcome.Timeout:
                        ReportLog.Log(Status.Info, "Failed with Timeout issues");
                        AddScreenShot(Client, "Failed");
                        break;
                    case UnitTestOutcome.Inconclusive:
                        ReportLog.Log(Status.Info, "Failed - Inconclusive");
                        AddScreenShot(Client, "Failed");
                        break;
                    default:
                        ReportLog.Log(Status.Fail, "Failed - unknown");
                        AddScreenShot(Client, "Failed - unknown");
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Exception Message: " + exception);
            }
        }

        private void AddScreenShot(WebClient client, string title)
        {
            var filename = Guid.NewGuid();
            var projectPath = DirUtils.GetProjectReportsDirectory();
            var filePath = Path.Combine(projectPath, $"{filename}.png");
            client.Browser.Driver.WaitForTransaction(new TimeSpan(0, 0, 5));
            client.Browser.TakeWindowScreenShot(filePath, ScreenshotImageFormat.Png);
            var imgBase64String = GetBase64StringForImage(filePath);
            GetMethod().Info(title + "<BR>",
                MediaEntityBuilder.CreateScreenCaptureFromBase64String(imgBase64String).Build());
        }

        public void LogExceptionAndFail(Exception exception)
        {
            var message = exception.Message + Environment.NewLine + exception.StackTrace.Trim();
            var markup = MarkupHelper.CreateCodeBlock(message);
            GetMethod().Log(Status.Error, markup);
            throw exception;
        }

        public void LogException(Exception exception, WebClient client)
        {
            AddScreenShot(client, "Test failed from Log Exception");
            var message = exception.Message + Environment.NewLine + exception.StackTrace.Trim();
            var markup = MarkupHelper.CreateCodeBlock(message);
            GetMethod().Log(Status.Error, markup);
            throw exception;
        }

        private static string GetBase64StringForImage(string imgPath)
        {
            var imageBytes = File.ReadAllBytes(imgPath);
            var based64String = Convert.ToBase64String(imageBytes);
            return based64String;
        }

        public enum AppUrl
        {
            Qa,
            Dev,
            Uat
        }

        [TestCleanup]
        public void Cleanup()
        {
            PwrApp.Browser.Driver?.Quit();
        }
    }
}
