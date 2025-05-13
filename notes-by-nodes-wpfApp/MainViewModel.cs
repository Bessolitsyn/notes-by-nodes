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
using notes_by_nodes.Entities;

namespace notes_by_nodes_wpfApp
{
    public partial class MainViewModel : ObservableObject
    {
        //private readonly ModelsPresenter _presenter;
        private readonly INoteService _notesService;


        [ObservableProperty]
        private List<UserViewModel> _users = [];
        [ObservableProperty]
        private UserViewModel _user; 
        

        [ObservableProperty]
        private string _note = "No Note";
        
        public ObservableCollection<INodeViewModel> NodesTree { get; } = [];

        [ObservableProperty]
        private INodeViewModel _selectedNode;
        partial void OnSelectedNodeChanging(INodeViewModel value)
        {
            if (!value.IsLoaded)
            {
                value.LoadChildNodes();               
            }
            foreach (var note in value.ChildNodes)
            {
                if (!note.IsLoaded) note.LoadChildNodes();
            }
        }


        public MainViewModel(INoteService notesService, IOptions<NotesByNodesSettings> options)
        {
            //_presenter = (ModelsPresenter)presenter;
            _notesService = notesService;   
            
        }

        [RelayCommand]
        private void UpdateData()
        {
            //User = "Anton";
            Note = "My first Note";
            
        }
        public void Init()
        {
            //_presenter.Attach(this);
            InitUsers();
            SelectUser();
            InitNodesTree();


        }

        void InitUsers()
        {
            var users =  _notesService.GetUsers().Select(user => new UserViewModel(user.Item1, user.Item2, user.Item3));
            Users.AddRange(users);                
        }
        void SelectUser()
        {
            var activeUser = Users.FirstOrDefault() ?? throw new NullReferenceException("Users collection is empty");
            _notesService.SelectUser(activeUser.Uid);
            User = activeUser;
        }
        void InitNodesTree()
        {
            var boxes = _notesService.GetBoxes().Select(box => new BoxViewModel(box.Item1, box.Item2, box.Item3));
            foreach (var box in boxes)
            {
                box.LoadChildNodes();
                NodesTree.Add(box);
            }
        }

    }
}
