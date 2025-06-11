using CommunityToolkit.Mvvm.Input;
using notes_by_nodes.Entities;
using notes_by_nodes_wpfApp.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace notes_by_nodes_wpfApp.ViewModel
{
    [Obsolete]
    public class NodeTabItem : TabItem
    {
        public int NodeUid { get; set; }
        [Obsolete]
        public static NodeTabItem CastToTabItem(TabItem tabItem, int nodeUid)
        {
            return new NodeTabItem()
            {
                NodeUid = nodeUid,
                Header = tabItem.Header,
                Content = tabItem.Content
            };          
        }
    }
    
}
