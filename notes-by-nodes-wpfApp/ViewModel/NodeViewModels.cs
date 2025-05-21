using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using notes_by_nodes.Entities;
using notes_by_nodes.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace notes_by_nodes_wpfApp.ViewModel
{
    public partial class BoxViewModel : NodeViewModel, INoteViewModel
    {
        //public ObservableCollection<INoteViewModel> ChildNodes { get; set; } = [];

        //[ObservableProperty]
        //private int uid = uid;
        //[ObservableProperty]
        //private string name = path;
        //[ObservableProperty]
        //private string description = desc;
        //[ObservableProperty]
        //private string text = text;
        //protected override ITreeNode ParentNode { get; init; } = parentNode;
        //partial void OnDescriptionChanged(string? oldValue, string newValue)
        //{
        //    if (IsLoaded && newValue != null && newValue != String.Empty)
        //        NoteService.ModifyBox((INodeDto)this);
        //}
        public BoxViewModel(int uid, string path, string desc, string text, ITreeNode parentNode)
            : base(uid, path, desc, text, parentNode)
        { 
        }
        public override void LoadChildNodes()
        {
            var notes = NoteService.GetChildNodesOfTheBox(Uid).Select(note=> new NoteViewModel(note.Uid, Uid, note.Name, note.Description, note.Text, this));
            foreach (var note in notes)
            {
                ChildNodes.Add(note);
            }
            IsLoaded= true;
        }

        public override void TrySaveChanges()
        {
            try
            {
                if (IsLoaded)
                    NoteService.ModifyBox((INodeDto)this);
            }
            catch (Exception)
            {
#warning TO DO error message box
            }
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }

        public void RemoveChild(INoteViewModel childNote)
        {
            ChildNodes.Remove(childNote);
        }

        public void NewNote()
        {
            NewChild(Uid, this);
        }
    }
    public partial class NoteViewModel : NodeViewModel, INoteViewModel
    {
        
        [ObservableProperty]
        private int boxUid;

        public NoteViewModel(int uid, int boxUid, string name, string desc, string text, INoteViewModel parent)
            :base(uid, name, desc, text, parent)
        {
            this.boxUid = boxUid;
        }

        public override void TrySaveChanges()
        {
            try
            {
                if (IsLoaded)
                    NoteService.ModifyNote(BoxUid, (INodeDto)this);
            }
            catch (Exception ex )
            {
#warning TO DO error message box
                
            }  
        }
        public override void LoadChildNodes()
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
            NoteService.Remove(BoxUid, this);
        }


        public void NewNote()
        {
            NewChild(BoxUid, this);
        }
    }

    public abstract partial class NodeViewModel : ObservableObject, INodeDto
    {
        public bool IsLoaded { get; set; }
        public ObservableCollection<INoteViewModel> ChildNodes { get; set; } = [];
        public ITreeNode ParentNode { get; init; }
        protected INoteService NoteService { get; init; }

        [ObservableProperty]
        private int uid;
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string description;
        [ObservableProperty]
        private string text;
        //[ObservableProperty]
        //private FlowDocument textDoc;
        

        partial void OnDescriptionChanged(string value)
        {
            TrySaveChanges();
        }
        partial void OnTextChanged(string value)
        {
            TrySaveChanges();
        }
        partial void OnNameChanged(string value)
        {
            TrySaveChanges();
        }

        public NodeViewModel(int uid, string name, string desc, string text, ITreeNode parent)
        {   
            this.uid = uid;
            this.name = name;
            this.description = desc;
            this.text = text;
            ParentNode = parent;
            NotesByNodesApp app = (NotesByNodesApp)NotesByNodesApp.Current;
            NoteService = app.ServiceProvider.GetRequiredService<INoteService>();
            IsLoaded = false;
            
        }
        abstract public void TrySaveChanges();
        abstract public void Remove();
        //abstract public void NewChild();
        abstract public void LoadChildNodes();
        protected void Load()
        {
            if (!IsLoaded)
            {
                LoadChildNodes();
            }
        }
        protected void NewChild(int boxUid, INoteViewModel parent)
        {
            Load();
            var note = new NoteViewModel(0, boxUid, "note.Item2", "note.Item3", "Text", parent);
            var newNote = NoteService.NewNote(boxUid, (INodeDto)parent, (INodeDto)note);
            note.Uid = newNote.Uid;
            note.IsLoaded = true;
            ChildNodes.Insert(0, note);
        }
        public void RemoveChild(INoteViewModel childNote)
        {
            ChildNodes.Remove(childNote);
        }
    }
    public interface INoteViewModel: ITreeNode
    {
        ObservableCollection<INoteViewModel> ChildNodes { get; set; }
        int Uid { get;set; }
        string Name { get;set; }
        string Description { get;set; }
        string Text { get; set; }
        bool IsLoaded { get; set; }        
        void LoadChildNodes();
        void Remove();
        void NewNote();
    }
    public interface ITreeNode
    {
        ITreeNode ParentNode { get; init; }
        void RemoveChild(INoteViewModel childNote);
    }


}
