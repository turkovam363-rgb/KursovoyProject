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

        public string Slot1Text => _mainViewModel.SaveService.GetSaveDate(1);
        public string Slot2Text => _mainViewModel.SaveService.GetSaveDate(2);
        public string Slot3Text => _mainViewModel.SaveService.GetSaveDate(3);
        public string Slot4Text => _mainViewModel.SaveService.GetSaveDate(4);
        public string Slot5Text => _mainViewModel.SaveService.GetSaveDate(5);
        public string Slot6Text => _mainViewModel.SaveService.GetSaveDate(6);

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

        private void ExecuteSelectSlot(object? parameter)
        {
            if (parameter is not string slotStr || !int.TryParse(slotStr, out int slotId))
            {
                return;
            }

            if (_isSaveMode)
            {
                bool saved = _mainViewModel.SaveService.SaveGame(slotId, _currentStepIndex);
                if (saved)
                {
                    _mainViewModel.DialogService.ShowMessage("Сохранение", $"Игра успешно сохранена в слот {slotId}!");
                    _mainViewModel.CurrentView = _previousView;
                }
                else
                {
                    _mainViewModel.DialogService.ShowMessage("Ошибка", "Не удалось сохранить игру.");
                }
            }
            else
            {
                int savedStep = _mainViewModel.SaveService.LoadGame(slotId);
                if (savedStep != -1)
                {
                    var novelVm = new NovelViewModel(_mainViewModel, savedStep);
                    if (!novelVm.IsStoryLoaded)
                    {
                        _mainViewModel.DialogService.ShowMessage("Ошибка", "Не удалось загрузить сценарий из базы данных.");
                        return;
                    }

                    _mainViewModel.CurrentView = novelVm;
                }
                else
                {
                    _mainViewModel.DialogService.ShowMessage("Загрузка", "Этот слот пуст!");
                }
            }
        }

        private void ExecuteBack(object? parameter)
        {
            _mainViewModel.CurrentView = _previousView;
        }
    }
}