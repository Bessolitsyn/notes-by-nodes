using notes_by_nodes.Entities;
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
        private INoteStorage Storage { get; init; }
                
        public LocalNote(LocalNote parentNode, string name = "", string desc = "") :base(parentNode)
        {
            Storage = parentNode.Storage;
            Type = "LocalNote";
            Name = name;
            Description = desc;

        }
        public LocalNote(LocalBox rootNode, string name = "", string desc = "") : base(rootNode)
        {
            Storage = rootNode.Storage;
            Type = "LocalNote";

        }

        public override IEnumerable<Content> HasContent => throw new NotImplementedException();

        public override IEnumerable<Note> HasReference => getReference();

        private IEnumerable<Note> getReference()
        {
            hasReference.AddRange(Storage.GetReferencedNotes(this));
            return hasReference;
        }

#warning TO DO исправить ошибку в имени
        public override IEnumerable<Note> IsRefernced => getReferenced();

        private IEnumerable<Note> getReferenced()
        {
            isRefernced.AddRange(Storage.GetNotesHasReferenceToIt(this));
            return isRefernced;
        }

        public override IEnumerable<Node> HasChildNodes => GetChildNodes();


        public override User HasOwner => hasOwner;

        public override Node HasParentNode => hasParentNode;             

        protected override INoteStorage GetStorage()
        {
            return Storage;
        }

    }
}
