using notes_by_nodes.Entities;
using notes_by_nodes.UseCases.AppRules;

namespace notes_by_nodes.Storage
{
    public interface INodeStorage
    {
        IEnumerable<Node> GetChildNodes(Node parentNode);
    }

    public interface INoteStorage : INodeStorage
    {
        IEnumerable<Note> GetNotesHasReferenceToIt(Note note);
        IEnumerable<Note> GetReferencedNotes(Note note);
        LocalNote GetNote(int Uid);
        void SaveNote(LocalNote note);
        Task SaveNoteAsync(LocalNote note);
    }
    public interface IBoxStorage : INodeStorage
    {
        LocalBox GetBox(int Uid);
        void SaveBox(LocalBox box);
        Task SaveBoxAsync(LocalBox box);
    }
    public interface IUserStorage : INodeStorage
    {
        LocalUser [] GetUsers();
        LocalUser GetUser(int Uid);
        void SaveUser(LocalUser user);

    }
}