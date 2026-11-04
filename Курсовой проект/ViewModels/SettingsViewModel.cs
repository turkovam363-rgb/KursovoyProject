using System;
using System.Windows;
using System.Windows.Input;
using Курсовой_проект.Commands;
using Курсовой_проект.Services;

namespace Курсовой_проект.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ViewModelBase? _previousView;
        public double Volume
        {
            get => SettingsService.Current.Volume;
            set
            {
                if (SettingsService.Current.Volume != value)
                {
                    SettingsService.Current.Volume = value;
                    OnPropertyChanged();
                    SettingsService.Save();
                    AudioService.SetVolume(value);
                }
            }
        }

        public double EffectsVolume
        {
            get => SettingsService.Current.EffectsVolume;
            set
            {
                if (SettingsService.Current.EffectsVolume != value)
                {
                    SettingsService.Current.EffectsVolume = value;
                    OnPropertyChanged();
                    SettingsService.Save();
                }
            }
        }

        public double UiVolume
        {
            get => SettingsService.Current.UiVolume;
            set
            {
                if (SettingsService.Current.UiVolume != value)
                {
                    SettingsService.Current.UiVolume = value;
                    OnPropertyChanged();
                    SettingsService.Save();
                }
            }
        }

        public bool IsMusicMuted
        {
            get => SettingsService.Current.IsMusicMuted;
            set
            {
                if (SettingsService.Current.IsMusicMuted != value)
                {
                    SettingsService.Current.IsMusicMuted = value;
                    OnPropertyChanged();
                    SettingsService.Save();
                }
            }
        }

        public double TextSpeed
        {
            get => SettingsService.Current.TextSpeed;
            set
            {
                if (SettingsService.Current.TextSpeed != value)
                {
                    SettingsService.Current.TextSpeed = value;
                    OnPropertyChanged();
                    SettingsService.Save();
                }
            }
        }

        public double AutoReadSpeed
        {
            get => SettingsService.Current.AutoReadSpeed;
            set
            {
                if (SettingsService.Current.AutoReadSpeed != value)
                {
                    SettingsService.Current.AutoReadSpeed = value;
                    OnPropertyChanged();
                    SettingsService.Save();
                }
            }
        }

        public bool SkipUnseen
        {
            get => SettingsService.Current.SkipUnseen;
            set
            {
                if (SettingsService.Current.SkipUnseen != value)
                {
                    SettingsService.Current.SkipUnseen = value;
                    OnPropertyChanged();
                    SettingsService.Save();
                }
            }
        }

        public int TextSize
        {
            get => SettingsService.Current.TextSize;
            set
            {
                if (SettingsService.Current.TextSize != value)
                {
                    SettingsService.Current.TextSize = value;
                    OnPropertyChanged();
                    SettingsService.Save();
                }
            }
        }

        public bool HighlightReadText
        {
            get => SettingsService.Current.HighlightReadText;
            set
            {
                if (SettingsService.Current.HighlightReadText != value)
                {
                    SettingsService.Current.HighlightReadText = value;
                    OnPropertyChanged();
                    SettingsService.Save();
                }
            }
        }

        public bool IsFullscreen
        {
            get => SettingsService.Current.IsFullscreen;
            set
            {
                if (SettingsService.Current.IsFullscreen != value)
                {
                    SettingsService.Current.IsFullscreen = value;
                    OnPropertyChanged();
                    SettingsService.Save();
                    ToggleFullscreen(value);
                }
            }
        }

        public ICommand CloseCommand { get; }

        public SettingsViewModel(MainViewModel mainViewModel, ViewModelBase? previousView = null)
        {
            _mainViewModel = mainViewModel;
            _previousView = previousView ?? new MenuViewModel(_mainViewModel);
            CloseCommand = new RelayCommand(CloseWindow);

            ToggleFullscreen(SettingsService.Current.IsFullscreen);
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