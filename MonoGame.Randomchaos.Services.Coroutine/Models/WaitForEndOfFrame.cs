
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections;

namespace MonoGame.Randomchaos.Services.Coroutine.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    /// An engine Coroutine that will be used to wait for end of frame before finishing.
    /// </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class WaitForEndOfFrame : Coroutine, IWaitCoroutine
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public WaitForEndOfFrame(Game game) : base(game)
        {
            Routine = routine();
            CoroutineManager.StartCoroutine(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the routine. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <returns>   An IEnumerator. </returns>
        ///-------------------------------------------------------------------------------------------------

        IEnumerator routine()
        {
            yield break;
        }
    }
}
