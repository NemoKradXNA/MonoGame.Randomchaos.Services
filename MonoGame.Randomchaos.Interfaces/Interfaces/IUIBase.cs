using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Interfaces
{
    public interface IUIBase
    {
        Point Position { get; set; }
        Point Size { get; set; }
        Rectangle Rectangle { get; }
        Color Tint { get; set; }

        bool IsMouseOver { get; set; }
    }
}
