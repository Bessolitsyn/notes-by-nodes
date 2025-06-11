using Newtonsoft.Json;
using System.Globalization;
using System.Xml;

namespace EasyObjectFileStorage
{
    public abstract class FileStorage(string pathToRootFolder)
    {
        readonly string RootFolder = pathToRootFolder;

        public event EventHandler<string>? LogEvent;

        protected bool TrySaveFile(string path, string filename, string fileContent, out string fileid)
        {
            fileid = path + filename;
            try
            {
                System.IO.File.WriteAllText(@RootFolder + path + filename, fileContent);
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }

        protected void SaveFile(string path, string filename, string fileContent, out string fileid)
        {
            fileid = path + filename;
            System.IO.File.WriteAllText(@RootFolder + path + filename, fileContent);
        }
        protected bool TrySaveFile(string path, string filename, byte[] fileContent, out string fileid)
        {
            fileid = path + filename;
            try
            {
                System.IO.File.WriteAllBytes(@RootFolder + path + filename, fileContent);
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }
        protected void SaveFile(string path, string filename, byte[] fileContent, out string fileid)
        {
            fileid = path + filename;
            System.IO.File.WriteAllBytes(@RootFolder + path + filename, fileContent);
        }

        protected bool TryRemoveFile(string path, string filename)
        {
            try
            {
                System.IO.File.Delete(@RootFolder + path + filename);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
