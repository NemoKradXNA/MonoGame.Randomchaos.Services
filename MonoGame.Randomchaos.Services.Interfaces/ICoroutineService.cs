using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface ICoroutineService
    {
        List<ICoroutine> Coroutines { get; }

        void Update(GameTime gameTime);

        void UpdateEndFrame(GameTime gameTime);

        ICoroutine StartCoroutine(IEnumerator routine);

        void StopCoroutine(IEnumerator coroutine);

        ICoroutine StartCoroutine(ICoroutine coroutine);
        void StopCoroutine(ICoroutine coroutine);
    }
}
