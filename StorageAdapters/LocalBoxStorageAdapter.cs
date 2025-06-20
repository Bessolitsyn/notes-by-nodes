using EasyObjectFileStorage;
using notes_by_nodes.AppRules;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace notes_by_nodes.StorageAdapters
{
    internal class LocalBoxStorageAdapter : NodeStorageAdapter, IBoxStorage
    {
        //private static Dictionary<int, string> matchingBoxesUidToTheirFoldersList = [];

        //private Dictionary<int, BoxDataset> loadedBoxDatasets = [];
        private Dictionary<int, LocalBox> _createdLocalBoxes = [];

        internal LocalBoxStorageAdapter(INodeStorageProvider storageProvider, INodeBuilder nodeBuilder, string pathToRootFolder, string subfolder) : base(storageProvider, nodeBuilder, pathToRootFolder, subfolder, "lbox")
        {

        }
        public async Task<LocalBox> GetBoxAsync(int Uid)
        {
            //if (_createdLocalBoxes.Count == 0)
            //    ReadNodes().Wait();

            if (!_createdLocalBoxes.TryGetValue(Uid, out LocalBox box))
            {
                box = (LocalBox) await GetNodeAsync(Uid);
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

        protected override Node GetNodeFromNodeDataset(NodeDataset boxDataset)
        {
            LocalBox box = NewLocalBoxFromDataset(boxDataset);
            loadedNodes[box.Uid] = box;
            return box;
        }

        protected override async Task ReadNodes()
        {
            var nodes = await GetAllObjectAsync<BoxDataset>();
            foreach (var node in nodes)
            {
                AddtoLoadedNodeDatasets(node);
            }
        }
        protected override async Task ReadNode(int uid)
        {
            BoxDataset? node = await GetObjectAsync<BoxDataset>(uid.ToString());
            if (node is not null)
                AddtoLoadedNodeDatasets(node);
        }
        private LocalBox NewLocalBoxFromDataset(NodeDataset dataset)
        {
            var users = nodeStorageProvider.GetUserStorage();
            var owner = users.GetUser(int.Parse(dataset.HasOwner));
            var box = nodeBuilder.NewLocalBox(owner, dataset.Name, dataset.Description);
            box.Uid = dataset.Uid;
            box.CreationDate = dataset.CreationDate;
            box.Text = dataset.Text;
            //box.SetNoteStorage(nodeStorageProvider.GetNoteStorage(box));           

            AddLocalBoxToCreatedLocalBoxes(box);
            return box;
        }


        public async Task SaveBoxAsync(LocalBox box)
        {
            BoxDataset boxDS;
            boxDS = NewDatasetFromLocalBox(box);
            AddLocalBoxToCreatedLocalBoxes(box);

            await SaveNodeAsync(boxDS, box);
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
            _createdLocalBoxes[box.Uid] = box;
        }

    }

    //    this.UserStorage = userStorage;

}
