using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;
using System;

namespace notes_by_nodes_winUI.Helpers
{
    public static class DialogManager
    {
        public static async Task<string?> ShowInputDialogAsync(string title, string message)
        {
            var textBox = new TextBox { PlaceholderText = "Введите значение" };
            
            var dialog = new ContentDialog
            {
                Title = title,
                Content = textBox,
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancel",
                XamlRoot = App.MainWindow?.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary && !string.IsNullOrWhiteSpace(textBox.Text))
            {
                return textBox.Text;
            }
            return null;
        }

        public static async Task<string?> ShowNewUserDialogAsync()
        {
            return await ShowInputDialogAsync("New User", "Введите имя пользователя");
        }

        public static async Task<string?> ShowNewBoxDialogAsync()
        {
            return await ShowInputDialogAsync("New Box", "Введите имя папки для хранения заметок");
        }
    }
}
