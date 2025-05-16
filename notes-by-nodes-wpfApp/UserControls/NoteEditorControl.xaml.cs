using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using notes_by_nodes_wpfApp.ViewModel;

namespace notes_by_nodes_wpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for NoteEditorControl.xaml
    /// </summary>
    public partial class NoteEditorControl : UserControl
    {
        public NoteEditorControl(INoteViewModel note)
        {
            InitializeComponent();
            DataContext = note;
        }
    }
}
