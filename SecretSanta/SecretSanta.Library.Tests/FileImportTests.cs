using System;
using System.Net;
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void OpenFile_NullFile_ThrowArgumentException()
        {
            FileImporter.OpenFile(null);
        }
    }
}
