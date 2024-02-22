using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Animation.Animation2D;
using MonoGame.Randomchaos.Animation.Animation2D.ContentReaders;
using Sample.MonoGme.Randomchaos.Animation2D.Models;
using System.Reflection;

namespace Sample.MonoGme.Randomchaos.Animation2D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _font;

        protected Sprite playerAvatar;
        SpriteAnimator figherAnimator;

        /// <summary>   The input service. </summary>
        IInputStateService inputService { get { return Services.GetService<IInputStateService>(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the scene service. </summary>
        ///
        /// <value> The scene service. </value>
        ///-------------------------------------------------------------------------------------------------

        ISceneService sceneService { get { return Services.GetService<ISceneService>(); } }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            new InputHandlerService(this, new KeyboardStateManager(this), new MouseStateManager(this));
        }

        protected override void Initialize()
        {
            figherAnimator = new SpriteAnimator(this, "FighterAnimator", "Animators/FighterSheet1Animator");
            _font = Content.Load<SpriteFont>("Fonts/font");

            figherAnimator.StartAnimation("Idle");

            Components.Add(figherAnimator);

            playerAvatar = new Sprite(this, figherAnimator, new Point(32, 40));

            playerAvatar.Position = new Vector2(100, 100);

            Components.Add(playerAvatar);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (inputService.KeyboardManager.KeyPress(Keys.Escape))
            {
                Exit();
            }

            if (inputService.KeyboardManager.KeyDown(Keys.A))
            {
                playerAvatar.StartAnimation("WalkLeft");
            }
            else if (inputService.KeyboardManager.KeyDown(Keys.D))
            {
                playerAvatar.StartAnimation("WalkRight");
            }
            else if (inputService.KeyboardManager.KeyDown(Keys.W))
            {
                playerAvatar.StartAnimation("WalkUp");
            }
            else if (inputService.KeyboardManager.KeyDown(Keys.S))
            {
                playerAvatar.StartAnimation("WalkDown");
            }
            else
            {
                playerAvatar.StartAnimation("Idle");
            }

            if (playerAvatar.CurrentAnimation != "Idle")
            {
                float spd = .5f;
                switch (playerAvatar.CurrentAnimation)
                {
                    case "WalkRight":
                        playerAvatar.Position += new Vector2(1, 0) * spd;
                        break;
                    case "WalkLeft":
                        playerAvatar.Position += new Vector2(-1, 0) * spd;
                        break;
                    case "WalkUp":
                        playerAvatar.Position += new Vector2(0, -1) * spd;
                        break;
                    case "WalkDown":
                        playerAvatar.Position += new Vector2(0, 1) * spd;
                        break;
                }

                Vector2 p = playerAvatar.Position;

                p.X = MathHelper.Min(GraphicsDevice.Viewport.Width - playerAvatar.Size.X, MathHelper.Max(0, p.X));
                p.Y = MathHelper.Min(GraphicsDevice.Viewport.Height - playerAvatar.Size.Y, MathHelper.Max(0, p.Y));

                playerAvatar.Position = p;

            }

            // TODO: Add your update logic here
            inputService.PreUpdate(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            Vector2 s = Vector2.One * -1;
            Vector2 p = new Vector2(8, 8);
            _spriteBatch.DrawString(_font, "Esc - Exit", p, Color.Black);
            _spriteBatch.DrawString(_font, "Esc - Exit", p + s, Color.Gold);

            p.Y += _font.LineSpacing;
            _spriteBatch.DrawString(_font, "WASD - Translate Dude", p, Color.Black);
            _spriteBatch.DrawString(_font, "WASD - Translate Dude", p + s, Color.Gold);

            string animNam = playerAvatar.CurrentAnimation;

            p = playerAvatar.Position + new Vector2(16 + _font.MeasureString(animNam).X / -2, -_font.LineSpacing);
            _spriteBatch.DrawString(_font, animNam, p, Color.Black);
            _spriteBatch.DrawString(_font, animNam, p + s, Color.Red);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
