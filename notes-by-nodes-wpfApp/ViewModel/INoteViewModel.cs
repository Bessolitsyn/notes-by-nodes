using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using notes_by_nodes.Entities;
using notes_by_nodes.Service;
using notes_by_nodes.Dto;
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
    public interface INoteViewModel
    {
        ObservableCollection<INoteViewModel> ChildNodes { get; set; }
        int Uid { get; init; }
        string Name { get; set; }
        string Description { get; set; }
        string Text { get; set; }
        bool IsLoaded { get; set; }
        bool IsExpanded { get; set; }
        INoteViewModel ParentNode { get; init; }
        Task LoadChildNodesAsync();
        Task RemoveAsync();
        void RemoveChild(INoteViewModel childNote);
        Task NewNoteAsync();
    }

    public abstract partial class NodeViewModel : ObservableObject, INoteViewModel, INodeDto
    {
        public bool IsLoaded { get; set; }

        public ObservableCollection<INoteViewModel> ChildNodes { get; set; } = [];
        public INoteViewModel ParentNode { get; init; }
        protected INoteService NoteService { get; init; }
        public int Uid { get; init; }


        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string description;
        [ObservableProperty]
        private string text;
        [ObservableProperty]
        private bool isExpanded;
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
        partial void OnIsExpandedChanged(bool value)
        {
            if (value)
            {
                try
                {
                    void loadChilds(INoteViewModel note)
                    {
                        foreach (var child in note.ChildNodes)
                        {
                            if (!child.IsLoaded)
                                child.LoadChildNodesAsync();
                        };
                    }

                    if (!IsLoaded)
                    {
                        LoadChildNodesAsync().ContinueWith(task =>
                        {
                            loadChilds(this);
                        });

                    }
                    else
                        loadChilds(this);                   
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.ToString());
                }
            }
        }

        public NodeViewModel(int uid, string name, string desc, string text, INoteViewModel parent)
        {
            this.Uid = uid;
            this.name = name;
            this.description = desc;
            this.text = text;
            this.isExpanded = false;
            IsLoaded = false;
            
            ParentNode = parent;
            NotesByNodesApp app = (NotesByNodesApp)NotesByNodesApp.Current;
            NoteService = app.ServiceProvider.GetRequiredService<INoteService>();

        }
        abstract public void TrySaveChanges();
        abstract public Task RemoveAsync();

        protected readonly SemaphoreSlim loadChildNodesSemaphore = new SemaphoreSlim(1, 1);
        abstract public Task LoadChildNodesAsync();
        abstract public Task NewNoteAsync();
        protected async Task Load()
        {
            if (!IsLoaded)
            {
                await LoadChildNodesAsync();
            }
        }
        protected async Task NewChild(int boxUid, int parentNoteUid)
        {
            await Load();
            //INodeDto notedto = new NodeDto(0, "untitled", "desc", "Text");
            INodeDto notedto = await NoteService.NewNote(boxUid, parentNoteUid);
            var note = new NoteViewModel(notedto.Uid, boxUid, notedto.Name, notedto.Description, notedto.Text, this);
            ChildNodes.Insert(0, note);
        }
        public void RemoveChild(INoteViewModel childNote)
        {
            ChildNodes.Remove(childNote);
        }
    }

    public partial class BoxViewModel : NodeViewModel
    {       
        public BoxViewModel(int uid, string path, string desc, string text)
            : base(uid, path, desc, text, null)
        {
            
        }
        
        public override async Task LoadChildNodesAsync()
        {

            if (!await loadChildNodesSemaphore.WaitAsync(TimeSpan.Zero))
            {
                return;
            }

            try
            {                
                var notes = await NoteService.GetChildNodesOfTheBox(Uid);
                //notes.Select(note=> new NoteViewModel(note.Uid, Uid, note.Name, note.Description, note.Text, this));
                foreach (var note in notes)
                {
                    ChildNodes.Add(new NoteViewModel(note.Uid, Uid, note.Name, note.Description, note.Text, this));
                }
            }
            finally
            {
                IsLoaded = true;
                loadChildNodesSemaphore.Release();
            }
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

        public override async Task RemoveAsync()
        {
            throw new NotImplementedException();
        }

        public override async Task NewNoteAsync()
        {
            await NewChild(Uid, Uid);
        }
    }
    public partial class NoteViewModel : NodeViewModel
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
        public override async Task LoadChildNodesAsync()
        {
            if (!await loadChildNodesSemaphore.WaitAsync(TimeSpan.Zero)) 
                    return;

            try
            {
                var notes = await NoteService.GetChildNodes(BoxUid, Uid);
                foreach (var note in notes)
                {
                    ChildNodes.Add(new NoteViewModel(note.Uid, BoxUid, note.Name, note.Description, note.Text, this));
                }
            }
            finally
            {
                IsLoaded = true;
                loadChildNodesSemaphore.Release();
            }
        }
    

        public override async Task RemoveAsync()
        {
            await NoteService.Remove(BoxUid, Uid);
        }

        public override async Task NewNoteAsync()
        {
            await NewChild(BoxUid, Uid);
        }
    }

    
    
    


}
