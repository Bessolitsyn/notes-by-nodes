using Newtonsoft.Json;
using System.Globalization;
using System.Xml;

namespace EasyObjectFileStorage
{
    public abstract class FileStorage(string pathToRootFolder)
    {
        readonly string RootFolder = pathToRootFolder;

        public event EventHandler<string>? LogEvent;

        protected void SaveFile(string path, string filename, string fileContent, out string fileid)
        {
            fileid = path + filename;
            System.IO.File.WriteAllText(@RootFolder + path + filename, fileContent);
        }

        protected void SaveFile(string path, string filename, byte[] fileContent, out string fileid)
        {
            fileid = path + filename;
            System.IO.File.WriteAllBytes(@RootFolder + path + filename, fileContent);
        }

        protected void Logging(string message)
        {
            LogEvent?.Invoke(this, message);
        }

        protected string[] GetAllFiles(string path, string fileExt)
        {
            string[] files = Directory.GetFiles(@RootFolder + path, $"*.{fileExt}");
            return files;
        }







    }
}
