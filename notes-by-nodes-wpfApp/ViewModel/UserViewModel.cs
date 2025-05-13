using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using notes_by_nodes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes_wpfApp.ViewModel
{
    public partial class UserViewModel: ObservableObject
    {

        public int Uid { get; init; }
        public string Name { get; init; }
        
        private INoteService _noteService { get; init; }

        [ObservableProperty]
        private string email;

        partial void OnEmailChanged(string? oldValue, string newValue)
        {
            if (oldValue != null && oldValue != String.Empty)
                _noteService.ModifyUser(0, newValue, "");
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
