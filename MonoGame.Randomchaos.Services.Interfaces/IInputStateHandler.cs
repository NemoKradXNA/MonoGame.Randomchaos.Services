
namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface IInputStateHandler : IInputStateManager
    {
        IKeyboardStateManager KeyboardManager { get; set; }
        IGamePadManager GamePadManager { get; set; }
        IMouseStateManager MouseManager { get; set; }
    }
}
