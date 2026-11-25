using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Курсовой_проект.Services
{
    public class DialogService : IDialogService
    {
        public void ShowMessage(string title, string message)
        {
            var dialog = new CustomMessageBox(title, message);
            SetOwner(dialog);
            dialog.ShowDialog();
        }

        public bool ShowConfirm(string message)
        {
            var dialog = new ConfirmDialogWindow(message);
            SetOwner(dialog);
            return dialog.ShowDialog() == true;
        }

        public bool ShowExitDialog()
        {
            var dialog = new ExitWindow();
            SetOwner(dialog);
            return dialog.ShowDialog() == true;
        }

        private static void SetOwner(Window dialog)
        {
            var mainWindow = Application.Current?.MainWindow;
            if (mainWindow != null && mainWindow != dialog && mainWindow.IsLoaded)
            {
                dialog.Owner = mainWindow;
            }
        }
    }
}
