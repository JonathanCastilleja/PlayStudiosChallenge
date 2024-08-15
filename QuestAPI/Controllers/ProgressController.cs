using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuestAPI.Models;

namespace QuestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly QuestConfig _questConfig;
        public ProgressController(IOptions<QuestConfig> questConfig)
        {
            _questConfig = questConfig.Value;
        }

        [HttpPost]
        public IActionResult Post(ProgressData progressData){
            int questPointsEarned = (int)((progressData.ChipAmountBet * _questConfig.RateFromBet) + (progressData.PlayerLevel * _questConfig.LevelBonusRate));
            double totalQuestPercentCompleted = (double) 100 * questPointsEarned / _questConfig.TotalQuestPointsToComplete;
            int milestonesCompleted = questPointsEarned / (_questConfig.TotalQuestPointsToComplete / _questConfig.MilestonesPerQuest);
            var response = new {
                QuestPointsEarned = questPointsEarned,
                TotalQuestPercentCompleted = totalQuestPercentCompleted,
                MilestonesCompleted = milestonesCompleted
            };
            return Ok(response);
        }
    }
}
