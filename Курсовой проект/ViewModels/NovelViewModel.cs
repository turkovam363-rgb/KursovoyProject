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
        private readonly List<HistoryItem> _dialogueHistoryLog = new List<HistoryItem>();
        private readonly MainViewModel _mainViewModel;
        private List<DialogueStep> _storyContent;
        private int _currentStepIndex = 0;
        private string _lastPlayedMusic = "";
        private readonly Services.SaveService _saveService = new Services.SaveService();

        private System.Windows.Threading.DispatcherTimer _textTimer;
        private System.Windows.Threading.DispatcherTimer _autoReadDelayTimer;
        private const double AutoReadDelaySeconds = 2.0;

        private string _fullTargetText = string.Empty;
        private int _currentLeftCharIndex = 0;
        private bool _isAutoReadActive;
        private bool _isCurrentLineAlreadyRead;
        private bool _isTextFinished;

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

        public bool IsAutoReadActive
        {
            get => _isAutoReadActive;
            set
            {
                if (_isAutoReadActive != value)
                {
                    _isAutoReadActive = value;
                    OnPropertyChanged();
                    if (!value)
                    {
                        _autoReadDelayTimer?.Stop();
                    }
                }
            }
        }

        public bool IsCurrentLineAlreadyRead
        {
            get => _isCurrentLineAlreadyRead;
            set { _isCurrentLineAlreadyRead = value; OnPropertyChanged(); }
        }

        public bool IsTextFinished
        {
            get => _isTextFinished;
            set { _isTextFinished = value; OnPropertyChanged(); }
        }

        public ICommand NextDialogueCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand OpenLoadMenuCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand ReturnToMenuCommand { get; }
        public ICommand SkipCommand { get; }
        public ICommand HistoryCommand { get; }
        public ICommand ToggleAutoReadCommand { get; }

        public NovelViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            NextDialogueCommand = new RelayCommand(ExecuteNextDialogue);
            SaveCommand = new RelayCommand(ExecuteSave);
            OpenLoadMenuCommand = new RelayCommand(ExecuteOpenLoadMenu);
            OpenSettingsCommand = new RelayCommand(ExecuteOpenSettings);
            ReturnToMenuCommand = new RelayCommand(ExecuteReturnToMenu);
            SkipCommand = new RelayCommand(ExecuteSkip);
            HistoryCommand = new RelayCommand(ExecuteHistory);
            ToggleAutoReadCommand = new RelayCommand(ExecuteToggleAutoRead);

            AudioService.StopBackgroundMusic();

            InitStory();
            InitializeNovelTimers();
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
            SkipCommand = new RelayCommand(ExecuteSkip);
            HistoryCommand = new RelayCommand(ExecuteHistory);
            ToggleAutoReadCommand = new RelayCommand(ExecuteToggleAutoRead);

            AudioService.StopBackgroundMusic();

            InitStory();
            _currentStepIndex = startStepIndex;
            InitializeNovelTimers();
            ShowCurrentStep();
        }

        private void InitializeNovelTimers()
        {
            _textTimer = new System.Windows.Threading.DispatcherTimer();
            _textTimer.Tick += TextTimer_Tick;

            _autoReadDelayTimer = new System.Windows.Threading.DispatcherTimer();
            _autoReadDelayTimer.Interval = TimeSpan.FromSeconds(AutoReadDelaySeconds);
            _autoReadDelayTimer.Tick += AutoReadDelayTimer_Tick;
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
                _textTimer?.Stop();
                _autoReadDelayTimer?.Stop();

                var step = _storyContent[_currentStepIndex];
                CharacterName = step.CharacterName;
                CurrentBackground = step.BackgroundPath;
                CurrentSprite = step.SpritePath;

                IsCurrentLineAlreadyRead = ReadHistoryService.IsRead(step.Id);
                IsTextFinished = false;

                if (!string.IsNullOrEmpty(step.MusicPath) && step.MusicPath != _lastPlayedMusic)
                {
                    AudioService.SetCurrentSceneMusic(step.MusicPath);
                    AudioService.PlayBackgroundMusic(step.MusicPath);
                    _lastPlayedMusic = step.MusicPath;
                }

                _fullTargetText = step.Text;
                _currentLeftCharIndex = 0;
                DialogueText = string.Empty;

                if (Services.SettingsService.Current.TextSpeed >= 100)
                {
                    DialogueText = _fullTargetText;
                    IsTextFinished = true;
                    OnTextDisplayFinished();
                }
                else
                {
                    double intervalMs = Math.Max(1, (100 - Services.SettingsService.Current.TextSpeed) * 1.5);
                    _textTimer.Interval = TimeSpan.FromMilliseconds(intervalMs);
                    _textTimer.Start();
                }
            }
            else
            {
                StopAllTimers();
                _mainViewModel.CurrentView = new MenuViewModel(_mainViewModel);
            }
        }

        private void TextTimer_Tick(object sender, EventArgs e)
        {
            if (_currentLeftCharIndex < _fullTargetText.Length)
            {
                DialogueText += _fullTargetText[_currentLeftCharIndex];
                _currentLeftCharIndex++;
            }
            else
            {
                _textTimer.Stop();
                IsTextFinished = true;
                OnTextDisplayFinished();
            }
        }

        private void OnTextDisplayFinished()
        {
            if (_storyContent != null && _currentStepIndex < _storyContent.Count)
            {
                var step = _storyContent[_currentStepIndex];
                ReadHistoryService.MarkAsRead(step.Id);
                IsCurrentLineAlreadyRead = true;

                if (!_dialogueHistoryLog.Any(h => h.StepId == step.Id))
                {
                    _dialogueHistoryLog.Add(new HistoryItem
                    {
                        StepId = step.Id,
                        Speaker = string.IsNullOrEmpty(CharacterName) ? "Автор" : CharacterName,
                        Message = _fullTargetText
                    });
                }
            }

            if (IsAutoReadActive)
            {
                StartAutoReadDelay();
            }
        }

        private void StartAutoReadDelay()
        {
            _autoReadDelayTimer.Stop();
            _autoReadDelayTimer.Interval = TimeSpan.FromSeconds(AutoReadDelaySeconds);
            _autoReadDelayTimer.Start();
        }

        private void AutoReadDelayTimer_Tick(object sender, EventArgs e)
        {
            _autoReadDelayTimer.Stop();

            if (IsAutoReadActive)
            {
                _currentStepIndex++;
                ShowCurrentStep();
            }
        }

        private void ExecuteToggleAutoRead(object parameter)
        {
            IsAutoReadActive = !IsAutoReadActive;

            if (IsAutoReadActive)
            {
                if (!IsTextFinished && _textTimer != null && _textTimer.IsEnabled)
                {
                    _textTimer.Stop();
                    DialogueText = _fullTargetText;
                    IsTextFinished = true;
                    OnTextDisplayFinished();
                }
                else if (IsTextFinished)
                {
                    StartAutoReadDelay();
                }
            }
            else
            {
                _autoReadDelayTimer?.Stop();
            }
        }

        private void ExecuteNextDialogue(object parameter)
        {
            if (IsAutoReadActive)
            {
                IsAutoReadActive = false;
                _autoReadDelayTimer?.Stop();
            }

            if (_textTimer != null && _textTimer.IsEnabled)
            {
                _textTimer.Stop();
                DialogueText = _fullTargetText;
                IsTextFinished = true;
                OnTextDisplayFinished();
                return;
            }

            _currentStepIndex++;
            ShowCurrentStep();
        }

        private void ExecuteSkip(object parameter)
        {
            IsAutoReadActive = false;
            _autoReadDelayTimer?.Stop();

            _currentStepIndex++;
            ShowCurrentStep();
        }

        private void ExecuteHistory(object parameter)
        {
            StopAllTimers();
            _mainViewModel.CurrentView = new HistoryViewModel(_mainViewModel, this, _dialogueHistoryLog);
        }

        private void StopAllTimers()
        {
            _textTimer?.Stop();
            _autoReadDelayTimer?.Stop();
            IsAutoReadActive = false;
        }

        private void ExecuteSave(object parameter)
        {
            StopAllTimers();
            _mainViewModel.CurrentView = new LoadMenuViewModel(_mainViewModel, this, _currentStepIndex);
        }

        private void ExecuteOpenLoadMenu(object parameter)
        {
            StopAllTimers();
            _mainViewModel.CurrentView = new LoadMenuViewModel(_mainViewModel, this);
        }

        private void ExecuteOpenSettings(object parameter)
        {
            StopAllTimers();
            _mainViewModel.CurrentView = new SettingsViewModel(_mainViewModel, this);
        }

        private void ExecuteReturnToMenu(object parameter)
        {
            StopAllTimers();
            var dialog = new ConfirmDialogWindow("Вы действительно хотите выйти в главное меню?\nНесохраненный прогресс будет потерян.");

            if (Application.Current.MainWindow != null)
            {
                dialog.Owner = Application.Current.MainWindow;
            }

            if (dialog.ShowDialog() == true)
            {
                SaveReadHistory();
                _mainViewModel.CurrentView = new MenuViewModel(_mainViewModel);
            }
            else
            {
                ShowCurrentStep();
            }
        }

        public void SaveReadHistory()
        {
            ReadHistoryService.Save();
        }
    }
}