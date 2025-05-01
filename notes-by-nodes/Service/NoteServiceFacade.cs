using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;
using notes_by_nodes.UseCases;
using Microsoft.VisualBasic;

namespace notes_by_nodes.Services
{
    public class NoteServiceFacade : INoteService
    {
        private readonly UserInteractor userInteractor;
        private readonly INodeStorageFactory storageFactory;
        private readonly INotePresenter notePresenter;

        private LocalUser activeUser;
        private CoreInteractor coreInteractor;

        public NoteServiceFacade(INotePresenter notePresenter, INodeStorageFactory storageFactory) {

            this.notePresenter = notePresenter;
            this.storageFactory = storageFactory;
            this.userInteractor = new UserInteractor(storageFactory);

        }

        public void AddNewBox(string name)
        {
            throw new NotImplementedException();
        }

        public void GetBoxes()
        {
            var boxes = coreInteractor.GetBoxes();
            notePresenter.SetBoxes(boxes.Select(b => (b.Uid, b.Name, b.Description)));
        }

        public void GetUsers()
        {
            var users = userInteractor.GetUsers();
            if (users != null && users.Length > 0)
                notePresenter.SetUsers(users.Select(u => (u.Uid, u.Name, u.Email)));
            else throw new NullRefernceUseCaseException("No users");
        }

        public void LoadNotesIntoParentNode(int boxUid)
        {
            throw new NotImplementedException();
        }

        public void MakeUserProfile()
        {
            throw new NotImplementedException();
        }

        public void SetActiveUser(int userUid)
        {
            activeUser = userInteractor.GetUser(userUid);
            coreInteractor = new CoreInteractor(storageFactory, activeUser);
            


        }

    }
}
