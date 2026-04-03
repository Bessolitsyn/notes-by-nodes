using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using notes_by_nodes_winUI.ViewModel;
using System;
using Windows.Foundation;
using Windows.Devices.Input;
using System.Threading.Tasks;

namespace notes_by_nodes_winUI
{
    public sealed partial class MainWindow : Window
    {
        private bool isLoadedDataContext = false;
        public MainViewModel ViewModel { get; }

        public MainWindow(MainViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();



            InitializeContextMenu();

            this.Activated += async (s, e) =>
            {
                if (e.WindowActivationState != WindowActivationState.Deactivated && !isLoadedDataContext)
                {
                    await LoadDataContextAsync();
                }
            };
        }
        //TODO перенести во вьюмодель и там сделать обработчик всех ошибок
        private async Task LoadDataContextAsync()
        {
            try
            {
                await ViewModel.InitAsync();
                //NodeTreeView.ItemsSource = ViewModel.NodesTree;
                isLoadedDataContext = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void HandleException(Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }

        private void InitializeContextMenu()
        {
            var menuFlyout = new MenuFlyout();

            var newNoteItem = new MenuFlyoutItem { Text = "New note" };
            newNoteItem.Command = ViewModel.NewChildNodeCommand;
            menuFlyout.Items.Add(newNoteItem);

            var removeItem = new MenuFlyoutItem { Text = "Remove" };
            removeItem.Command = ViewModel.RemoveNodeCommand;
            menuFlyout.Items.Add(removeItem);

            var openInNewTabItem = new MenuFlyoutItem { Text = "Open in new tab" };
            openInNewTabItem.CommandParameter = ViewModel.RightTappedNode;
            openInNewTabItem.Command = ViewModel.ShowNoteInNewTabCommand;
            menuFlyout.Items.Add(openInNewTabItem);

            var openGraphViewerItem = new MenuFlyoutItem { Text = "Open graph viewer" };
            openGraphViewerItem.Command = ViewModel.OpenGraphViewerCommand;
            menuFlyout.Items.Add(openGraphViewerItem);

            NodeTreeView.RightTapped += (s, e) =>
            {
                try
                {
                    // Получаем элемент, на котором был клик
                    var element = e.OriginalSource as FrameworkElement;
                    if (element != null && element.DataContext is INoteViewModel node)
                    {
                        ViewModel.RightTappedNode = node;
                        //NodeTreeView.SelectedItem = node;
                        //ViewModel.SelectTreeNodeItemCommand.Execute(node);
                    }
                    menuFlyout.ShowAt(NodeTreeView, e.GetPosition(s as UIElement));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Menu error: {ex.Message}");
                }
            };
        }
    }
}
