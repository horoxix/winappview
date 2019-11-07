using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Interactions;

namespace winappview
{
    /**
     * Test Fixture parent class for all tests.
     * Contains useful methods and the base setup for each test.
     **/
    [TestFixture]
    public class TestSession
    {
        // Test Session Properties
        protected readonly string WindowsApplicationDriverUrl = ConfigurationManager.AppSettings["WindowsApplicationDriverUrl"];
        protected readonly string Directory = ConfigurationManager.AppSettings["Directory"];
        protected readonly string DeviceName = ConfigurationManager.AppSettings["DeviceName"];
        protected static WindowsDriver<WindowsElement> driver;
        protected static AppiumOptions opt;

        //Types of element finding methods.
        public enum WaitType
        {
            XPATH,
            ACCESSIBILITY_ID,
            NAME
        }

        //Waits for element to appear based on type of element.
        public void Wait(WaitType waitType, String waitString, bool firstAttempt = true)
        {
            var wait = new DefaultWait<WindowsDriver<WindowsElement>>(driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromSeconds(1)
            };
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException));
            WindowsElement waitOption = null;
            try
            {
                wait.Until(driver =>
                {
                    switch (waitType)
                    {
                        case WaitType.ACCESSIBILITY_ID:
                            waitOption = driver.FindElementByAccessibilityId(waitString);
                            break;
                        case WaitType.NAME:
                            waitOption = driver.FindElementByName(waitString);
                            break;
                        case WaitType.XPATH:
                            waitOption = driver.FindElementByXPath(waitString);
                            break;
                    }
                    return waitOption != null;
                });
            }
            // If no element was found in the given time frame, press "Enter" to close a popup or error message and try again.
            catch (Exception ex)
            {
                if (ex is WebDriverTimeoutException || ex is WebDriverException)
                {
                    if (firstAttempt)
                    {
                        Actions actions = new Actions(driver);
                        actions.SendKeys(Keys.Enter);
                        actions.Perform();
                        Wait(waitType, waitString, false);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        //Generates a new connection to the driver
        public void NewSession()
        {
            driver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), opt);
        }

        #region Setup
        /**
         * Base setup for all test session children.
         * Performs setup of session driver.
         **/
        [SetUp]
        public virtual void Setup()
        {
            if (driver == null)
            {
                opt = new AppiumOptions();
                opt.AddAdditionalCapability("app", Directory);
                opt.AddAdditionalCapability("deviceName", DeviceName);
                driver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), opt);
                Assert.IsNotNull(driver);
                Assert.IsNotNull(driver.SessionId);
                // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
            }
        }
        #endregion

        #region Teardown
        /**
         * Closes the driver,
         * Quits the application.
         **/
        [TearDown]
        public virtual void TearDown()
        {
            // Close the application and delete the driver
            if (driver != null)
            {
                driver.Close();

                try
                {
                    // Dismiss Save dialog if it is blocking the exit
                    driver.FindElementByName("Don't Save").Click();
                }
                catch { }

                driver.Quit();
                driver = null;
            }
        }
        #endregion
    }
}
