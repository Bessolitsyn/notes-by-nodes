using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;

namespace EasyObjectFileStorage
{
    public abstract class FileStorage
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _fileLockers = [];
        readonly DirectoryInfo _rootFolder;

        public event EventHandler<string>? LogEvent;
        public FileStorage(string pathToRootFolder)
        {
            _rootFolder = new DirectoryInfo(pathToRootFolder.TrimEnd());
            if (!_rootFolder.Exists) _rootFolder.Create();
        }

        protected async Task<string> SaveFileAsync(string path, string filename, string fileContent)
        {
            async Task WriteAllTextAsync(FileInfo file, string content)
            {
                using var s = file.CreateText();
                await s.WriteAsync(content);
                s.Close();
            }
            var fileid = Path.Combine(path, filename);
            var filePath = new FileInfo(Path.Combine(_rootFolder.FullName, fileid));

            await SafeFileActionAsync(filePath.FullName, async () => await WriteAllTextAsync(filePath, fileContent));
            return fileid;

        }

        protected async Task<string> SaveFileAsync(string path, string filename, byte[] fileContent)
        {
            async Task WriteAllBytesAsync(FileInfo file, byte[] content)
            {
                using var fs = file.Create();
                fs.Position = 0;
                await fs.WriteAsync(content, 0, content.Length);
                fs.Close();

            }
            var fileid = Path.Combine(path, filename);
            var filePath = new FileInfo(Path.Combine(_rootFolder.FullName, fileid));
            await SafeFileActionAsync(filePath.FullName, async () =>  await WriteAllBytesAsync(filePath, fileContent));
            return fileid;

        }
        protected void RemoveFile(string path, string filename)
        {
            var fileid = Path.Combine(path, filename);
            var filePath = new FileInfo(Path.Combine(_rootFolder.FullName, fileid));

            var _ = SafeFileActionAsync(filePath.FullName, () => Task.Run(() => filePath.Delete()));           
        }

        protected void Logging(string message)
        {
            LogEvent?.Invoke(this, message);
        }

        protected FileInfo[] GetAllFiles(string path, string fileExt)
        {
            var dirInfo = new DirectoryInfo(Path.Join(_rootFolder.FullName, path.TrimEnd()));
            var  files = dirInfo.GetFiles($"*.{fileExt}");//Directory.GetFiles(_rootFolder + path, $"*.{fileExt}");
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
