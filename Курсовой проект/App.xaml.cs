using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using Курсовой_проект.Helpers;
using Курсовой_проект.Services;

namespace Курсовой_проект
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            SettingsService.Load();
            ReadHistoryService.Load();
            AudioService.ApplySettings(SettingsService.Current);

            ButtonSoundHelper.Initialize();

            EventManager.RegisterClassHandler(
                typeof(Button),
                ButtonBase.ClickEvent,
                new RoutedEventHandler(OnButtonClick));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AudioService.StopBackgroundMusic();
            ReadHistoryService.Save();
            SettingsService.Save();
            base.OnExit(e);
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            ButtonSoundHelper.PlayClick();
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine($"Необработанная ошибка: {e.Exception}");
            MessageBox.Show(
                "Произошла непредвиденная ошибка. Попробуйте повторить действие.",
                "Sweet Candies",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            e.Handled = true;
        }
    }
}