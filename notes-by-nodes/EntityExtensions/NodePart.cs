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
        
        protected Node(Node parentNode):this()
        {
            hasParentNode = parentNode;
            hasOwner = parentNode.HasOwner;

            parentNode.AddIntoChildNodes(this);
#warning TODO
            // Таким образом список из того чем владеет owner со временем может стать очень большим!
            hasOwner.AddIntoOwnerOf(this);
        }
        protected Node()
        {
            CreationDate = DateTime.Now;
            Uid = GetUID();
            Type = this.GetType().ToString();
            //IsFullyLoaded = false;
        }

        //protected abstract INodeStorage GetStorage();

        public void AddIntoChildNodes(Node item)
        {
            //uploadNodesChildNodesIfItEmpty();
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
                item.AddIntoOwnerOf(this);
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
        //там внутр вызывается конструктор нода и дочерние ноды добавляютя в соответсвующую 
        //internal void LoadChildNodes()
        //{
        //    INodeStorage storage = GetStorage();
        //    if (storage == null)
        //        throw new Exception("Storage wasn't set. Node doesn't have a storage");
        //    storage.LoadChildNodes(this);
        //}

        //internal async Task LoadChildNodesAsync()
        //{
        //    await Task.Run(LoadChildNodes);
        //}
       
        //protected IEnumerable<Node> GetChildNodes()
        //{
        //    INodeStorage storage = GetStorage();
        //    if (storage == null)
        //        throw new Exception("Storage wasn't set. Node doesn't have a storage");
        //    return storage.LoadChildNodes(this);
        //}

        protected int GetUID()
        {
            return DateTime.Now.GetHashCode();
        }

        
    }

}
