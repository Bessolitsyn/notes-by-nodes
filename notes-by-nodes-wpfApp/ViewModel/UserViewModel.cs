using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using notes_by_nodes.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes_wpfApp.ViewModel
{
    public partial class UserViewModel: ObservableObject, IParentNode
    {

        public int Uid { get; init; }
        
        private INoteService _noteService { get; init; }

        [ObservableProperty]
        private string email;
        [ObservableProperty]
        private string name;

        partial void OnEmailChanged(string? oldValue, string newValue)
        {
            if (oldValue != null && oldValue != String.Empty)
                _noteService.ModifyUser((IUserDto)this);
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

        public UserViewModel(int uid, string name, string email)
        {
            Email = email;
            Uid = uid;
            Name = name;
            NotesByNodesApp app = (NotesByNodesApp)NotesByNodesApp.Current;
            _noteService = app.ServiceProvider.GetRequiredService<INoteService>();



        }
    }
}
