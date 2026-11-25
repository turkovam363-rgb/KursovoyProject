using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой_проект.Services
{
    public interface IWindowService
    {
        void SetFullscreen(bool isFullscreen);
        void Shutdown();
    }
}
