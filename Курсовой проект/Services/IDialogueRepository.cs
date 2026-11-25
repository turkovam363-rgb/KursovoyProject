using System.Collections.Generic;
using Курсовой_проект.ViewModels;
using Курсовой_проект.Models;

namespace Курсовой_проект.Services
{
    public interface IDialogueRepository
    {
        List<DialogueStep> GetStoryContent();
    }
}