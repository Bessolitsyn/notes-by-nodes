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
    public partial class BoxViewModel(int uid, string path, string desc, string text, IParentNode parentNode) : NodeViewModel(), INoteViewModel
    {
        //public ObservableCollection<INoteViewModel> ChildNodes { get; set; } = [];

        [ObservableProperty]
        private int uid = uid;
        [ObservableProperty]
        private string name = path;
        [ObservableProperty]
        private string desc = desc;
        [ObservableProperty]
        private string text = text;
        protected override IParentNode ParentNode { get; init; } = parentNode;
        partial void OnDescChanged(string? oldValue, string newValue)
        {
            if (IsLoaded && newValue != null && newValue != String.Empty)
                NoteService.ModifyBox((INodeDto)this);
        }
        public void LoadChildNodes()
        {
            var notes = NoteService.GetChildNodesOfTheBox(Uid).Select(note=> new NoteViewModel(note.Uid, Uid, note.Name, note.Description, note.Text, this));
            foreach (var note in notes)
            {
                ChildNodes.Add(note);
            }
            IsLoaded= true;
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }

        public override void NewChild()
        {
            var note = new NoteViewModel(0, Uid, "note.Item2", "note.Item3", "text", this);
            ChildNodes.Add(note);
        }

        public void RemoveChild(INoteViewModel childNote)
        {
            ChildNodes.Remove(childNote);
        }
    }
    public partial class NoteViewModel(int uid, int boxUid, string name, string desc, string text, INoteViewModel parent ) : NodeViewModel(), INoteViewModel
    {
        
        [ObservableProperty]
        private int uid = uid;
        [ObservableProperty]
        private int boxUid = boxUid;
        [ObservableProperty]
        private string name = name;
        [ObservableProperty]
        private string desc = desc;
        [ObservableProperty]
        private string text = text;

        protected override IParentNode ParentNode { get; init; } = parent;

        partial void OnDescChanged(string? oldValue, string newValue)
        {            
            if (IsLoaded && newValue != null&& newValue != String.Empty)
                NoteService.ModifyNote(boxUid, (INodeDto)this);
        }
        public void LoadChildNodes()
        {
            var notes = NoteService.GetChildNodes(BoxUid, Uid).Select(note => new NoteViewModel(note.Uid, BoxUid, note.Name, note.Description, note.Text, this));
            foreach (var note in notes)
            {
                ChildNodes.Add(note);
            }
            IsLoaded = true;
        }

        public override void Remove()
        {
            ParentNode?.RemoveChild(this);
        }

        public override void NewChild()
        {
            var note = new NoteViewModel(0, BoxUid, "note.Item2", "note.Item3", "Text", this);
            ChildNodes.Add(note);
        }

        public void RemoveChild(INoteViewModel childNote)
        {
            ChildNodes.Remove(childNote);
        }
    }

    public abstract partial class NodeViewModel : ObservableObject
    {
        public bool IsLoaded { get; set; }
        public ObservableCollection<INoteViewModel> ChildNodes { get; set; } = [];
        protected abstract IParentNode ParentNode { get; init; }
        protected INoteService NoteService { get; init; }
        public NodeViewModel()
        {   
            NotesByNodesApp app = (NotesByNodesApp)NotesByNodesApp.Current;
            NoteService = app.ServiceProvider.GetRequiredService<INoteService>();
            IsLoaded = false;
        }
        
        abstract public void Remove();
        abstract public void NewChild();
    }

    public interface INoteViewModel:IParentNode 
    {
        ObservableCollection<INoteViewModel> ChildNodes { get; set; }
        int Uid { get;set; }
        string Name { get;set; }
        string Desc { get;set; }
        bool IsLoaded { get; set; }        
        void LoadChildNodes();
        void Remove();
        void NewChild();
    }
    public interface IParentNode
    {
        void RemoveChild(INoteViewModel childNote);
    }


}
