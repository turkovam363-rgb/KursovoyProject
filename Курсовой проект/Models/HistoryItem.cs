using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой_проект.Models
{
    public class HistoryItem
    {
        public int StepId { get; set; }
        public string Speaker { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
