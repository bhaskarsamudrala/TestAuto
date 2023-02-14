using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
//using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Company.Platform.automation.App.web.Common
{
    public class SeleniumActions
    {
        private const int ExplicitTimeOutInSeconds = 10;
        private const int ImplicitTimeOutInSeconds = 5;
        private const int PageLoadTimeOutInSeconds = 10;
        private WebDriverWait _webDriverWait;
        private readonly Actions _actions;
        private readonly IWebDriver _iWebDriver;

        public SeleniumActions(IWebDriver iWebDriver)
        {
            iWebDriver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(PageLoadTimeOutInSeconds));
            iWebDriver.Manage().Timeouts().ImplicitWait.Add(TimeSpan.FromSeconds(ImplicitTimeOutInSeconds));
            _actions = new Actions(iWebDriver);
            _webDriverWait = new WebDriverWait(iWebDriver, new TimeSpan(0, 0, ExplicitTimeOutInSeconds));
            _iWebDriver = iWebDriver;
        }

        private void Fail(string failureMessage)
        {
            Assert.Fail(failureMessage);
        }

        [Obsolete]
        public void ClickElement(ObjectLocator ObjectLocator)
        {
            try
            {
                _webDriverWait = new WebDriverWait(_iWebDriver, TimeSpan.FromSeconds(20));
                _webDriverWait.Until(ExpectedConditions.ElementToBeClickable(ObjectLocator.LocatorValue));
                _iWebDriver.FindElement(ObjectLocator.LocatorValue).Click();
            }
            catch (WebDriverException webDriverException)
            {
                Fail(webDriverException.Message);
            }
        }

        [Obsolete]
        public void DoubleClickElement(ObjectLocator ObjectLocator)
        {
            try
            {
                _webDriverWait = new WebDriverWait(_iWebDriver, TimeSpan.FromSeconds(20));
                _webDriverWait.Until(ExpectedConditions.ElementToBeClickable(ObjectLocator.LocatorValue));
                var element = _iWebDriver.FindElement(ObjectLocator.LocatorValue);
                _actions.DoubleClick(element).Perform();
            }
            catch (WebDriverException webDriverException)
            {
                Fail(webDriverException.Message);
            }
        }

        [Obsolete]
        public void EnterTextInTextBoxElement(ObjectLocator ObjectLocator, string text)
        {
            try
            {
                WaitForElementToBeVisible(ObjectLocator);
                _iWebDriver.FindElement(ObjectLocator.LocatorValue).SendKeys(text);
            }
            catch (WebDriverException webDriverException)
            {
                Fail(webDriverException.Message);
            }
        }

        [Obsolete]
        public string GetTextPresentInTheElement(ObjectLocator ObjectLocator)
        {
            var textFromElement = "";
            try
            {
                WaitForElementToBeVisible(ObjectLocator);
                textFromElement = _iWebDriver.FindElement(ObjectLocator.LocatorValue).Text;
            }
            catch (WebDriverException webDriverException)
            {
                Fail(webDriverException.Message);
            }

            return textFromElement;
        }



        public void selectElementByTextFromTheList(ObjectLocator listValuesLocator, string valueText)
        {
            try
            {
                waitForElementsUntilLocated(listValuesLocator, 10);
                var listOfWebElements = _iWebDriver.FindElements(listValuesLocator.LocatorValue);
                foreach (var listElement in listOfWebElements)
                    if (listElement.Text == valueText)
                        listElement.Click();
            }
            catch (StaleElementReferenceException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        [Obsolete]
        public void VerifyTextPresentInElement(ObjectLocator ObjectLocator, string textToVerify)
        {
            try
            {
                WaitForElementToBeVisible(ObjectLocator);
                var textFromElement = _iWebDriver.FindElement(ObjectLocator.LocatorValue).Text;
                if (!textFromElement.Contains(textToVerify))
                    Fail("Text from element - '" + textFromElement + "' does not match expected text - '" + textToVerify + "'");
            }
            catch (WebDriverException webDriverException)
            {
                Fail(webDriverException.Message);
            }
        }


        public int getElementsCount(ObjectLocator objectLocator)
        {
            var elementCount = 0;
            try
            {
                elementCount = _iWebDriver.FindElements(objectLocator.LocatorValue).Count;
            }
            catch (WebDriverException exception)
            {
                Fail(exception.Message);
            }

            return elementCount;
        }

        public List<IWebElement> GetListOfWebElements(ObjectLocator ObjectLocator)
        {
            var webElements = new List<IWebElement>();
            try
            {
                WaitForElementToBeVisible(ObjectLocator);
                webElements = new List<IWebElement>(_iWebDriver.FindElements(ObjectLocator.LocatorValue));
            }
            catch (WebDriverException webDriverException)
            {
                Fail(webDriverException.Message);
            }

            return webElements;
        }


        // wait Specific Methods

        public Boolean isElementDisplayed(ObjectLocator objectLocator)
        {
            try
            {
                var displayed = _iWebDriver.FindElement(objectLocator.LocatorValue).Displayed;
                return displayed;
            }
            catch (NoSuchElementException exception)
            {
                return false;
            }

        }

        public void waitForElementsUntilLocated(ObjectLocator listValuesLocators, int seconds)
        {
            try
            {
                _webDriverWait = new WebDriverWait(_iWebDriver, TimeSpan.FromSeconds(seconds));
                _webDriverWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(listValuesLocators.LocatorValue));
            }
            catch (Exception webDriverException)
            {
                Fail(webDriverException.Message);
            }
        }

        public void waitForPageToLoad()
        {
            try
            {
                _iWebDriver.WaitForPageToLoad();
            }
            catch (WebDriverException exception)
            {
                Fail(exception.Message);
            }
        }


        public void waitForTransaction()
        {
            try
            {
                _iWebDriver.WaitForTransaction();
            }
            catch (WebDriverException exception)
            {
                Fail(exception.Message);
            }
        }

        [Obsolete]
        private void WaitForElementToBeVisible(ObjectLocator ObjectLocator)
        {
            try
            {
                _webDriverWait = new WebDriverWait(_iWebDriver, TimeSpan.FromSeconds(10));
                _webDriverWait.Until(ExpectedConditions.ElementIsVisible(ObjectLocator.LocatorValue));
            }
            catch (WebDriverException webDriverException)
            {
                Fail("Timed out waiting for page object - Locator value - " +
                     ObjectLocator.LocatorValue
                     + "\n" + webDriverException.Message);
            }
        }


        public void waitForElementNotToBeVisible(ObjectLocator ObjectLocator)
        {
            try
            {
                _webDriverWait = new WebDriverWait(_iWebDriver, TimeSpan.FromSeconds(20));
                _webDriverWait.Until(ExpectedConditions.InvisibilityOfElementLocated(ObjectLocator.LocatorValue));
            }
            catch (WebDriverException webDriverException)
            {
                Fail("Timed out waiting for page object - Locator value - " +
                     ObjectLocator.LocatorValue
                     + "\n" + webDriverException.Message);
            }
        }


        // Dailogs
        public void HandleModalDialogs(ObjectLocator modalObjectLocator, ObjectLocator buttonObjectLocator)
        {
            try
            {
                WaitForElementToBeVisible(modalObjectLocator);
                var modalElement = _iWebDriver.FindElement(modalObjectLocator.LocatorValue);
                modalElement.FindElement(buttonObjectLocator.LocatorValue).Click();
            }
            catch (WebDriverException webDriverException)
            {
                Fail(webDriverException.Message);
            }
        }


        // Frames

        public void SwitchToFrame(ObjectLocator ObjectLocator)
        {
            try
            {
                var frameElement = _iWebDriver.FindElement(ObjectLocator.LocatorValue);
                _iWebDriver.SwitchTo().Frame(frameElement);
            }
            catch (WebDriverException webDriverException)
            {
                Fail("Timed out waiting for page object - Locator value - " +
                     ObjectLocator.LocatorValue
                     + "\n" + webDriverException.Message);
            }
        }

        public string GetWindowHandle()
        {
            return _iWebDriver.CurrentWindowHandle;
        }

        public void SwitchToParentFrame()
        {
            _iWebDriver.SwitchTo().ParentFrame();
        }

        // Application Specific 

        public string GetRowValue(ObjectLocator tableLocator, int rowNumber, int columnNumber)
        {
            var rowValue = string.Empty;
            try
            {
                WaitForElementToBeVisible(tableLocator);
                var tableElement = _iWebDriver.FindElement(tableLocator.LocatorValue);
                var rowElements = new List<IWebElement>(tableElement.FindElements(By.TagName("tr")));

                if (rowElements.Count > 0)
                    for (var i = 0; i <= rowNumber; i++)
                    {
                        var colElements =
                            new List<IWebElement>(rowElements[i].FindElements(By.TagName("td")));
                        if (colElements.Count > 0)
                            for (var j = 0; j <= columnNumber; j++)
                                rowValue = colElements[j].Text;
                        else
                            Console.WriteLine("Check the index value");
                    }
                else
                    Console.WriteLine("Please check the row elements count");
            }
            catch (WebDriverException webDriverException)
            {
                Fail(webDriverException.Message);
            }

            return rowValue;
        }

        public string GetHeaderControlListValue(string name)
        {
            var value = string.Empty;

            var _headerControlList = new ObjectLocator("//div[contains(@id,'headerControlsList')]//div[@data-preview_orientation='column']", ObjectLocator.XPATH);

            try
            {
                WaitForElementToBeVisible(_headerControlList);
                var headerNameElements = new List<IWebElement>(_iWebDriver.FindElements(_headerControlList.LocatorValue));
                if (headerNameElements.Count > 0)
                {
                    foreach (var element in headerNameElements)
                    {
                        var elements = element.FindElements(By.TagName("div"));
                        if (elements.Count <= 0) continue;
                        if (elements.Last().Text.Equals(name))
                        {
                            value = elements.First().Text;
                        }
                    }
                }
            }
            catch (WebDriverException exception)
            {
                Fail(exception.Message);
            }
            return value;
        }

        public string getRowValueFromDocumentsSharePointTable(int rowNumber, int colNumber)
        {
            string rowValue = String.Empty;
            var _frameDocumentTable = new ObjectLocator("WebResource_xrmforyousp", ObjectLocator.ID);
            var _attachmentsTable = new ObjectLocator("//*[@id='table_sharepointtable']/tbody", ObjectLocator.XPATH);
            try
            {
                SwitchToParentFrame();
                WaitForElementToBeVisible(_frameDocumentTable);
                SwitchToFrame(_frameDocumentTable);
                rowValue = GetRowValue(_attachmentsTable, rowNumber, colNumber);
            }
            catch (WebDriverException exception)
            {
                Fail(exception.Message);
            }

            return rowValue;
        }


        //  Driver

        public void CloseDriver()
        {
            try
            {
                _iWebDriver.Close();
                _iWebDriver.Quit();
                _iWebDriver.Dispose();
            }
            catch (WebDriverException exception)
            {
                Fail(exception.Message);
            }

        }

    }
}