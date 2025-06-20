using EasyObjectFileStorage;
using notes_by_nodes.AppRules;
using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace notes_by_nodes.StorageAdapters
{
    internal abstract class NodeStorageAdapter : JsonFileStorage, INodeStorage, IDisposable
    {
        //protected static NodeDataset[] loadedNodes = [];
        protected static ConcurrentDictionary<int, NodeDataset> loadedNodeDatasets = [];
        //protected static Node[] loadedNodes = [];
        protected static ConcurrentDictionary<int, Node> loadedNodes = [];
        protected readonly INodeBuilder nodeBuilder;
        protected INodeStorageProvider nodeStorageProvider;


        public NodeStorageAdapter(INodeStorageProvider storageProvider, INodeBuilder nodeBuilder, string pathToRootFolder, string subfolder, string fileExtension = "node") : base(pathToRootFolder, subfolder, fileExtension)
        {
            this.nodeBuilder = nodeBuilder;
            nodeStorageProvider = storageProvider;
#warning TODO Возможно загружать все заметки в боксе в память сразу неправильно - но этот метод вызывается и для узлов типа юзер и бокс            
            //ReadNodes();
        }

        public async Task<IEnumerable<Node>> LoadChildNodesAsync(Node parentNode)
        {
            var childNodes = new List<Node>();
            NodeDataset dataset = await GetNodeDatasetAsync(parentNode.Uid);
            foreach (var strUid in dataset.HasChildNodes)
            {
                if (int.TryParse(strUid, out int uid))
                {
                    //parentNode.AddIntoChildNodes(GetNodeAsync(uid));
                    var node = await GetNodeAsync(uid);
                    childNodes.Add(node);
                }
            }
            return childNodes;
        }
        protected abstract Task ReadNodes();
        protected abstract Task ReadNode(int uid);

        public async Task<Node> GetNodeAsync(int uid)
        {   
            if (!loadedNodes.TryGetValue(uid, out var node))
            {
                var nodeDS = await GetNodeDatasetAsync(uid);
                node =  GetNodeFromNodeDataset(nodeDS);
            }
            return node ?? throw StorageException.NewException(StorageErrorCode.NoNode);
        }
        public Node GetNode(int uid)
        {
            if (!loadedNodes.TryGetValue(uid, out var node))
            {
                var nodeDS = GetNodeDataset(uid);
                node = GetNodeFromNodeDataset(nodeDS);
            }
            return node ?? throw StorageException.NewException(StorageErrorCode.NoNode);
        }
        protected List<Node> GetNodes(string[] uids, out List<(string, string)> uidsWithErrors)
        {
            List<Node> nodes = new List<Node>();
            uidsWithErrors = new List<(string, string)>();
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
        protected async Task<NodeDataset> GetNodeDatasetAsync(int uid)
        {
            loadedNodeDatasets.TryGetValue(uid, out var userDataset);
            if (userDataset == null)
            {
                Task t = ReadNode(uid);
                await t;
                loadedNodeDatasets.TryGetValue(uid, out userDataset);
                if (userDataset == null) throw StorageException.NewException(StorageErrorCode.NoNode);
            }
            return userDataset;
        }
        protected NodeDataset GetNodeDataset(int uid)
        {
            loadedNodeDatasets.TryGetValue(uid, out var userDataset);
            if (userDataset == null)
            {
                ReadNode(uid).RunSynchronously();
                loadedNodeDatasets.TryGetValue(uid, out userDataset);
                if (userDataset == null) throw StorageException.NewException(StorageErrorCode.NoNode);
            }
            return userDataset;
        }
        protected async Task SaveNodeAsync(NodeDataset nodeds, Node node)
        {
            AddtoLoadedNodeDatasets(nodeds);
            AddtoLoadedNodesDict(node);
            await SaveObjectAsync(nodeds, nodeds.Uid.ToString());
        }
        public void RemoveNode(Node node)
        {
            try
            {
                RemoveObject(node.Uid.ToString());
                loadedNodeDatasets.Remove(node.Uid, out _);
            }
            catch (Exception)
            {
                throw StorageException.NewException(StorageErrorCode.RemoveError);
            }
        }
        protected abstract Node? GetNodeFromNodeDataset(NodeDataset nodeds);

        protected void AddtoLoadedNodeDatasets(NodeDataset nodeds)
        {
            loadedNodeDatasets[nodeds.Uid] = nodeds;
        }
        protected void AddtoLoadedNodesDict(Node node)
        {
            loadedNodes[node.Uid] = node;
        }
        public void Dispose()
        {
            loadedNodeDatasets = [];
            loadedNodes = [];
        }
        
    }

}

   
