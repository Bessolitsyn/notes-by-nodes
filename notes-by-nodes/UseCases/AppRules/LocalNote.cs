using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("TestProject")]
namespace notes_by_nodes.UseCases.AppRules
{
    internal class LocalNote : Note
    {
        private INoteStorage NoteStorage { get; init; }
                
        public LocalNote(LocalNote parentNode, string name = "", string desc = "") :base(parentNode)
        {
            NoteStorage = parentNode.NoteStorage;
            Type = "LocalNote";
            Name = name;
            Description = desc;

        }
        public LocalNote(LocalBox rootNode, string name = "", string desc = "") : base(rootNode)
        {
            NoteStorage = rootNode.NoteStorage;
            Type = "LocalNote";

        }

        public override IEnumerable<Content> HasContent => throw new NotImplementedException();

        public override IEnumerable<Note> HasReference => getReference();

        private IEnumerable<Note> getReference()
        {
            hasReference.AddRange(NoteStorage.GetReferencedNotes(this));
            return hasReference;
        }

#warning TO DO исправить ошибку в имени
        public override IEnumerable<Note> IsRefernced => getReferenced();

        private IEnumerable<Note> getReferenced()
        {
            isRefernced.AddRange(NoteStorage.GetNotesHasReferenceToIt(this));
            return isRefernced;
        }

        public override IEnumerable<Node> HasChildNodes => GetChildNodes();


        public override User HasOwner => hasOwner;

        public override Node HasParentNode => hasParentNode;             

        protected override INodeStorage GetStorage()
        {
            return NoteStorage;
        }

    }
}
