using notes_by_nodes.UseCases.AppRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes.Services
{
    public interface INoteService
    {
        void SetUserProfile(string profilePath);
        void GetUsers();
        void SetActiveUser(int userUid);
        void MakeUserProfile();
        void GetBoxes();
        void AddNewBox(string name);

    }
}
