using notes_by_nodes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.UseCases
{
    internal class NoteInsideInteractor : INoteInsideInteractor
    {

        public NoteInsideInteractor() { }
        public IEnumerable<Node> GetChildNodes(Node parentNode)
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
    }
}
