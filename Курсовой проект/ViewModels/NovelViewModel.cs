using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

        public NovelViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            NextDialogueCommand = new RelayCommand(ExecuteNextDialogue);

            AudioService.StopBackgroundMusic();

            InitStory();
            ShowCurrentStep();
        }

        private void InitStory()
        {
            Services.IDialogueRepository repository = new Services.SqliteDialogueRepository();
            _storyContent = repository.GetStoryContent();
        }

        private void ShowCurrentStep()
        {
            if (_currentStepIndex < _storyContent.Count)
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
    }
}