using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("TestProject")]
namespace notes_by_nodes.AppRules
{
    public class LocalNote : Note
    {
        //[Obsolete]
        //public INoteStorage NoteStorage { get; init; }

        internal LocalNote(Node parentNode, string name = "", string text = "") : base(parentNode)
        {

            //NoteStorage = parentNode is LocalBox box ? box.NoteStorage : 
            //              parentNode is LocalNote note ? note.NoteStorage: 
            //              throw new NullRefernceUseCaseException("NoteStorage is null in parentNode when LocalNote is constructing");
            Type = "LocalNote";
            Name = name;
            Text = text;
        }

        public override IEnumerable<Content> HasContent => [];

        public override IEnumerable<Note> HasReference => hasReference; // getReference();

        //private IEnumerable<Note> getReference()
        //{
        //    hasReference.AddRange(NoteStorage.GetReferencedNotes(this));
        //    return hasReference;
        //}

#warning TO DO исправить ошибку в имени
        public override IEnumerable<Note> IsRefernced => isRefernced; // getReferenced();

        //private IEnumerable<Note> getReferenced()
        //{
        //    isRefernced.AddRange(NoteStorage.GetNotesHasReferenceToIt(this));
        //    return isRefernced;
        //}

        public override IEnumerable<Node> HasChildNodes => hasChildNodes;


        public override User HasOwner => hasOwner;

        public override Node HasParentNode => hasParentNode;

        //protected override INodeStorage GetStorage()
        //{
        //    return NoteStorage;
        //}

    }
}
