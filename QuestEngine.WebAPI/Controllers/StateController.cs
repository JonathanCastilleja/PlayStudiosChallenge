using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuestEngine.WebAPI.Data;
using QuestEngine.WebAPI.Models;

namespace QuestEngine.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly QuestConfig _questConfig;
        private readonly QuestDbContext _context;
        public StateController(IOptions<QuestConfig> questConfig, QuestDbContext context)
        {
            _questConfig = questConfig.Value;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get(string playerId){
            var playerQuestState = _context.QuestStates.FirstOrDefault(qs => qs.PlayerId == playerId);

            if (playerQuestState == null){
                return NotFound();
            }
            
            var response = new {
                TotalQuestPercentCompleted = (double) 100 * playerQuestState.TotalQuestPoints / _questConfig.TotalQuestPointsToComplete,
                LastMilestoneIndexCompleted = playerQuestState.LastMilestoneIndexCompleted
            };

            return Ok(response);
        }
    }
}
