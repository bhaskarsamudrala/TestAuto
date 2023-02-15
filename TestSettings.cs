using System;
using System.IO;
using System.Security;
using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using Company.Platform.automation.App.web.Utils;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;

namespace Company.Platform.automation.App.web
{
    internal static class TestSettings
    {
        public static Uri PowerAppUrl { get; set; }
        public static string AzureDevopsToken { get; set; }
        public static string AzureTenantId { get; set; }
        public static string KeyVaultName { get; set; }
        public static string TestUserId { get; set; }
        public static class SystemAdmin
        {
            public static SecureString UName { get; set; }
            public static SecureString PWord { get; set; }
        }
        public static class NonAdmin
        {
            public static SecureString UName { get; set; }
            public static SecureString PWord { get; set; }
        }
        public static string Type { get; set; }
        public static string RemoteType { get; set; }
        public static string RemoteHubServerUrl { get; set; }

        public static class PowerAppAdmin
        {
            public static readonly SecureString UName = System.Configuration.ConfigurationManager.AppSettings["AdminUsername"].ToSecureString();
            public static readonly SecureString PWord = System.Configuration.ConfigurationManager.AppSettings["AdminPassword"].ToSecureString();
        }

        public static readonly BrowserOptions Options = new BrowserOptions
        {
            PrivateMode = false,
            FireEvents = false,
            Headless = false,
            UserAgent = false,
            DefaultThinkTime = 2000,
            BrowserType = (BrowserType)Enum.Parse(typeof(BrowserType), "Chrome"),
            RemoteBrowserType = (BrowserType)Enum.Parse(typeof(BrowserType), "Chrome"),
            RemoteHubServer = new Uri("http://:4444/wd/hub"),
            UCITestMode = true,
            StartMaximized = false,
            //DriversPath = /*DriversPath*/Path.IsPathRooted(DriversPath) ? DriversPath : Path.Combine(Directory.GetCurrentDirectory(), DriversPath)
            Height = 1080,
            Width = 1920
        };


        public static string GetRandomString(int minLen, int maxLen)
        {
            var Alphabet = ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcefghijklmnopqrstuvwxyz0123456789").ToCharArray();
            var m_randomInstance = new Random();
            var m_randLock = new object();

            var alphabetLength = Alphabet.Length;
            int stringLength;
            lock (m_randLock) { stringLength = m_randomInstance.Next(minLen, maxLen); }
            var str = new char[stringLength];

            // max length of the randomizer array is 5
            var randomizerLength = (stringLength > 5) ? 5 : stringLength;

            var rndInts = new int[randomizerLength];
            var rndIncrements = new int[randomizerLength];

            // Prepare a "randomizing" array
            for (int i = 0; i < randomizerLength; i++)
            {
                int rnd = m_randomInstance.Next(alphabetLength);
                rndInts[i] = rnd;
                rndIncrements[i] = rnd;
            }

            // Generate "random" string out of the alphabet used
            for (int i = 0; i < stringLength; i++)
            {
                int indexRnd = i % randomizerLength;
                int indexAlphabet = rndInts[indexRnd] % alphabetLength;
                str[i] = Alphabet[indexAlphabet];

                // Each rndInt "cycles" characters from the array, 
                // so we have more or less random string as a result
                rndInts[indexRnd] += rndIncrements[indexRnd];
            }
            return (new string(str));
        }

        public static string GetRandomStringNumber(int length)
        {
            {
                var random = new Random();
                var rNumber = string.Empty;
                for (var i = 0; i < length; i++)
                    rNumber = string.Concat(rNumber, random.Next(10).ToString());
                return rNumber;
            }
        }

        public static class UciAppName
        {
            public const string CompanyApplications = "Company Applications";
        }
    }
}