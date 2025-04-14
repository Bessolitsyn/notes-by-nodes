using notes_by_nodes.Entities;
using notes_by_nodes.UseCases.AppRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Factories
{
    public static class AppRulesLevelTests
    {
        
        public static object TestUserConstructor()
        {
            var storageFactory = new NodeStorageTestFactory();
            LocalUser user = new("Anton", "Anton@mailbox", storageFactory.GetUserStorage());
            return user;
        }
        public static object? TestBoxConstructor(object user)
        {
            var storageFactory = new NodeStorageTestFactory();
            if (user is LocalUser user1)
            {
                LocalBox box = new(user1, storageFactory.GetBoxStorage(), "Name of the test box", "Description of the test box");            
                return box;
            }
            return null;
        }

        public static object? TestNodeConstructor(object box)
        {
            var storageFactory = new NodeStorageTestFactory();
            if (box is LocalBox box1)
            {
                LocalNote note = new LocalNote(box1, "Untitled 1", "Description of UntitledNote1");
                
                
            }
            return null;
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
