using EasyObjectFileStorage;
using notes_by_nodes.AppRules;
using notes_by_nodes.Dto;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace notes_by_nodes.StorageAdapters
{
    internal class LocalUserStorageAdapter : NodeStorageAdapter, IUserStorage
    {
        
        private readonly Dictionary<string, LocalUser> _users = [];        
        private readonly Dictionary<string, UserDataset> _userDatasets = [];        
        private IBoxStorage BoxStorage => nodeStorageProvider.GetBoxStorage();


        internal LocalUserStorageAdapter(INodeStorageProvider storageProvider, INodeBuilder nodeBuilder, string pathToRootFolder, string subfolder) : base(storageProvider, nodeBuilder, pathToRootFolder, subfolder, "luser")
        {
            //loadedUsers = GetAllObjectAsync<UserDataset>();
            //ReadNodes();
            //createdLocalUsers.Clear();

        }

        
        public async Task<LocalUser> GetUser(string name)
        {
            if (_userDatasets.Count == 0)
            await ReadNodes();

            if (_userDatasets.TryGetValue(name, out var userDS))
            {

                var user = GetUser(userDS.Uid);
                var _ = await BoxStorage.LoadChildNodesAsync(user);
                return user;
            }
            else
                //throw StorageException.NewException(StorageErrorCode.NoUser);
                throw new NoUsersNoteCoreException();
        }

        public LocalUser GetUser(int Uid)
        {
            LocalUser user = (LocalUser)GetNode(Uid);
            return user;

        }

        //public LocalUser GetUser(string name)
        //{
        //    return [.. _users.Values];
        //}

        //private LocalBox GetLocalBox(string uid)
        //{
        //    int boxUid = int.Parse(uid);
        //    var box = BoxStorage.GetBox(boxUid);
        //    return box;

        //}

        protected override async Task ReadNodes()
        {
            var nodes = await GetAllObjectAsync<UserDataset>();
            foreach (var node in nodes)
            {
                AddtoLoadedNodeDatasets(node);
                _userDatasets[node.Name] = node;
            }
        }
        protected override async Task ReadNode(int uid)
        {
            UserDataset? node = await GetObjectAsync<UserDataset>(uid.ToString());
            if (node is not null)
                AddtoLoadedNodeDatasets(node);
        }

        private void SetIsOwnerOf(LocalUser user)
        {
            //UserDataset dataset =(UserDataset) GetNodeDataset(user.Uid);
            UserDataset dataset = GetNodeDataset(user.Uid) as UserDataset;
            AddNodesToOwnerOf(user, [.. GetNodes(dataset.IsOwnerOf, out var _)]);
        }

        private void AddNodesToOwnerOf(LocalUser user, Node[] nodes)
        {
            foreach (var node in nodes)
            {
                user.AddIntoOwnerOf(node);                
            }

        }

        public async Task SaveUserAsync (LocalUser user)
        {
            UserDataset userDS;
            userDS = NewDatasetFromLocalUser(user);
            AddUserToCreatedUsersDict(user);
            _userDatasets[user.Name] = userDS;
            await SaveNodeAsync(userDS, user);

        }
        internal void AddUserToCreatedUsersDict(LocalUser user)
        {
            _users[user.Name] = user;
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
        protected override Node? GetNodeFromNodeDataset(NodeDataset userDataset)
        {
            if (userDataset is UserDataset userDS)
            {
                var user = GetUserFromDataset(userDS);
                AddtoLoadedNodesDict(user);
                return user;
            }
            return default;
        }
        private LocalUser GetUserFromDataset(UserDataset dataset)
        {
            var user = nodeBuilder.NewLocalUser(dataset.Name, dataset.Description, BoxStorage);
            user.Uid = dataset.Uid;
            user.CreationDate = dataset.CreationDate;
            SetIsOwnerOf(user);
            AddUserToCreatedUsersDict(user);

            return user;
        }
    }
}
