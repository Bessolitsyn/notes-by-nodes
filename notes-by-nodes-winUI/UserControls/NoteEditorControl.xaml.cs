using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using notes_by_nodes_winUI.ViewModel;

namespace notes_by_nodes_winUI.UserControls
{
    public sealed partial class NoteEditorControl : UserControl
    {
        public INoteViewModel? Note { get; private set; }

        public NoteEditorControl()
        {
            InitializeComponent();
        }

        public void SetNote(INoteViewModel note)
        {
            Note = note;
            TitleTextBox.Text = note.Name;
            DescriptionTextBox.Text = note.Description;
            ContentTextBox.Text = note.Text;

            TitleTextBox.LostFocus += (s, e) => { if (Note != null) Note.Name = TitleTextBox.Text; };
            DescriptionTextBox.LostFocus += (s, e) => { if (Note != null) Note.Description = DescriptionTextBox.Text; };
            ContentTextBox.LostFocus += (s, e) => { if (Note != null) Note.Text = ContentTextBox.Text; };
        }
    }
}
