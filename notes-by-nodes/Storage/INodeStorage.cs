using notes_by_nodes.AppRules;
using notes_by_nodes.Entities;

namespace notes_by_nodes.Storage
{
    public interface INodeStorage
    {
        //void LoadChildNodes(Node parentNode);
        Task<IEnumerable<Node>> LoadChildNodesAsync(Node parentNode);
        void RemoveNode(Node note);
    }

    public interface INoteStorage : INodeStorage
    {
        IEnumerable<Note> GetNotesHasReferenceToIt(Note note);
        IEnumerable<Note> GetReferencedNotes(Note note);
        Task<LocalNote> GetNoteAsync(int Uid);
        void RemoveNote(LocalNote note);
        Task SaveNoteAsync(LocalNote note);
    }
    public interface IBoxStorage : INodeStorage
    {
        Task<LocalBox> GetBoxAsync(int Uid);
        //void SaveBox(LocalBox box);
        Task SaveBoxAsync(LocalBox box);
    }
    public interface IUserStorage : INodeStorage
    {        
        Task<LocalUser> GetUser(string name);
        LocalUser GetUser(int Uid);
        Task SaveUserAsync(LocalUser user);

    }
}