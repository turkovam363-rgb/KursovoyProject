using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using Курсовой_проект.Commands;
using Курсовой_проект.Models;
using Курсовой_проект.Services;

namespace Курсовой_проект.ViewModels
{
    public class NovelViewModel : ViewModelBase
    {
        private readonly List<HistoryItem> _dialogueHistoryLog = new List<HistoryItem>();
        private readonly MainViewModel _mainViewModel;
        private readonly DispatcherTimer _textTimer = new DispatcherTimer();
        private readonly DispatcherTimer _autoReadDelayTimer = new DispatcherTimer();
        private readonly DispatcherTimer _skipTimer = new DispatcherTimer();

        private List<DialogueStep> _storyContent = new List<DialogueStep>();
        private int _currentStepIndex = 0;
        private string _lastPlayedMusic = string.Empty;

        private string _fullTargetText = string.Empty;
        private int _currentLeftCharIndex = 0;
        private bool _wasCurrentStepAlreadyRead;
        private bool _isAutoReadActive;
        private bool _isSkipActive;
        private bool _isCurrentLineAlreadyRead;
        private bool _isTextFinished;

        private string _characterName = string.Empty;
        private string _dialogueText = string.Empty;
        private string _currentBackground = string.Empty;
        private string _currentSprite = string.Empty;

        public bool IsStoryLoaded => _storyContent.Count > 0;

        public string CharacterName
        {
            get => _characterName;
            set { _characterName = value; OnPropertyChanged(); }
        }

        public string DialogueText
        {
            get => _dialogueText;
            set { _dialogueText = value; OnPropertyChanged(); }
        }

