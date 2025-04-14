
using notes_by_nodes.Storage;

namespace notes_by_nodes.Entities
{
    public interface INodeStorage
    {
        IEnumerable<Node> GetChildNodes(Node parentNode);
    }

    public interface INoteStorage : INodeStorage
    {
        IEnumerable<Note> GetNotesHasReferenceToIt(Note note);
        IEnumerable<Note> GetReferencedNotes(Note note);
    }
    public interface IBoxStorage : INoteStorage
    {

    }
    public interface IUserStorage : INodeStorage
    {

    }

    public interface INodeStorageFactory
    {
        IBoxStorage GetBoxStorage();
        IUserStorage GetUserStorage();
        INoteStorage GetNoteStorage();
    }
}