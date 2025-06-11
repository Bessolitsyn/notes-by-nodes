using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Entities
{
    public abstract partial class User : Node, IUser
    {
        public string Email { get; set; }
        protected User(string name, string email) : base()
        {
            Name = name;
            Email = email;
        }

        public void AddIntoOwnerOf(Node item)
        {
            if (!isOwnerOf.Contains(item)) { 
                isOwnerOf.Add(item);            
                item.SetOwner(this);
            }

        }

        public void RemoveFromOwnedNodes(Node item)
        {            
                isOwnerOf.Remove(item);
        }

    }
}
