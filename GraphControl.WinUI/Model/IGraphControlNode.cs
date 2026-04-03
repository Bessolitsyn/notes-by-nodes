using System.Collections.ObjectModel;

namespace GraphControl.Model
{
    public interface IGraphControlNode
    {
        ObservableCollection<IGraphControlNode> ChildNodes { get; set; }
        int Uid { get; }
        string Name { get; set; }
        string Description { get; set; }
        string Text { get; set; }
        bool IsLoaded { get; set; }
        bool IsExpanded { get; set; }
        IGraphControlNode ParentNode { get; }
    }
}
