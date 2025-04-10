using notes_by_nodes.Entities;
using notes_by_nodes.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities
{
    public class LocalNote : Note
    {
        public string Type { get; init; }

        public override IEnumerable<Content> HasContent => throw new NotImplementedException();

        public override IEnumerable<Note> HasReference => getReference();

        private IEnumerable<Note> getReference()
        {
            hasReference.AddRange(InsideInteractor.GetReferencedNotes(this));
            return hasReference;
        }

#warning TO DO исправить ошибку в имени
        public override IEnumerable<Note> IsRefernced => getReferenced();

        private IEnumerable<Note> getReferenced()
        {
            isRefernced.AddRange(InsideInteractor.GetNotesHasReferenceToIt(this));
            return isRefernced;
        }

        public override IEnumerable<Node> HasChildNodes => getChildNodes();

        private IEnumerable<Node> getChildNodes()
        {
            hasChildNodes.AddRange(InsideInteractor.GetChildNodes(this));
            return hasChildNodes;
        }

        public override User HasOwner => throw new NotImplementedException();

        public override Node HasParentNode => throw new NotImplementedException();

        

        public override void AddIntoChildNodes(Node item)
        {
            throw new NotImplementedException();
        }

        public override void AddIntoContent(Content item)
        {
            throw new NotImplementedException();
        }

        public override void AddIntoReference(Note item)
        {
            throw new NotImplementedException();
        }

        public override void AddIntoRefernced(Note item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveFromChildNodes(Node item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveFromContent(Content item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveFromReference(Note item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveFromRefernced(Note item)
        {
            throw new NotImplementedException();
        }

        public override void SetOwner(User item)
        {
            throw new NotImplementedException();
        }

        public override void SetParentNode(Node item)
        {
            throw new NotImplementedException();
        }
        protected INoteInsideInteractor InsideInteractor { get; init; }

        public LocalNote(INoteInsideInteractor insideInteractor)
        {
            InsideInteractor = insideInteractor;
            Type = "LocalNote";

        }
    }
}
