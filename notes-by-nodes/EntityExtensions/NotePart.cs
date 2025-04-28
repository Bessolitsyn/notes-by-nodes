using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities
{
    public abstract partial class Note(Node parentNode) : Node(parentNode), INote
    {
        //protected Note(Node parentNode) : base(parentNode)
        //{ 
        //}

        public void AddIntoContent(Content item)
        {
            if (!hasContent.Contains(item))
                hasContent.Add(item);
            
        }

        public void AddIntoReference(Note item)
        {
            if (!hasReference.Contains(item))
                hasReference.Add(item);
            item.AddIntoRefernced(this);
        }

        public void AddIntoRefernced(Note item)
        {
            if (!isRefernced.Contains(item))
                isRefernced.Add(item);            
        }

        public void RemoveFromContent(Content item)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromReference(Note item)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromRefernced(Note item)
        {
            throw new NotImplementedException();
        }
    }
}
