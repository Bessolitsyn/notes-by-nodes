using notes_by_nodes.Entities;
using notes_by_nodes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes_wpfApp.ViewModel
{
    internal class MainViewModelPresenter : INotePresenter
    {
        private MainViewModel _mainViewModel;
        public MainViewModelPresenter() { }

        public void SetBoxes(IEnumerable<(int, string, string)> boxes)
        {
            foreach (var box in boxes)
            {
               _mainViewModel.NodesTree.Add(new BoxViewModel(box.Item1, box.Item2, box.Item3));
            }
        }

        public void SetChildNotes(int parentNodeUid, int[] childNodes)
        {
            throw new NotImplementedException();
        }

        public void SetMainViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public void SetUsers(IEnumerable<(int, string, string)> users)
        {
            foreach (var user in users)
            {
                _mainViewModel.Users.Add(new UserViewModel(user.Item1, user.Item2, user.Item3));
            }
        }
    }
}
