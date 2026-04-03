using System.Collections.ObjectModel;

namespace GraphControl.Model
{
    public class GraphControlNode : IGraphControlNode
    {
        public ObservableCollection<IGraphControlNode> ChildNodes { get; set; } = new ObservableCollection<IGraphControlNode>();

        public int Uid { get; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool IsLoaded { get; set; }
        public bool IsExpanded { get; set; }

        public IGraphControlNode ParentNode { get; }

        public GraphControlNode(int uid, IGraphControlNode? parent = null)
        {
            Uid = uid;
            ParentNode = parent ?? this;
        }

        public GraphControlNode CreateAndAddChild(int uid, string name = "")
        {
            var child = new GraphControlNode(uid, this) { Name = name };
            ChildNodes.Add(child);
            return child;
        }

        public override string ToString() => $"{Name} (Uid={Uid})";
    }
}
