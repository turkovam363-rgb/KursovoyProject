using System;
using System.Windows;
using Курсовой_проект.Commands;

namespace Курсовой_проект
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            VolumeSlider.Value = AudioService.CurrentVolume;

            if (Application.Current.MainWindow != null)
            {
                FullscreenCheck.IsChecked = Application.Current.MainWindow.WindowStyle == WindowStyle.None;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AudioService.SetVolume(e.NewValue);
        }

        private void FullscreenCheck_Click(object sender, RoutedEventArgs e)
        {
            Window mainWin = Application.Current.MainWindow;
            if (mainWin == null) return;

            if (FullscreenCheck.IsChecked == true)
            {
                mainWin.WindowStyle = WindowStyle.None;
                mainWin.WindowState = WindowState.Maximized;
            }
            else
            {
                mainWin.WindowStyle = WindowStyle.SingleBorderWindow;
                mainWin.WindowState = WindowState.Normal;
                mainWin.Width = 1280;
                mainWin.Height = 720;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
