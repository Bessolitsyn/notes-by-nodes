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
using notes_by_nodes.Services;
using notes_by_nodes_wpfApp.ViewModel;
using System.Collections.ObjectModel;

namespace notes_by_nodes_wpfApp
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly MainViewModelPresenter _presenter;
        private readonly INoteService _notesService;

        public ObservableCollection<NodeViewModel> NodesTree { get; } = [];

        [ObservableProperty]
        private List<UserViewModel> _users = [];

        [ObservableProperty]
        private string _user = "No User";

        [ObservableProperty]
        private string _note = "No Note";

        [ObservableProperty]
        private NodeViewModel _selectedNode;

        //props changing events handlers
        
        //


        public MainViewModel(INotePresenter presenter, INoteService notesService, IOptions<NotesByNodesSettings> options)
        {
            _presenter = (MainViewModelPresenter)presenter;
            _notesService = notesService;        
        }

        [RelayCommand]
        private void UpdateData()
        {
            User = "Anton";
            Note = "My first Note";
            
        }
        public void Init()
        {
            _presenter.SetMainViewModel(this);
            _notesService.GetUsers();
            var activeUser = Users.FirstOrDefault() ?? throw new NullReferenceException("Users collection is empty");
            User = activeUser.Name;
            _notesService.SetActiveUser(activeUser.Uid);
            _notesService.GetBoxes();


        }
    }
}
