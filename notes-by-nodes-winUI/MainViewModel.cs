using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using notes_by_nodes.Dto;
using notes_by_nodes.Service;
using notes_by_nodes.UseCases;
using notes_by_nodes_winUI.Helpers;
using notes_by_nodes_winUI.Service;
using notes_by_nodes_winUI.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace notes_by_nodes_winUI
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ISingleUserNoteService _notesService;

        [ObservableProperty]
        private List<UserViewModel> _users = [];

        [ObservableProperty]
        private UserViewModel? _user;

        public ObservableCollection<INoteViewModel> NodesTree { get; } = [];
        public ObservableCollection<TabViewItem> Tabs { get; } = [];

        [ObservableProperty]
        private INoteViewModel? _selectedNode;

        //public partial INoteViewModel? SelectedNode { get; set; }

        [ObservableProperty]
        private int _selectedTabIndex = 0;

        partial void OnSelectedNodeChanging(INoteViewModel? value)
        {
            ShowNoteInActiveTab(value);
        }
        public INoteViewModel? RightTappedNode { get; set; }

        public MainViewModel(ISingleUserNoteService notesService)
        {
            _notesService = notesService ?? throw new ArgumentNullException(nameof(notesService));
        }

        public async Task InitAsync()
        {
            try
            {
                await InitUserAsync();
                await InitNodesTreeAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR in InitAsync: {ex}");
                throw;
            }
        }

        private async Task InitUserAsync()
        {
            try
            {
                //TODO  функциональность работы с юзерами.
#warning используется захардкоженое имя пользователя
                var userName = "Anton"; //
                await SelectUserAsync(userName);
                
                Debug.WriteLine($"User found: {userName}");
            }
            catch (NoUsersNoteCoreException)
            {
                var userName = await CreateNewUserAsync();
                await SelectUserAsync(userName);
                //пока выберем первого юзера как текущего.
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"InitUser error: {ex}");
                throw;
            }

        }

        private async Task<string> CreateNewUserAsync()
        {
            var newUserName = await DialogManager.ShowNewUserDialogAsync();
            if (!string.IsNullOrEmpty(newUserName))
            {
#warning используется захардкоженое имя пользователя
                newUserName = "Anton";
                var newuser = await _notesService.NewUser(new UserDto(0, newUserName, ""));
            }
            return newUserName;
        }

        private async Task SelectUserAsync(string name)
        {
            //var activeUser = Users.FirstOrDefault() ?? throw new NullReferenceException("Users collection is empty");
            IUserDto userDTO = await _notesService.SelectUser(name);
            var user = new UserViewModel(userDTO.Uid, userDTO.Name, userDTO.Email, _notesService);
            Users.Add(user);
            User = user;
            Debug.WriteLine($"Selected user: {name}");
        }

        private async Task InitNodesTreeAsync()
        {
            var boxes = _notesService.GetBoxes();
            var boxViewModels = boxes.Select(box =>
            {
                return new BoxViewModel(box.Uid, box.Name, box.Description, box.Text);
            }).ToList();

            foreach (var box in boxViewModels)
            {
                await box.LoadChildNodesAsync();
                NodesTree.Add(box);
            }
        }

        private void RemoveBoxFromNodesTree(BoxViewModel boxViewModel)
        {
            NodesTree.Remove(boxViewModel);
        }

        #region EVENTS HANDLERS
        public void NodeTreeView_SelectionChanged(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewSelectionChangedEventArgs args)
        {
            if (sender.SelectedItems[0] is INoteViewModel selectedNode)
            {
                //SelectTreeNodeItemCommand.Execute(selectedNode);
            }
            
        }
        #endregion

        #region COMMANDS

        [RelayCommand]
        public void CloseTab(TabViewItem tab)
        {
            if (tab != null)
                Tabs.Remove(tab);
        }

        [RelayCommand]
        public async Task RemoveNodeAsync()
        {
            try
            {
                if (SelectedNode != null)
                {
                    await TryExecuteUseCase(SelectedNode.RemoveAsync);
                    if (SelectedNode is BoxViewModel boxViewModel)
                    {
                        RemoveBoxFromNodesTree(boxViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Remove error: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task NewChildNodeAsync()
        {
            try
            {
                if (SelectedNode == null)
                {
                    MessageBox.Show("Please select a node first");
                    return;
                }
                await TryExecuteUseCase(SelectedNode.NewNoteAsync);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"New note error: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task NewBoxAsync()
        {
            try
            {
                var newFolder = await DialogManager.ShowNewBoxDialogAsync();
                if (!string.IsNullOrEmpty(newFolder))
                {

                    INodeDto newboxDto = await _notesService.NewBox(new NodeDto(0, newFolder.TrimEnd(), "New Box", ""));
                    var newbox = new BoxViewModel(newboxDto.Uid, newboxDto.Name, newboxDto.Description, newboxDto.Text);
                    NodesTree.Insert(0, newbox);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"New box error: {ex.Message}");
            }
        }

        [RelayCommand]
        void ShowNoteInActiveTab(INoteViewModel? node)
        {
            //TODO попробовать сделать через SelectedTab
            if (node == null) return;

            if (Tabs.Count == 0)
            {
                var tabItem = NoteTabItemBuilder.GetNoteEditorTab(node, CloseTabCommand);
                Tabs.Insert(0, tabItem);
                SelectedTabIndex = 0;
            }
            else
            {
                var selectedTabIndex = SelectedTabIndex;
                Tabs[selectedTabIndex] = NoteTabItemBuilder.GetNoteEditorTab(node, CloseTabCommand);
                SelectedTabIndex = selectedTabIndex;
                //if (Tabs.Any(t => t.IsSelected))
                //{
                //    var tabItem = Tabs.Single(t => t.IsSelected);
                //    tabItem.IsSelected = false;

                //    int currentTabIndex = Tabs.IndexOf(tabItem);
                //    Tabs[currentTabIndex] = NoteTabItemBuilder.GetNoteEditorTab(node, CloseTabCommand);
                //    //Tabs[currentTabIndex].IsSelected = true;
                //}
            }
        }

        [RelayCommand]
        void ShowNoteInNewTab(INoteViewModel? node)
        {
            if (node == null) node = RightTappedNode;
            var tabItem = NoteTabItemBuilder.GetNoteEditorTab(node, CloseTabCommand);
            Tabs.Add(tabItem);
            tabItem.IsSelected = true;
        }

        [RelayCommand]
        void SelectTreeNodeItem(INoteViewModel selectedNode)
        {
            SelectedNode = selectedNode;
        }

        [RelayCommand]
        public void OpenGraphViewer()
        {
            if (SelectedNode != null)
            {
                var tabItem = NoteTabItemBuilder.GetGraphViewerTab(SelectedNode, CloseTabCommand);
                Tabs.Add(tabItem);
                tabItem.IsSelected = true;
            }
        }

        static async Task TryExecuteUseCase(Func<Task> action)
        {
            try
            {
                if (action != null)
                    await action.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TryExecuteUseCase error: {ex}");
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion
    }
}
