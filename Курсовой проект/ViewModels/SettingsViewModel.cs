using System;
using System.Windows;
using System.Windows.Input;
using Курсовой_проект.Commands;

namespace Курсовой_проект.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ViewModelBase _previousView;

        private double _volume = AudioService.CurrentVolume;
        public double Volume
        {
            get => _volume;
            set
            {
                if (_volume != value)
                {
                    _volume = value;
                    OnPropertyChanged();
                    AudioService.SetVolume(value);
                }
            }
        }

        private bool _isFullscreen = Application.Current.MainWindow?.WindowStyle == WindowStyle.None;
        public bool IsFullscreen
        {
            get => _isFullscreen;
            set
            {
                if (_isFullscreen != value)
                {
                    _isFullscreen = value;
                    OnPropertyChanged();
                    ToggleFullscreen(value);
                }
            }
        }

        public ICommand CloseCommand { get; }

        public SettingsViewModel(MainViewModel mainViewModel, ViewModelBase previousView = null)
        {
            _mainViewModel = mainViewModel;
            _previousView = previousView ?? new MenuViewModel(_mainViewModel);
            CloseCommand = new RelayCommand(CloseWindow);
        }

        private void ToggleFullscreen(bool isFull)
        {
            Window mainWin = Application.Current.MainWindow;
            if (mainWin == null) return;

            if (isFull)
            {
                mainWin.WindowStyle = WindowStyle.None;
                mainWin.WindowState = WindowState.Maximized;
            }
            else
            {
                mainWin.WindowStyle = WindowStyle.SingleBorderWindow;
                mainWin.WindowState = WindowState.Normal;
                mainWin.Width = 1280;
                mainWin.Height = 720;
            }
        }

        private void CloseWindow(object parameter)
        {
            _mainViewModel.CurrentView = _previousView;
        }
    }
}