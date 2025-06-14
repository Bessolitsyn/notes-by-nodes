﻿
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
namespace notes_by_nodes.AppRules
{
    public class LocalUser : User, IUserDto
    {
        public IBoxStorage NoteStorage { get; init; }

        internal LocalUser(string name, string email, IBoxStorage storage) : base(name, email)
        {
            NoteStorage = storage;
            Type = "LocalUser";
            hasOwner = this;
            hasParentNode = this;


        }
        public override IEnumerable<Node> HasChildNodes => hasChildNodes;

        public override User HasOwner => hasOwner;

        public override Node HasParentNode => hasParentNode;

        public override IEnumerable<Node> IsOwnerOf => isOwnerOf;

        //protected override INodeStorage GetStorage()
        //{
        //    return NoteStorage;
        //}






    }
}
