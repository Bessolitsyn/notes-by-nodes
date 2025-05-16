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
        //private TabControl _noteTabControl;


        [ObservableProperty]
        private List<UserViewModel> _users = [];
        [ObservableProperty]
        private UserViewModel _user; 
        

        [ObservableProperty]
        private string _note = "No Note";
        
        public ObservableCollection<INoteViewModel> NodesTree { get; } = [];
        public ObservableCollection<NodeTabItem> Tabs { get; } = [];

        [ObservableProperty]
        private INoteViewModel _selectedNode;
        partial void OnSelectedNodeChanging(INoteViewModel value)
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
            AddNoteTabToTabControl(value);

        }

        public ICommand CloseTabCommand => new RelayCommand<NodeTabItem>(tab =>
        {
            if (tab!=null)
                Tabs.Remove(tab);
        });
        public ICommand RemoveNodeCommand => new RelayCommand<INoteViewModel>(node =>
        {
            if(node != null)
                TryExecuteUseCase(node.Remove);

        });
        public ICommand NewChildNodeCommand => new RelayCommand<INoteViewModel>(node =>
        {
            if (node!=null)
                TryExecuteUseCase(node.NewChild);
        });

        void TryExecuteUseCase(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception)
            {
#warning TO DO uniwersal error message box;
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
        public void Init()//TabControl noteTab)
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

        void AddNoteTabToTabControl(INoteViewModel node)
        {
            var tabItem = NoteTabItemBuilder.GetNoteEditorTabItem(node, CloseTabCommand);
            //var nodeTabItem = NodeTabItem.CastToTabItem(tabItem, node.Uid);

            if (!Tabs.Any(t => t.NodeUid == node.Uid))
            {
                Tabs.Add(tabItem);
                tabItem.IsSelected = true;
            }
            else {
                Tabs.Single(t => t.NodeUid == node.Uid).IsSelected = true;
            }

        }



    }

    public class CustomTemplateSelector : DataTemplateSelector
    {
        public HierarchicalDataTemplate BoxTemplate { get; set; }
        public HierarchicalDataTemplate NoteTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is BoxViewModel)
                return BoxTemplate;
            else if (item is NoteViewModel)
                return NoteTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
