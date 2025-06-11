using notes_by_nodes.Entities;
using notes_by_nodes_wpfApp.UserControls;
using notes_by_nodes_wpfApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace notes_by_nodes_wpfApp.Service
{
    public class NoteTabItemBuilder
    {
        public static TabItem GetNoteEditorTab(INoteViewModel note, ICommand closeTabCommand)
        {
            var userControl = new NoteEditorControl(note);
            return NewNoteEditorTab(note, userControl, closeTabCommand);
        }
        static TabItem NewNoteEditorTab(INoteViewModel note, UserControl userControl, ICommand closeTabCommand)
        {
            var tabItem = new TabItem();
            tabItem.Content = userControl;

            var headerStack = new StackPanel { Orientation = Orientation.Horizontal };
            headerStack.Children.Add(new TextBlock { Text = note.Name });

            // Кнопка закрытия
            var closeButton = new Button
            {
                Command = closeTabCommand,
                CommandParameter = tabItem,
                Content = "×",
                Width = 20,
                Height = 20,
                Margin = new Thickness(5, 0, 0, 0),
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent
                //Style = (Style)FindResource("ClosableTabItemStyle")
            };

            //closeButton.Click += (s, e) => parent.Items.RemoveAsync(this);
            headerStack.Children.Add(closeButton);
            tabItem.Header = headerStack;
            return tabItem;

        }
    }
}

