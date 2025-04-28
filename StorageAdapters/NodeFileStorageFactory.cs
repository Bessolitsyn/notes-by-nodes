using notes_by_nodes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Storage;
using System.Runtime.CompilerServices;
using System.IO;
using notes_by_nodes.UseCases.AppRules;
[assembly: InternalsVisibleTo("TestProject")]
namespace notes_by_nodes.StorageAdapters
{
    public class NodeFileStorageFactory: INodeStorageFactory
    {
        //public static INodeStorageFactory Instance { get => instance; }
        //public static INodeStorageFactory instance;
        private LocalUserStorageAdapter _userstorageAdapter;
        private LocalBoxStorageAdapter _boxStorageAdapter;
        private Dictionary<int, LocalNoteStorageAdapter> _noteStorageAdapters = [];
        private readonly INodeBuilder nodeBuilder;
        public NodeFileStorageFactory(INodeBuilder nodeBuilder, string profileFolder, string usersFolder="", string boxesFolder = "") {

            this.nodeBuilder = nodeBuilder;

            if (!Directory.Exists(profileFolder + "\\" + usersFolder))
            { 
                Directory.CreateDirectory(profileFolder + "\\" + usersFolder);
            }
            _userstorageAdapter = new LocalUserStorageAdapter(this.nodeBuilder, profileFolder, usersFolder);
            _userstorageAdapter.SetStorageFactory(this);

            if (!Directory.Exists(profileFolder + "\\" + boxesFolder))
            {
                Directory.CreateDirectory(profileFolder + "\\" + boxesFolder);
            }
            _boxStorageAdapter = new LocalBoxStorageAdapter(this.nodeBuilder, profileFolder, boxesFolder);
            _boxStorageAdapter.SetStorageFactory(this);

            _userstorageAdapter.ReadNodes();
            _boxStorageAdapter.ReadNodes();


        }

        public IBoxStorage GetBoxStorage()
        {
            return _boxStorageAdapter;
        }

        public INoteStorage GetNoteStorage(LocalBox box)
        {
            if (!_noteStorageAdapters.TryGetValue(box.Uid, out var storage))
            {
                storage = new LocalNoteStorageAdapter(nodeBuilder, box.Name, "");
                _noteStorageAdapters.Add(box.Uid, storage);
                storage.SetStorageFactory(this);
                storage.ReadNodes();
            }
            return storage;
        }

        public IUserStorage GetUserStorage()
        {
            return _userstorageAdapter;
        }

        internal void Dispose()
        {
            _userstorageAdapter.Dispose();
            _boxStorageAdapter.Dispose();
            _noteStorageAdapters.Clear();
        }
    }
}
