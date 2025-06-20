using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes_wpfApp.Service;
using Microsoft.Extensions.Options;
using notes_by_nodes_wpfApp.Settings;
using notes_by_nodes.Service;
using notes_by_nodes_wpfApp.ViewModel;
using System.Collections.ObjectModel;
using notes_by_nodes.Entities;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using System.Windows;
using notes_by_nodes_wpfApp.Helpers;
using notes_by_nodes.UseCases;
using notes_by_nodes.Dto;

namespace notes_by_nodes_wpfApp
{
    public partial class MainViewModel(INoteService notesService, IOptions<NotesByNodesSettings> options) : ObservableObject
    {
        //private readonly ModelsPresenter _presenter;
        private readonly INoteService _notesService = notesService ?? throw new ArgumentNullException(nameof(notesService));
        private readonly IOptions<NotesByNodesSettings> _options = options;

        [ObservableProperty]
        private List<UserViewModel> _users = [];

        [ObservableProperty]
        private UserViewModel? _user;

        public ObservableCollection<INoteViewModel> NodesTree { get; } = [];
        //public ObservableCollection<NodeTabItem> Tabs { get; } = [];
        public ObservableCollection<TabItem> Tabs { get; } = [];

        [ObservableProperty]
        private INoteViewModel? _selectedNode;
        partial void OnSelectedNodeChanging(INoteViewModel value)
        {
            ShowNoteInActiveTab(value);
            //ShowNoteInActiveTabCommand.Execute(value);
            //ShowNoteInNewTab(value);

        }

        public async Task InitAsync()
        {
            try
            {
                
                await InitUsers();
                await InitNodesTreeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
        //TO DO вынести из этого класса инициализацию юзера
        async Task InitUsers()
        {
            try
            {
                IUserDto userDTO =  await _notesService.SelectUser("Anton");
                var user = new UserViewModel(userDTO.Uid, userDTO.Name, userDTO.Email, notesService);
                Users.Add(user);
            }
            catch (NoUsersNoteCoreException)
            {
                await CreateNewUser();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                SelectUser();
            }
        }

        private async Task CreateNewUser()
        {
            if (DialogManager.ShowNewUserDialog(out var newUserName))
            {
                IUserDto newuser = new UserViewModel(0, newUserName, "", notesService);
                newuser = await _notesService.NewUser(new UserDto(0, newUserName, ""));
                Users.Add(new UserViewModel(newuser.Uid, newuser.Name, newuser.Email, notesService));
            }

        }

        void SelectUser()
        {
            var activeUser = Users.FirstOrDefault() ?? throw new NullReferenceException("Users collection is empty");
            _notesService.SelectUser(activeUser.Name);
            User = activeUser;
        }

        private async Task InitNodesTreeAsync()
        {

            var boxes = _notesService.GetBoxes().Select(box => new BoxViewModel(box.Uid, box.Name, box.Description, box.Text));
            foreach (var box in boxes)
            {
                await box.LoadChildNodesAsync();
                NodesTree.Add(box);
            }

        }
        
        public void RemoveBoxFromNodesTree(BoxViewModel boxViewModel)
        {
            NodesTree.Remove(boxViewModel);
        }

    }

}
