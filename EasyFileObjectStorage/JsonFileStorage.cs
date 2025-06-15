
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
        const string EX_MESSAGE_WHEN_DELETING = $"FileStorageMessage_ Error when deleting;";
        const string EX_MESSAGE_WHEN_READING = $"FileStorageMessage_ Error when reading;";
        readonly string FOLDER_FOR_DATASET_STORAGE;
        readonly string FILE_EXTENSION;


        public JsonFileStorage(string pathToRootFolder, string subfolder, string fileExtension = "json") : base(pathToRootFolder)
        {
            FOLDER_FOR_DATASET_STORAGE = subfolder;
            FILE_EXTENSION = fileExtension;
            
        }
        //REVIEW: лишний, наверное метод, достаточно GetAllObjects<T>
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
            //REVIEW: Можно добавить блок Finally чтобы вернуть new T[] { default } или default(T)
            //REVIEW: Тогда можно будет не бросать исключение для последующей обработки - есть же Logging
            try
            {
                //REVIEW: GetAllFiles можно свойством сделать. Проще читать будет.
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
        public bool TryGetObject<T>(string filename, out T? obj)
        {
            try
            {
                //REVIEW: GetAllFiles можно свойством сделать. Проще читать будет.
                string[] files = [.. GetAllFiles($"\\{FOLDER_FOR_DATASET_STORAGE}", FILE_EXTENSION).Where(f=>f.Contains(filename))];
                var contents = GetAllDeserializeObject<T>(files);

                if (contents.Length != 0)
                { 
                    obj = contents[0];
                    return true;
                }
                obj = default;
                return false;




            }
            catch (Exception ex)
            {
                string logmessage = $"{EX_MESSAGE_WHEN_READING} ex.message; {ex.Message}";
                Logging(logmessage);
                //TO DO Logging must be done!:) 
                throw;
            }
        }
        public void SaveNewObject(object obj)
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
                //REVIEW: Какой смысл в out var _ тут?
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
        public bool TryRemoveObject(string filename)
        {
            try
            {              
                filename = filename + "." + FILE_EXTENSION;
                return TryRemoveFile($"\\{FOLDER_FOR_DATASET_STORAGE}", filename);
            }
            catch (Exception ex)
            {
                string logmessage = $"{EX_MESSAGE_WHEN_DELETING} ex.message; {ex.Message}";
                Logging(logmessage);
                return false;
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
                //REVIEW: А если content == null
                if (content != null)
                    contents[i] = content;
            }
            return contents;
        }
    }
}
