using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using Курсовой_проект.Models.Курсовой_проект.Models;

namespace Курсовой_проект.Services
{
    public static class AudioService
    {
        private static readonly MediaPlayer _bgmPlayer = new MediaPlayer();
        private static readonly List<MediaPlayer> _activeEffectPlayers = new List<MediaPlayer>();
        private static double _musicVolume = 0.5;
        private static double _effectsVolume = 1.0;
        private static double _uiVolume = 1.0;
        private static string _currentSceneMusicPath = string.Empty;
        private static bool _isMuted = false;

        static AudioService()
        {
            _bgmPlayer.MediaEnded += OnBgmMediaEnded;
            _bgmPlayer.MediaFailed += OnBgmMediaFailed;
        }

        public static double MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = Math.Clamp(value, 0.0, 1.0);
                _bgmPlayer.Volume = _isMuted ? 0.0 : _musicVolume;
            }
        }

        public static double EffectsVolume
        {
            get => _effectsVolume;
            set => _effectsVolume = Math.Clamp(value, 0.0, 1.0);
        }

        public static double UiVolume
        {
            get => _uiVolume;
            set => _uiVolume = Math.Clamp(value, 0.0, 1.0);
        }

        public static void ApplySettings(GameSettings settings)
        {
            if (settings == null) return;
            _musicVolume = Math.Clamp(settings.Volume, 0.0, 1.0);
            _effectsVolume = Math.Clamp(settings.EffectsVolume, 0.0, 1.0);
            _uiVolume = Math.Clamp(settings.UiVolume, 0.0, 1.0);
            _isMuted = settings.IsMusicMuted;
            _bgmPlayer.Volume = _isMuted ? 0.0 : _musicVolume;
        }

        public static void SetCurrentSceneMusic(string path)
        {
            _currentSceneMusicPath = path ?? string.Empty;
        }

        public static void PlayBackgroundMusic(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            _currentSceneMusicPath = fileName;

            if (_isMuted) return;

            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "аудио", fileName);
                if (!File.Exists(fullPath))
                {
                    Debug.WriteLine($"Аудиофайл не найден: {fullPath}");
                    return;
                }

                _bgmPlayer.Open(new Uri(fullPath, UriKind.Absolute));
                _bgmPlayer.Volume = _musicVolume;
                _bgmPlayer.Play();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка музыки: {ex.Message}");
            }
        }

        public static void StopBackgroundMusic()
        {
            try
            {
                _bgmPlayer.Stop();
                _bgmPlayer.Close();
                _currentSceneMusicPath = string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка остановки музыки: {ex.Message}");
            }
        }

        public static void SetMuted(bool muted)
        {
            _isMuted = muted;
            try
            {
                if (muted)
                {
                    _bgmPlayer.Pause();
                }
                else if (!string.IsNullOrEmpty(_currentSceneMusicPath))
                {
                    PlayBackgroundMusic(_currentSceneMusicPath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка переключения звука: {ex.Message}");
            }
        }

        public static void PlayEffect(string fileName)
        {
            PlayOneShot(fileName, _effectsVolume);
        }

        public static void PlayUiEffect(string fileName)
        {
            PlayOneShot(fileName, _uiVolume);
        }

        private static void PlayOneShot(string fileName, double volume)
        {
            if (string.IsNullOrWhiteSpace(fileName) || volume <= 0.0) return;

            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "аудио", fileName);
                if (!File.Exists(fullPath))
                {
                    Debug.WriteLine($"Файл эффекта не найден: {fullPath}");
                    return;
                }

                var effectPlayer = new MediaPlayer();
                effectPlayer.Volume = volume;
                _activeEffectPlayers.Add(effectPlayer);

                void Cleanup(object? sender, EventArgs args)
                {
                    effectPlayer.MediaEnded -= Cleanup;
                    effectPlayer.MediaFailed -= OnEffectFailed;
                    effectPlayer.Close();
                    _activeEffectPlayers.Remove(effectPlayer);
                }

                void OnEffectFailed(object? sender, ExceptionEventArgs args)
                {
                    Debug.WriteLine($"Ошибка воспроизведения эффекта: {args.ErrorException?.Message}");
                    Cleanup(sender, EventArgs.Empty);
                }

                effectPlayer.MediaEnded += Cleanup;
                effectPlayer.MediaFailed += OnEffectFailed;
                effectPlayer.Open(new Uri(fullPath, UriKind.Absolute));
                effectPlayer.Play();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка эффекта: {ex.Message}");
            }
        }

        private static void OnBgmMediaEnded(object? sender, EventArgs e)
        {
            if (_isMuted) return;
            try
            {
                _bgmPlayer.Position = TimeSpan.Zero;
                _bgmPlayer.Play();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка зацикливания музыки: {ex.Message}");
            }
        }

        private static void OnBgmMediaFailed(object? sender, ExceptionEventArgs e)
        {
            Debug.WriteLine($"Ошибка декодирования музыки: {e.ErrorException?.Message}");
        }
    }
}