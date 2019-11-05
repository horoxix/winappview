using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using OpenQA.Selenium;

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
        protected readonly string TapDirectory = ConfigurationManager.AppSettings["TapDirectory"];
        protected readonly string DeviceName = ConfigurationManager.AppSettings["DeviceName"];
        protected static WindowsDriver<WindowsElement> driver;
        protected static DesiredCapabilities appCapabilities;

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
            catch (WebDriverTimeoutException)
            {
                if (firstAttempt)
                {
                    driver.Keyboard.SendKeys(Keys.Enter);
                    Wait(waitType, waitString, false);
                }
                else
                {
                    throw;
                }
            }
        }

        //Generates a new connection to the driver
        public void NewSession()
        {
            driver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
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
                appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("deviceName", DeviceName);
                appCapabilities.SetCapability("app", TapDirectory);
                driver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
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
