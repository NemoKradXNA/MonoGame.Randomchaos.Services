using System.Collections;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface ICoroutine
    {
        ICoroutineService CoroutineManager { get; }
        IEnumerator Routine { get; set; }
        ICoroutine WaitForCoroutine { get; set; }
        bool Finished { get; set; }
    }
}
