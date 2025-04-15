using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyObjectFileStorage;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;

namespace notes_by_nodes.StorageAdapters
{
    internal class LocalUserStorageAdapter : JsonFileStorage, IUserStorage
    {
        private static Dictionary<int, string> matchingBoxesUidToTheirFoldersList = [];

        private static UserDataset[] loadedUsers = [];

        public LocalUserStorageAdapter(string pathToRootFolder, string subfolder, string fileExtension = "*.luser") : base(pathToRootFolder, subfolder, fileExtension)
        {
            loadedUsers = GetAllObject<UserDataset>();
        }

        public IEnumerable<LocalUser> GetBoxes(LocalBox parentNode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Node> GetChildNodes(Node parentNode)
        {
            throw new NotImplementedException();
        }

        public LocalUser GetUser(int Uid)
        {
            var userDataset = loadedUsers.FirstOrDefault(u => u.Uid == Uid);
            if (userDataset != null)
            {
                var user = new LocalUser(userDataset.Name, userDataset.Description, this);
                user.Uid = Uid;
                user.CreationDate = userDataset.CreationDate;
                user.CreationDate = userDataset.CreationDate;
                foreach (string item in userDataset.IsOwnerOf)
                {
                    int boxUid=int.Parse(item);
                    IBoxStorage boxStorage = NodeStorageFactory.Instance.GetBoxStorage(matchingBoxesUidToTheirFoldersList[boxUid]);
                    var boxDataset = boxStorage.GetBox(boxUid);
                    LocalBox box = new LocalBox(user, boxStorage, boxDataset.Name, boxDataset.Description);
                    box.Uid = boxUid;
                }
                return user;
            }
            else
                throw StorageException.NewException(StorageErrorCode.NoUser);
        }
    }
}
