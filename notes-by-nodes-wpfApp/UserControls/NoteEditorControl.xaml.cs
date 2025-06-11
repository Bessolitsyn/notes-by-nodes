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
            if (note != null ) 
                LoadPlainTextToRichTextBox(note.Text);

            // Подписка на изменения документа
            NoteContentRTBox.Document.DataContextChanged += (s, e) =>
            {
                //var range = new TextRange(richTextBox.Document.ContentStart,
                //                         richTextBox.Document.ContentEnd);
                //using (var stream = new MemoryStream())
                //{
                //    range.Save(stream, DataFormats.Rtf);
                //    (DataContext as EditorVM).RtfContent = Encoding.UTF8.GetString(stream.ToArray());
                //}
            };
        }
        void LoadPlainTextToRichTextBox(string text)
        {

            var doc = NoteContentRTBox.Document;
            TextRange range = new TextRange(doc.ContentStart, doc.ContentEnd);
            range.Text = text ?? string.Empty;
            
            //Загрузка RTF-файла
            //using (FileStream fs = new FileStream("document.rtf", FileMode.Open))
            //{
            //    TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            //    range.Load(fs, DataFormats.Rtf);
            //}
        }
        string getPlainTextFromFlowDoc(FlowDocument textDoc)
        {
            TextRange range = new TextRange(textDoc.ContentStart, textDoc.ContentEnd);
            return range.Text;


            //// Получить обычный текст
            //string plainText = new TextRange(
            //    richTextBox.Document.ContentStart,
            //    richTextBox.Document.ContentEnd
            //).Text;

            //// Сохранить в RTF (Rich Text Format)
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            //    range.Save(stream, DataFormats.Rtf);
            //    string rtfText = Encoding.UTF8.GetString(stream.ToArray());
            //}
        }

        private void RichTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            (DataContext as INoteViewModel).Text = getPlainTextFromFlowDoc(NoteContentRTBox.Document);
        }


    }
}
