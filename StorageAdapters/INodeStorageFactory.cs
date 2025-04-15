using notes_by_nodes.Entities;
using notes_by_nodes.UseCases.AppRules;

namespace notes_by_nodes.Storage
{
    public interface INodeStorageFactory
    {
        IBoxStorage GetBoxStorage(string storageFolder);
        IUserStorage GetUserStorage(string storageFolder);
        
    }
}