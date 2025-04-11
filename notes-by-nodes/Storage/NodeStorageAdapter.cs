using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Entities;

namespace notes_by_nodes.Storage
{
    internal class NodeStorageAdapter : INodeStorage
    {
        public IEnumerable<Node> GetChildNodes(Node parentNode)
        {
            throw new NotImplementedException();
        }
    }
}
