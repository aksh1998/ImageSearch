using ImageSearch.Interface;
using System.Windows;

namespace ImageSearch.Service
{
    public class DialogService : IDialogService
    {
        public void ShowMessageBox(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK);
        }
    }
}
