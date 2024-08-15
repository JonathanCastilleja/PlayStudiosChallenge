using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuestEngine.WebAPI.Data;
using QuestEngine.WebAPI.Models;

namespace QuestEngine.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly QuestConfig _questConfig;
        private readonly QuestDbContext _context;
        public ProgressController(IOptions<QuestConfig> questConfig, QuestDbContext context)
        {
            _questConfig = questConfig.Value;
            _context = context;
        }

        [HttpPost]
        public IActionResult Post([FromBody]ProgressData progressData){
            var playerQuestState = _context.QuestStates.FirstOrDefault(qs => qs.PlayerId == progressData.PlayerID);

            int questPointsEarned = (int)((progressData.ChipAmountBet * _questConfig.RateFromBet) + (progressData.PlayerLevel * _questConfig.LevelBonusRate));
            int totalQuestPoints = questPointsEarned + (playerQuestState?.TotalQuestPoints ?? 0); 
            double totalQuestPercentCompleted = (double) 100 * totalQuestPoints / _questConfig.TotalQuestPointsToComplete;
            var milestonesCompleted = _questConfig.Milestones?.Where(milestone => totalQuestPoints >= milestone.MilestonePointsToComplete).OrderBy(milestone => milestone.MilestonePointsToComplete);
            int milestoneIndex = milestonesCompleted?.Count() ?? 0;
            
            var response = new PostProgressResponse{
                QuestPointsEarned = questPointsEarned,
                TotalQuestPercentCompleted = totalQuestPercentCompleted,
                MilestonesCompleted = new Milestone{
                    MilestoneIndex = milestoneIndex,
                    ChipsAwarded = playerQuestState?.LastMilestoneIndexCompleted == milestoneIndex? 0:
                        milestonesCompleted?.LastOrDefault()?.ChipsAward ?? 0
                }
            };

            if (playerQuestState != null){
                playerQuestState.TotalQuestPoints = totalQuestPoints;
                playerQuestState.LastMilestoneIndexCompleted = milestoneIndex;
                _context.QuestStates.Update(playerQuestState);
            }
            else{
                _context.QuestStates.Add(new QuestState{
                    PlayerId = progressData.PlayerID,
                    TotalQuestPoints = totalQuestPoints,
                    LastMilestoneIndexCompleted = milestoneIndex
                    }
                );
            }

            _context.SaveChanges();

            return Ok(response);
        }
    }
}
