using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SecretSanta.Library
{
    public class FileImport
    {
        public void ReadFile(string path, out List<string> fileContents)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path is null or empty.");
            }

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
            if (!string.IsNullOrEmpty(header) && header.StartsWith("Name:"))
            {
                string[] tokenizedNames;

                string[] tokenizedHeader = header.Split(':');
                tokenizedHeader[1] = tokenizedHeader[1].Trim();
                              

                if (tokenizedHeader[1].Contains(","))
                {
                    tokenizedNames = tokenizedHeader[1].Split(',');
                    user.FirstName = tokenizedNames[1].Trim();
                    user.LastName = tokenizedNames[0].Trim();

                }
                else
                {
                    tokenizedNames = tokenizedHeader[1].Split(' ');
                    user.FirstName = tokenizedNames[0].Trim();
                    user.LastName = tokenizedNames[1].Trim();
                }

                return true;
            }

            user = null;
            return false;
        }

        public User Import(string path)
        {
            User user;
            List<string> fileContents;

            ReadFile(path, out fileContents);
            ParseHeader(fileContents[0], out user);

            return user;
        }
    }
}
