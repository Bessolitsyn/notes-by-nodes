using notes_by_nodes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Storage;

namespace notes_by_nodes.StorageAdapters
{
    internal class NodeStorageFactory: INodeStorageFactory
    {
        public static  NodeStorageFactory Instance { get => instance; }
        private static NodeStorageFactory instance = new NodeStorageFactory();
        NodeStorageFactory() { }

        public IBoxStorage GetBoxStorage(string storageFolder)
        {
            throw new NotImplementedException();
        }

        public IUserStorage GetUserStorage(string storageFolder)
        {
            throw new NotImplementedException();
        }
    }
}
