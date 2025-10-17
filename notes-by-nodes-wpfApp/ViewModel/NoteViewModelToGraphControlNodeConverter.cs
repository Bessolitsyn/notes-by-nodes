using GraphControl.Model;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace notes_by_nodes_wpfApp.ViewModel
{
    public static class NoteViewModelConverter
    {
        public static IGraphControlNode ToIGraphControlNode(INoteViewModel noteViewModel)
        {
            var graphNode = new GraphControlNode(noteViewModel.Uid)
            {
                Name = noteViewModel.Name,
                Description = noteViewModel.Description,
                Text = noteViewModel.Text,
                IsLoaded = noteViewModel.IsLoaded,
                IsExpanded = noteViewModel.IsExpanded,
                ChildNodes = new ObservableCollection<IGraphControlNode>()
            };

            // Convert child nodes recursively
            foreach (var child in noteViewModel.ChildNodes)
            {
                graphNode.ChildNodes.Add(ToIGraphControlNode(child));
            }

            return graphNode;
        }
    }


    public class NoteViewModelToGraphControlNodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not INoteViewModel noteViewModel)
                return null;

            var graphNode = NoteViewModelConverter.ToIGraphControlNode(noteViewModel);

            return graphNode;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
