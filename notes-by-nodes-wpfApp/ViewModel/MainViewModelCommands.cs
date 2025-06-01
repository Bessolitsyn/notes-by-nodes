using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using notes_by_nodes.Dto;
using notes_by_nodes.Entities;
using notes_by_nodes.Service;
using notes_by_nodes_wpfApp.Helpers;
using notes_by_nodes_wpfApp.Services;
using notes_by_nodes_wpfApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace notes_by_nodes_wpfApp
{
    public partial class MainViewModel
    {
        #region COMMANDS

        [RelayCommand]
        public void CloseTab(NodeTabItem tab)
        {
            if (tab != null)
                Tabs.Remove(tab);
        }
        
        [RelayCommand]
        public async Task RemoveNode()
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


        //public ICommand RemoveNodeCommand => new RelayCommand<INoteViewModel>(node =>
        //{
        //    if (node != null)
        //        TryExecuteUseCase(node.RemoveAsync);

        //});
        [RelayCommand]
        public async Task NewChildNode()
        {
            if (SelectedNode != null)
                await TryExecuteUseCase(SelectedNode.NewNoteAsync);            
        }

        [RelayCommand]
        public async Task NewBox()
        {
            if (DialogManager.ShowNewBoxDialog(out var newFolder))
            {
                INodeDto newboxDto = _notesService.NewBox(new NodeDto(0, newFolder, "New Box", ""));
                var newbox = new BoxViewModel(newboxDto.Uid, newboxDto.Name, newboxDto.Description, newboxDto.Text);
                //await newbox.LoadChildNodesAsync();
                //await Task.Delay(2000);
                NodesTree.Insert(0, newbox);
            }

        }
        [RelayCommand]
        void ShowNoteInActiveTab(INoteViewModel node)
        {

            if (Tabs.Count == 0)
            {                
                var tabItem = NoteTabItemBuilder.GetNoteEditorTab(node, CloseTabCommand);
                Tabs.Insert(0, tabItem);
                tabItem.IsSelected = true;
            }
            else
            {
                var tabItem = Tabs.Single(t => t.IsSelected);
                tabItem.IsSelected = false;               

                ////TO DO прочитать про эту штуку!
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    int currentTabIndex = Tabs.IndexOf(tabItem);
                    Tabs[currentTabIndex] = NoteTabItemBuilder.GetNoteEditorTab(node, CloseTabCommand);
                    Tabs[currentTabIndex].IsSelected = true;

                }), DispatcherPriority.Loaded);

            }

        }

        [RelayCommand]
        void ShowNoteInNewTab(INoteViewModel node)
        {
            var tabItem = NoteTabItemBuilder.GetNoteEditorTab(node, CloseTabCommand);
            Tabs.Add(tabItem);
            tabItem.IsSelected = true;
        }

        [RelayCommand]
        void SelectTreeNodeItem(INoteViewModel selectedNode)
        {
            SelectedNode = selectedNode;
        }

        static async Task TryExecuteUseCase(Func<Task> action)
        {
            try
            {
                if (action !=null)
                    await action.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
    }
}
