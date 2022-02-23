using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface IUIBase
    {
        Point Position { get; set; }
        Point Size { get; set; }
        Rectangle Rectangle { get; }
        Color Tint { get; set; }

    }
}
