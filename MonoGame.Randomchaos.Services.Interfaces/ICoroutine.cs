
using System.Collections;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for coroutine. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface ICoroutine
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the manager for coroutine. </summary>
        ///
        /// <value> The coroutine manager. </value>
        ///-------------------------------------------------------------------------------------------------

        ICoroutineService CoroutineManager { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the routine. </summary>
        ///
        /// <value> The routine. </value>
        ///-------------------------------------------------------------------------------------------------

        IEnumerator Routine { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the wait for coroutine. </summary>
        ///
        /// <value> The wait for coroutine. </value>
        ///-------------------------------------------------------------------------------------------------

        ICoroutine WaitForCoroutine { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the finished. </summary>
        ///
        /// <value> True if finished, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        bool Finished { get; set; }
    }
}
