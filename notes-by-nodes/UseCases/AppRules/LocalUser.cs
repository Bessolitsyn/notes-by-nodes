
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Entities;

[assembly: InternalsVisibleTo("TestProject")]
namespace notes_by_nodes.UseCases.AppRules
{
    internal class LocalUser : User
    {
        public IUserStorage Storage { get; init; }
        
        public LocalUser(string name, string email, IUserStorage storage) : base(name, email)
        { 
            Storage = storage;
            Type = "LocalUser";
            hasOwner = this;
            hasParentNode = this;

        }
        public override IEnumerable<Node> HasChildNodes => GetChildNodes();

        public override User HasOwner => hasOwner;

        public override Node HasParentNode => hasParentNode;

        public override IEnumerable<Node> IsOwnerOf => isOwnerOf;

        protected override INodeStorage GetStorage()
        {
            return Storage;
        }

    }
}
