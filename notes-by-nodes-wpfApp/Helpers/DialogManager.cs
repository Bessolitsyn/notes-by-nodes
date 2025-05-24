using Microsoft.Win32;
using notes_by_nodes_wpfApp.UserControls;
using notes_by_nodes_wpfApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace notes_by_nodes_wpfApp.Helpers
{
    public static class DialogManager
    {

        private static bool ShowDialog(string title, string message, out string answer)
        {
            answer = "";
            bool result = false;
            using (DialogViewModel context = new DialogViewModel(message))
            {
                var modalDialog = new DialogWindow(context);
                context.SetDialogWindow(modalDialog);
                modalDialog.Title = title;
                if (modalDialog.ShowDialog() == true)
                {
                    answer = context.Answer;
                    result = true;
                }
            }
            
            return result;
        }
        public static bool ShowNewUserDialog(out string user)
        {
            return ShowDialog("New User", "Введите имя пользователя", out user);
        }
        public static bool ShowNewBoxDialog(out string folder)
        {
            folder = "";
            bool result = false;
            OpenFolderDialog openFolderDialog = new OpenFolderDialog()
            {
                Title = "Выберите папку в которой будут хранится ваши заметки",
                Multiselect = false,
            };

            if (openFolderDialog.ShowDialog()==true)
            {
                folder = openFolderDialog.FolderName;
                result = true;
            }
            return result;
        }
    }
}
