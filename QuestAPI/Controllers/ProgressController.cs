using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestAPI.Models;

namespace QuestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(ProgressData progressData){
            double rateFromBet = 0.1; //TODO move to config file
            double levelBonusRate = 0.5; //TODO move to config file
            int totalQuestPointsToComplete = 1000; //TODO move to config file
            int milestonesPerQuest = 10; //TODO move to config file
            int questPointsEarned = (int)((progressData.ChipAmountBet * rateFromBet) + (progressData.PlayerLevel * levelBonusRate));
            double totalQuestPercentCompleted = (double) 100 * questPointsEarned / totalQuestPointsToComplete;
            int milestonesCompleted = questPointsEarned / (totalQuestPointsToComplete / milestonesPerQuest);
            var response = new {
                QuestPointsEarned = questPointsEarned,
                TotalQuestPercentCompleted = totalQuestPercentCompleted,
                MilestonesCompleted = milestonesCompleted
            };
            return Ok(response);
        }
    }
}
