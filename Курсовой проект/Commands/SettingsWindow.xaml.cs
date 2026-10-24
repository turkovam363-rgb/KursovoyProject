using System.Windows;
using Курсовой_проект.ViewModels;

namespace Курсовой_проект
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }
    }
}