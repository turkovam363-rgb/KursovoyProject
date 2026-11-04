using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Курсовой_проект.Commands;

namespace Курсовой_проект.ViewModels
{
    public class DisclaimerViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;

        public ICommand ProceedCommand { get; }

        public DisclaimerViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ProceedCommand = new RelayCommand(ExecuteProceed);
        }

        private void ExecuteProceed(object parameter)
        {
            _mainViewModel.CurrentView = new MenuViewModel(_mainViewModel);
        }
    }
}
