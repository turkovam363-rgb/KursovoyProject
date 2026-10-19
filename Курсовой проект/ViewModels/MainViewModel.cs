using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой_проект.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;

        public ViewModelBase CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(); 
            }
        }

        public MainViewModel()
        {
            CurrentView = new MenuViewModel(this);
        }
    }
}
