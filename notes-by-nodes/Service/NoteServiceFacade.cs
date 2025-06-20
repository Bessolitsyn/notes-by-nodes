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
using notes_by_nodes.Dto;

namespace notes_by_nodes.Service
{
    public class NoteServiceFacade : INoteService
    {
        private readonly UserInteractor _userInteractor;
        private readonly INodeStorageProvider _storageProvider;
        private LocalUser _activeUser;
        private CoreInteractor _coreInteractor;

        public NoteServiceFacade(INodeStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
            _userInteractor = new UserInteractor(storageProvider);
        }


        public IEnumerable<INodeDto> GetBoxes()
        {
            var boxes = _coreInteractor.GetBoxes();
            return boxes.Cast<INodeDto>();
        }
       
        public async Task<IEnumerable<INodeDto>> GetChildNodesOfTheBox(int boxUid)
        {
            IEnumerable<INodeDto> childNodes = [];
            var box = await _coreInteractor.GetBox(boxUid);
            childNodes = box.HasChildNodes.Cast<INodeDto>();
            return childNodes;
        }

        public async Task<IEnumerable<INodeDto>> GetChildNodes(int boxUid, int parentNodeUid)
        {
            IEnumerable<INodeDto> childNodes = [];
            if (boxUid == parentNodeUid)
            {
                childNodes = await GetChildNodesOfTheBox(boxUid);
            }
            else
            {
                var result = await _coreInteractor.LoadChildNodes(boxUid, parentNodeUid);
                if (result != null)
                    childNodes = result.Cast<INodeDto>();
            }
            return childNodes;
        }


        public async Task ModifyBox(INodeDto boxDto)
        {
            var box = await _coreInteractor.GetBox(boxDto.Uid);
            box.Name = boxDto.Name;
            box.Description = boxDto.Description;
            box.Text = boxDto.Text;
            await _coreInteractor.SaveBox(box);
        }

        public async Task ModifyNote(int boxUid, INodeDto noteDto)
        {
            //var box = coreInteractor.GetBox(boxUid);
            var note = await _coreInteractor.GetNote(boxUid, noteDto.Uid);
            note.Description = noteDto.Description;
            note.Name = noteDto.Name;
            note.Text = noteDto.Text;
            await _coreInteractor.SaveNote(boxUid, note);
        }

        public async Task<INodeDto> NewNote(int boxUid, int parenNoteUid)
        {
            string titleForNewNote = "Untitled";

            LocalNote childNote;
            if (boxUid != parenNoteUid)
            {
                var note = await _coreInteractor.GetNote(boxUid, parenNoteUid);
                childNote = new LocalNote(note, titleForNewNote, "");
                await _coreInteractor.SaveNote(boxUid, note);
                await _coreInteractor.SaveNote(boxUid, childNote);

            }
            else
            {
                var box = await _coreInteractor.GetBox(boxUid);
                childNote = new LocalNote(box, titleForNewNote, "");
                await _coreInteractor.SaveBox(box);
                await _coreInteractor.SaveNote(box.Uid, childNote);
            }
            return childNote;
        }

        public async Task ModifyUser(IUserDto user)
        {
            _activeUser.Name = user.Name;
            _activeUser.Email = user.Email;
            await _userInteractor.SaveUser(_activeUser);
        }


        public async Task<IUserDto> SelectUser(string name)
        {
            _activeUser =await _userInteractor.GetUser(name);
            _coreInteractor = new CoreInteractor(_storageProvider, _activeUser);
            return new UserDto(_activeUser.Uid, _activeUser.Name, _activeUser.Email);

        }

        public async Task Remove(int boxUid, int noteUid)
        {

            var note = await _coreInteractor.GetNote(boxUid, noteUid);
            var box = await _coreInteractor.GetBox(boxUid);
            await _coreInteractor.RemoveNote(box, note);
        }
        public async Task Remove(int boxUid)
        {
            var box = await _coreInteractor.GetBox(boxUid);
            await _coreInteractor.RemoveBox(box);

        }

        public async Task<IUserDto> NewUser(IUserDto user)
        {
            user = await _userInteractor.MakeUser(user.Name, user.Email);
            return user;

        }

        public async Task<INodeDto> NewBox(INodeDto box)
        {
            box = await _coreInteractor.NewBox(box.Name, box.Description);
            return box;
        }
    }
}
