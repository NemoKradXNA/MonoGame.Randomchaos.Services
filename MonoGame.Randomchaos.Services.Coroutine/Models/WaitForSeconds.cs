using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Randomchaos.Services.Coroutine.Models
{
    public class WaitForSeconds : Coroutine, IWaitCoroutine
    {
        protected virtual TimeSpan timer { get; set; }
        protected virtual float _seconds { get; set; }

        public WaitForSeconds(Game game, float seconds) : base(game)
        {
            _seconds = seconds;
            timer = DateTime.Now.TimeOfDay + new TimeSpan(0, 0, 0, 0, (int)(_seconds * 1000));

            Routine = waitForSeconds();
            CoroutineManager.StartCoroutine(this);
        }

        IEnumerator waitForSeconds()
        {
            while (DateTime.Now.TimeOfDay <= timer && !Finished)
                yield return null;

            yield break;
        }
    }
}
