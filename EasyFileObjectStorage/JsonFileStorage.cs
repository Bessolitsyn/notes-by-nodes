
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyObjectFileStorage
{
    public class JsonFileStorage : FileStorage 
    {
        const string EX_MESSAGE_WHEN_SAVING = $"FileStorageMessage_ Error when saving;";
        const string EX_MESSAGE_WHEN_READING = $"FileStorageMessage_ Error when reading;";
        readonly string FOLDER_FOR_DATASET_STORAGE;
        readonly string FILE_EXTENSION;


        public JsonFileStorage(string pathToRootFolder, string subfolder, string fileExtension = "json") : base(pathToRootFolder)
        {
            FOLDER_FOR_DATASET_STORAGE = subfolder;
            FILE_EXTENSION = fileExtension;
            
        }
        
        public object[] GetAllObjects()
        {
            try
            {
                string[] files = GetAllFiles($"\\{FOLDER_FOR_DATASET_STORAGE}", FILE_EXTENSION);
                var contents = GetAllDeserializeObject<object>(files);
                return contents;
            }
            catch (Exception ex)
            {
                string logmessage = $"{EX_MESSAGE_WHEN_READING} ex.message; {ex.Message}";
                Logging(logmessage);
                //TO DO Logging must be done!:) 
                throw;
            }
        }

        public T[] GetAllObject<T>()
        {
            try
            {
                string[] files = GetAllFiles($"\\{FOLDER_FOR_DATASET_STORAGE}", FILE_EXTENSION);
                var contents = GetAllDeserializeObject<T>(files);
                return contents;
            }
            catch (Exception ex)
            {
                string logmessage = $"{EX_MESSAGE_WHEN_READING} ex.message; {ex.Message}";
                Logging(logmessage);
                //TO DO Logging must be done!:) 
                throw;
            }
        }
        public T GetObject<T>(string filename)
        {
            try
            {
                string[] files = [.. GetAllFiles($"\\{FOLDER_FOR_DATASET_STORAGE}", FILE_EXTENSION).Where(f=>f==filename)];
                var contents = GetAllDeserializeObject<T>(files);
                return contents[0];
            }
            catch (Exception ex)
            {
                string logmessage = $"{EX_MESSAGE_WHEN_READING} ex.message; {ex.Message}";
                Logging(logmessage);
                //TO DO Logging must be done!:) 
                throw;
            }
        }
        public void SaveObject(object obj)
        {
            var filename = obj.GetHashCode().ToString(CultureInfo.InvariantCulture);
            SaveObject(obj, filename);
        }
        public void SaveObject(object obj, string filename)
        {
            try
            {
                var fileContent = JsonConvert.SerializeObject(obj, Formatting.Indented);
                filename = filename + "." + FILE_EXTENSION;
                SaveFile($"\\{FOLDER_FOR_DATASET_STORAGE}", filename, fileContent, out var _);
            }
            catch (Exception ex)
            {
                string logmessage = $"{EX_MESSAGE_WHEN_SAVING} ex.message; {ex.Message}";
                Logging(logmessage);
                //TO DO Logging must be done!:) 
                throw;
            }
        }

        public void SaveObjectStreamToFile(MemoryStream sourceStream, string filename, out string fileid)
        {
            try
            {
                filename = filename + "." + FILE_EXTENSION;
                SaveFile($"\\{FOLDER_FOR_DATASET_STORAGE}", filename, sourceStream.ToArray(), out fileid);
            }
            catch (Exception ex)
            {
                string logmessage = $"{EX_MESSAGE_WHEN_SAVING} ex.message; {ex.Message}";
                //TO DO Logging must be done!:) 
                Logging(logmessage);
                throw;
            }
        }

        static T[] GetAllDeserializeObject<T>(string[] files)
        {
            T[] contents = new T[files.Length];
            for (int i = 0; i < files.Length; i++)
            {

                var content = JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(files[i]));
                if (content != null)
                    contents[i] = content;
            }
            return contents;
        }
    }
}
