using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SecretSanta.Library
{
    public class FileImport
    {
        private string _FilePath;
        public string FilePath
        {
            get => _FilePath;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("FilePath cannot be null or empty.", nameof(value));
                }

                if (!File.Exists(value))
                {
                    throw new FileNotFoundException("The file does not exist.", nameof(value));
                }

                _FilePath = value;
            }
        }

        public FileImport() { }

        public FileImport(string filePath)
        {
            FilePath = filePath;
        }

        public void ReadFile(out List<string> fileContents)
        {
            fileContents = new List<string>();
            string line = "";

            using (StreamReader streamReader = new StreamReader(FilePath))
            {
                while (!streamReader.EndOfStream)
                {
                    line = streamReader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        fileContents.Add(line);
                    }
                }
            }
        }

        public User ParseHeader(string header)
        {
            if (string.IsNullOrWhiteSpace(header))
            {
                throw new ArgumentException("Header cannot be null or empty.", nameof(header));
            }

            string pattern = @"^Name:\s([a-zA-z]{0,50}),?\s([a-zA-Z]{0,50})$";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(header);

            if (!match.Success)
            {
                throw new FormatException("Invalid header format. Headers must follow the format of 'Name: <FirstName> <LastName>'" +
                    "or 'Name: <LastName>, <FirstName>'.");
            }

            string[] tokenizedNames;

            User user = new User();
            string[] tokenizedHeader = header.Split(':');

            tokenizedHeader[1] = tokenizedHeader[1].Trim();

            if (tokenizedHeader[1].Contains(","))
            {
                tokenizedNames = tokenizedHeader[1].Split(',');
                user.FirstName = tokenizedNames[1];
                user.LastName = tokenizedNames[0];

            }
            else
            {
                tokenizedNames = tokenizedHeader[1].Split(' ');
                user.FirstName = tokenizedNames[0];
                user.LastName = tokenizedNames[1];
            }

            return user;
        }

        public User ImportUserAndGifts(string path)
        {
            User user;
            List<string> fileContents;

            ReadFile(out fileContents);
            user = ParseHeader(fileContents[0]);

            for (int idx = 1; idx < fileContents.Count; idx++)
            {
                user.Gifts.Add(new Gift { Title = fileContents[idx] });
            }

            return user;
        }

        // completely ripped off from Cameron Osborne
        public List<Gift> GetGiftsForUser(User user)
        {
            return File.ReadLines(FilePath)
                .Skip(1)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .Select(line => new Gift
                {
                    Title = line,
                    User = user
                })
                .ToList();
        }
    }
}
