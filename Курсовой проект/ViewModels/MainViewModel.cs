using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Курсовой_проект.Services;

namespace Курсовой_проект.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;

        public IDialogService DialogService { get; }
        public IWindowService WindowService { get; }
        public ISaveService SaveService { get; }
        public IDialogueRepository DialogueRepository { get; }

        public ViewModelBase CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(
            IDialogService? dialogService = null,
            IWindowService? windowService = null,
            ISaveService? saveService = null,
            IDialogueRepository? dialogueRepository = null)
        {
            DialogService = dialogService ?? new DialogService();
            WindowService = windowService ?? new WindowService();
            SaveService = saveService ?? new SaveService();
            DialogueRepository = dialogueRepository ?? new SqliteDialogueRepository();

            _currentView = new DisclaimerViewModel(this);
        }
    }
}
