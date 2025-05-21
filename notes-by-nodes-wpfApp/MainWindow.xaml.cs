using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using notes_by_nodes_wpfApp.ViewModel;

namespace notes_by_nodes_wpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.Init();
            //NodeTreeView.MouseDoubleClick += viewModel.NodeTreeView_MouseDoubleClick;
            //NodeTreeView.SelectedItemChanged += NodeTreeView_SelectedItemChanged;

        }
        private void NodeTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.SelectedNode = (INoteViewModel)e.NewValue;
            }
        }

    }
}