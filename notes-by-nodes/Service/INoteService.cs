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
        Task<IUserDto> SelectUser(string name);
        IEnumerable<INodeDto> GetBoxes();
        Task<IEnumerable<INodeDto>> GetChildNodes(int boxUid, int parentNodeUid);
        Task<IEnumerable<INodeDto>> GetChildNodesOfTheBox(int boxUid);
        Task ModifyUser(IUserDto user);
        Task ModifyBox(INodeDto box);
        Task ModifyNote(int boxUid, INodeDto note);
        Task<INodeDto> NewNote(int boxUid, int parenNoteUid);      
        Task Remove(int boxUid, int noteUId);
        Task Remove(int boxUid);
        Task<IUserDto> NewUser(IUserDto user);
        Task<INodeDto> NewBox(INodeDto box);
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
