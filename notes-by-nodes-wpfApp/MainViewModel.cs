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
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using System.Windows;

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
        
        public ObservableCollection<INoteViewModel> NodesTree { get; } = [];
        public ObservableCollection<NodeTabItem> Tabs { get; } = [];

        [ObservableProperty]
        private INoteViewModel _selectedNode;
        partial void OnSelectedNodeChanging(INoteViewModel value)
        {
            try
            {
                if (!value.IsLoaded)
                {
                    //TryExecuteUseCase(value.LoadChildNodes);
                    value.LoadChildNodes();
                }
                foreach (var note in value.ChildNodes)
                {
                    if (!note.IsLoaded) //TryExecuteUseCase(value.LoadChildNodes);
                        note.LoadChildNodes();
                }
                ShowNoteInActiveTab(value);
            }
            catch (Exception ex )
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }         

        }

        //#region EVENTS

        //public void NodeTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
            
        //}

        //#endregion EVENTS

        public MainViewModel(INoteService notesService, IOptions<NotesByNodesSettings> options)
        {
            //_presenter = (ModelsPresenter)presenter;
            _notesService = notesService;               
        }

        public void Init()
        {
            //_presenter.Attach(this);
            //_noteTabControl = noteTab;
            //_noteTabControl.ItemsSource = Tabs;
            InitUsers();
            SelectUser();
            InitNodesTree();
        }

        void InitUsers()
        {
            var users =  _notesService.GetUsers().Select(user => new UserViewModel(user.Uid, user.Name, user.Email));
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
            var boxes = _notesService.GetBoxes().Select(box => new BoxViewModel(box.Uid, box.Name, box.Description, box.Text, _user));
            foreach (var box in boxes)
            {
                box.LoadChildNodes();
                NodesTree.Add(box);
            }
        }

        void ShowNoteInActiveTab(INoteViewModel node)
        {
            var tabItem = NoteTabItemBuilder.GetNoteEditorTabItem(node, CloseTabCommand);
            //var nodeTabItem = NodeTabItem.CastToTabItem(tabItem, node.Uid);
            //Tabs.Clear();
            if (Tabs.Count>0)
                Tabs.RemoveAt(0);
            Tabs.Insert(0, tabItem);
            tabItem.IsSelected = true;
        }



    }
    
}
