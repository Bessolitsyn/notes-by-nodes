using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases.AppRules;
using notes_by_nodes.UseCases;

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
            throw new NotImplementedException();
        }

        public void GetUsers()
        {
            throw new NotImplementedException();
        }

        public void MakeUserProfile()
        {
            throw new NotImplementedException();
        }

        public void SetActiveUser(int userUid)
        {
            coreInteractor = new CoreInteractor(storageFactory, activeUser);            
        }

        public void SetUserProfile(string profilePath)
        {
            throw new NotImplementedException();
        }
    }
}
