using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой_проект.Services
{
    public interface IDialogService
    {
        void ShowMessage(string title, string message);
        bool ShowConfirm(string message);
        bool ShowExitDialog();
    }

}
