using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.UseCases
{
    internal class UserInteractor(INodeStorageFactory storageFactory)
    {
        protected INodeStorageFactory StorageFactory { get; init; } = storageFactory ?? throw new ArgumentNullException();

        internal LocalUser[] GetUsers()
        {
            var userStorage = storageFactory.GetUserStorage();
            var users = userStorage.GetUsers();            
            return users;
        }
        internal LocalUser MakeUser(string name, string email)
        {
            LocalUser user = new(name, email, StorageFactory.GetBoxStorage());
            var userStorage = storageFactory.GetUserStorage();
            userStorage.SaveUser(user);
            return user;
        }
    }
}

