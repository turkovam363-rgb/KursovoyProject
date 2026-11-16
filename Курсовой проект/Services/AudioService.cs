using System;
using System.IO;
using System.Windows.Media;

namespace Курсовой_проект.Services
{
    public static class AudioService
    {
        private static MediaPlayer _bgmPlayer = new MediaPlayer();
        private static double _musicVolume = 0.5;
        private static double _effectsVolume = 1.0;
        private static string _currentSceneMusicPath = "";
        private static bool _isMuted = false;

        public static double MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = value;
                _bgmPlayer.Volume = _isMuted ? 0 : _musicVolume;
            }
        }

        public static double EffectsVolume
        {
            get => _effectsVolume;
            set => _effectsVolume = value;
        }

        public static void SetCurrentSceneMusic(string path)
        {
            _currentSceneMusicPath = path;
        }

        public static void PlayBackgroundMusic(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            _currentSceneMusicPath = fileName;

            if (_isMuted) return;

            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "аудио", fileName);
                _bgmPlayer.Open(new Uri(fullPath, UriKind.Absolute));
                _bgmPlayer.Volume = _musicVolume;
                _bgmPlayer.MediaEnded += (s, e) =>
                {
                    _bgmPlayer.Position = TimeSpan.Zero;
                    _bgmPlayer.Play();
                };
                _bgmPlayer.Play();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка музыки: {ex.Message}");
            }
        }

        public static void StopBackgroundMusic()
        {
            _bgmPlayer.Stop();
            _bgmPlayer.Close();
            _currentSceneMusicPath = "";
        }

        public static void SetMuted(bool muted)
        {
            _isMuted = muted;
            if (muted)
            {
                _bgmPlayer.Pause();
            }
            else
            {
                if (!string.IsNullOrEmpty(_currentSceneMusicPath))
                {
                    PlayBackgroundMusic(_currentSceneMusicPath);
                }
            }
        }

        public static void PlayEffect(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "аудио", fileName);
                var effectPlayer = new MediaPlayer();
                effectPlayer.Volume = _effectsVolume;
                effectPlayer.Open(new Uri(fullPath, UriKind.Absolute));
                effectPlayer.Play();

                effectPlayer.MediaEnded += (s, e) => effectPlayer.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка эффекта: {ex.Message}");
            }
        }
    }
}