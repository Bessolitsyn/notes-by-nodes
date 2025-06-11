using notes_by_nodes.AppRules;
using notes_by_nodes.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Storage
{
    public interface INodeBuilder
    {
        LocalUser NewLocalUser(string name, string email, IBoxStorage storage);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="name">Path to folder that will store note files</param>
        /// <param name="desc">Description</param>
        /// <returns></returns>
        LocalBox NewLocalBox(User owner, string name, string desc = "");
        LocalNote NewLocalNote(Node parentNode, string name, string text="");
    }
}
