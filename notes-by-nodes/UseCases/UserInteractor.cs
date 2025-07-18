﻿using notes_by_nodes.AppRules;
using notes_by_nodes.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.UseCases
{
    internal class UserInteractor(INodeStorageProvider storageFactory)
    {
        protected INodeStorageProvider StorageFactory { get; init; } = storageFactory ?? throw new ArgumentNullException();

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
        internal LocalUser GetUser(int uid)
        {
            var storage = storageFactory.GetUserStorage();
            var user = storage.GetUser(uid);
            StorageFactory.GetBoxStorage().LoadChildNodes(user);
            return user;
        }
        internal void SaveUser(LocalUser user)
        {
            StorageFactory.GetUserStorage().SaveUser(user);
        }
    }
}

