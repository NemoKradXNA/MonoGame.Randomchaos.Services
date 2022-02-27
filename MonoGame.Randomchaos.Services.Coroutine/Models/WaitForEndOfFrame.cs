using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Randomchaos.Services.Coroutine.Models
{
    /// <summary>
    /// An engine Coroutine that will be used to wait for end of frame before finishing
    /// </summary>
    public class WaitForEndOfFrame : Coroutine, IWaitCoroutine
    {
        public WaitForEndOfFrame(Game game) : base(game)
        {
            Routine = routine();
            CoroutineManager.StartCoroutine(this);
        }

        IEnumerator routine()
        {
            yield break;
        }
    }
}
