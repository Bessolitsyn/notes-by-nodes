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
        public NodeFileStorageFactory(string profileFolder, string usersFolder="", string boxesFolder = "") {


            if (!Directory.Exists(profileFolder + "\\" + usersFolder))
            { 
                Directory.CreateDirectory(profileFolder + "\\" + usersFolder);
            }
            _userstorageAdapter = new LocalUserStorageAdapter(profileFolder, usersFolder);

            if (!Directory.Exists(profileFolder + "\\" + boxesFolder))
            {
                Directory.CreateDirectory(profileFolder + "\\" + boxesFolder);
            }
            _boxStorageAdapter = new LocalBoxStorageAdapter(profileFolder, boxesFolder);
            _boxStorageAdapter.SetStorageFactory(this);
           
        }

        public IBoxStorage GetBoxStorage()
        {
            return _boxStorageAdapter;
        }

        public INoteStorage GetNoteStorage(LocalBox box)
        {
            return new LocalNoteStorageAdapter(box.Name, "");            
        }

        public IUserStorage GetUserStorage()
        {
            return _userstorageAdapter;
        }
    }
}
