using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой_проект.Services
{
    public interface ISaveService
    {
        bool SaveGame(int slotId, int stepIndex);
        int LoadGame(int slotId);
        string GetSaveDate(int slotId);
    }
}
