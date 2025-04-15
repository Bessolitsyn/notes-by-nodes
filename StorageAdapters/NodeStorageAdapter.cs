using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyObjectFileStorage;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;

namespace notes_by_nodes.StorageAdapters
{
    internal class NodeStorageAdapter : INodeStorage
    {
        public IBoxStorage GetBoxStorage(string storageFolder)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Node> GetChildNodes(Node parentNode)
        {
            throw new NotImplementedException();
        }

        public IUserStorage GetUserStorage(string storageFolder)
        {
            throw new NotImplementedException();
        }
    }

}
