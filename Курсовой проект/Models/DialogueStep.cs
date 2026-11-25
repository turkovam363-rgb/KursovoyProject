using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой_проект.Models
{
    public class DialogueStep
    {
        public int Id { get; set; }
        public string CharacterName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string BackgroundPath { get; set; } = string.Empty;
        public string MusicPath { get; set; } = string.Empty;
        public string SpritePath { get; set; } = string.Empty;
    }
}
