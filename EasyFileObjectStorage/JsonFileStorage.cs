
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
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

        public async Task<T[]> GetAllObjectAsync<T>()
        {
            try
            {
                string[] files = GetAllFiles($"\\{FOLDER_FOR_DATASET_STORAGE}", FILE_EXTENSION);
                var objects = await GetAllDeserializeObjectAsync<T>(files);
                return objects;
            }
            catch (Exception ex)
            {
                string logmessage = $"{EX_MESSAGE_WHEN_READING} ex.message; {ex.Message}";
                Logging(logmessage);
                //TO DO Logging must be done!:) 
                throw;
            }
        }
        public async Task<T?> GetObjectAsync<T>(string filename)
        {
            try
            {
                string[] files = [.. GetAllFiles($"\\{FOLDER_FOR_DATASET_STORAGE}", FILE_EXTENSION).Where(f => f.Contains(filename))];
                var contents = await GetAllDeserializeObjectAsync<T>(files);

                if (contents.Length != 0)
                {
                    return contents[0];
                }
                return default;
            }
            catch (Exception ex)
            {
                string logmessage = $"{EX_MESSAGE_WHEN_READING} ex.message; {ex.Message}";
                Logging(logmessage);
                //TO DO Logging must be done!:) 
                throw;
            }
        }
        public async Task SaveNewObjectAsync(object obj)
        {
            var filename = obj.GetHashCode().ToString(CultureInfo.InvariantCulture);
            await SaveObjectAsync(obj, filename);
        }
        public async Task SaveObjectAsync(object obj, string filename)
        {
            try
            {
                var fileContent = JsonConvert.SerializeObject(obj, Formatting.Indented);
                filename = filename + "." + FILE_EXTENSION;
                var _ = await SaveFileAsync($"\\{FOLDER_FOR_DATASET_STORAGE}", filename, fileContent);
            }
            catch (Exception ex)
            {
                string logmessage = $"{EX_MESSAGE_WHEN_SAVING} ex.message; {ex.Message}";
                Logging(logmessage);
                //TO DO Logging must be done!:) 
                throw;
            }
        }
        public void RemoveObject(string filename)
        {
            try
            {
                filename = filename + "." + FILE_EXTENSION;
                RemoveFile($"\\{FOLDER_FOR_DATASET_STORAGE}", filename);
            }
            catch (Exception ex)
            {
                string logmessage = $"{EX_MESSAGE_WHEN_DELETING} ex.message; {ex.Message}";
                Logging(logmessage);
                throw;
            }
        }

        public async Task<string> SaveObjectStreamToFileAsync(MemoryStream sourceStream, string filename)
        {
            try
            {
                filename = filename + "." + FILE_EXTENSION;
                var fileid = await SaveFileAsync($"\\{FOLDER_FOR_DATASET_STORAGE}", filename, sourceStream.ToArray());
                return fileid;
            }
            catch (Exception ex)
            {
                string logmessage = $"{EX_MESSAGE_WHEN_SAVING} ex.message; {ex.Message}";
                //TO DO Logging must be done!:) 
                Logging(logmessage);
                throw;
            }
        }

        static async Task<T[]> GetAllDeserializeObjectAsync<T>(string[] files)
        {
            T[] objects = new T[files.Length];
            for (int i = 0; i < files.Length; i++)
            {                
                await SafeFileActionAsync(files[i], async () =>
                {
                    var content = await System.IO.File.ReadAllTextAsync(files[i]);
                    var obj = JsonConvert.DeserializeObject<T>(content  );
                    if (obj != null)
                        objects[i] = obj;
                });
            }
            return objects;
        }
    }
}
