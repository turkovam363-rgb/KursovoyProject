using System.Windows;
using Курсовой_проект.Services;
using Курсовой_проект.ViewModels;

namespace Курсовой_проект
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainViewModel;

        public MainWindow()
        {
            _mainViewModel = new MainViewModel();
            DataContext = _mainViewModel;
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SettingsService.Current.IsFullscreen)
            {
                _mainViewModel.WindowService.SetFullscreen(true);
            }
        }
    }
}