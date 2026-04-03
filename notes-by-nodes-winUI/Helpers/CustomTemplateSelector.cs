using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using notes_by_nodes_winUI.ViewModel;
using System.ComponentModel;

namespace notes_by_nodes_winUI.Helpers
{
    public class CustomTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BoxTemplate { get; set; }
        public DataTemplate NoteTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return item switch
            {
                BoxViewModel _ => BoxTemplate,
                NoteViewModel _ => NoteTemplate,
                _ => base.SelectTemplateCore(item)
            };
        }
    }
}
