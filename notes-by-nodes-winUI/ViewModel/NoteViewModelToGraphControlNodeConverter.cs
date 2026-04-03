using GraphControl.Model;
using System.Collections.ObjectModel;

namespace notes_by_nodes_winUI.ViewModel
{
    public static class NoteViewModelConverter
    {
        public static IGraphControlNode ToIGraphControlNode(INoteViewModel noteViewModel)
        {
            var graphNode = new GraphControl.Model.GraphControlNode(noteViewModel.Uid)
            {
                Name = noteViewModel.Name,
                Description = noteViewModel.Description,
                Text = noteViewModel.Text,
                IsLoaded = noteViewModel.IsLoaded,
                IsExpanded = noteViewModel.IsExpanded,
                ChildNodes = new ObservableCollection<IGraphControlNode>()
            };

            foreach (var child in noteViewModel.ChildNodes)
            {
                graphNode.ChildNodes.Add(ToIGraphControlNode(child));
            }

            return graphNode;
        }
    }
}
