using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using notes_by_nodes.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes_wpfApp.ViewModel
{
    public partial class UserViewModel: ObservableObject, IUserDto
    {

        public int Uid { get; init; }
        
        private INoteService NoteService { get; init; }

        [ObservableProperty]
        private string email;
        [ObservableProperty]
        private string name;

        partial void OnEmailChanged(string? oldValue, string newValue)
        {
            if (oldValue != null && oldValue != String.Empty)
                NoteService.ModifyUser((IUserDto)this);
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }

        public void NewChild()
        {
            //new Box
            throw new NotImplementedException();
        }

        public void RemoveChild(INoteViewModel childNote)
        {
            //remove Box
            throw new NotImplementedException();
        }

        public UserViewModel(int uid, string name, string email, INoteService noteService)
        {
            Email = email;
            Uid = uid;
            Name = name;
            // NotesByNodesApp app = (NotesByNodesApp)NotesByNodesApp.Current;
            NoteService = noteService;


        }
    }
}
