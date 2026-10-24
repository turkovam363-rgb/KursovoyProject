using System.Collections.Generic;
using Курсовой_проект.ViewModels;

namespace Курсовой_проект.Services
{
    public interface IDialogueRepository
    {
        List<DialogueStep> GetStoryContent();
    }
}