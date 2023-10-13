using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MonoGame.Randomchaos.KestrelServer.Models;
using System.Threading.Tasks;

namespace Sample.MonoGame.Randomchaos.KestrelServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APIController
    {
        protected readonly IConfiguration _configuration;

        protected readonly Game1 _game;

        public APIController(IConfiguration config)
        {
            _configuration = config;
            _game = (Game1)KestrelBackgroundService.Options.GameInstance;
        }

        [HttpGet]
        [ActionName("GetGameValues")]
        [Route("/api/[action]")]
        public async Task<IActionResult> OnGetGameValues()
        {
            return new JsonResult(new { RedSquares = _game.RedSquares.Count, BlackSquares = _game.BlackSquares.Count });
        }
    }
}
