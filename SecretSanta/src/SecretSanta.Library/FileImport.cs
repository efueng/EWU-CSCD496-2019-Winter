using System;
using System.IO;

namespace SecretSanta.Library
{
    public class FileImport
    {
        private FileStream FileStream { get; set; }
        public void OpenFile(string path)
        {
            Path
            FileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        }
    }
}
