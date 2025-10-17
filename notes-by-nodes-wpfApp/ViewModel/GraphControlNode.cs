using GraphControl.Model;
using System.Collections.ObjectModel;

namespace notes_by_nodes_wpfApp.ViewModel
{
    internal class GraphControlNode : IGraphControlNode
    {
        // Child nodes collection
        public ObservableCollection<IGraphControlNode> ChildNodes { get; set; } = new ObservableCollection<IGraphControlNode>();

        // Unique identifier (init-only per interface)
        public int Uid { get; init; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool IsLoaded { get; set; }
        public bool IsExpanded { get; set; }

        // Parent — for root nodes ParentNode points to self to keep non-null invariant
        public IGraphControlNode ParentNode { get; init; }

        public GraphControlNode(int uid, IGraphControlNode? parent = null)
        {
            Uid = uid;
            ParentNode = parent ?? this;

            // Optional: keep ChildNodes consistent if external code adds items later
        }

        // Convenience factory + add: creates a child with Parent set to this and adds it
        public GraphControlNode CreateAndAddChild(int uid, string name = "")
        {
            var child = new GraphControlNode(uid, this) { Name = name };
            ChildNodes.Add(child);
            return child;
        }

        public override string ToString() => $"{Name} (Uid={Uid})";
    }
}