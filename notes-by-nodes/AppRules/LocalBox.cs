using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Entities;
using notes_by_nodes.Service;
using notes_by_nodes.Storage;

[assembly: InternalsVisibleTo("TestProject")]
namespace notes_by_nodes.AppRules //REVIEW: Наверное namespace лучше назвать NotesByNotes. С подчёркиваним не очень читабельно
{
    //REVIEW: Лучше не наследоваться от INodeDto, а сделать такое внутри LocalBox
    public class LocalBox : Box, INodeDto
    {
        //public INoteStorage NoteStorage { get; private set; }

        internal LocalBox(User owner, string name, string desc = "") : base(owner)
        {
            Type = "LocalBox";
            Name = name;
            Description = desc;
            hasOwner = owner;

        }
        //REVIEW: Зачем перегружать. Можно в базовом классе обозначить
        public override IEnumerable<Node> HasChildNodes => hasChildNodes; //LoadChildNodes();
        //REVIEW: Зачем перегружать. Можно в базовом классе обозначить
        public override User HasOwner => hasOwner;
        //REVIEW: Зачем перегружать. Можно в базовом классе обозначить
        public override Node HasParentNode => hasParentNode;


        //public void SetNoteStorage(INoteStorage noteStorage)
        //{
        //    NoteStorage = noteStorage;
        //}

        //protected override INodeStorage GetStorage()
        //{
        //    return NoteStorage;
        //}
    }
}
