using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes_wpfApp.Services;
using Microsoft.Extensions.Options;
using notes_by_nodes_wpfApp.Settings;

namespace notes_by_nodes_wpfApp
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly INotePresenter _userService;
        private readonly INoteService _notesService;

        [ObservableProperty]
        private string _user;

        [ObservableProperty]
        private string _note;

        public MainViewModel(INotePresenter userService, INoteService notesService, IOptions<NotesByNodesSettings> options)
        {
            _userService = userService;
            _notesService = notesService;
        }

        [RelayCommand]
        private void UpdateData()
        {
            _user = "Anton";
            _note = "My first Note";
            
        }
    }
}
