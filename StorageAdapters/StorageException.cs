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
            "Ошибка при чтении или файл не существует",
            "Неизвестная ошибка",
            "Ошибка при удалении",
            "Папка хранилища не найдена"
            ];
        

        internal StorageException(string message) : base(message)
        {

        }
        public static StorageException NewException(StorageErrorCode errorCode)
        {
            if (Enumerable.Range(0, 2).Contains((int)errorCode))
                return new NoNodeInStorageException(StorageException.ErrorMessage[(int)errorCode]);
            if (Enumerable.Range(4, 1).Contains((int)errorCode))
                return new NoFolderStorageException(StorageException.ErrorMessage[(int)errorCode]);
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
    internal class NoFolderStorageException(string message) : StorageException(message)
    {
    }
    public enum StorageErrorCode : int
    {
        NoUser = 0,
        NoNode = 1,
        Unknown = 2,
        RemoveError = 3
    }
}
