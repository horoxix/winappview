using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winappview.Views.NotepadView
{
    public partial class NotepadView : View
    {
        public NotepadView(WindowsDriver<WindowsElement> driver)
        {
            this.driver = driver;
        }

        public void Input(string inputString, WindowsElement element)
        {
            element.SendKeys(inputString);
        }

        public void Save(bool save)
        {
            if (save)
            {
                SaveButton.Click();
            }
            else
            {
                DontSaveButton.Click();
            }
        }
    }
}
