using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой_проект.Models
{
    namespace Курсовой_проект.Models
    {
        public class GameSettings : INotifyPropertyChanged
        {
            private double _volume = 1.0;
            private double _effectsVolume = 1.0;
            private double _uiVolume = 1.0;
            private bool _isMusicMuted = false;
            private double _textSpeed = 50.0;
            private double _autoReadSpeed = 2.0;
            private bool _skipUnseen = false;
            private int _textSize = 24;
            private bool _highlightReadText = true;
            private bool _isFullscreen = false;

            public event PropertyChangedEventHandler? PropertyChanged;

            public double Volume
            {
                get => _volume;
                set { if (_volume != value) { _volume = value; OnPropertyChanged(); } }
            }

            public double EffectsVolume
            {
                get => _effectsVolume;
                set { if (_effectsVolume != value) { _effectsVolume = value; OnPropertyChanged(); } }
            }

            public double UiVolume
            {
                get => _uiVolume;
                set { if (_uiVolume != value) { _uiVolume = value; OnPropertyChanged(); } }
            }

            public bool IsMusicMuted
            {
                get => _isMusicMuted;
                set { if (_isMusicMuted != value) { _isMusicMuted = value; OnPropertyChanged(); } }
            }

            public double TextSpeed
            {
                get => _textSpeed;
                set { if (_textSpeed != value) { _textSpeed = value; OnPropertyChanged(); } }
            }

            public double AutoReadSpeed
            {
                get => _autoReadSpeed;
                set { if (_autoReadSpeed != value) { _autoReadSpeed = value; OnPropertyChanged(); } }
            }

            public bool SkipUnseen
            {
                get => _skipUnseen;
                set { if (_skipUnseen != value) { _skipUnseen = value; OnPropertyChanged(); } }
            }

            public int TextSize
            {
                get => _textSize;
                set { if (_textSize != value) { _textSize = value; OnPropertyChanged(); } }
            }

            public bool HighlightReadText
            {
                get => _highlightReadText;
                set { if (_highlightReadText != value) { _highlightReadText = value; OnPropertyChanged(); } }
            }

            public bool IsFullscreen
            {
                get => _isFullscreen;
                set { if (_isFullscreen != value) { _isFullscreen = value; OnPropertyChanged(); } }
            }

            protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}