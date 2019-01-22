using System;
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

            var _path = Path.GetFullPath(Path.Combine(RelativePath, path));
            FileStream fileStream = new FileStream(_path, FileMode.Open);

            return fileStream;
        }

        public string ReadFile(string path)
        {
            string line = string.Empty;
            using (FileStream fileStream = OpenFile(path))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    line = streamReader.ReadLine();
                }
            }

            return line;
        }

        public bool ParseHeader(string header)
        {
            if (header.StartsWith("Name:"))
            {

            }

            return false;
        }
    }
}
