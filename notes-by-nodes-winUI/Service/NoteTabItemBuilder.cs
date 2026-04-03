using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using notes_by_nodes_winUI.UserControls;
using notes_by_nodes_winUI.ViewModel;
using System.Windows.Input;
using GraphControl.Model;

namespace notes_by_nodes_winUI.Service
{
    public static class NoteTabItemBuilder
    {
        static ICommand? _closeTabCommand;
        public static TabViewItem GetNoteEditorTab(INoteViewModel note, ICommand closeTabCommand)
        {
            var userControl = new NoteEditorControl();
            userControl.SetNote(note);
            return NewTab(note, userControl, closeTabCommand);
        }

        public static TabViewItem GetGraphViewerTab(INoteViewModel note, ICommand closeTabCommand)
        {
            var graphControl = new GraphControl.GraphControl();
            var graphNode = NoteViewModelConverter.ToIGraphControlNode(note);
            graphControl.LoadFrom(graphNode);
            return NewTab(note, graphControl, closeTabCommand);
        }

        static TabViewItem NewTab(INoteViewModel note, UIElement content, ICommand closeTabCommand)
        {
            _closeTabCommand = closeTabCommand;
            var tabItem = new TabViewItem
            {
                Content = content,
                IsClosable = true,
                Header = note.Name
                
            };
            tabItem.CloseRequested += TabItem_CloseRequested;

            //// Кнопка закрытия в заголовке
            //var closeButton = new Button
            //{
            //    Command = closeTabCommand,
            //    CommandParameter = tabItem,
            //    Content = "×",
            //    Width = 20,
            //    Height = 20,
            //    Margin = new Thickness(5, 0, 0, 0),
            //    Background = null,
            //    BorderBrush = null
            //};

            //// Создаем заголовок с кнопкой закрытия
            //var headerStack = new StackPanel { Orientation = Orientation.Horizontal };
            //var headerText = new TextBlock { Text = note.Name, VerticalAlignment = VerticalAlignment.Center };
            //headerStack.Children.Add(headerText);
            //headerStack.Children.Add(closeButton);

            //tabItem.Header = headerStack;

            return tabItem;
        }

        private static void TabItem_CloseRequested(TabViewItem sender, TabViewTabCloseRequestedEventArgs args)
        {
            _closeTabCommand?.Execute(sender);
        }
    }
}
