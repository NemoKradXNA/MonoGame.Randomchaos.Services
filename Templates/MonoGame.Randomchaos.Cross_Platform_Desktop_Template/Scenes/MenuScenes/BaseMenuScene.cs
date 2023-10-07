using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Scene.Models;
using MonoGame.Randomchaos.UI;

namespace MonoGame.Randomchaos.Cross_Platform_Desktop_Template.Scenes.MenuScenes
{
    public abstract class BaseMenuScene : SceneFadeBase
    {
        /// <summary>   The font. </summary>
        protected SpriteFont font;


        /// <summary>   The next scene. </summary>
        protected string NextScene;

        public BaseMenuScene(Game game, string name, string audioAsset = null) : base(game, name, audioAsset)
        {

        }


        protected abstract void BtnExit_OnMouseClick(IUIBase sender, IMouseStateManager mouseState);

        protected virtual UIButton CreateButton(string text, Point pos, Point buttonSize, Color? tint = null, Color? hightlight = null)
        {
            UIButton btn = new UIButton(Game, pos, buttonSize)
            {
                BackgroundTexture = Game.Content.Load<Texture2D>("Textures/UI/ButtonBox"),
                Text = text,
                Tint = tint != null ? tint.Value : Color.Black,
                HighlightColor = hightlight != null ? hightlight.Value : Color.SteelBlue,
                Font = font,
                Segments = new Rectangle(4, 4, 4, 4),
                ScaledSegments = false
            };

            btn.OnMouseClick += BtnExit_OnMouseClick;

            return btn;
        }

        protected virtual UISlider CreateSlider(string text, Point pos, Point buttonSize, float value = 0)
        {
            Texture2D bar = new Texture2D(GraphicsDevice, 1, 1);
            bar.SetData(new Color[] { Color.White });

            UISlider sldr = new UISlider(Game, pos, new Point(256,24), 4,buttonSize)
            {
                BarTexture = Game.Content.Load<Texture2D>("Textures/UI/SliderBar"),
                Font = font,
                Label = text,
                LabelTint = Color.White,
                SliderColor = Color.DarkGray,
                SliderHoverColor = Color.Red,
                Tint = Color.DarkGray,
                Value = value,
                SliderTexture = Game.Content.Load<Texture2D>("Textures/UI/SliderButton"),
                
            };

            return sldr;
        }

    }
}
