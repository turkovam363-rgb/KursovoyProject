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
    public class MenuViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;

        public ICommand SettingsCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand LoadGameCommand { get; } 

        public MenuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            SettingsCommand = new RelayCommand(ExecuteSettings);
            ExitCommand = new RelayCommand(ExecuteExit);
            StartGameCommand = new RelayCommand(ExecuteStartGame);
            LoadGameCommand = new RelayCommand(ExecuteLoadGame); 

            AudioService.PlayBackgroundMusic("bg_music.mp3");
        }

        private void ExecuteStartGame(object parameter)
        {
            _mainViewModel.CurrentView = new NovelViewModel(_mainViewModel);
        }

        private void ExecuteLoadGame(object parameter)
        {
            _mainViewModel.CurrentView = new LoadMenuViewModel(_mainViewModel, this);
        }

        private void ExecuteSettings(object parameter)
        {
            _mainViewModel.CurrentView = new SettingsViewModel(_mainViewModel, this);
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