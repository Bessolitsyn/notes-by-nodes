using notes_by_nodes.Entities;
using notes_by_nodes.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes_wpfApp.ViewModel
{
    [Obsolete]
    internal class ModelsPresenter : INotePresenter
    {
        private List<INotePresenterObserver> observers = [];

        public void Attach(INotePresenterObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(INotePresenterObserver observer)
        {
            observers.Remove(observer);
        }

        public void SetBoxes(IEnumerable<(int, string, string)> boxes)
        {
            //NotifyObservers<BoxViewModel>(boxes.Select(box => new BoxViewModel(box.Item1, box.Item2, box.Item3)));
        }

        public void SetChildNotes(int parentNodeUid, (int, string, string)[] childNodes)
        {
            //NotifyObservers<INodeViewModel>(childNodes.Select(box => new NoteViewModel(box.Item1, box.Item2, box.Item3)));
        }

        //public void SetMainViewModel(MainViewModel mainViewModel)
        //{
        //    this.mainViewModel = mainViewModel;
        //}

        public void SetUsers(IEnumerable<(int, string, string)> users)
        {
            NotifyObservers<UserViewModel>(users.Select(user => new UserViewModel(user.Item1, user.Item2, user.Item3)));            
        }

        private void NotifyObservers<T>(IEnumerable<T> items)
        {
            foreach (var observer in observers)
            {
                //observer.UpdatedViewModel<T>(items);
            }
        }
        private void NotifyObservers<T>(T items)
        {
            foreach (var observer in observers)
            {
                //observer.UpdateViewModel<T>(items);
            }
        }
    }

}
