using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using notes_by_nodes_wpfApp.Service;
using notes_by_nodes_wpfApp.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;

namespace notes_by_nodes_wpfApp.ViewModel
{
    public partial class DialogViewModel : ObservableObject, IDisposable
    {
        public DialogViewModel(string message)
        {            
            this.message = message ?? string.Empty;

        }
        private DialogWindow _dialogWindow;

        [ObservableProperty]
        private string message;
        [ObservableProperty]
        private string answer;

        [RelayCommand]
        void Cancel()
        {
            _dialogWindow.DialogResult = false;
            _dialogWindow.Close();
        }

        [RelayCommand]
        void Ok()
        {
            if (answer != string.Empty)
            { 
                _dialogWindow.DialogResult = true;
                _dialogWindow.Close();
            }

        }

        public void SetDialogWindow(DialogWindow dialogWindow)
        {
            _dialogWindow = dialogWindow;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