        public string CurrentBackground
        {
            get => _currentBackground;
            set { _currentBackground = value; OnPropertyChanged(); }
        }

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
                        _autoReadDelayTimer.Stop();
                    }
                }
            }
        }

        public bool IsSkipActive
        {
            get => _isSkipActive;
            set
            {
                if (_isSkipActive != value)
                {
                    _isSkipActive = value;
                    OnPropertyChanged();
                    if (!value)
                    {
                        _skipTimer.Stop();
                        ReadHistoryService.Save();
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

        public NovelViewModel(MainViewModel mainViewModel) : this(mainViewModel, 0)
        {
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

            InitializeNovelTimers();
            InitStory();

            if (!IsStoryLoaded)
            {
                return;
            }

            AudioService.StopBackgroundMusic();
            _currentStepIndex = (startStepIndex >= 0 && startStepIndex < _storyContent.Count) ? startStepIndex : 0;
            ShowCurrentStep();
        }

        private void InitializeNovelTimers()
        {
            _textTimer.Tick += TextTimer_Tick;
            _autoReadDelayTimer.Tick += AutoReadDelayTimer_Tick;
            _skipTimer.Interval = TimeSpan.FromMilliseconds(60);
            _skipTimer.Tick += SkipTimer_Tick;
        }

        private void InitStory()
        {
            _storyContent = _mainViewModel.DialogueRepository.GetStoryContent() ?? new List<DialogueStep>();
        }

        private void ShowCurrentStep()
        {
            if (_currentStepIndex >= 0 && _currentStepIndex < _storyContent.Count)
            {
                _textTimer.Stop();
                _autoReadDelayTimer.Stop();
                _skipTimer.Stop();

                var step = _storyContent[_currentStepIndex];
                CharacterName = step.CharacterName;
                CurrentBackground = step.BackgroundPath;
                CurrentSprite = step.SpritePath;

                _wasCurrentStepAlreadyRead = ReadHistoryService.IsRead(step.Id);
                IsCurrentLineAlreadyRead = _wasCurrentStepAlreadyRead;
                IsTextFinished = false;

                if (!string.IsNullOrEmpty(step.MusicPath) && step.MusicPath != _lastPlayedMusic)
                {
                    AudioService.SetCurrentSceneMusic(step.MusicPath);
                    AudioService.PlayBackgroundMusic(step.MusicPath);
                    _lastPlayedMusic = step.MusicPath;
                }

                _fullTargetText = step.Text ?? string.Empty;
                _currentLeftCharIndex = 0;
                DialogueText = string.Empty;

                if (SettingsService.Current.TextSpeed >= 100 || string.IsNullOrEmpty(_fullTargetText))
                {
                    DialogueText = _fullTargetText;
                    IsTextFinished = true;
                    OnTextDisplayFinished();
                }
                else
                {
                    double intervalMs = Math.Max(1, (100 - SettingsService.Current.TextSpeed) * 1.5);
                    _textTimer.Interval = TimeSpan.FromMilliseconds(intervalMs);
                    _textTimer.Start();
                }
            }
            else
            {
                StopAllTimers();
                SaveReadHistory();
                _mainViewModel.CurrentView = new MenuViewModel(_mainViewModel);
            }
        }

        private void ShowStepForSkip(DialogueStep step)
        {
            _textTimer.Stop();
            _autoReadDelayTimer.Stop();

            CharacterName = step.CharacterName;
            CurrentBackground = step.BackgroundPath;
            CurrentSprite = step.SpritePath;

            _wasCurrentStepAlreadyRead = ReadHistoryService.IsRead(step.Id);
            IsCurrentLineAlreadyRead = _wasCurrentStepAlreadyRead;

            if (!string.IsNullOrEmpty(step.MusicPath) && step.MusicPath != _lastPlayedMusic)
            {
                AudioService.SetCurrentSceneMusic(step.MusicPath);
                AudioService.PlayBackgroundMusic(step.MusicPath);
                _lastPlayedMusic = step.MusicPath;
            }

            _fullTargetText = step.Text ?? string.Empty;
            _currentLeftCharIndex = _fullTargetText.Length;
            DialogueText = _fullTargetText;
            IsTextFinished = true;

            ReadHistoryService.MarkAsRead(step.Id, false);
            AddStepToHistory(step);

            _skipTimer.Start();
        }

        private void AddStepToHistory(DialogueStep step)
        {
            if (!_dialogueHistoryLog.Any(h => h.StepId == step.Id))
            {
                _dialogueHistoryLog.Add(new HistoryItem
                {
                    StepId = step.Id,
                    Speaker = string.IsNullOrEmpty(step.CharacterName) ? "Автор" : step.CharacterName,
                    Message = step.Text ?? string.Empty
                });
            }
        }

        private void TextTimer_Tick(object? sender, EventArgs e)
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
            if (_currentStepIndex >= 0 && _currentStepIndex < _storyContent.Count)
            {
                var step = _storyContent[_currentStepIndex];
                ReadHistoryService.MarkAsRead(step.Id);
                AddStepToHistory(step);
            }

            if (IsAutoReadActive && !IsSkipActive)
            {
                StartAutoReadDelay();
            }
        }

        private void StartAutoReadDelay()
        {
            _autoReadDelayTimer.Stop();
            double delay = Math.Max(0.5, SettingsService.Current.AutoReadSpeed);
            _autoReadDelayTimer.Interval = TimeSpan.FromSeconds(delay);
            _autoReadDelayTimer.Start();
        }

        private void AutoReadDelayTimer_Tick(object? sender, EventArgs e)
        {
            _autoReadDelayTimer.Stop();

            if (IsAutoReadActive && !IsSkipActive)
            {
                _currentStepIndex++;
                ShowCurrentStep();
            }
        }

        private bool CanStartSkipping()
        {
            if (SettingsService.Current.SkipUnseen)
            {
                return true;
            }

            if (_wasCurrentStepAlreadyRead)
            {
                return true;
            }

            if (IsTextFinished && _currentStepIndex + 1 < _storyContent.Count)
            {
                return ReadHistoryService.IsRead(_storyContent[_currentStepIndex + 1].Id);
            }

            return false;
        }

        private void SkipTimer_Tick(object? sender, EventArgs e)
        {
            _skipTimer.Stop();

            if (!IsSkipActive)
            {
                return;
            }

            int nextIndex = _currentStepIndex + 1;
            if (nextIndex >= _storyContent.Count)
            {
                StopAllTimers();
                SaveReadHistory();
                _mainViewModel.CurrentView = new MenuViewModel(_mainViewModel);
                return;
            }

            var nextStep = _storyContent[nextIndex];
            bool isNextRead = ReadHistoryService.IsRead(nextStep.Id);

            if (!SettingsService.Current.SkipUnseen && !isNextRead)
            {
                IsSkipActive = false;
                _currentStepIndex = nextIndex;
                ShowCurrentStep();
                return;
            }

            _currentStepIndex = nextIndex;
            ShowStepForSkip(nextStep);
        }

        private void PauseAndFinishCurrentText()
        {
            IsAutoReadActive = false;
            IsSkipActive = false;
            _autoReadDelayTimer.Stop();
            _skipTimer.Stop();

            if (_textTimer.IsEnabled)
            {
                _textTimer.Stop();
                DialogueText = _fullTargetText;
                IsTextFinished = true;
                OnTextDisplayFinished();
            }
        }

        private void ExecuteToggleAutoRead(object? parameter)
        {
            IsSkipActive = false;
            IsAutoReadActive = !IsAutoReadActive;

            if (IsAutoReadActive)
            {
                if (!IsTextFinished && _textTimer.IsEnabled)
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
                _autoReadDelayTimer.Stop();
            }
        }

        private void ExecuteSkip(object? parameter)
        {
            if (IsSkipActive)
            {
                IsSkipActive = false;
                return;
            }

            IsAutoReadActive = false;

            if (!CanStartSkipping())
            {
                return;
            }

            IsSkipActive = true;

            if (_textTimer.IsEnabled)
            {
                _textTimer.Stop();
                DialogueText = _fullTargetText;
                IsTextFinished = true;
                OnTextDisplayFinished();
            }

            _skipTimer.Start();
        }

        private void ExecuteNextDialogue(object? parameter)
        {
            if (IsSkipActive)
            {
                IsSkipActive = false;
                return;
            }

            if (IsAutoReadActive)
            {
                IsAutoReadActive = false;
                _autoReadDelayTimer.Stop();
            }

            if (_textTimer.IsEnabled)
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

        private void ExecuteHistory(object? parameter)
        {
            PauseAndFinishCurrentText();
            _mainViewModel.CurrentView = new HistoryViewModel(_mainViewModel, this, _dialogueHistoryLog);
        }

        private void StopAllTimers()
        {
            _textTimer.Stop();
            _autoReadDelayTimer.Stop();
            _skipTimer.Stop();
            IsAutoReadActive = false;
            IsSkipActive = false;
        }

        private void ExecuteSave(object? parameter)
        {
            PauseAndFinishCurrentText();
            SaveReadHistory();
            _mainViewModel.CurrentView = new LoadMenuViewModel(_mainViewModel, this, _currentStepIndex);
        }

        private void ExecuteOpenLoadMenu(object? parameter)
        {
            PauseAndFinishCurrentText();
            SaveReadHistory();
            _mainViewModel.CurrentView = new LoadMenuViewModel(_mainViewModel, this);
        }

        private void ExecuteOpenSettings(object? parameter)
        {
            PauseAndFinishCurrentText();
            _mainViewModel.CurrentView = new SettingsViewModel(_mainViewModel, this);
        }

        private void ExecuteReturnToMenu(object? parameter)
        {
            PauseAndFinishCurrentText();
            bool confirmed = _mainViewModel.DialogService.ShowConfirm(
                "Вы действительно хотите выйти в главное меню?\nНесохраненный прогресс будет потерян.");

            if (confirmed)
            {
                SaveReadHistory();
                _mainViewModel.CurrentView = new MenuViewModel(_mainViewModel);
            }
        }

        public void SaveReadHistory()
        {
            ReadHistoryService.Save();
        }
    }
}