using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Курсовой_проект.Services
{
    public class WindowService : IWindowService
    {
        public void SetFullscreen(bool isFullscreen)
        {
            var mainWindow = Application.Current?.MainWindow;
            if (mainWindow == null) return;

            if (isFullscreen)
            {
                mainWindow.WindowStyle = WindowStyle.None;
                mainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.Width = 1280;
                mainWindow.Height = 720;
            }
        }

        public void Shutdown()
        {
            Application.Current?.Shutdown();
        }
    }
}
