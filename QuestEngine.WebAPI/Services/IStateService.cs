using QuestEngine.WebAPI.Models;

namespace QuestEngine.WebAPI.Services
{
    public interface IStateService
    {
        GetStateResponse? GetState(string playerId);
    }
}
