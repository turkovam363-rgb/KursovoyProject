using System;
using System.IO;
using System.Windows.Media;

namespace Курсовой_проект.Commands
{
    public static class AudioService
    {
        private static readonly MediaPlayer _bgmPlayer = new MediaPlayer();
        public static double CurrentVolume { get; private set; } = 0.3;

        public static void PlayBackgroundMusic(string fileName)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(basePath, "аудио", fileName);

            _bgmPlayer.Close();
            _bgmPlayer.Open(new Uri(fullPath, UriKind.Absolute));
            _bgmPlayer.MediaEnded += (s, e) => { _bgmPlayer.Position = TimeSpan.Zero; _bgmPlayer.Play(); };
            _bgmPlayer.Volume = CurrentVolume;
            _bgmPlayer.Play();
        }

        public static void SetVolume(double volume)
        {
            CurrentVolume = volume;
            _bgmPlayer.Volume = volume;
        }

        public static void StopBackgroundMusic()
        {
            _bgmPlayer.Stop();
            _bgmPlayer.Close();
        }
    }
}

