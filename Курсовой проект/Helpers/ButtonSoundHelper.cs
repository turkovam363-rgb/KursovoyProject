using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Курсовой_проект.Helpers
{
    public static class ButtonSoundHelper
    {
        private static bool _isInitialized = false;

        public static void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            EventManager.RegisterClassHandler(
                typeof(ButtonBase),
                ButtonBase.ClickEvent,
                new RoutedEventHandler(OnGlobalButtonClick),
                true);
        }

        private static void OnGlobalButtonClick(object sender, RoutedEventArgs e)
        {
            PlayClick();
        }

        public static void PlayClick()
        {
            try
            {
                string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "аудио", "клик.wav");
                if (!File.Exists(soundPath))
                {
                    soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "клик.wav");
                }

                if (File.Exists(soundPath))
                {
                    using (SoundPlayer player = new SoundPlayer(soundPath))
                    {
                        player.Play();
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Звуковой файл не найден по пути: " + soundPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка воспроизведения звука: " + ex.Message);
            }
        }
    }
}