using notes_by_nodes.AppRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Services
{
    public interface INoteService
    {
        void SelectUser(int userUid);
        IEnumerable<(int, string, string)> GetUsers();
        IEnumerable<(int, string, string)> GetBoxes();
        void AddNewBox(string name);
        IEnumerable<(int, string, string)> GetChildNodes(int boxUid, int parentNodeUid);
        IEnumerable<(int, string, string)> GetChildNodesOfTheBox(int boxUid);
        void ModifyUser(int userUid, string name, string email);
        void ModifyBox(int boxUid, string name, string desc);
        void ModifyNote(int boxUid, int noteUid, string name, string desc);
    }
}
