using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static notes_by_nodes.Storage.StorageException;

namespace notes_by_nodes.Storage
{
    internal class StorageException : Exception
    {
        static string[] ErrorMessage { get; } = [
            "Пользователь не найден",
            "Неизвестная ошибка"
            ];
        

        private StorageException(string message) : base(message)
        {

        }
        public static StorageException NewException(StorageErrorCode errorCode)
        {
            return new StorageException(StorageException.ErrorMessage[(int)errorCode]);
        }
        
        
    }
    public enum StorageErrorCode : int
    {
        NoUser = 0,
        Unknown = 1
    }
}
