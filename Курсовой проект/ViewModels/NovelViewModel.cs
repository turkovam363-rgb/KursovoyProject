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
    public class DialogueStep
    {
        public int Id { get; set; }
        public string CharacterName { get; set; }
        public string Text { get; set; }
        public string BackgroundPath { get; set; }
        public string MusicPath { get; set; }
        public string SpritePath { get; set; }
    }

    public class NovelViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private List<DialogueStep> _storyContent;
        private int _currentStepIndex = 0;
        private string _lastPlayedMusic = "";
        private readonly Services.SaveService _saveService = new Services.SaveService();

        private string _characterName;
        public string CharacterName
        {
            get => _characterName;
            set { _characterName = value; OnPropertyChanged(); }
        }

        private string _dialogueText;
        public string DialogueText
        {
            get => _dialogueText;
            set { _dialogueText = value; OnPropertyChanged(); }
        }

        private string _currentBackground;
        public string CurrentBackground
        {
            get => _currentBackground;
            set { _currentBackground = value; OnPropertyChanged(); }
        }

        private string _currentSprite;
        public string CurrentSprite
        {
            get => _currentSprite;
            set { _currentSprite = value; OnPropertyChanged(); }
        }

        public ICommand NextDialogueCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand OpenLoadMenuCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand ReturnToMenuCommand { get; } 

        public NovelViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            NextDialogueCommand = new RelayCommand(ExecuteNextDialogue);
            SaveCommand = new RelayCommand(ExecuteSave);
            OpenLoadMenuCommand = new RelayCommand(ExecuteOpenLoadMenu);
            OpenSettingsCommand = new RelayCommand(ExecuteOpenSettings);
            ReturnToMenuCommand = new RelayCommand(ExecuteReturnToMenu);

            AudioService.StopBackgroundMusic();

            InitStory();
            ShowCurrentStep();
        }

        public NovelViewModel(MainViewModel mainViewModel, int startStepIndex)
        {
            _mainViewModel = mainViewModel;
            NextDialogueCommand = new RelayCommand(ExecuteNextDialogue);
            SaveCommand = new RelayCommand(ExecuteSave);
            OpenLoadMenuCommand = new RelayCommand(ExecuteOpenLoadMenu);
            OpenSettingsCommand = new RelayCommand(ExecuteOpenSettings);
            ReturnToMenuCommand = new RelayCommand(ExecuteReturnToMenu);

            AudioService.StopBackgroundMusic();

            InitStory();
            _currentStepIndex = startStepIndex;
            ShowCurrentStep();
        }

        private void InitStory()
        {
            Services.IDialogueRepository repository = new Services.SqliteDialogueRepository();
            _storyContent = repository.GetStoryContent();
        }

        private void ShowCurrentStep()
        {
            if (_storyContent != null && _currentStepIndex < _storyContent.Count)
            {
                var step = _storyContent[_currentStepIndex];
                CharacterName = step.CharacterName;
                DialogueText = step.Text;
                CurrentBackground = step.BackgroundPath;
                CurrentSprite = step.SpritePath;

                if (!string.IsNullOrEmpty(step.MusicPath) && step.MusicPath != _lastPlayedMusic)
                {
                    AudioService.StopBackgroundMusic();
                    AudioService.PlayBackgroundMusic(step.MusicPath);
                    _lastPlayedMusic = step.MusicPath;
                }
            }
            else
            {
                _mainViewModel.CurrentView = new MenuViewModel(_mainViewModel);
            }
        }

        private void ExecuteNextDialogue(object parameter)
        {
            _currentStepIndex++;
            ShowCurrentStep();
        }

        private void ExecuteSave(object parameter)
        {
            _mainViewModel.CurrentView = new LoadMenuViewModel(_mainViewModel, this, _currentStepIndex);
        }

        private void ExecuteOpenLoadMenu(object parameter)
        {
            _mainViewModel.CurrentView = new LoadMenuViewModel(_mainViewModel, this);
        }

        private void ExecuteOpenSettings(object parameter)
        {
            _mainViewModel.CurrentView = new SettingsViewModel(_mainViewModel, this);
        }

        private void ExecuteReturnToMenu(object parameter)
        {
            var dialog = new ConfirmDialogWindow("Вы действительно хотите выйти в главное меню?\nНесохраненный прогресс будет потерян.");

            if (Application.Current.MainWindow != null)
            {
                dialog.Owner = Application.Current.MainWindow;
            }

            if (dialog.ShowDialog() == true)
            {
                _mainViewModel.CurrentView = new MenuViewModel(_mainViewModel);
            }
        }
    }
}