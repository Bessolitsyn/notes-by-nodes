using Microsoft.Extensions.Options;
using notes_by_nodes.AppRules;
using notes_by_nodes.Service;
using notes_by_nodes.Storage;
using notes_by_nodes.StorageAdapters;
using notes_by_nodes_winUI.Settings;

namespace notes_by_nodes_winUI.Service
{
    internal class NodeFileStorageAdapter : INodeStorageProvider
    {
        private readonly NodeFileStorageProvider _storage;

        public NodeFileStorageAdapter(IOptions<NotesByNodesSettings> settings)
        {
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
