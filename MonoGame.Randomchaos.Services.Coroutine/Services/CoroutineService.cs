using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Coroutine.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGame.Randomchaos.Services.Coroutine
{
    public class CoroutineService : ServiceBase<CoroutineService>, ICoroutineService
    {
        public List<ICoroutine> Coroutines { get; }

        public CoroutineService(Game game) : base(game)
        {
            Coroutines = new List<ICoroutine>();

            Game.Services.AddService(typeof(ICoroutineService), this);
            Game.Components.Add(this);
        }

        /// <summary>
        /// Update called during engine Update.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            foreach (ICoroutine coroutine in Coroutines.Reverse<ICoroutine>())
            {
                if (coroutine is WaitForEndOfFrame)
                    continue;

                ProcessCoroutine(coroutine);
            }
        }

        /// <summary>
        /// Update called at the end of an engine render frame
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateEndFrame(GameTime gameTime)
        {
            foreach (ICoroutine coroutine in Coroutines.Reverse<ICoroutine>())
            {
                if (!(coroutine is WaitForEndOfFrame))
                    continue;

                ProcessCoroutine(coroutine);
            }
        }

        protected virtual void ProcessCoroutine(ICoroutine coroutine)
        {
            if (coroutine.Routine.Current is ICoroutine)
                coroutine.WaitForCoroutine = coroutine.Routine.Current as ICoroutine;

            if (coroutine.WaitForCoroutine != null && coroutine.WaitForCoroutine.Finished)
                coroutine.WaitForCoroutine = null;

            if (coroutine.WaitForCoroutine == null)
            {
                if (!coroutine.Routine.MoveNext() || coroutine.Finished)
                {
                    Coroutines.Remove(coroutine);
                    StopCoroutine(coroutine);
                }
            }
        }

        /// <summary>
        /// Method to start a coroutine
        /// </summary>
        /// <param name="routine"></param>
        /// <returns></returns>
        public ICoroutine StartCoroutine(IEnumerator routine)
        {
            ICoroutine coroutine = new Models.Coroutine(Game, routine);

            Coroutines.Add(coroutine);

            return coroutine;
        }

        /// <summary>
        /// Method to stop a coroutine
        /// </summary>
        /// <param name="coroutine"></param>
        public void StopCoroutine(IEnumerator coroutine)
        {
            List<ICoroutine> engineCoroutines = Coroutines.Where(c => c.Routine.ToString() == coroutine.ToString()).ToList();

            foreach (ICoroutine engineCoroutine in engineCoroutines)
                StopCoroutine(engineCoroutine);
        }

        /// <summary>
        /// Method to stop a coroutine
        /// </summary>
        /// <param name="coroutine"></param>
        public void StopCoroutine(ICoroutine coroutine)
        {
            if (coroutine != null)
            {
                coroutine.Finished = true;
                StopCoroutine(coroutine.WaitForCoroutine);
            }
        }

        /// <summary>
        /// Method to start a coroutine
        /// </summary>
        /// <param name="routine"></param>
        /// <returns></returns>
        public ICoroutine StartCoroutine(ICoroutine coroutine)
        {
            Coroutines.Add(coroutine);
            return coroutine;
        }
    }
}
