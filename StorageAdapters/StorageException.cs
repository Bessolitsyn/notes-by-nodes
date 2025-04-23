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
        
        public static string[] ErrorMessage { get; } = [
            "Пользователь не найден",
            "Неизвестная ошибка",
            "Node не найден"
            ];
        

        internal StorageException(string message) : base(message)
        {

        }
        public static StorageException NewException(StorageErrorCode errorCode)
        {
            if (Enumerable.Range(0, 1).Contains((int)errorCode))
                return new NoNodeInStorageException(StorageException.ErrorMessage[(int)errorCode]);
            else
                return new UnknownStorageException();
        }
        
        
    }

    internal class NoNodeInStorageException(string message) : StorageException(message)
    {
    }
    internal class UnknownStorageException() : StorageException("Неизвестная ошибка")
    {
    }
    public enum StorageErrorCode : int
    {
        NoUser = 0,
        NoNode = 1,
        Unknown = 2,
    }
}
