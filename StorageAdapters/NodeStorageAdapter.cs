﻿using System;
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
using notes_by_nodes.AppRules;

namespace notes_by_nodes.StorageAdapters
{
    internal abstract class NodeStorageAdapter : JsonFileStorage, INodeStorage , IDisposable
    {
        //protected static NodeDataset[] loadedNodes = [];
        protected static Dictionary<int,NodeDataset> loadedNodeDatasets = [];
        //protected static Node[] loadedNodes = [];
        protected static Dictionary<int,Node> loadedNodes = [];
        protected readonly INodeBuilder nodeBuilder;
        protected INodeStorageProvider nodeStorageFactory;
        

        public NodeStorageAdapter(INodeBuilder nodeBuilder, string pathToRootFolder, string subfolder, string fileExtension = "node") : base(pathToRootFolder, subfolder, fileExtension)
        {
            this.nodeBuilder = nodeBuilder;
#warning TODO Возможно загружать все заметки в боксе в память сразу неправильно - но этот метод вызывается и для узлов типа юзер и бокс
            //ReadNodes();
        }
        internal void SetStorageFactory(INodeStorageProvider factory)
        {
            nodeStorageFactory = factory;
        }

        public IEnumerable<Node> LoadChildNodes(Node parentNode)
        {
            var childNodes = new List<Node>();
            NodeDataset dataset = GetNodeDataset(parentNode.Uid);
            foreach (var strUid in dataset.HasChildNodes)
            {
                if (int.TryParse(strUid, out int uid))
                {
                    //parentNode.AddIntoChildNodes(GetNode(uid));
                    var node = GetNode(uid);
                    childNodes.Add(node);
                }
            }
            return childNodes;
        }
        internal abstract void ReadNodes();
        protected abstract void ReadNode(int uid);

        public Node GetNode(int uid)
        {
            loadedNodes.TryGetValue(uid, out var node);
            //Node node = loadedNodes.ContainsKey(uid) ? loadedNodes[uid]: null;
            if (node == null)
            {
                var nodeDS = GetNodeDataset(uid);
                node = GetLocalNodeFromDataset(nodeDS, in loadedNodes);
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
                loadedNodeDatasets.TryGetValue(uid, out userDataset);
                if (userDataset == null) throw StorageException.NewException(StorageErrorCode.NoNode);
            }                
            return userDataset;
        }
        protected void SaveNode(NodeDataset nodeds, Node node)
        {
            AddtoLoadedNodeDatasets(nodeds);
            AddtoLoadedNodes(node);
            SaveObject(nodeds, nodeds.Uid.ToString());
        }
        public void RemoveNode(Node node)
        {
            if (!TryRemoveObject(node.Uid.ToString())) throw StorageException.NewException(StorageErrorCode.RemoveError);
            loadedNodeDatasets.Remove(node.Uid);
        }
        protected abstract Node GetLocalNodeFromDataset(NodeDataset nodeds, in Dictionary<int, Node> createdLoadedNodes);

        protected void AddtoLoadedNodeDatasets(NodeDataset nodeds)
        {
            loadedNodeDatasets[nodeds.Uid] = nodeds;
        }
        protected void AddtoLoadedNodes(Node node)
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
