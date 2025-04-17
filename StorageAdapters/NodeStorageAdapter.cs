using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EasyObjectFileStorage;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;

namespace notes_by_nodes.StorageAdapters
{
    internal abstract class NodeStorageAdapter : JsonFileStorage, INodeStorage , IDisposable
    {
        //protected static NodeDataset[] loadedNodes = [];
        protected static Dictionary<int,NodeDataset> loadedNodeDatasets = [];
        //protected static Node[] createdLoadedNodes = [];
        protected static Dictionary<int,Node> createdLoadedNodes = [];
        public NodeStorageAdapter(string pathToRootFolder, string subfolder, string fileExtension = "node") : base(pathToRootFolder, subfolder, fileExtension)
        {
#warning TODO Возможно загружать все заметки в боксе в память сразу неправильно - но этот метод вызывается и для узлов типа юзер и бокс
            ReadNodes();
        }

        public IEnumerable<Node> GetChildNodes(Node parentNode)
        {            
            NodeDataset dataset = GetNodeDataset(parentNode.Uid);
            foreach (var strUid in dataset.HasChildNodes)
            {
                if (int.TryParse(strUid, out int uid))
                {
                        yield return GetNode(uid);
                }
            }
        }
        protected abstract void ReadNodes();
        protected abstract void ReadNode(int uid);

        protected Node GetNode(int uid)
        {
            createdLoadedNodes.TryGetValue(uid, out var node);
            //Node node = createdLoadedNodes.ContainsKey(uid) ? createdLoadedNodes[uid]: null;
            if (node == null)
            {
                var userDS = GetNodeDataset(uid);
                node = GetLocalNodeFromDataset(userDS, in createdLoadedNodes);
                return node;                 
            }
            
            throw StorageException.NewException(StorageErrorCode.NoNode);
        }
        protected NodeDataset GetNodeDataset(int uid)
        {
            loadedNodeDatasets.TryGetValue(uid, out var userDataset);
            if (userDataset == null)
            { 
                ReadNode(uid);
                userDataset = loadedNodeDatasets[uid] ?? throw StorageException.NewException(StorageErrorCode.NoNode);
            }                
            return userDataset;
        }
        protected void SaveNode(NodeDataset node)
        {
            loadedNodeDatasets[node.Uid] = node;
            SaveObject(node, node.Uid.ToString());
        }
        protected abstract Node GetLocalNodeFromDataset(NodeDataset userDataset, in Dictionary<int, Node> createdLoadedNodes);

        public void Dispose()
        {
            loadedNodeDatasets = [];
            createdLoadedNodes = [];
        }
    }

}
