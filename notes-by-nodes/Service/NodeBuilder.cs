using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Service
{
    public class NodeBuilder : INodeBuilder
    {
        public LocalBox NewLocalBox(User owner, string name, string desc = "")
        {
            var box = new LocalBox(owner, name, desc);
            return box;
        }

        public LocalNote NewLocalNote(Node parentNode, string name, string text = "")
        {
            return new LocalNote(parentNode, name, text);
        }

        public LocalUser NewLocalUser(string name, string email, IBoxStorage storage)
        {
            return new LocalUser(name, email, storage);
        }
    }
}
