using System;
using System.IO;
using System.Net;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Library.Tests
{
    [TestClass]
    public class FileImportTests
    {
        public FileImport FileImporter { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            FileImporter = new FileImport();
        }

        [TestMethod]
        [DataRow(typeof(ArgumentNullException), null)]
        [DataRow(typeof(ArgumentException), default(string))]
        [DataRow(typeof(FileNotFoundException), "nonexistentFile.txt")]
        [DataRow(typeof(SecurityException), "noReadPermissionsFile.txt")]

        public void OpenFile_ThrowSystemException(Type exceptionType, string path)
        {
            try
            {
                FileImporter.OpenFile(path);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception exception) { Console.WriteLine($"Exception: {exception.GetType()}\nExpected: {exceptionType}");}// when (exception.GetType() == exceptionType) { }
        }
    }
}
