using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;

namespace notes_by_nodes.UseCases
{
    internal class NodeStorageAdapter : INodeStorage
    {
        public IEnumerable<Node> GetChildNodes(Node parentNode)
        {
            throw new NotImplementedException();
        }
    }
}
