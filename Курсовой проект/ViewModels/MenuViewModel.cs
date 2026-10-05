using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Курсовой_проект.Commands;

namespace Курсовой_проект.ViewModels
{
    public class MenuViewModel
    {
        public ICommand ExitCommand { get; }

        public MenuViewModel()
        {
            ExitCommand = new RelayCommand(ExecuteExit);
        }
        private void ExecuteExit(object parameter)
        {
            ExitWindow dialog = new ExitWindow();

            if (Application.Current.MainWindow != null)
            {
                dialog.Owner = Application.Current.MainWindow;
            }

            if (dialog.ShowDialog() == true)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
