using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SecretSanta.Library
{
    public class FileImport
    {
        private string RelativePath
        {
            get
            {
                return Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, @"..\..\..\..\data\");
            }
        }
            

        public FileStream OpenFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                FileStream f = new FileStream(path, FileMode.Open);
            }

            var fullPath = Path.GetFullPath(Path.Combine(RelativePath, path));
            FileStream fileStream = new FileStream(fullPath, FileMode.Open);

            return fileStream;
        }

        public static void ReadFile(string path, out List<string> fileContents)
        {
            fileContents = new List<string>();
            using (StreamReader streamReader = new StreamReader(path))
            {
                while (!streamReader.EndOfStream)
                {
                    fileContents.Add(streamReader.ReadLine());
                }
            }
        }

        public static bool ParseHeader(string header, out User user)
        {
            user = new User();
            if (header.StartsWith("Name:"))
            {
                string[] tokenizedHeader = header.Split(':');
                string[] tokenizedNames;
                if (tokenizedHeader[1].Contains(","))
                {
                    tokenizedNames = tokenizedHeader[1].Split(',');
                    user.FirstName = tokenizedNames[1];
                    user.LastName = tokenizedNames[0];
                    Console.WriteLine($"tHeader[0]: {tokenizedHeader[0]}\ntHeader[1]: {tokenizedHeader[1]}");
                    Console.WriteLine($"tNames[0]: {tokenizedNames[0]}\ntNames[1]: {tokenizedNames[1]}");

                }
                else
                {
                    tokenizedNames = tokenizedHeader[1].Split(' ');
                    user.FirstName = tokenizedNames[0];
                    user.LastName = tokenizedNames[1];
                    Console.WriteLine($"tHeader[0]: {tokenizedHeader[0]}\ntHeader[1]: {tokenizedHeader[1]}");
                    Console.WriteLine($"tNames[0]: {tokenizedNames[0]}\ntNames[1]: {tokenizedNames[1]}");

                }

                return true;
            }

            user = null;
            return false;
        }
    }
}
