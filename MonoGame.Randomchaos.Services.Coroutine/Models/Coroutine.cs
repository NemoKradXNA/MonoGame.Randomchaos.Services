
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections;


namespace MonoGame.Randomchaos.Services.Coroutine.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A coroutine. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Coroutine : ICoroutine
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the manager for coroutine. </summary>
        ///
        /// <value> The coroutine manager. </value>
        ///-------------------------------------------------------------------------------------------------

        public ICoroutineService CoroutineManager
        {
            get
            {
                if (Game != null)
                    return Game.Services.GetService<ICoroutineService>();

                return null;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The IEnumerator routine to execute. </summary>
        ///
        /// <value> The routine. </value>
        ///-------------------------------------------------------------------------------------------------

        public IEnumerator Routine { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Used to mark if another routine needs to be waited for. </summary>
        ///
        /// <value> The wait for coroutine. </value>
        ///-------------------------------------------------------------------------------------------------

        public ICoroutine WaitForCoroutine { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Set to true when finished. </summary>
        ///
        /// <value> True if finished, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool Finished { get; set; }

        /// <summary>   The game. </summary>
        protected Game Game = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public Coroutine(Game game) { Game = game; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">     The game. </param>
        /// <param name="routine">  The IEnumerator routine to execute. </param>
        ///-------------------------------------------------------------------------------------------------

        public Coroutine(Game game, IEnumerator routine) : this(game)
        {
            Routine = routine;
        }
    }
}
