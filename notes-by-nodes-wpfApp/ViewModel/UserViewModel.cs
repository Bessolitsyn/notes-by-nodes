using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notes_by_nodes_wpfApp.ViewModel
{
    public partial class UserViewModel(int uid, string name, string email) : ObservableObject
    {

        public int Uid { get; init; } = uid;
        public string Name { get; init; } = name;

        [ObservableProperty]
        private string email = email;
    }
}
