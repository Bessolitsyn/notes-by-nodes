using CommunityToolkit.Mvvm.ComponentModel;
using notes_by_nodes.Service;
using System;

namespace notes_by_nodes_winUI.ViewModel
{
    public partial class UserViewModel : ObservableObject, IUserDto
    {
        public int Uid { get; init; }

        private ISingleUserNoteService NoteService { get; init; }

        [ObservableProperty]
        private string email;
        [ObservableProperty]
        private string name;

        partial void OnEmailChanged(string? oldValue, string newValue)
        {
            if (oldValue != null && oldValue != string.Empty)
                NoteService.ModifyUser((IUserDto)this);
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }

        public void NewChild()
        {
            // new Box
            throw new NotImplementedException();
        }

        public void RemoveChild(INoteViewModel childNote)
        {
            // remove Box
            throw new NotImplementedException();
        }

        public UserViewModel(int uid, string name, string email, ISingleUserNoteService noteService)
        {
            Email = email;
            Uid = uid;
            Name = name;
            NoteService = noteService;
        }
    }
}
