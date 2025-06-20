using System;
using System.Collections.Concurrent;
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
        //private Dictionary<int, LocalNote> createdLocalNotes = [];
        private readonly ConcurrentDictionary<int, LocalNote> _createdLocalNotes = [];
        


        internal LocalNoteStorageAdapter(INodeStorageProvider storageProvider, INodeBuilder nodeBuilder, string pathToRootFolder, string subfolder) : base(storageProvider, nodeBuilder, pathToRootFolder, subfolder, "lnote")
        {
            //Load+= async (s, e) => await ReadNodes();
        }

        public IEnumerable<Note> GetNotesHasReferenceToIt(Note note)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Note> GetReferencedNotes(Note note)
        {
            throw new NotImplementedException();
        }

        protected override Node GetNodeFromNodeDataset(NodeDataset dataset)
        {
            LocalNote note = NewLocalNoteFromDatasetAsync((NoteDataset)dataset);
            loadedNodes[note.Uid] = note;
            return note;
        }

        protected override async Task ReadNodes()
        {
            var nodes = await GetAllObjectAsync<NoteDataset>();
            foreach (var node in nodes)
            {
                AddtoLoadedNodeDatasets(node);
            }
        }
        protected override async Task ReadNode(int uid)
        {
            NoteDataset? node = await GetObjectAsync<NoteDataset>(uid.ToString());
            if (node is not null)
                AddtoLoadedNodeDatasets(node);
        }

        private LocalNote NewLocalNoteFromDatasetAsync(NoteDataset dataset)
        {
            var owner =  nodeStorageProvider.GetUserStorage().GetUser(int.Parse(dataset.HasOwner));
            var parentNode = GetNode(int.Parse(dataset.HasParentNode));
            var note = nodeBuilder.NewLocalNote(parentNode, dataset.Name, dataset.Text);
            note.Uid = dataset.Uid;
            note.CreationDate = dataset.CreationDate;
            note.Description = dataset.Description;
            AddLocalNoteToCreatedLocalNotes(note);
            return note;
        }
        public async Task SaveNoteAsync(LocalNote note)
        {
            NoteDataset noteDS;
            noteDS = NewDatasetFromLocalNote(note);
            AddLocalNoteToCreatedLocalNotes(note);

            await SaveNodeAsync(noteDS, note);
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
            _createdLocalNotes[note.Uid] = note;
        }

        public async Task<LocalNote> GetNoteAsync(int Uid)
        {
            if (!_createdLocalNotes.TryGetValue(Uid, out LocalNote? note))
            {
                note = (LocalNote) await GetNodeAsync(Uid);
            }
            return note;
        }
        public void RemoveNote(LocalNote note)
        {
            _createdLocalNotes.Remove(note.Uid, out _);
            RemoveNode(note);
        }
    }
}
