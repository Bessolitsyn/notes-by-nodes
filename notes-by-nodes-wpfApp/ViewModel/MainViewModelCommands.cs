using CommunityToolkit.Mvvm.Input;
using notes_by_nodes.Entities;
using notes_by_nodes_wpfApp.Services;
using notes_by_nodes_wpfApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        //public ICommand CloseTabCommand2 => new RelayCommand<NodeTabItem>(tab =>
        //{
        //    if (tab != null)
        //        Tabs.Remove(tab);
        //});

        //[RelayCommand]
        //public ICommand TreeNodeDoubleClickCommand => new RelayCommand(() =>
        //{

        //});
        [RelayCommand]
        public void RemoveNode()
        {
            if (SelectedNode != null)
                if (TryExecuteUseCase(SelectedNode.Remove))
                {
                    SelectedNode.ParentNode.RemoveChild(SelectedNode);
                };
                
        }
        //public ICommand RemoveNodeCommand => new RelayCommand<INoteViewModel>(node =>
        //{
        //    if (node != null)
        //        TryExecuteUseCase(node.Remove);

        //});
        [RelayCommand]
        public void NewChildNode()
        {
            if (SelectedNode != null)
                TryExecuteUseCase(SelectedNode.NewNote);
            
        }
        //public ICommand NewChildNodeCommand => new RelayCommand<INoteViewModel>(node =>
        //{
        //    if (node != null)
        //        TryExecuteUseCase(node.NewChild);
        //});

        [RelayCommand]
        void ShowNoteInNewTab()
        {
            var tabItem = NoteTabItemBuilder.GetNoteEditorTabItem(SelectedNode, CloseTabCommand);
            //var nodeTabItem = NodeTabItem.CastToTabItem(tabItem, node.Uid);
            Tabs.Add(tabItem);
            tabItem.IsSelected = true;

            //if (!Tabs.Any(t => t.NodeUid == node.Uid))
            //{

            //}
            //else {
            //    Tabs.Single(t => t.NodeUid == node.Uid).IsSelected = true;
            //}

        }

        [RelayCommand]
        void SelectTreeNodeItem(INoteViewModel selectedNode)
        {
            SelectedNode = selectedNode;
        }

        static bool TryExecuteUseCase(Action action)
        {
            try
            {
                action?.Invoke();
                return true;
            }
            catch (Exception ex)
            {

#warning TO DO uniwersal error message box;
                System.Windows.MessageBox.Show(ex.ToString());
                return false;
            }
        }
        #endregion
    }
}
