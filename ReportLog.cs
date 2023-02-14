using System.Collections.Generic;
using System.Threading;
using AventStack.ExtentReports;

namespace Company.Platform.automation.App.web.ReportsHelper
{
    public class ReportLog
    {
        private static readonly Dictionary<string, ExtentTest> ParentTestMap = new Dictionary<string, ExtentTest>();
        private static readonly ThreadLocal<ExtentTest> ParentTest = new ThreadLocal<ExtentTest>();
        private static readonly ThreadLocal<ExtentTest> ChildTest = new ThreadLocal<ExtentTest>();

        private static readonly object Synclock = new object();

        // creates a parent test
        public static ExtentTest CreateTest(string testName, string description = null)
        {
            lock (Synclock)
            {
                ExtentTest parentTest = null;
                parentTest = ReportService.Instance.CreateTest(testName, description);
                ParentTestMap.Add(testName, parentTest);
                return ParentTest.Value;
            }
        }

        // creates a node
        // node is added to the parent using the parentName
        // if the parent is not available, it will be created
        public static ExtentTest CreateMethod(string parentName, string testName, string description = null)
        {
            lock (Synclock)
            {
                ExtentTest parentTest = null;
                if (!ParentTestMap.ContainsKey(parentName))
                {
                    parentTest = ReportService.Instance.CreateTest(testName);
                    ParentTestMap.Add(parentName, parentTest);
                }
                else
                {
                    parentTest = ParentTestMap[parentName];
                }

                ParentTest.Value = parentTest;
                ChildTest.Value = parentTest.CreateNode(testName, description);
                return ChildTest.Value;
            }
        }

        public static ExtentTest CreateMethod(string testName)
        {
            lock (Synclock)
            {
                ChildTest.Value = ParentTest.Value.CreateNode(testName);
                return ChildTest.Value;
            }
        }

        public static ExtentTest GetMethod()
        {
            lock (Synclock)
            {
                return ChildTest.Value;
            }
        }

        public static ExtentTest GetTest()
        {
            lock (Synclock)
            {
                return ParentTest.Value;
            }
        }

        public static void Info(string message)
        {
            GetMethod().Info(message);
        }

        public static void Info(string message, MediaEntityBuilder medaia = null)
        {
            GetMethod().Info(message);
        }

        public static void Log(Status status, string message)
        {
            GetMethod().Log(status, message);
        }
    }
}