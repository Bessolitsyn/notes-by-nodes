using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;

namespace notes_by_nodes.StorageAdapters
{
    internal class LocalNoteStorageAdapter : NodeStorageAdapter, INoteStorage
    {
        
        private Dictionary<int, LocalNote> createdLocalNotes = [];

        internal LocalNoteStorageAdapter(string pathToRootFolder, string subfolder, string fileExtension = "lnote") : base(pathToRootFolder, subfolder, fileExtension)
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
            var note = new LocalNote(parentNode, dataset.Name, dataset.Description);
            note.Uid = dataset.Uid;
            note.CreationDate = dataset.CreationDate;
            note.Text = dataset.Text;
           

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
            if (!createdLocalNotes.TryGetValue(Uid, out LocalNote note))
            {
                note = (LocalNote)GetNode(Uid);
            }
            return note;
        }
    }
}
