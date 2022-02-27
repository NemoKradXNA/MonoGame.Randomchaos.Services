
namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface IInputStateService : IInputStateManager
    {
        IKeyboardStateManager KeyboardManager { get; set; }
        IGamePadManager GamePadManager { get; set; }
        IMouseStateManager MouseManager { get; set; }

        IAccelerometerHandler AccelerometerHandler { get; set; }

        ITouchCollectionManager TouchCollectionManager { get; set; }
    }
}
