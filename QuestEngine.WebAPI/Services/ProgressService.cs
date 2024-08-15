using Microsoft.Extensions.Options;
using QuestEngine.WebAPI.Data;
using QuestEngine.WebAPI.Models;

namespace QuestEngine.WebAPI.Services
{
    public class ProgressService : IProgressService
    {
        private readonly QuestConfig _questConfig;
        private readonly QuestDbContext _context;

        public ProgressService(IOptions<QuestConfig> questConfig, QuestDbContext context)
        {
            _questConfig = questConfig.Value;
            _context = context;
        }

        public PostProgressResponse CalculateProgress(ProgressData progressData)
        {
            var playerQuestState = _context.QuestStates.FirstOrDefault(qs => qs.PlayerId == progressData.PlayerId);

            int questPointsEarned = (int)((progressData.ChipAmountBet * _questConfig.RateFromBet) + (progressData.PlayerLevel * _questConfig.LevelBonusRate));
            int totalQuestPoints = questPointsEarned + (playerQuestState?.TotalQuestPoints ?? 0); 
            double totalQuestPercentCompleted = (double) 100 * totalQuestPoints / _questConfig.TotalQuestPointsToComplete;
            var milestonesCompleted = _questConfig.Milestones?.Where(milestone => totalQuestPoints >= milestone.MilestonePointsToComplete).OrderBy(milestone => milestone.MilestonePointsToComplete);
            int milestoneIndex = milestonesCompleted?.Count() ?? 0;
            
            return new PostProgressResponse{
                QuestPointsEarned = questPointsEarned,
                TotalQuestPercentCompleted = totalQuestPercentCompleted,
                MilestonesCompleted = new Milestone{
                    MilestoneIndex = milestoneIndex,
                    ChipsAwarded = playerQuestState?.LastMilestoneIndexCompleted == milestoneIndex ? 0 :
                        milestonesCompleted?.LastOrDefault()?.ChipsAward ?? 0
                }
            };
        }

        public void SaveProgress(ProgressData progressData)
        {
            var playerQuestState = _context.QuestStates.FirstOrDefault(qs => qs.PlayerId == progressData.PlayerId);
            int questPointsEarned = (int)((progressData.ChipAmountBet * _questConfig.RateFromBet) + (progressData.PlayerLevel * _questConfig.LevelBonusRate));
            int totalQuestPoints = questPointsEarned + (playerQuestState?.TotalQuestPoints ?? 0);
            int milestoneIndex = _questConfig.Milestones?.Count(milestone => totalQuestPoints >= milestone.MilestonePointsToComplete) ?? 0;

            if (playerQuestState != null)
            {
                playerQuestState.TotalQuestPoints = totalQuestPoints;
                playerQuestState.LastMilestoneIndexCompleted = milestoneIndex;
                _context.QuestStates.Update(playerQuestState);
            }
            else
            {
                _context.QuestStates.Add(new QuestState
                {
                    PlayerId = progressData.PlayerId,
                    TotalQuestPoints = totalQuestPoints,
                    LastMilestoneIndexCompleted = milestoneIndex
                });
            }

            _context.SaveChanges();
        }
    }
}