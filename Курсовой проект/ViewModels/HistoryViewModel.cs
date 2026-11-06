using System.Collections.Generic;
using System.Windows.Input;
using Курсовой_проект.Commands;

namespace Курсовой_проект.ViewModels
{
    public class HistoryItem
    {
        public string Speaker { get; set; }
        public string Message { get; set; }
    }

    public class HistoryViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ViewModelBase _previousView;

        public List<HistoryItem> DialogueHistory { get; }

        public ICommand CloseCommand { get; }

        public HistoryViewModel(MainViewModel mainViewModel, ViewModelBase previousView, List<HistoryItem> history)
        {
            _mainViewModel = mainViewModel;
            _previousView = previousView;
            DialogueHistory = history;

            CloseCommand = new RelayCommand(CloseHistory);
        }

        private void CloseHistory(object parameter)
        {
            _mainViewModel.CurrentView = _previousView;
        }
    }
}
