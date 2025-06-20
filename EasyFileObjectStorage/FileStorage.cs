using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;

namespace EasyObjectFileStorage
{
    public abstract class FileStorage(string pathToRootFolder)
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _fileLockers = [];
        readonly string _rootFolder = pathToRootFolder;

        public event EventHandler<string>? LogEvent;

        protected async Task<string> SaveFileAsync(string path, string filename, string fileContent)
        {
            var fileid = path + filename;
            var filePath = _rootFolder + path + filename;
            await SafeFileActionAsync(filePath, async () => await System.IO.File.WriteAllTextAsync(filePath, fileContent));
            return fileid;

        }
        protected void SaveFile(string path, string filename, string fileContent, out string fileid)
        {
            fileid = path + filename;
            System.IO.File.WriteAllText(_rootFolder + path + filename, fileContent);
        }       
        protected void SaveFile(string path, string filename, byte[] fileContent, out string fileid)
        {
            fileid = path + filename;
            System.IO.File.WriteAllBytes(_rootFolder + path + filename, fileContent);
        }
        protected async Task<string> SaveFileAsync(string path, string filename, byte[] fileContent)
        {
            var fileid = path + filename;
            var filePath = _rootFolder + path + filename;
            await SafeFileActionAsync(filePath, async () =>  await System.IO.File.WriteAllBytesAsync(filePath, fileContent));
            return fileid;

        }
        protected void RemoveFile(string path, string filename)
        {
            var filePath = _rootFolder + path + filename;
            var _ = SafeFileActionAsync(filePath, () => Task.Run(() => File.Delete(filePath)));
           
        }

        protected void Logging(string message)
        {
            LogEvent?.Invoke(this, message);
        }

        protected string[] GetAllFiles(string path, string fileExt)
        {
            string[] files = Directory.GetFiles(_rootFolder + path, $"*.{fileExt}");
            return files;
        }

        protected static async Task SafeFileActionAsync(string filePath, Func<Task> operation)
        {
            SemaphoreSlim locker = _fileLockers.GetOrAdd(filePath, _ => new SemaphoreSlim(1, 1));
            await locker.WaitAsync();
            try
            {
                await operation();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                locker.Release();

                if (locker.CurrentCount == 1)
                {
                    _fileLockers.TryRemove(filePath, out _);
                }
            }
        }







    }
}
