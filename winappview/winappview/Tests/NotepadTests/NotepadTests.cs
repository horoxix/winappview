using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winappview.Views.NotepadView;

namespace winappview.Tests.NotepadTests
{
    [TestFixture]
    public class NotepadTests : TestSession
    {
        #region Views
        // Views Needed
        NotepadView notepadView;
        #endregion

        #region Setup
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            //NewSession();
            notepadView = new NotepadView(driver);
        }
        #endregion

        #region Tests
        [Test]
        public void InputTextTest()
        {
            // Arrange
            string inputNumbers = "12345";
            string inputCharacters = "abcde";
            string inputSpecial = "!@#$%";

            // Act
            notepadView.TextEditor.Click();
            notepadView.Input(inputNumbers, notepadView.TextEditor);

            // Assert
            Assert.AreEqual(inputNumbers, notepadView.TextEditor.Text);
            notepadView.Input(inputCharacters, notepadView.TextEditor);
            Assert.AreEqual(inputNumbers + inputCharacters, notepadView.TextEditor.Text);
            notepadView.Input(inputSpecial, notepadView.TextEditor);
            Assert.AreEqual(inputNumbers + inputCharacters + inputSpecial, notepadView.TextEditor.Text);
        }
        #endregion

        #region Teardown
        /**
         * Ends session.
         **/
        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }
        #endregion
    }
}
