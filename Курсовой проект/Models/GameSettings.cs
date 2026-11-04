using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой_проект.Models
{
    namespace Курсовой_проект.Models
    {
        public class GameSettings
        {
            public double Volume { get; set; } = 1.0;
            public double EffectsVolume { get; set; } = 1.0;
            public double UiVolume { get; set; } = 1.0;
            public bool IsMusicMuted { get; set; } = false;

            public double TextSpeed { get; set; } = 50.0;
            public double AutoReadSpeed { get; set; } = 2.0;
            public bool SkipUnseen { get; set; } = false;
            public int TextSize { get; set; } = 24;
            public bool HighlightReadText { get; set; } = true;

            public bool IsFullscreen { get; set; } = false;
        }
    }
}
