using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Storage;

namespace notes_by_nodes.Entities
{
    public abstract partial class Node : INode
    {
        public Node(Node parentNode):this()
        {
            hasParentNode = parentNode;
            hasOwner = parentNode.HasOwner;
            parentNode.AddIntoChildNodes(this);
#warning TODO
            // Таким образом список из того чем владеет owner со временем может стать очень большим!
            hasOwner.AddIntoOwner(this);
        }
        public Node()
        {
            CreationDate = DateTime.Now;
            Uid = GetUID();
            Type = this.GetType().ToString();
        }

        protected abstract INodeStorage GetStorage();

        public void AddIntoChildNodes(Node item)
        {
            uploadNodesChildNodesIfItEmpty();
            if (!hasChildNodes.Contains(item))
            { 
                hasChildNodes.Add(item);
                item.SetParentNode(this);
            }

        }

        public void RemoveFromChildNodes(Node item)
        {
            hasChildNodes.Remove(item);
        }

        public void SetOwner(User item)
        {
            if (hasOwner.Uid != item.Uid)
            {
                hasOwner = item;
                item.AddIntoOwner(this);
            }
        }

        public void SetParentNode(Node item)
        {
            if (hasParentNode.Uid != item.Uid)
            { 
                hasParentNode = item;
                item.AddIntoChildNodes(this);
            }
        }
        protected IEnumerable<Node> GetChildNodes()
        {
            uploadNodesChildNodesIfItEmpty();
            return hasChildNodes;
        }
        private void uploadNodesChildNodesIfItEmpty()
        {
            if (hasChildNodes.Count == 0)
                hasChildNodes.AddRange(getChildNodes());
        }
        private IEnumerable<Node> getChildNodes()
        {
            INodeStorage storage = GetStorage();
            if (storage == null)
                throw new Exception("Storage wasn't set. Node doesn't have a storage");
            return storage.GetChildNodes(this);
        }

        protected int GetUID()
        {
            return DateTime.Now.GetHashCode();
        }

        
    }

}
