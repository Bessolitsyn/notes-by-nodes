using System;
using System.Collections.Generic;
using System.Data;
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

        protected INodeStorageFactory nodeStorageFactory;
        

        public NodeStorageAdapter(string pathToRootFolder, string subfolder, string fileExtension = "node") : base(pathToRootFolder, subfolder, fileExtension)
        {
#warning TODO Возможно загружать все заметки в боксе в память сразу неправильно - но этот метод вызывается и для узлов типа юзер и бокс
            //ReadNodes();
        }
        internal void SetStorageFactory(INodeStorageFactory factory)
        {
            nodeStorageFactory = factory;
        }

        public IEnumerable<Node> GetChildNodes(Node parentNode)
        {
            var childNodes = new List<Node>();
            NodeDataset dataset = GetNodeDataset(parentNode.Uid);
            foreach (var strUid in dataset.HasChildNodes)
            {
                if (int.TryParse(strUid, out int uid))
                {
                    childNodes.Add(GetNode(uid));
                }
            }
            return childNodes;
        }
        internal abstract void ReadNodes();
        protected abstract void ReadNode(int uid);

        public Node GetNode(int uid)
        {
            createdLoadedNodes.TryGetValue(uid, out var node);
            //Node node = createdLoadedNodes.ContainsKey(uid) ? createdLoadedNodes[uid]: null;
            if (node == null)
            {
                var nodeDS = GetNodeDataset(uid);
                node = GetLocalNodeFromDataset(nodeDS, in createdLoadedNodes);
            }            
            return node;                 
        }
        protected List<Node> GetNodes(string[] uids, out List<(string, string)>uidsWithErrors)
        {
            List<Node> nodes = new List<Node>();
            uidsWithErrors = new List<(string, string)> ();
            foreach (string uid in uids)
            {
                try
                {
                    var node = GetNode(int.Parse(uid));
                    nodes.Add(node);
                }
                catch (Exception ex)
                {
                    uidsWithErrors.Add((uid, ex.Message));
                }
            }
            return nodes;
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
            AddtoLoadedNodeDatasets(node);
            SaveObject(node, node.Uid.ToString());
        }
        protected abstract Node GetLocalNodeFromDataset(NodeDataset userDataset, in Dictionary<int, Node> createdLoadedNodes);

        protected void AddtoLoadedNodeDatasets(NodeDataset node)
        {
            loadedNodeDatasets[node.Uid] = node;
        }
        public void Dispose()
        {
            loadedNodeDatasets = [];
            createdLoadedNodes = [];
        }
    }

}
