using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities
{
    public partial class Node
    {
        public Node(Node parentNode)
        {
            hasParentNode = parentNode;
            hasOwner = parentNode.hasOwner;
            parentNode.AddIntoChildNodes(this);
            
        }

    }

    public partial class Note(Node parentNode) : Node(parentNode)
    {
    }
}
