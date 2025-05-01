using CommunityToolkit.Mvvm.ComponentModel;
using notes_by_nodes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes_wpfApp.ViewModel
{
    public partial class BoxViewModel(int uid, string path, string desc) : ObservableObject, NodeViewModel
    {

        [ObservableProperty]
        private int uid = uid;
        [ObservableProperty]
        private string name = path;
        [ObservableProperty]
        private string desc = desc;
       
    }
    public partial class NoteViewModel(int uid, string name, string desc) : ObservableObject, NodeViewModel
    {

        [ObservableProperty]
        private int uid = uid;
        [ObservableProperty]
        private string name = name;
        [ObservableProperty]
        private string desc = desc;
    }

    public interface NodeViewModel
    {

        int Uid { get;set; }
        string Name { get;set; }
        string Desc { get;set; }
    }

}
