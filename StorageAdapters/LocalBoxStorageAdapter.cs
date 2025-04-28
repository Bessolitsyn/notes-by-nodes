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
using System.Xml.Linq;

namespace notes_by_nodes.StorageAdapters
{
    internal class LocalBoxStorageAdapter : NodeStorageAdapter, IBoxStorage
    {
        //private static Dictionary<int, string> matchingBoxesUidToTheirFoldersList = [];

        //private Dictionary<int, BoxDataset> loadedBoxDatasets = [];
        private Dictionary<int, LocalBox> createdLocalBoxes = [];

        internal LocalBoxStorageAdapter(INodeBuilder nodeBuilder, string pathToRootFolder, string subfolder) : base(nodeBuilder, pathToRootFolder, subfolder, "lbox")
        {
        }
        public LocalBox GetBox(int Uid)
        {
            if (!createdLocalBoxes.TryGetValue(Uid, out LocalBox box))
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

        protected override Node GetLocalNodeFromDataset(NodeDataset boxDataset, in Dictionary<int, Node> createdLoadedNodes)
        {
            LocalBox box = NewLocalBoxFromDataset(boxDataset);
            createdLoadedNodes[box.Uid] = box;
            return box;
        }

        internal override void ReadNodes()
        {
            foreach (var node in GetAllObject<BoxDataset>())
            {
                AddtoLoadedNodeDatasets(node);
            }

        }
        protected override void ReadNode(int uid)
        {
            if (TryGetObject<BoxDataset>(uid.ToString(), out BoxDataset? node))
                AddtoLoadedNodeDatasets(node);
        }


        private LocalBox NewLocalBoxFromDataset(NodeDataset dataset)
        {
            var users = nodeStorageFactory.GetUserStorage();
            var owner = users.GetUser(int.Parse(dataset.HasOwner));
            var box = nodeBuilder.NewLocalBox(owner, dataset.Name, dataset.Description);
            box.Uid = dataset.Uid;
            box.CreationDate = dataset.CreationDate;
            box.Text = dataset.Text;
            box.SetNoteStorage(nodeStorageFactory.GetNoteStorage(box));           

            AddLocalBoxToCreatedLocalBoxes(box);
            return box;
        }


        public void SaveBox(LocalBox box)
        {
            BoxDataset boxDS;
            boxDS = NewDatasetFromLocalBox(box);
            AddLocalBoxToCreatedLocalBoxes(box);

            SaveNode(boxDS);
        }
        private static BoxDataset NewDatasetFromLocalBox(LocalBox box)
        {
            BoxDataset boxDS = new(box.CreationDate,
                box.Description,
                box.Name,
                box.Text,
                box.Type,
                box.Uid,
                [.. box.HasChildNodes.Select(u => u.Uid.ToString())],
                box.HasOwner.Uid.ToString(),
                box.HasParentNode.Uid.ToString()
                );
            return boxDS;
        }
        internal void AddLocalBoxToCreatedLocalBoxes(LocalBox box)
        {
            createdLocalBoxes[box.Uid] = box;
        }

        public async Task SaveBoxAsync(LocalBox box)
        {
            await Task.Run(()=>SaveBox(box));
        }
    }

    //    this.UserStorage = userStorage;

}
