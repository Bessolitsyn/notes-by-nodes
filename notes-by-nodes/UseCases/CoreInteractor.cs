using Microsoft.VisualBasic;
using notes_by_nodes.AppRules;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

[assembly: InternalsVisibleTo("TestProject")]
namespace notes_by_nodes.UseCases
{
    internal class CoreInteractor
    {
        protected INodeStorageFactory StorageFactory { get; init; }
        protected LocalUser ActiveUser { get; init; }
        private IBoxStorage boxStorage { get; init; }

        internal CoreInteractor(INodeStorageFactory storageFactory, LocalUser activeUser)
        {
            ActiveUser = activeUser;
            StorageFactory = storageFactory;
            boxStorage = StorageFactory.GetBoxStorage();

        }

        internal IEnumerable<LocalBox> GetBoxes()
        {            
            foreach (var item in ActiveUser.HasChildNodes)
            {
                if (item is LocalBox lbox)
                {
                    yield return lbox;
                }
            }
        }

        //internal async Task<LocalNote> AddNoteToBox(LocalBox box, string title, string text)
        //{
        //    LocalNote note = new LocalNote(box, title, text);
        //    await boxStorage.SaveBoxAsync(box);
        //    await note.NoteStorage.SaveNoteAsync(note);
        //    return note;
        //}
        //internal async Task<LocalNote> AddNoteToNote(LocalNote pnote, string title, string text)
        //{
        //    LocalNote note = new LocalNote(pnote, title, text);
        //    await note.NoteStorage.SaveNoteAsync(pnote);
        //    await note.NoteStorage.SaveNoteAsync(note);
        //    return note;
        //}

        internal void SaveNodes(Node[] nodes) 
        {
           
        }
        internal void SaveNode(LocalBox box, LocalNote note)
        {
            StorageFactory.GetNoteStorage(box).SaveNote(note);
        }
        internal LocalBox GetBox(int uid)
        {  
            var box = boxStorage.GetBox(uid);
            StorageFactory.GetNoteStorage(box).LoadChildNodes(box);
            //box.LoadChildNodes();
            return box;
        }
        internal LocalNote GetNote(int boxUid, int uid)
        {
            var box = boxStorage.GetBox(boxUid);
            var note = StorageFactory.GetNoteStorage(box).GetNote(uid);
            return note;
        }
        internal IEnumerable<Node> LoadChildNodes(int boxUid, int nodeUid)
        {
            var box = boxStorage.GetBox(boxUid);
            var noteStorage = StorageFactory.GetNoteStorage(box);
            var node = noteStorage.GetNote(nodeUid);
            noteStorage.LoadChildNodes(node);
            return node.HasChildNodes;
        }

        internal void NewBox(string folderToBox, string desc)
        {
            LocalBox box = new(ActiveUser, folderToBox, desc);
            //box.SetNoteStorage(StorageFactory.GetNoteStorage(box));
            ActiveUser.AddIntoChildNodes(box);
            boxStorage.SaveBox(box);
            StorageFactory.GetUserStorage().SaveUser(ActiveUser);

        }

        internal void SaveBox(LocalBox box)
        {
            boxStorage.SaveBox(box);
        }
        internal void SaveNote(int boxUid, LocalNote note)
        {
            var box = boxStorage.GetBox(boxUid);
            StorageFactory.GetNoteStorage(box).SaveNote(note);
        }


    }
}
