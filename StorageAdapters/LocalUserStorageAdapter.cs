using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EasyObjectFileStorage;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;

namespace notes_by_nodes.StorageAdapters
{
    internal class LocalUserStorageAdapter : NodeStorageAdapter, IUserStorage
    {
        //private static Dictionary<int, string> matchingBoxesUidToTheirFoldersList = [];

        //private Dictionary<int, UserDataset> loadedUserDatasets = [];
        private Dictionary<int, LocalUser> createdLocalUsers = [];

        INodeStorageFactory nodeStorageFactory;
        LocalBoxStorageAdapter boxStorage;





        public LocalUserStorageAdapter(string pathToRootFolder, string subfolder, string fileExtension = "luser") : base(pathToRootFolder, subfolder, fileExtension)
        {
            //loadedUsers = GetAllObject<UserDataset>();
            //ReadNodes();
            //createdLocalUsers.Clear();
            boxStorage = new LocalBoxStorageAdapter(pathToRootFolder, subfolder);
        }

        public IEnumerable<Node> GetChildNodes(Node parentNode)
        {
            throw new NotImplementedException();
        }

        public LocalUser GetUser(int Uid)
        {

            var user = createdLocalUsers[Uid];
            if (user == null)
            {
                user = (LocalUser)GetNode(Uid);
            }
            return user;

        }

        public LocalUser[] GetUsers()
        {
            return [.. createdLocalUsers.Values];
        }

        private LocalBox GetLocalBox(string uid)
        {
            int boxUid = int.Parse(uid);
            var box = boxStorage.GetBox(boxUid);
            return box;

        }

        protected override void ReadNodes()
        {
            foreach (var item in GetAllObject<UserDataset>())
            {

                loadedNodeDatasets[item.Uid] = item;
                LocalUser user = (LocalUser)GetNode(item.Uid);
                AddLocalUserToCreatedLocalUsers(user);
            }
        }
        protected override void ReadNode(int uid)
        {
            UserDataset node = GetObject<UserDataset>(uid.ToString());
            loadedNodeDatasets.Add(node.Uid, node);
            //loadedUserDatasets.Add(node.Uid, node);
        }

        internal void SetStorageFactory(INodeStorageFactory factory)
        {
            nodeStorageFactory = factory;
        }
        private LocalUser NewLocalUserFromDataset(UserDataset dataset)
        {
            var user = new LocalUser(dataset.Name, dataset.Description, boxStorage);
            user.Uid = dataset.Uid;
            user.CreationDate = dataset.CreationDate;
            foreach (string item in dataset.IsOwnerOf)
            {
                var localBox = GetLocalBox(item);
                user.AddIntoOwner(localBox);
            }

            AddLocalUserToCreatedLocalUsers(user);
            return user;
        }

        internal void AddLocalUserToCreatedLocalUsers(LocalUser user)
        {
            createdLocalUsers[user.Uid] = user;
        }

        public void SaveUser(LocalUser user)
        {
            UserDataset userDS;
            userDS = NewDatasetFromLocalUser(user);
            AddLocalUserToCreatedLocalUsers(user);

            SaveNode(userDS);

        }
        private static UserDataset NewDatasetFromLocalUser(LocalUser user)
        {
            UserDataset userds = new(user.CreationDate,
                user.Email,
                user.Name,
                user.Text,
                user.Type,
                user.Uid,
                [.. user.HasChildNodes.Select(u => u.Uid.ToString())],
                user.HasOwner.Uid.ToString(),
                user.HasParentNode.Uid.ToString(),
                [.. user.IsOwnerOf.Select(u => u.Uid.ToString())]
                );
            return userds;
        }
        protected override Node GetLocalNodeFromDataset(NodeDataset userDataset, in Dictionary<int, Node> createdLoadedNodes)
        {
            if (userDataset is UserDataset userDS)
            {
                var user = NewLocalUserFromDataset(userDS);                
                return user;
            }
            return null;
        }
    }
}
