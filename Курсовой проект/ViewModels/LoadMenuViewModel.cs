using System.Windows.Input;
using Курсовой_проект.Commands;

namespace Курсовой_проект.ViewModels
{
    public class LoadMenuViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ViewModelBase _previousView;
        private readonly bool _isSaveMode;
        private readonly int _currentStepIndex;
        private readonly Services.SaveService _saveService = new Services.SaveService();

        public string Slot1Text => _saveService.GetSaveDate(1);
        public string Slot2Text => _saveService.GetSaveDate(2);
        public string Slot3Text => _saveService.GetSaveDate(3);
        public string Slot4Text => _saveService.GetSaveDate(4);
        public string Slot5Text => _saveService.GetSaveDate(5);
        public string Slot6Text => _saveService.GetSaveDate(6);

        public string TitleText => _isSaveMode ? "Выберите слот для сохранения" : "Выбор слота сохранения";

        public ICommand SelectSlotCommand { get; }
        public ICommand BackCommand { get; }

        public LoadMenuViewModel(MainViewModel mainViewModel, ViewModelBase previousView)
        {
            _mainViewModel = mainViewModel;
            _previousView = previousView;
            _isSaveMode = false;
            SelectSlotCommand = new RelayCommand(ExecuteSelectSlot);
            BackCommand = new RelayCommand(ExecuteBack);
        }

        public LoadMenuViewModel(MainViewModel mainViewModel, ViewModelBase previousView, int currentStepIndex)
        {
            _mainViewModel = mainViewModel;
            _previousView = previousView;
            _isSaveMode = true;
            _currentStepIndex = currentStepIndex;
            SelectSlotCommand = new RelayCommand(ExecuteSelectSlot);
            BackCommand = new RelayCommand(ExecuteBack);
        }

        private void ExecuteSelectSlot(object parameter)
        {
            if (parameter is string slotStr && int.TryParse(slotStr, out int slotId))
            {
                if (_isSaveMode)
                {
                    _saveService.SaveGame(slotId, _currentStepIndex);
                    System.Windows.MessageBox.Show($"Игра успешно сохранена в слот {slotId}!", "Сохранение", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    _mainViewModel.CurrentView = _previousView;
                }
                else
                {
                    int savedStep = _saveService.LoadGame(slotId);
                    if (savedStep != -1)
                    {
                        _mainViewModel.CurrentView = new NovelViewModel(_mainViewModel, savedStep);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Этот слот пуст!", "Загрузка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void ExecuteBack(object parameter)
        {
            _mainViewModel.CurrentView = _previousView;
        }
    }
}