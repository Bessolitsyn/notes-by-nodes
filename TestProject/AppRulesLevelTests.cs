using notes_by_nodes.Entities;
using notes_by_nodes.UseCases.AppRules;
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
            LocalUser user = new("Anton", "Anton@mailbox", storageFactory.GetUserStorage());
            LocalBox box = new(user, storageFactory.GetBoxStorage(), "Name of the test box", "Description of the test box");
            
            bool boxWasAddedToUser = user.HasChildNodes.Count()==1 && box.HasParentNode==user;
            box.RemoveFromChildNodes(box);
            bool boxWasDeletedFromUser = user.HasChildNodes.Count()==0;
            Assert.True(boxWasDeletedFromUser && boxWasDeletedFromUser);

        }

        [Fact]
        public static void TestUserConstructor()
        {
            var storageFactory = new NodeStorageTestFactory();
            LocalUser user = new("Anton", "Anton@mailbox", storageFactory.GetUserStorage());
            Assert.True(true);

        }
        [Fact]
        public static void TestBoxConstructor()
        {
            var storageFactory = new NodeStorageTestFactory();
            LocalUser user = new("Anton", "Anton@mailbox", storageFactory.GetUserStorage());
            LocalBox box = new(user, storageFactory.GetBoxStorage(), "Name of the test box", "Description of the test box");
            Assert.True(true);

        }

        [Fact]
        public static void TestLocalNoteConstructor()
        {
            var storageFactory = new NodeStorageTestFactory();
            LocalUser user = new("Anton", "Anton@mailbox", storageFactory.GetUserStorage());
            LocalBox box = new(user, storageFactory.GetBoxStorage(), "Name of the test box", "Description of the test box");
            LocalNote note = new LocalNote(box, "Untitled 1", "Description of UntitledNote1");
            LocalNote note2 = new LocalNote(note, "Untitled 2", "Description of UntitledNote2");
            Assert.True(box.HasChildNodes.Contains(note) &&
                note.HasParentNode == box &&
                note.HasChildNodes.Contains(note2) &&
                note2.HasParentNode == note
                );
        }
    }

    class NodeStorageTestFactory : INodeStorageFactory
    {
        public NodeStorageTestFactory() 
        {
            //folder.Create();

        }
        public IBoxStorage GetBoxStorage()
        {
            return new TestStorage();
        }

        public INoteStorage GetNoteStorage()
        {
            throw new NotImplementedException();
        }

        public IUserStorage GetUserStorage()
        {
            return new TestStorage();
        }
    }

    class TestStorage : IUserStorage, IBoxStorage
    {
        public IEnumerable<Node> GetChildNodes(Node parentNode)
        {
            return [];
        }

        public IEnumerable<Note> GetNotesHasReferenceToIt(Note note)
        {
            return [];
        }

        public IEnumerable<Note> GetReferencedNotes(Note note)
        {
            return [];
        }
    }

}
