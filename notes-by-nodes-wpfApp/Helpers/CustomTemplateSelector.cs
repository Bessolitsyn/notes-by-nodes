using notes_by_nodes_wpfApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace notes_by_nodes_wpfApp.ViewHelpers
{
    public class CustomTemplateSelector : DataTemplateSelector
    {
        public HierarchicalDataTemplate BoxTemplate { get; set; }
        public HierarchicalDataTemplate NoteTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is BoxViewModel)
                return BoxTemplate;
            else if (item is NoteViewModel)
                return NoteTemplate;

            return base.SelectTemplate(item, container);
        }
    }

}
