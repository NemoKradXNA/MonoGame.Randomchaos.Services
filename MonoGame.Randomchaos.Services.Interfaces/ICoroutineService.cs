
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for coroutine service. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ICoroutineService
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the coroutines. </summary>
        ///
        /// <value> The coroutines. </value>
        ///-------------------------------------------------------------------------------------------------

        List<ICoroutine> Coroutines { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        void Update(GameTime gameTime);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the end frame described by gameTime. </summary>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        void UpdateEndFrame(GameTime gameTime);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts a coroutine. </summary>
        ///
        /// <param name="routine">  The routine. </param>
        ///
        /// <returns>   An ICoroutine. </returns>
        ///-------------------------------------------------------------------------------------------------

        ICoroutine StartCoroutine(IEnumerator routine);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Stops a coroutine. </summary>
        ///
        /// <param name="coroutine">    The coroutine. </param>
        ///-------------------------------------------------------------------------------------------------

        void StopCoroutine(IEnumerator coroutine);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts a coroutine. </summary>
        ///
        /// <param name="coroutine">    The coroutine. </param>
        ///
        /// <returns>   An ICoroutine. </returns>
        ///-------------------------------------------------------------------------------------------------

        ICoroutine StartCoroutine(ICoroutine coroutine);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Stops a coroutine. </summary>
        ///
        /// <param name="coroutine">    The coroutine. </param>
        ///-------------------------------------------------------------------------------------------------

        void StopCoroutine(ICoroutine coroutine);
    }
}
