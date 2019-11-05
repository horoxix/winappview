using OpenQA.Selenium.Appium.Windows;

namespace winappview.Views.NotepadView
{
    public partial class NotepadView : View
    {
        public readonly string TextEditorLocator = "Text Editor";
        public readonly string SaveButtonLocator = "Save";
        public readonly string DontSaveButtonLocator = "Don't Save";

        public WindowsElement TextEditor => driver.FindElementByName(TextEditorLocator);
        public WindowsElement SaveButton => driver.FindElementByName(SaveButtonLocator);
        public WindowsElement DontSaveButton => driver.FindElementByName(DontSaveButtonLocator);
    }
}