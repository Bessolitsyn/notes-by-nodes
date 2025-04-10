using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.UseCases
{
    internal class NoteInsideInteractor: NodeInsideInteractor, INoteInsideInteractor
    {
        private INoteStorage Storage { get; init; }
        public NoteInsideInteractor(INoteStorage storage):base(storage) 
        {
            Storage = storage;
        }

        public IEnumerable<Note> GetNotesHasReferenceToIt(Note note)
        {
            return Storage.GetNotesHasReferenceToIt(note);
        }

        public IEnumerable<Note> GetReferencedNotes(Note note)
        {
            return Storage.GetReferencedNotes(note);
        }
    }

    internal abstract class NodeInsideInteractor : INodeInsideInteractor
    {
        private INodeStorage Storage { get; init; }
        public NodeInsideInteractor(INodeStorage storage)
        { 
            Storage = storage;
        }
        public IEnumerable<Node> GetChildNodes(Node parentNode)
        {
            return Storage.GetChildNodes(parentNode);
        }
    }
}
