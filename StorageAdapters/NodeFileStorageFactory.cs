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
        private LocalUserStorageAdapter userstorageAdapter;
        private LocalBoxStorageAdapter boxStorageAdapter;
        private Dictionary<int, LocalNoteStorageAdapter> noteStorageAdapters = [];
        private readonly INodeBuilder nodeBuilder;
        public NodeFileStorageFactory(INodeBuilder nodeBuilder, string profileFolder, string usersFolder="", string boxesFolder = "") {

            this.nodeBuilder = nodeBuilder;

            if (!Directory.Exists(profileFolder + "\\" + usersFolder))
            { 
                Directory.CreateDirectory(profileFolder + "\\" + usersFolder);
            }
            userstorageAdapter = new LocalUserStorageAdapter(this.nodeBuilder, profileFolder, usersFolder);
            userstorageAdapter.SetStorageFactory(this);

            if (!Directory.Exists(profileFolder + "\\" + boxesFolder))
            {
                Directory.CreateDirectory(profileFolder + "\\" + boxesFolder);
            }
            boxStorageAdapter = new LocalBoxStorageAdapter(this.nodeBuilder, profileFolder, boxesFolder);
            boxStorageAdapter.SetStorageFactory(this);

            userstorageAdapter.ReadNodes();
            boxStorageAdapter.ReadNodes();


        }

        public IBoxStorage GetBoxStorage()
        {
            return boxStorageAdapter;
        }

        public INoteStorage GetNoteStorage(LocalBox box)
        {
            if (!noteStorageAdapters.TryGetValue(box.Uid, out var storage))
            {
                storage = new LocalNoteStorageAdapter(nodeBuilder, box.Name, "");
                noteStorageAdapters.Add(box.Uid, storage);
                storage.SetStorageFactory(this);
                storage.ReadNodes();
            }
            return storage;
        }

        public IUserStorage GetUserStorage()
        {
            return userstorageAdapter;
        }

        internal void Dispose()
        {
            userstorageAdapter.Dispose();
            boxStorageAdapter.Dispose();
            noteStorageAdapters.Clear();
        }
    }
}
