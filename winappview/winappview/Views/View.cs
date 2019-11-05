using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace winappview.Views
{
    public class View
    {
        protected WindowsDriver<WindowsElement> driver;

        public bool IsElementPresent(WindowsDriver<WindowsElement> driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
