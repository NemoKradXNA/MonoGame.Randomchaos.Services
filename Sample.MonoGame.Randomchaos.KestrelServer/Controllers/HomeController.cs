using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MonoGame.Randomchaos.KestrelServer.Models;

namespace Sample.MonoGame.Randomchaos.KestrelServer.Controllers
{
    
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        /// <summary>   (Immutable) the configuration. </summary>
        protected readonly IConfiguration _configuration;

        protected readonly Game1 _game;

        public HomeController(IConfiguration config)
        {
            _configuration = config;
            _game = (Game1)KestrelBackgroundService.Options.GameInstance;
        }

        [HttpGet]
        [ActionName("Index")]
        [Route("[action]")]

        public ActionResult Index()
        {

            return View(_game);
        }
    }
}
