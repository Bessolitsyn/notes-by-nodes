using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.AppRules;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;

namespace notes_by_nodes.StorageAdapters
{
    internal class LocalNoteStorageAdapter : NodeStorageAdapter, INoteStorage
    {        
        //kind of cashing 
        private Dictionary<int, LocalNote> createdLocalNotes = [];
        


        internal LocalNoteStorageAdapter(INodeBuilder nodeBuilder, string pathToRootFolder, string subfolder) : base(nodeBuilder, pathToRootFolder, subfolder, "lnote")
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

        protected override Node GetLocalNodeFromDataset(NodeDataset dataset, in Dictionary<int, Node> createdLoadedNodes)
        {
            LocalNote note = NewLocalNoteFromDataset((NoteDataset)dataset);
            createdLoadedNodes[note.Uid] = note;
            return note;
        }

        protected override void ReadNode(int uid)
        {
            if (TryGetObject<NoteDataset>(uid.ToString(), out NoteDataset? node))
                AddtoLoadedNodeDatasets(node);
        }

        internal override void ReadNodes()
        {
            foreach (var node in GetAllObject<NoteDataset>())
            {
                AddtoLoadedNodeDatasets(node);
            }
        }
        private LocalNote NewLocalNoteFromDataset(NoteDataset dataset)
        {
            var owner = nodeStorageFactory.GetUserStorage().GetUser(int.Parse(dataset.HasOwner));
            var parentNode = GetNode(int.Parse(dataset.HasParentNode));
            var note = nodeBuilder.NewLocalNote(parentNode, dataset.Name, dataset.Text);
            note.Uid = dataset.Uid;
            note.CreationDate = dataset.CreationDate;
            note.Description = dataset.Description;
           

            AddLocalNoteToCreatedLocalNotes(note);
            return note;
        }
        public void SaveNote(LocalNote note)
        {
            NoteDataset noteDS;
            noteDS = NewDatasetFromLocalNote(note);
            AddLocalNoteToCreatedLocalNotes(note);

            SaveNode(noteDS);
        }
        private static NoteDataset NewDatasetFromLocalNote(LocalNote note)
        {
            NoteDataset noteDS = new(note.CreationDate,
                note.Description,
                note.Name,
                note.Text,
                note.Type,
                note.Uid,
                [.. note.HasChildNodes.Select(u => u.Uid.ToString())],
                note.HasOwner.Uid.ToString(),
                note.HasParentNode.Uid.ToString(),
                [],
                [.. note.HasReference.Select(u => u.Uid.ToString())],
                [.. note.IsRefernced.Select(u => u.Uid.ToString())]
                );
            return noteDS;
        }
        internal void AddLocalNoteToCreatedLocalNotes(LocalNote note)
        {
            createdLocalNotes[note.Uid] = note;
        }

        public LocalNote GetNote(int Uid)
        {
            if (!createdLocalNotes.TryGetValue(Uid, out LocalNote? note))
            {
                note = (LocalNote)GetNode(Uid);
            }
            return note;
        }

        public async Task SaveNoteAsync(LocalNote note)
        {
            await Task.Run(()=>SaveNote(note));
        }

        public void RemoveNote(LocalNote note)
        {
            createdLocalNotes.Remove(note.Uid);
            RemoveNode(note);
        }
    }
}
