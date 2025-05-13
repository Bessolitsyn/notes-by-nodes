using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Storage;
using notes_by_nodes.UseCases;
using Microsoft.VisualBasic;
using notes_by_nodes.AppRules;

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

        public IEnumerable<(int, string, string)> GetBoxes()
        {
            var boxes = coreInteractor.GetBoxes();
            return boxes.Select(b => (b.Uid, b.Name, b.Description));
        }
        
        public IEnumerable<(int, string, string)> GetUsers()
        {
            var users = userInteractor.GetUsers();
            if (users != null && users.Length > 0)
                return users.Select(u => (u.Uid, u.Name, u.Email));
            else throw new NullRefernceUseCaseException("No users");
        }
        public IEnumerable<(int, string, string)> GetChildNodesOfTheBox(int boxUid)
        {
            var box = coreInteractor.GetBox(boxUid);
            var childNodes = box.HasChildNodes.Select(u => (u.Uid, u.Name, u.Description));
            return childNodes;
        }

        public IEnumerable<(int, string, string)> GetChildNodes(int boxUid, int parentNodeUid)
        {
            (int, string, string)[] childNotes = [];
            var childNodes = coreInteractor.LoadChildNodes(boxUid, parentNodeUid).Select(u => (u.Uid, u.Name, u.Description));
            return childNodes;
        }


        public void ModifyBox(int nodeUid, string name, string desc)
        {
            var box = coreInteractor.GetBox(nodeUid);
            box.Name = name;
            box.Description = desc;
            coreInteractor.SaveBox(box);
        }

        public void ModifyNote(int boxUid, int noteUid, string name, string desc)
        {
            //var box = coreInteractor.GetBox(boxUid);
            var note = coreInteractor.GetNote(boxUid, noteUid);
            note.Description = desc;
            note.Name = name;
            coreInteractor.SaveNote(boxUid, note);
        }

        public void ModifyUser(int userUid, string name, string email)
        {
            activeUser.Name = name;
            userInteractor.SaveUser(activeUser);
        }


        public void SelectUser(int userUid)
        {
            activeUser = userInteractor.GetUser(userUid);
            coreInteractor = new CoreInteractor(storageFactory, activeUser);      

        }

    }
}
