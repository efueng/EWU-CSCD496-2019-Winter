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
        void UpdateTempFile(string tempFile, string text)
        {
            using (StreamWriter streamWriter = File.AppendText(tempFile))
            {
                streamWriter.WriteLine(text);
            }
        }

        void DeleteTempFile(string tempFile)
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }

        private FileImport FileImporter { get; set; }
        private string TempFile
        {
            get
            {
                string path = string.Empty;

                path = Path.GetTempFileName();
                FileInfo fileInfo = new FileInfo(path);
                fileInfo.Attributes = FileAttributes.Temporary;

                return path;
            }
        }
        //private string RelativePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
        //    @"..\..\..\.\..\test\data\");

        

        [TestInitialize]
        public void TestInitialize()
        {
            FileImporter = new FileImport();
            //RelativePath = @"test/data/";
        }

        //[TestCleanup]
        //public void

        [TestMethod]
        [DataRow(typeof(ArgumentNullException), null)]
        [DataRow(typeof(ArgumentException), @"")]
        [DataRow(typeof(DirectoryNotFoundException), @"nonexistentFile.txt")]
        public void OpenFile_ThrowSystemException(Type exceptionType, string path)
        {
            //try
            //{
            //    Console.WriteLine($"Path: {path}");
            //    Console.WriteLine($"Reflection: {System.Reflection.Assembly.GetExecutingAssembly().Location}");
            //    FileImporter.OpenFile(path);

            //    Assert.Fail("Expected exception was not thrown.");
            //}
            //catch (Exception exception) when (exception.GetType() == exceptionType) { }

            try
            {
                FileImporter.OpenFile(path);
            }
            catch (SystemException exception)
            {
                Assert.AreEqual(exceptionType, exception.GetType());
            }
        }

        [TestMethod]
        public void OpenAndReadFile()
        {
            string tempFile = TempFile;
            UpdateTempFile(tempFile, "This is a test.");

            Assert.AreEqual("This is a test.", FileImporter.ReadFile(tempFile));
            DeleteTempFile(tempFile);
        }

        [TestMethod]
        [DataRow("Name: Edmond Dantes")]
        public void ParseHeader(string header)
        {
            Assert.IsTrue(FileImporter.ParseHeader(header));
        }
    }
}
