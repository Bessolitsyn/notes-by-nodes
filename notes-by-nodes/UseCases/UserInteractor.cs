using notes_by_nodes.AppRules;
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

        internal async Task<LocalUser> GetUser(string name)
        {
            var userStorage = storageFactory.GetUserStorage();            
            var user = await userStorage.GetUser(name);            
            return user;
        }
        internal async Task<LocalUser> MakeUser(string name, string email)
        {
            LocalUser user = new(name, email, StorageFactory.GetBoxStorage());
            var userStorage = storageFactory.GetUserStorage();
            await userStorage.SaveUserAsync(user);
            return user;
        }
        //internal async Task<LocalUser> GetUser(int uid)
        //{
        //    var storage = storageFactory.GetUserStorage();
        //    var user = storage.GetUser(uid);
        //    await StorageFactory.GetBoxStorage().LoadChildNodesAsync(user);
        //    return user;
        //}
        internal Task SaveUser(LocalUser user)
        {
            return StorageFactory.GetUserStorage().SaveUserAsync(user);
        }
    }
}

