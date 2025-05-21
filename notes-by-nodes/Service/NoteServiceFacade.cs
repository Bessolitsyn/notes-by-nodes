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
using notes_by_nodes.Entities;

namespace notes_by_nodes.Services
{
    public class NoteServiceFacade : INoteService
    {
        private readonly UserInteractor userInteractor;
        private readonly INodeStorageFactory storageFactory;
        private LocalUser activeUser;
        private CoreInteractor coreInteractor;

        public NoteServiceFacade(INodeStorageFactory storageFactory)
        {


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
            return boxes.Cast<INodeDto>();
        }

        public IEnumerable<IUserDto> GetUsers()
        {
            var users = userInteractor.GetUsers();
            if (users != null && users.Length > 0)
                return users;
            else throw new NullRefernceUseCaseException("No users");
        }
        public IEnumerable<INodeDto> GetChildNodesOfTheBox(int boxUid)
        {
            var box = coreInteractor.GetBox(boxUid);
            var childNodes = box.HasChildNodes.Cast<INodeDto>();
            return childNodes;
        }

        public IEnumerable<INodeDto> GetChildNodes(int boxUid, int parentNodeUid)
        {
            var childNodes = coreInteractor.LoadChildNodes(boxUid, parentNodeUid).Cast<INodeDto>();
            return childNodes;
        }


        public void ModifyBox(INodeDto box)
        {
            var _box = coreInteractor.GetBox(box.Uid);
            _box.Name = box.Name;
            _box.Description = box.Description;
            _box.Text = box.Text;
            coreInteractor.SaveBox(_box);
        }

        public void ModifyNote(int boxUid, INodeDto note)
        {
            //var box = coreInteractor.GetBox(boxUid);
            var _note = coreInteractor.GetNote(boxUid, note.Uid);
            _note.Description = note.Description;
            _note.Name = note.Name;
            _note.Text = note.Text;
            coreInteractor.SaveNote(boxUid, _note);
        }

        public INodeDto NewNote(int boxUid, INodeDto note, INodeDto childNote)
        {
            LocalNote _childNote;
            if (boxUid != note.Uid)
            {
                var _note = coreInteractor.GetNote(boxUid, note.Uid);
                _childNote = new LocalNote(_note, childNote.Name, childNote.Text) { Description = childNote.Description };
                coreInteractor.SaveNote(boxUid, _note);
                coreInteractor.SaveNote(boxUid, _childNote);

            }
            else {
                var _box = coreInteractor.GetBox(boxUid);
                _childNote = new LocalNote(_box, childNote.Name, childNote.Text) { Description = childNote.Description };
                coreInteractor.SaveBox(_box);
                coreInteractor.SaveNote(_box.Uid, _childNote);
            }
            return _childNote;
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

        public void Remove(int boxUid, INodeDto note)
        {
            var _note = coreInteractor.GetNote(boxUid, note.Uid);
            coreInteractor.Remove(boxUid, _note);
        }
    }
}
