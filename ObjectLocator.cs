using OpenQA.Selenium;

namespace Company.Platform.automation.App.web.Common
{
    public class ObjectLocator
    {
        public const string ID = "ID";
        public const string CSS = "CSS";
        public const string XPATH = "XPATH";
        public const string LINK_TEXT = "LINKTEXT";
        public const string PARTIAL_LINK_TEXT = "PARTIALLINKTEXT";
        public const string CLASS = "CLASS";
        public const string NAME = "NAME";
        public readonly string _objectValue;
        public readonly string _strLocatorType;
        public readonly string LocatorDescription;

        public ObjectLocator(string locator, string locatorType)
        {
            _objectValue = locator;
            LocatorValue = GetObjectLocator(_objectValue, locatorType);
            _strLocatorType = locatorType;
        }

        public By LocatorValue { get; set; }

        private By GetObjectLocator(string locator, string locatorType)
        {
            switch (locatorType.ToUpper())
            {
                case ID:
                    LocatorValue = By.Id(locator);
                    break;
                case CSS:
                    LocatorValue = By.CssSelector(locator);
                    break;
                case XPATH:
                    LocatorValue = By.XPath(locator);
                    break;
                case LINK_TEXT:
                    LocatorValue = By.LinkText(locator);
                    break;
                case CLASS:
                    LocatorValue = By.ClassName(locator);
                    break;
                case NAME:
                    LocatorValue = By.Name(locator);
                    break;
                case PARTIAL_LINK_TEXT:
                    LocatorValue = By.PartialLinkText(locator);
                    break;
            }

            return LocatorValue;
        }
    }
}