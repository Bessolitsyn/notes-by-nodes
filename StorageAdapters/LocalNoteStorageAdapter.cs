using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;

namespace notes_by_nodes.StorageAdapters
{
    internal class LocalNoteStorageAdapter : NodeStorageAdapter, INoteStorage
    {
        public LocalNoteStorageAdapter(string pathToRootFolder, string subfolder, string fileExtension = "lnote") : base(pathToRootFolder, subfolder, fileExtension)
        {
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
            throw new NotImplementedException();
        }

        protected override void ReadNode(int uid)
        {
            throw new NotImplementedException();
        }

        protected override void ReadNodes()
        {
            throw new NotImplementedException();
        }
    }
}
