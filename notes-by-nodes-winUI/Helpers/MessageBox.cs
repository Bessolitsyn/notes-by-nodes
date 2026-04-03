using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace notes_by_nodes_winUI
{
    public static class MessageBox
    {
        public static async void Show(string message)
        {
            try
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = message,
                    CloseButtonText = "OK",
                    XamlRoot = App.MainWindow?.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
            catch (Exception)
            {
                // Ignore if dialog fails
            }
        }
    }
}
