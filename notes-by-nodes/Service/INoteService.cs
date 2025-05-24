using notes_by_nodes.AppRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Service
{
    public interface INoteService
    {
        void SelectUser(int userUid);
        IEnumerable<IUserDto> GetUsers();
        IEnumerable<INodeDto> GetBoxes();
        void AddNewBox(string name);
        Task<IEnumerable<INodeDto>> GetChildNodes(int boxUid, int parentNodeUid);
        Task<IEnumerable<INodeDto>> GetChildNodesOfTheBox(int boxUid);
        void ModifyUser(IUserDto user);
        void ModifyBox(INodeDto box);
        void ModifyNote(int boxUid, INodeDto note);
        Task<INodeDto> NewNote(int boxUid, int parenNoteUid);      
        Task Remove(int boxUid, int noteUId);
        Task Remove(int boxUid);
        IUserDto NewUser(IUserDto user);
        INodeDto NewBox(INodeDto box);
    }

    public interface INodeDto
    {
        int Uid { get; }
        string Name { get; set; }
        string Description { get; set; }
        string Text { get; set; }
    }
    public interface IUserDto
    {
        int Uid { get; }
        string Name { get; set; }
        string Email { get; set; }
    }

}
