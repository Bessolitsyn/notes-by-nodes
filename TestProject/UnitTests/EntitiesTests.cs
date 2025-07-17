using notes_by_nodes.AppRules;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.UnitTests
{
    // Test of business logic level
    //TO DO эти тесты нужно переделать в модульные тесты классов бизнес-логики
    public class EntitiesTests
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

    class NodeStorageTestFactory : INodeStorageProvider
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
        public Task<LocalBox> GetBoxAsync(int Uid)
        {
            throw new NotImplementedException();
        }

        public Task<LocalNote> GetNoteAsync(int Uid)
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

        public Task<LocalUser> GetUser(string name)
        {
            throw new NotImplementedException();
        }

        public LocalUser GetUser(int Uid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Node>> LoadChildNodesAsync(Node parentNode)
        {
            throw new NotImplementedException();
        }

        public void RemoveNode(Node note)
        {
            throw new NotImplementedException();
        }

        public void RemoveNote(LocalNote note)
        {
            throw new NotImplementedException();
        }

        public Task SaveBoxAsync(LocalBox box)
        {
            throw new NotImplementedException();
        }

        public Task SaveNoteAsync(LocalNote note)
        {
            throw new NotImplementedException();
        }

        public Task SaveUserAsync(LocalUser user)
        {
            throw new NotImplementedException();
        }
    }

}
