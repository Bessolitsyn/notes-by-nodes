using notes_by_nodes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities
{
    public abstract partial class Box(Node parentNode) : Node(parentNode), IBox
    {
    }
}
