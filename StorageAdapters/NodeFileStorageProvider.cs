using notes_by_nodes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Storage;
using System.Runtime.CompilerServices;
using System.IO;
using notes_by_nodes.AppRules;
[assembly: InternalsVisibleTo("TestProject")]
namespace notes_by_nodes.StorageAdapters
{
    public class NodeFileStorageProvider: INodeStorageProvider
    {
        //public static INodeStorageProvider Instance { get => instance; }
        //public static INodeStorageProvider instance;
        private LocalUserStorageAdapter userstorageAdapter;
        private LocalBoxStorageAdapter boxStorageAdapter;
        private Dictionary<int, LocalNoteStorageAdapter> noteStorageAdapters = [];
        private readonly INodeBuilder nodeBuilder;
        public NodeFileStorageProvider(INodeBuilder nodeBuilder, string profileFolder, string usersFolder="", string boxesFolder = "") {

            this.nodeBuilder = nodeBuilder;

            if (!Directory.Exists(profileFolder + "\\" + usersFolder))
            { 
                Directory.CreateDirectory(profileFolder + "\\" + usersFolder);
            }
            userstorageAdapter = new LocalUserStorageAdapter(this, this.nodeBuilder, profileFolder, usersFolder);
            

            if (!Directory.Exists(profileFolder + "\\" + boxesFolder))
            {
                Directory.CreateDirectory(profileFolder + "\\" + boxesFolder);
            }
            boxStorageAdapter = new LocalBoxStorageAdapter(this, this.nodeBuilder, profileFolder, boxesFolder);            


        }

        public IBoxStorage GetBoxStorage()
        {
            return boxStorageAdapter;
        }

        public INoteStorage GetNoteStorage(LocalBox box)
        {
            if (!noteStorageAdapters.TryGetValue(box.Uid, out var storage))
            {
                storage = new LocalNoteStorageAdapter(this, nodeBuilder, box.Name, "");
                noteStorageAdapters.Add(box.Uid, storage);
                //storage.OnLoad();
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
