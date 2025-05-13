using notes_by_nodes.AppRules;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class AppRulesLevelTests
    {
        [Fact]
        public static void TestUserBoxNodes()
        {
            var storageFactory = new NodeStorageTestFactory();
            LocalUser user = new("Anton", "Anton@mailbox", storageFactory.GetBoxStorage());
            LocalBox box = new(user, "Name of the test box", "Description of the test box");
          
            bool boxWasAddedToUser = user.HasChildNodes.Count()==1 && box.HasParentNode==user;
            user.RemoveFromChildNodes(box);
            bool boxWasDeletedFromUser = user.HasChildNodes.Count()==0;
            Assert.True(boxWasDeletedFromUser && boxWasDeletedFromUser);

        }

        [Fact]
        public static void TestUserConstructor()
        {
            var storageFactory = new NodeStorageTestFactory();
            LocalUser user = new("Anton", "Anton@mailbox", storageFactory.GetBoxStorage());
            Assert.True(true);

        }
        [Fact]
        public static void TestBoxConstructor()
        {
            var storageFactory = new NodeStorageTestFactory();
            LocalUser user = new("Anton", "Anton@mailbox", storageFactory.GetBoxStorage());
            LocalBox box = new(user, "Name of the test box", "Description of the test box");
            //user.AddIntoChildNodes(box);
            Assert.True(user.HasChildNodes.Contains(box) && box.HasParentNode == user);

        }

        [Fact]
        public static void TestLocalNoteConstructor()
        {
            var storageFactory = new NodeStorageTestFactory();
            LocalUser user = new("Anton", "Anton@mailbox", storageFactory.GetBoxStorage());
            LocalBox box = new(user, "Name of the test box", "Description of the test box");
            //user.AddIntoChildNodes(box);
            //box.SetNoteStorage(storageFactory.GetNoteStorage(box));
            LocalNote note = new LocalNote(box, "Untitled 1", "Description of UntitledNote1");
            LocalNote note2 = new LocalNote(note, "Untitled 2", "Description of UntitledNote2");
            //box.AddIntoChildNodes(note);
            //note.AddIntoChildNodes(note2);

            Assert.True(box.HasChildNodes.Contains(note) &&
                note.HasParentNode == box &&
                note.HasChildNodes.Contains(note2) &&
                note2.HasParentNode == note
                );
        }
    }

    class NodeStorageTestFactory : INodeStorageFactory
    {
        public IBoxStorage GetBoxStorage()
        {
            return new FakeStorage();
        }

        public INoteStorage GetNoteStorage(LocalBox box)
        {
            return new FakeStorage();
        }

        public IUserStorage GetUserStorage()
        {
            return new FakeStorage();
        }
    }

    class FakeStorage : IUserStorage, IBoxStorage, INoteStorage
    {
        public LocalNote GetNote(int Uid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Note> GetNotesHasReferenceToIt(Note note)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Note> GetReferencedNotes(Note note)
        {
            throw new NotImplementedException();
        }

        public string[] GetUsers()
        {
            throw new NotImplementedException();
        }

        public void SaveBox(LocalBox box)
        {
            throw new NotImplementedException();
        }

        public Task SaveBoxAsync(LocalBox box)
        {
            throw new NotImplementedException();
        }

        public void SaveNote(LocalNote note)
        {
            throw new NotImplementedException();
        }

        public Task SaveNoteAsync(LocalNote note)
        {
            throw new NotImplementedException();
        }

        public void SaveUser(LocalUser user)
        {
            throw new NotImplementedException();
        }

        LocalBox IBoxStorage.GetBox(int Uid)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Node> INodeStorage.LoadChildNodes(Node parentNode)
        {
            return [];
        }

        

        LocalUser IUserStorage.GetUser(int Uid)
        {
            throw new NotImplementedException();
        }

        LocalUser[] IUserStorage.GetUsers()
        {
            throw new NotImplementedException();
        }
    }

}
