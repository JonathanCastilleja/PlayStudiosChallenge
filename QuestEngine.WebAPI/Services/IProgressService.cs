using QuestEngine.WebAPI.Models;

namespace QuestEngine.WebAPI.Services
{
    public interface IProgressService
    {
        PostProgressResponse CalculateProgress(ProgressData progressData);
        void SaveProgress(ProgressData progressData);
    }
}
