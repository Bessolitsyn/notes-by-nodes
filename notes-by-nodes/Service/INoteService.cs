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
        IEnumerable<IUserDto> GetUsers();
        IEnumerable<INodeDto> GetBoxes();
        void AddNewBox(string name);
        IEnumerable<INodeDto> GetChildNodes(int boxUid, int parentNodeUid);
        IEnumerable<INodeDto> GetChildNodesOfTheBox(int boxUid);
        void ModifyUser(IUserDto user);
        void ModifyBox(INodeDto box);
        void ModifyNote(int boxUid, INodeDto note);
        INodeDto NewNote(int boxUid, INodeDto note, INodeDto childNote);
        void Remove(int boxUid, INodeDto note);
    }

    public interface INodeDto
    {
        int Uid { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Text { get; set; }
    }
    public interface IUserDto
    {
        int Uid { get; set; }
        string Name { get; set; }
        string Email { get; set; }
    }

}
