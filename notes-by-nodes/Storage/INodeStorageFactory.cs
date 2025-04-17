using notes_by_nodes.Entities;
using notes_by_nodes.UseCases.AppRules;

namespace notes_by_nodes.Storage
{
    public interface INodeStorageFactory
    {
        IBoxStorage GetBoxStorage();
        IUserStorage GetUserStorage();
        INoteStorage GetNoteStorage(LocalBox box);

    }
}