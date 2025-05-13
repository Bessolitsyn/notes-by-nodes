using notes_by_nodes.AppRules;
using notes_by_nodes.Entities;

namespace notes_by_nodes.Storage
{
    public interface INodeStorageFactory
    {
        IBoxStorage GetBoxStorage();
        IUserStorage GetUserStorage();
        INoteStorage GetNoteStorage(LocalBox box);

    }
}