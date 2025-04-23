using notes_by_nodes.Entities;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("TestProject")]
namespace notes_by_nodes.UseCases
{
    internal class InsideInteractor
    {
        protected IUserStorage Storage { get; init; }
        protected INodeStorageFactory StorageFactor { get; init; }
        protected LocalUser ActiveUser { get; init; }

        internal InsideInteractor(INodeStorageFactory storageFactory, int activeUserUID)
        {
            Storage = storageFactory.GetUserStorage();
            ActiveUser = Storage.GetUser(activeUserUID);
        }


        internal IEnumerable<LocalBox> GetBoxes()
        {
            foreach (var item in ActiveUser.HasChildNodes)
            {
                if (item is LocalBox lbox)
                {
                    yield return lbox;
                }
            }
        }


        //internal IEnumerable<string> NewBox()
        //{
        //    var boxStorage = StorageFactor.GetBoxStorage();
        //    boxStorage.Ne


        //}


    }
}
