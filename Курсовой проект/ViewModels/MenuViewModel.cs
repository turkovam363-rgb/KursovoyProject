using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Курсовой_проект.Commands;
using Курсовой_проект.Services;

namespace Курсовой_проект.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;

        public ICommand SettingsCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand LoadGameCommand { get; }
        public ICommand GalleryCommand { get; }

        public MenuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            SettingsCommand = new RelayCommand(ExecuteSettings);
            ExitCommand = new RelayCommand(ExecuteExit);
            StartGameCommand = new RelayCommand(ExecuteStartGame);
            LoadGameCommand = new RelayCommand(ExecuteLoadGame);
            GalleryCommand = new RelayCommand(ExecuteGallery);

            AudioService.PlayBackgroundMusic("bg_music.mp3");
        }

        private void ExecuteStartGame(object? parameter)
        {
            var novelVm = new NovelViewModel(_mainViewModel);
            if (!novelVm.IsStoryLoaded)
            {
                _mainViewModel.DialogService.ShowMessage("Ошибка", "Не удалось загрузить сценарий из базы данных.");
                AudioService.PlayBackgroundMusic("bg_music.mp3");
                return;
            }

            _mainViewModel.CurrentView = novelVm;
        }

        private void ExecuteLoadGame(object? parameter)
        {
            _mainViewModel.CurrentView = new LoadMenuViewModel(_mainViewModel, this);
        }

        private void ExecuteSettings(object? parameter)
        {
            _mainViewModel.CurrentView = new SettingsViewModel(_mainViewModel, this);
        }

        private void ExecuteExit(object? parameter)
        {
            if (_mainViewModel.DialogService.ShowExitDialog())
            {
                ReadHistoryService.Save();
                SettingsService.Save();
                AudioService.StopBackgroundMusic();
                _mainViewModel.WindowService.Shutdown();
            }
        }

        private void ExecuteGallery(object? parameter)
        {
            _mainViewModel.DialogService.ShowMessage("Sweet Candies", "Галерея CG-артов находится в разработке!");
        }
    }
}