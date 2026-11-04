using System.Windows;
using Курсовой_проект.Helpers;
using Курсовой_проект.Services; 
namespace Курсовой_проект
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SettingsService.Load();
            ButtonSoundHelper.Initialize();
        }
    }
}