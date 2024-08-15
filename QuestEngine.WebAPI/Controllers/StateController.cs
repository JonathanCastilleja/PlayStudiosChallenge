using Microsoft.AspNetCore.Mvc;
using QuestEngine.WebAPI.Services;

namespace QuestEngine.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        [HttpGet]
        public IActionResult Get(string playerId)
        {
            var response = _stateService.GetState(playerId);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
