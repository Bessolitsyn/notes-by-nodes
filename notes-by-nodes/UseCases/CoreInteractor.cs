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
        protected INodeStorageProvider StorageFactory { get; init; }
        protected LocalUser ActiveUser { get; init; }
        private IBoxStorage boxStorage { get; init; }

        internal CoreInteractor(INodeStorageProvider storageFactory, LocalUser activeUser)
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

        internal void SaveNodes(Node[] nodes) 
        {
           
        }
        internal Task SaveNode(LocalBox box, LocalNote note)
        {
            return StorageFactory.GetNoteStorage(box).SaveNoteAsync(note);
        }
        internal  async Task<LocalBox> GetBox(int uid)
        {  
            var box = await boxStorage.GetBoxAsync(uid);
            await StorageFactory.GetNoteStorage(box).LoadChildNodesAsync(box);
            //box.LoadChildNodes();
            return box;
        }
        internal async Task<LocalNote> GetNote(int boxUid, int uid)
        {
            var box = await boxStorage.GetBoxAsync(boxUid);
            var note = await StorageFactory.GetNoteStorage(box).GetNoteAsync(uid);
            return note;
        }
        internal async Task<IEnumerable<Node>> LoadChildNodes(int boxUid, int nodeUid)
        {
            var box = await boxStorage.GetBoxAsync(boxUid);
            var noteStorage = StorageFactory.GetNoteStorage(box);
            var node = await noteStorage.GetNoteAsync(nodeUid);
            await noteStorage.LoadChildNodesAsync(node);
            return node.HasChildNodes;
        }

        internal async Task<LocalBox> NewBox(string folderToBox, string desc)
        {
            LocalBox box = new(ActiveUser, folderToBox, desc);
            //box.SetNoteStorage(StorageFactory.GetNoteStorage(box));
            ActiveUser.AddIntoChildNodes(box);
            await boxStorage.SaveBoxAsync(box);
            await StorageFactory.GetUserStorage().SaveUserAsync(ActiveUser);
            return box;

        }

        internal Task SaveBox(LocalBox box)
        {
            return boxStorage.SaveBoxAsync(box);
        }
        internal async Task SaveNote(int boxUid, LocalNote note)
        {
            var box = await boxStorage.GetBoxAsync(boxUid);
            await StorageFactory.GetNoteStorage(box).SaveNoteAsync(note);
        }

        internal async Task RemoveNote(LocalBox box, Node note)
        {

            foreach (var item in note.HasChildNodes.ToList())
            {
                await RemoveNote(box, item);
            }
            note.HasParentNode.RemoveFromChildNodes(note);
            note.HasOwner.RemoveFromOwnedNodes(note);
            var storage = StorageFactory.GetNoteStorage(box);
            storage.RemoveNode(note);
            if (box.Uid != note.HasParentNode.Uid)
                await storage.SaveNoteAsync((LocalNote)note.HasParentNode);
            else
                await SaveBox((LocalBox)note.HasParentNode);

        }
        internal async Task RemoveBox(LocalBox box)
        {

            foreach (var item in box.HasChildNodes.ToList())
            {
                box.RemoveFromChildNodes(item);
            }
            box.HasParentNode?.RemoveFromChildNodes(box);
            box.HasOwner.RemoveFromOwnedNodes(box);
            var storage = StorageFactory.GetBoxStorage();
            storage.RemoveNode(box);
            var ownerUser = await StorageFactory.GetUserStorage().GetUser(ActiveUser.Name);
            await StorageFactory.GetUserStorage().SaveUserAsync(ownerUser);
        }
    }
}
