using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Entities;

namespace notes_by_nodes.Storage
{
    internal interface INodeStorage
    {
        IEnumerable<Node> GetChildNodes(Node parentNode);
    }

    internal interface INoteStorage : INodeStorage
    {
        IEnumerable<Note> GetNotesHasReferenceToIt(Note note);
        IEnumerable<Note> GetReferencedNotes(Note note);
    }
}
