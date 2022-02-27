using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections;


namespace MonoGame.Randomchaos.Services.Coroutine.Models
{
    public class Coroutine : ICoroutine
    {
        public ICoroutineService CoroutineManager
        {
            get
            {
                if (Game != null)
                    return Game.Services.GetService<CoroutineService>();

                return null;
            }
        }

        /// <summary>
        /// The IEnumerator routine to execute
        /// </summary>
        public IEnumerator Routine { get; set; }

        /// <summary>
        /// Used to mark if another routine needs to be waited for
        /// </summary>
        public ICoroutine WaitForCoroutine { get; set; }

        /// <summary>
        /// Set to true when finished.
        /// </summary>
        public bool Finished { get; set; }

        protected Game Game = null;
        public Coroutine(Game game) { Game = game; }
        public Coroutine(Game game, IEnumerator routine) : this(game)
        {
            Routine = routine;
        }
    }
}
