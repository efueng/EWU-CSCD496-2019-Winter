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

        public void ReadFile(string path, out List<string> fileContents)
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

        public bool ParseHeader(string header)
        {
            //if (header.StartsWith("Name:"))
            //{

            //}

            //return false;
            return header.StartsWith("Name:");
        }
    }
}
