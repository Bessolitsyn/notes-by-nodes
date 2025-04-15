using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("TestProject")]
namespace notes_by_nodes.UseCases
{
    internal class InsideInteractor
    {
        const string PATH_TO_PROFILES = "";
        protected IUserStorage Storage { get; init; }       

        InsideInteractor(IUserStorage storage)
        {
            Storage = Storage;
        }

        LocalUser GetUser(int uid)
        {
            return Storage.GetUser(uid);
        }

    }
}
