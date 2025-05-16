using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases;
using Microsoft.VisualBasic;
using notes_by_nodes.AppRules;
using System.Xml.Linq;

namespace notes_by_nodes.Services
{
    public class NoteServiceFacade : INoteService
    {
        private readonly UserInteractor userInteractor;
        private readonly INodeStorageFactory storageFactory;
        private LocalUser activeUser;
        private CoreInteractor coreInteractor;

        public NoteServiceFacade(INodeStorageFactory storageFactory) {

            
            this.storageFactory = storageFactory;
            this.userInteractor = new UserInteractor(storageFactory);

        }

        public void AddNewBox(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<INodeDto> GetBoxes()
        {
            var boxes = coreInteractor.GetBoxes();
            //return boxes.Select(b => (b.Uid, b.Name, b.Description));
            return boxes.Cast<INodeDto>();
        }
        
        public IEnumerable<IUserDto> GetUsers()
        {
            var users = userInteractor.GetUsers();
            if (users != null && users.Length > 0)
                //return users.Select(u => (u.Uid, u.Name, u.Email));
                return users;
                //return users.Cast<IUserDto>()
            else throw new NullRefernceUseCaseException("No users");
        }
        public IEnumerable<INodeDto> GetChildNodesOfTheBox(int boxUid)
        {
            var box = coreInteractor.GetBox(boxUid);
            //var childNodes = box.HasChildNodes.Select(u => (u.Uid, u.Name, u.Description));
            var childNodes = box.HasChildNodes.Cast<INodeDto>();
            return childNodes;
        }

        public IEnumerable<INodeDto> GetChildNodes(int boxUid, int parentNodeUid)
        {
            //INodeDto[] childNotes = [];
            //var childNodes = coreInteractor.LoadChildNodes(boxUid, parentNodeUid);
            var childNodes = coreInteractor.LoadChildNodes(boxUid, parentNodeUid).Cast<INodeDto>();
            return childNodes;
        }


        public void ModifyBox(INodeDto box)
        {
            var _box = coreInteractor.GetBox(box.Uid);
            _box.Name = box.Name;
            _box.Description = box.Description;
            coreInteractor.SaveBox(_box);
        }

        public void ModifyNote(int boxUid, INodeDto note)
        {
            //var box = coreInteractor.GetBox(boxUid);
            var _note = coreInteractor.GetNote(boxUid, note.Uid);
            _note.Description = note.Description;
            _note.Name = note.Name;
            coreInteractor.SaveNote(boxUid, _note);
        }

        public void ModifyUser(IUserDto user)
        {
            activeUser.Name = user.Name;
            activeUser.Email = user.Email;
            userInteractor.SaveUser(activeUser);
        }


        public void SelectUser(int userUid)
        {
            activeUser = userInteractor.GetUser(userUid);
            coreInteractor = new CoreInteractor(storageFactory, activeUser);      

        }

    }
}
