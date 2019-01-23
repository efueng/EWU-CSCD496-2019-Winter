using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

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

        [TestInitialize]
        public void TestInitialize()
        {
            FileImporter = new FileImport();
        }

        [TestMethod]
        public void ReadFile_PassedNullPath_ExpectException()
        {
            List<string> fileContents;
            Assert.ThrowsException<ArgumentException>(() =>
            {
                FileImporter.ReadFile(null, out fileContents);
            });
        }

        [TestMethod]
        public void ReadFile_PassedEmptyPath_ExpectException()
        {
            List<string> fileContents;
            Assert.ThrowsException<ArgumentException>(() =>
            {
                FileImporter.ReadFile("", out fileContents);
            });
        }

        [TestMethod]
        public void OpenAndReadFile()
        {
            string tempFile = TempFile;
            UpdateTempFile(tempFile, "Name: Edmond Dantes");

            List<string> fileContents;
            FileImporter.ReadFile(tempFile, out fileContents);
            DeleteTempFile(tempFile);

            Assert.AreEqual("Name: Edmond Dantes", fileContents[0]);
        }

        [TestMethod]
        [DataRow("Name: Edmond Dantes")]
        [DataRow("Name: Dantes, Edmond")]
        public void ParseHeader(string header)
        {
            var user = new User("test", "user");
            Assert.IsTrue(FileImport.ParseHeader(header, out user));
        }

        [TestMethod]
        public void ParseHeader_PassedNullHeader()
        {
            var user = new User();
            Assert.IsFalse(FileImport.ParseHeader(null, out user));
        }

        [TestMethod]
        public void Import_PassedGoodInput()
        {
            string tempFile = TempFile;
            UpdateTempFile(tempFile, "Name: Inigo Montoya");
            var user = FileImporter.Import(tempFile);
            DeleteTempFile(tempFile);

            Assert.IsNotNull(user);
            Assert.AreEqual("Inigo", user.FirstName);
            Assert.AreEqual("Montoya", user.LastName);
        }
        
    }
}
