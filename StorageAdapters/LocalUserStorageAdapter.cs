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
        
        private readonly Dictionary<int, LocalUser> createdLocalUsers = [];        
        private IBoxStorage BoxStorage => nodeStorageFactory.GetBoxStorage();


        internal LocalUserStorageAdapter(INodeBuilder nodeBuilder, string pathToRootFolder, string subfolder) : base(nodeBuilder, pathToRootFolder, subfolder, "luser")
        {
            //loadedUsers = GetAllObject<UserDataset>();
            //ReadNodes();
            //createdLocalUsers.Clear();
            
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
            var box = BoxStorage.GetBox(boxUid);
            return box;

        }

        internal override void ReadNodes()
        {
            foreach (var item in GetAllObject<UserDataset>())
            {
                AddtoLoadedNodeDatasets(item);
                LocalUser user = (LocalUser)GetNode(item.Uid);
                AddLocalUserToCreatedLocalUsers(user);
            }
        }
        protected override void ReadNode(int uid)
        {
            //UserDataset node;
            if (TryGetObject<UserDataset>(uid.ToString(), out UserDataset? node))
                AddtoLoadedNodeDatasets(node);
            //loadedUserDatasets.Add(node.Uid, node);
        }

        private LocalUser NewLocalUserFromDataset(UserDataset dataset)
        {
            var user = nodeBuilder.NewLocalUser(dataset.Name, dataset.Description, BoxStorage);
            user.Uid = dataset.Uid;
            user.CreationDate = dataset.CreationDate;
            SetIsOwnerOf(user);
            AddLocalUserToCreatedLocalUsers(user);
            return user;
        }

        private void SetIsOwnerOf(LocalUser user)
        {
            UserDataset dataset =(UserDataset)GetNodeDataset(user.Uid);
            AddNodesToOwnerOf(user, [.. GetNodes(dataset.IsOwnerOf, out var _)]);
        }

        private void AddNodesToOwnerOf(LocalUser user, Node[] nodes)
        {
            foreach (var node in nodes)
            {
                user.AddIntoOwnerOf(node);
                
            }

        }

        public void SaveUser(LocalUser user)
        {
            UserDataset userDS;
            userDS = NewDatasetFromLocalUser(user);
            AddLocalUserToCreatedLocalUsers(user);
            SaveNode(userDS);

        }
        internal void AddLocalUserToCreatedLocalUsers(LocalUser user)
        {
            createdLocalUsers[user.Uid] = user;
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
