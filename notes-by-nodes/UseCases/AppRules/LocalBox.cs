using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;

[assembly: InternalsVisibleTo("TestProject")]
namespace notes_by_nodes.UseCases.AppRules
{
    public class LocalBox : Box
    {
        public IBoxStorage Storage { get; init; }
        
        public LocalBox(User owner, IBoxStorage storage, string name="", string desc="") : base(owner)
        { 
            Storage = storage;
            Type = "LocalBox";
            Name = name;
            Description = desc;
            hasChildNodes.AddRange(GetChildNodes());

        }
        public override IEnumerable<Node> HasChildNodes => GetChildNodes();

        public override User HasOwner => hasOwner;

        public override Node HasParentNode => hasParentNode;

        protected override INodeStorage GetStorage()
        {
            return Storage;
        }
    }
}
