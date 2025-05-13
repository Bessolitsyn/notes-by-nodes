using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using notes_by_nodes.Entities;
using notes_by_nodes.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes_wpfApp.ViewModel
{
    public partial class BoxViewModel(int uid, string path, string desc) : NodeViewModel, INodeViewModel
    {
        public ObservableCollection<INodeViewModel> ChildNodes { get; set; } = [];

        [ObservableProperty]
        private int uid = uid;
        [ObservableProperty]
        private string name = path;
        [ObservableProperty]
        private string desc = desc;
        partial void OnDescChanged(string? oldValue, string newValue)
        {
            if (IsLoaded && newValue != null && newValue != String.Empty)
                NoteService.ModifyBox(Uid, Name, newValue);
        }
        public void LoadChildNodes()
        {
            var notes = NoteService.GetChildNodesOfTheBox(Uid).Select(note=> new NoteViewModel(note.Item1, Uid, note.Item2, note.Item3));
            foreach (var note in notes)
            {
                ChildNodes.Add(note);
            }
            IsLoaded= true;
        }

    }
    public partial class NoteViewModel(int uid, int boxUid, string name, string desc) : NodeViewModel, INodeViewModel
    {
        public ObservableCollection<INodeViewModel> ChildNodes { get; set; } = [];
        

        [ObservableProperty]
        private int uid = uid;
        [ObservableProperty]
        private int boxUid = boxUid;
        [ObservableProperty]
        private string name = name;
        [ObservableProperty]
        private string desc = desc;
        partial void OnDescChanged(string? oldValue, string newValue)
        {            
            if (IsLoaded && newValue != null&& newValue != String.Empty)
                NoteService.ModifyNote(boxUid, Uid, Name, newValue);
        }
        public void LoadChildNodes()
        {
            var notes = NoteService.GetChildNodes(BoxUid, Uid).Select(note => new NoteViewModel(note.Item1, BoxUid, note.Item2, note.Item3));
            foreach (var note in notes)
            {
                ChildNodes.Add(note);
            }
            IsLoaded = true;
        }


    }

    public class NodeViewModel : ObservableObject
    {
        public bool IsLoaded { get; set; }
        protected INoteService NoteService { get; init; }
        public NodeViewModel()
        {
            NotesByNodesApp app = (NotesByNodesApp)NotesByNodesApp.Current;
            NoteService = app.ServiceProvider.GetRequiredService<INoteService>();
            IsLoaded = false;
            
        }
    }

    public interface INodeViewModel
    {

        int Uid { get;set; }
        string Name { get;set; }
        string Desc { get;set; }
        bool IsLoaded { get; set; }
        ObservableCollection<INodeViewModel> ChildNodes { get; set; }
        void LoadChildNodes();

    }

}
