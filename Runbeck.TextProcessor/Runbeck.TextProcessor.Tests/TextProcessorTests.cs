using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Runbeck.TextProcessor.Tests
{
    [TestClass]
    public class TextProcessorTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Failed to process file. Failed to initialize File Path. Input file path entered is invalid.")]
        public void TextProcessor_InvalidPath()
        {
            new TextProcessor("invalid path value", "d", "3");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Failed to process file. Invalid delimiter entered.")]
        public void TextProcessor_Invalid_Delimiter()
        {
            new TextProcessor(@"c:\temp\input.txt", "x", "3");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Failed to process file. Invalid field count entered.")]
        public void TextProcessor_Invalid_Field_Count()
        {
            new TextProcessor(@"c:\temp\input.txt", "x", "3");
        }
    }
}
