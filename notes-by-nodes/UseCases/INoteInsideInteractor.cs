
namespace notes_by_nodes.Entities
{
    public interface INoteInsideInteractor : INodeInsideInteractor
    {
        IEnumerable<Note> GetNotesHasReferenceToIt(Note note);
        IEnumerable<Note> GetReferencedNotes(Note note);
    }

    public interface INodeInsideInteractor
    {
        abstract IEnumerable<Node> GetChildNodes(Node parentNode);
    }
}