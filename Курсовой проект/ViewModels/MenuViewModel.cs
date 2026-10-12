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
        public ICommand SettingsCommand { get; }
        public ICommand ExitCommand { get; }

        public MenuViewModel()
        {
            SettingsCommand = new RelayCommand(ExecuteSettings);
            ExitCommand = new RelayCommand(ExecuteExit);
            AudioService.PlayBackgroundMusic("bg_music.mp3");
        }

        private void ExecuteSettings(object parameter)
        {
            SettingsWindow settingsWin = new SettingsWindow();
            if (Application.Current.MainWindow != null)
            {
                settingsWin.Owner = Application.Current.MainWindow;
            }
            settingsWin.ShowDialog();
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
                AudioService.StopBackgroundMusic();
                Application.Current.Shutdown();
            }
        }
    }
}
