using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Library.Tests
{
    [TestClass]
    public class FileImportTests
    {

        private FileImport FileImporter { get; set; }
        private static string TempDirectory { get; } = Path.GetTempPath();

        [TestInitialize]
        public void TestInitialize()
        {
            if (File.Exists($"{TempDirectory}/tempFile.txt")) { File.Delete($"{TempDirectory}/tempFile.txt"); }
            if (File.Exists($"{TempDirectory}/tempFile2.txt")) { File.Delete($"{TempDirectory}/tempFile2.txt"); }
            if (File.Exists($"{TempDirectory}/tempFile3.txt")) { File.Delete($"{TempDirectory}/tempFile3.txt"); }
            if (File.Exists($"{TempDirectory}/tempFileWithGifts.txt")) { File.Delete($"{TempDirectory}/tempFileWithGifts.txt"); }

            File.WriteAllLines($"{TempDirectory}/tempFile.txt", new string[] { "Name: Edmond Dantes" });
            File.WriteAllLines($"{TempDirectory}/tempFile2.txt", new string[] { "Name: Dantes, Edmond" });
            File.WriteAllLines($"{TempDirectory}/tempFile3.txt", new string[] { "Name:  Edmond Dantes" });

            File.WriteAllLines($"{TempDirectory}/tempFileWithGifts.txt", new string[] { "Name: Edmond Dantes",
                                                                                        string.Empty,
                                                                                        "Sword",
                                                                                        string.Empty,
                                                                                        "Gold",
                                                                                        string.Empty,
                                                                                        "Revenge"});
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists($"{TempDirectory}/tempFile.txt")) { File.Delete($"{TempDirectory}/tempFile.txt"); }
            if (File.Exists($"{TempDirectory}/tempFile2.txt")) { File.Delete($"{TempDirectory}/tempFile2.txt"); }
            if (File.Exists($"{TempDirectory}/tempFile3.txt")) { File.Delete($"{TempDirectory}/tempFile3.txt"); }
            if (File.Exists($"{TempDirectory}/tempFileWithGifts.txt")) { File.Delete($"{TempDirectory}/tempFileWithGifts.txt"); }
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow(" ")]
        [DataRow(default(string))]
        [ExpectedException(typeof(ArgumentException), "FilePath cannot be null or empty.")]
        public void CreateFileImport_PassedNullEmptyWhiteSpace_ExpectsException(string path)
        {
            FileImporter = new FileImport(path);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException), "The file does not exist.")]
        public void CreateFileImport_PassedNonExistentFile_ExpectsException()
        {
            FileImporter = new FileImport($"{TempDirectory}/nonExistentFile.txt");
        }

        [TestMethod]
        public void ReadFile_PassedGoodInput()
        {
            FileImporter = new FileImport($"{TempDirectory}/tempFileWithGifts.txt");
            List<string> fileContents = new List<string>();
            FileImporter.ReadFile(out fileContents);
            Assert.AreEqual(4, fileContents.Count);
            Assert.AreEqual("Name: Edmond Dantes", fileContents[0]);
            Assert.AreEqual("Sword", fileContents[1]);
            Assert.AreEqual("Gold", fileContents[2]);
            Assert.AreEqual("Revenge", fileContents[3]);
        }

        [TestMethod]
        [DataRow("Name: Edmond Dantes")]
        [DataRow("Name: Dantes, Edmond")]
        public void ParseHeader_PassedValidHeader(string header)
        {
            FileImporter = new FileImport();
            var user = FileImporter.ParseHeader(header);
            Assert.AreEqual("Edmond", user.FirstName);
            Assert.AreEqual("Dantes", user.LastName);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow(" ")]
        [DataRow(default(string))]
        [ExpectedException(typeof(ArgumentException), "Header cannot be null or empty.")]
        public void ParseHeader_PassedNullEmptyWhiteSpace_ThrowsException(string header)
        {
            FileImporter = new FileImport();
            FileImporter.ParseHeader(header);
        }

        [TestMethod]
        [DataRow("name: Edmond Dantes")]
        [DataRow(" Name: Edmond Dantes")]
        [DataRow("Name Edmond Dantes")]
        [DataRow("Name:  Edmond Dantes")]
        [DataRow("Name: Edmond  Dantes")]
        [DataRow("Name: Edmond MiddleName Dantes")]
        [DataRow("Name: R2 D2")]
        [ExpectedException(typeof(FormatException),
            "Invalid header format. Headers must follow the format of 'Name: <FirstName> <LastName>'" +
                    "or 'Name: <LastName>, <FirstName>'.")]
        public void ParseHeader_PassedInvalidHeaderFormat_ExpectsException(string header)
        {
            FileImporter = new FileImport();
            FileImporter.ParseHeader(header);
        }

        [TestMethod]
        public void ImportUserAndGifts_PassedGoodInput()
        {
            FileImporter = new FileImport($"{TempDirectory}/tempFileWithGifts.txt");
            var user = FileImporter.ImportUserAndGifts(FileImporter.FilePath);

            Assert.IsNotNull(user);
            Assert.AreEqual("Edmond", user.FirstName);
            Assert.AreEqual("Dantes", user.LastName);
            Assert.AreEqual(3, user.Gifts.Count);
        }

        [TestMethod]
        public void GetGiftsForUser_PassedValidUser()
        {
            FileImporter = new FileImport($"{TempDirectory}/tempFileWithGifts.txt");
            var user = FileImporter.ImportUserAndGifts(FileImporter.FilePath);

            List<Gift> gifts = FileImporter.GetGiftsForUser(user);

            Assert.AreEqual(3, gifts.Count);
            Assert.AreEqual("Sword", gifts[0].Title);
            Assert.AreEqual("Gold", gifts[1].Title);
            Assert.AreEqual("Revenge", gifts[2].Title);
        }
    }
}
