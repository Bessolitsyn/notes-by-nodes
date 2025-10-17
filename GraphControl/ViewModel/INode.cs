using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphControl.Model
{
    public interface IGraphControlNode
    {
        ObservableCollection<IGraphControlNode> ChildNodes { get; set; }
        int Uid { get; init; }
        string Name { get; set; }
        string Description { get; set; }
        string Text { get; set; }
        bool IsLoaded { get; set; }
        bool IsExpanded { get; set; }
        IGraphControlNode ParentNode { get; init; }
    }
}
