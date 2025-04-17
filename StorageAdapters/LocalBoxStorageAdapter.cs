using EasyObjectFileStorage;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.StorageAdapters
{
    internal class LocalBoxStorageAdapter : NodeStorageAdapter, IBoxStorage
    {
        //private static Dictionary<int, string> matchingBoxesUidToTheirFoldersList = [];

        INodeStorageFactory nodeStorageFactory;
        //private Dictionary<int, BoxDataset> loadedBoxDatasets = [];
        private Dictionary<int, LocalBox> createdLocalBoxes = [];

        public LocalBoxStorageAdapter(string pathToRootFolder, string subfolder, string fileExtension = "lbox") : base(pathToRootFolder, subfolder, fileExtension)
        {
            
        }
        public LocalBox GetBox(int Uid)
        {
            var box = createdLocalBoxes[Uid];
            if (box == null)
            {
                box = (LocalBox)GetNode(Uid);
            }
            return box;
        }

        public IEnumerable<Note> GetNotesHasReferenceToIt(Note note)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Note> GetReferencedNotes(Note note)
        {
            throw new NotImplementedException();
        }

        protected override Node GetLocalNodeFromDataset(NodeDataset userDataset, in Dictionary<int, Node> createdLoadedNodes)
        {
            LocalBox box = NewLocalBoxFromDataset((UserDataset)userDataset);
            createdLoadedNodes[box.Uid] = box;
            return box;
        }

        protected override void ReadNodes()
        {
            foreach (var item in GetAllObject<BoxDataset>())
            {
                loadedNodeDatasets.Add(item.Uid, item);
                //loadedBoxDatasets.Add(item.Uid, item);
            }
            
        }
        protected override void ReadNode(int uid)
        {
            BoxDataset node = GetObject<BoxDataset>(uid.ToString());
            loadedNodeDatasets.Add(node.Uid, node);
        }


        private LocalBox NewLocalBoxFromDataset(UserDataset dataset)
        {
            var users = nodeStorageFactory.GetUserStorage();
            var owner = users.GetUser(int.Parse(dataset.HasOwner));
            var box = new LocalBox(owner, dataset.Name, dataset.Description);
            box.Uid = dataset.Uid;
            box.CreationDate = dataset.CreationDate;
            box.Text = dataset.Text;
            box.SetNoteStorage(nodeStorageFactory.GetNoteStorage(box));

            AddLocalBoxToCreatedLocalBoxes(box);
            return box;
        }

        internal void SetStorageFactory(INodeStorageFactory factory)
        {
            nodeStorageFactory = factory;
        }

        internal void AddLocalBoxToCreatedLocalBoxes(LocalBox box)
        {
            createdLocalBoxes[box.Uid] = box;
        }

        //    this.UserStorage = userStorage;
        //}
    }
}
