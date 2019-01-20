using System;
using System.IO;

namespace SecretSanta.Library
{
    public class FileImport
    {
        private FileStream FileStream { get; set; }
        public void OpenFile(string path)
        {
            
            var _path = Path.GetFullPath(path);
            FileStream = new FileStream(_path, FileMode.Open, FileAccess.Read);
        }
    }
}
