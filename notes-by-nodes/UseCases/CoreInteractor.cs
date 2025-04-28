using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;
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
        protected List<LocalBox> Boxes { get; init; } 

        internal CoreInteractor(INodeStorageFactory storageFactory, LocalUser activeUser)
        {
            ActiveUser = activeUser;
            StorageFactory = storageFactory;
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
        internal async Task<LocalNote> AddNoteToBox(LocalBox box, string title, string text)
        {
            LocalNote note = new LocalNote(box, title, text);
            var boxStorage = StorageFactory.GetBoxStorage();
            await boxStorage.SaveBoxAsync(box);
            await note.NoteStorage.SaveNoteAsync(note);
            return note;
        }
        internal async Task<LocalNote> AddNoteToNote(LocalNote pnote, string title, string text)
        {
            LocalNote note = new LocalNote(pnote, title, text);
            var boxStorage = StorageFactory.GetBoxStorage();
            await note.NoteStorage.SaveNoteAsync(pnote);
            await note.NoteStorage.SaveNoteAsync(note);
            return note;
        }

        internal void SaveNodes(Node[] nodes) 
        {
           
        }

    }
}
