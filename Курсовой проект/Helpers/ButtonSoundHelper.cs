using System;
using Курсовой_проект.Services;

namespace Курсовой_проект.Helpers
{
    public static class ButtonSoundHelper
    {
        public static void Initialize()
        {
        }

        public static void PlayClick()
        {
            AudioService.PlayUiEffect("клик.wav");
        }
    }
}