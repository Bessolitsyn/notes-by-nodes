using Microsoft.Extensions.Options;
using notes_by_nodes.AppRules;
using notes_by_nodes.Service;
using notes_by_nodes.Storage;
using notes_by_nodes.StorageAdapters;
using notes_by_nodes_wpfApp.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes_wpfApp.Services
{
    internal class NodeFileStorageAdapter: INodeStorageProvider
    {
        private readonly NodeFileStorageProvider _storage;
        public NodeFileStorageAdapter(IOptions<NotesByNodesSettings> settings) 
        {
#warning TODO наверно без ноде билдера надо обойтись
            INodeBuilder nodeBuilder = new NodeBuilder();

            string userProfile = settings.Value.UserProfile;
            _storage = new NodeFileStorageProvider(nodeBuilder, userProfile);

        }

        public IBoxStorage GetBoxStorage()
        {
            return _storage.GetBoxStorage();
        }

        public INoteStorage GetNoteStorage(LocalBox box)
        {
            return _storage.GetNoteStorage(box);
        }

        public IUserStorage GetUserStorage()
        {
            return _storage.GetUserStorage();
        }
    }
}
