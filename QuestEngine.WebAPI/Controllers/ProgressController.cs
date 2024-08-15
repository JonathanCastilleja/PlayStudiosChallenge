using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuestEngine.WebAPI.Models;
using QuestEngine.WebAPI.Services;

namespace QuestEngine.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProgressData progressData)
        {
            var response = _progressService.CalculateProgress(progressData);
            _progressService.SaveProgress(progressData);
            return Ok(response);
        }
    }
}
