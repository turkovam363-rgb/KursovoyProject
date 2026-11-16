using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls.Primitives;
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
            AudioService.PlayEffect("клик.wav");
        }
    }
}