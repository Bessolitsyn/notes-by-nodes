using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.StorageAdapters
{
    internal class LocalBoxStorage : IBoxStorage
    {
        public LocalBox GetBox(int Uid)
        {
            throw new NotImplementedException();
        }

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
