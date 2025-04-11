using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Entities;

namespace notes_by_nodes.UseCases.AppRules
{
    internal class LocalBox : Box
    {
        public IBoxStorage Storage { get; init; }
        
        public LocalBox(User owner, IBoxStorage storage) : base(owner)
        { 
            Storage = storage;
            Type = "LocalBox";
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
