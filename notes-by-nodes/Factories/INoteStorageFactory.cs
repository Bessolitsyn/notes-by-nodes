using notes_by_nodes.Entities;

namespace notes_by_nodes.Factories
{
    public interface INoteStorageFactory
    {
        public IEnumerable<Note> GetNotesFrom(Box box);
        public IEnumerable<Note> GetBoxes(User user);

    }
}