using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Курсовой_проект.Helpers;

namespace Курсовой_проект
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Services.SettingsService.Load();
            Services.ReadHistoryService.Load();

            ButtonSoundHelper.Initialize();

            EventManager.RegisterClassHandler(
                typeof(Button),
                ButtonBase.ClickEvent,
                new RoutedEventHandler(OnButtonClick));
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            ButtonSoundHelper.PlayClick();
        }
    }
}