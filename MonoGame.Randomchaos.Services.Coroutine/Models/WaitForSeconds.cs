
using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using System;
using System.Collections;

namespace MonoGame.Randomchaos.Services.Coroutine.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A wait for seconds. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class WaitForSeconds : Coroutine, IWaitCoroutine
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the timer. </summary>
        ///
        /// <value> The timer. </value>
        ///-------------------------------------------------------------------------------------------------

        protected virtual TimeSpan timer { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the seconds. </summary>
        ///
        /// <value> The seconds. </value>
        ///-------------------------------------------------------------------------------------------------

        protected virtual float _seconds { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">     The game. </param>
        /// <param name="seconds">  The seconds. </param>
        ///-------------------------------------------------------------------------------------------------

        public WaitForSeconds(Game game, float seconds) : base(game)
        {
            _seconds = seconds;
            timer = DateTime.Now.TimeOfDay + new TimeSpan(0, 0, 0, 0, (int)(_seconds * 1000));

            Routine = waitForSeconds();
            CoroutineManager.StartCoroutine(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Wait for seconds. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <returns>   An IEnumerator. </returns>
        ///-------------------------------------------------------------------------------------------------

        IEnumerator waitForSeconds()
        {
            while (DateTime.Now.TimeOfDay <= timer && !Finished)
                yield return null;

            yield break;
        }
    }
}
