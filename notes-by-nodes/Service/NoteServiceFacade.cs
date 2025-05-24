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

namespace notes_by_nodes.Service
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
            else throw new NoUsersNoteCoreException();
        }
        public async Task<IEnumerable<INodeDto>> GetChildNodesOfTheBox(int boxUid)
        {
            IEnumerable<INodeDto> childNodes = [];
            await Task.Run(() =>
            {
                var box = coreInteractor.GetBox(boxUid);
                childNodes = box.HasChildNodes.Cast<INodeDto>();
               
            });
            //await Task.Delay(1000);
            return childNodes;
        }

        public async Task<IEnumerable<INodeDto>> GetChildNodes(int boxUid, int parentNodeUid)
        {
            IEnumerable<INodeDto> childNodes = [];
            await Task.Run(() => childNodes = coreInteractor.LoadChildNodes(boxUid, parentNodeUid).Cast<INodeDto>());
            //await Task.Delay(1000);
            return childNodes;
        }


        public void ModifyBox(INodeDto boxDto)
        {
            var box = coreInteractor.GetBox(boxDto.Uid);
            box.Name = boxDto.Name;
            box.Description = boxDto.Description;
            box.Text = boxDto.Text;
            coreInteractor.SaveBox(box);
        }

        public void ModifyNote(int boxUid, INodeDto noteDto)
        {
            //var box = coreInteractor.GetBox(boxUid);
            var note = coreInteractor.GetNote(boxUid, noteDto.Uid);
            note.Description = noteDto.Description;
            note.Name = noteDto.Name;
            note.Text = noteDto.Text;
            coreInteractor.SaveNote(boxUid, note);
        }

        public async Task<INodeDto> NewNote(int boxUid, int parenNoteUid)
        {
            string titleForNewNote = "Untitled";

            return await Task.Run(() =>
            {
                LocalNote childNote;
                if (boxUid != parenNoteUid)
                {
                    var note = coreInteractor.GetNote(boxUid, parenNoteUid);
                    childNote = new LocalNote(note, titleForNewNote, "");
                    coreInteractor.SaveNote(boxUid, note);
                    coreInteractor.SaveNote(boxUid, childNote);

                }
                else
                {
                    var box = coreInteractor.GetBox(boxUid);
                    childNote = new LocalNote(box, titleForNewNote, "");
                    coreInteractor.SaveBox(box);
                    coreInteractor.SaveNote(box.Uid, childNote);
                }
            return childNote;
            });
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

        public Task Remove(int boxUid, int noteUid)
        {
            return Task.Run(() =>
            {
                var note = coreInteractor.GetNote(boxUid, noteUid);
                var box = coreInteractor.GetBox(boxUid);
                coreInteractor.RemoveNote(box, note);
            });
        }
        public Task Remove(int boxUid)
        {
            return Task.Run(() =>
            {
                var box = coreInteractor.GetBox(boxUid);
                coreInteractor.RemoveBox(box);
            });
        }

        public IUserDto NewUser(IUserDto user)
        {
            user = userInteractor.MakeUser(user.Name, user.Email);
            return user;

        }

        public INodeDto NewBox(INodeDto box)
        {
            box = coreInteractor.NewBox(box.Name, box.Description);
            return box;
        }
    }
}
