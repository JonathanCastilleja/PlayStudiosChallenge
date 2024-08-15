using System;
using Microsoft.Extensions.Options;
using QuestEngine.WebAPI.Data;
using QuestEngine.WebAPI.Models;

namespace QuestEngine.WebAPI.Services
{
    public class StateService : IStateService
    {
        private readonly QuestConfig _questConfig;
        private readonly QuestDbContext _context;

        public StateService(IOptions<QuestConfig> questConfig, QuestDbContext context)
        {
            _questConfig = questConfig.Value;
            _context = context;
        }

        public GetStateResponse? GetState(string playerId)
        {
            var playerQuestState = _context.QuestStates.FirstOrDefault(qs => qs.PlayerId == playerId);

            if (playerQuestState == null)
            {
                return null;
            }

            return new GetStateResponse
            {
                TotalQuestPercentCompleted = (double)100 * playerQuestState.TotalQuestPoints / _questConfig.TotalQuestPointsToComplete,
                LastMilestoneIndexCompleted = playerQuestState.LastMilestoneIndexCompleted
            };
        }
    }
}
