using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sample.MonoGame.Randomchaos.KestrelServer
{

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        IInputStateService inputService;
        /// <summary>   State of the kB. </summary>
        IKeyboardStateManager kbState;
        /// <summary>   The state. </summary>
        IMouseStateManager mState;

        Texture2D _texture;

        public string Title { get; set; } = "Kestrel Server Test";

        public List<Point> RedSquares { get; set; } = new List<Point>();
        public List<Point> BlackSquares { get; set; } = new List<Point>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            kbState = new KeyboardStateManager(this);
            mState = new MouseStateManager(this);
            inputService = new InputHandlerService(this, kbState, mState);

            Window.Title = Title;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Fonts/font");

            _texture = new Texture2D(GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.White });

            // TODO: use this.Content to load your game content here
            int w = GraphicsDevice.Viewport.Width / 32;
            int h = GraphicsDevice.Viewport.Height / 32;

            bool blackSquare = true;

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Point pos = new Point(x * 32, y * 32);

                    if (blackSquare)
                    {
                        BlackSquares.Add(pos);
                    }
                    else
                    {
                        RedSquares.Add(pos);
                    }

                    blackSquare = !blackSquare;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            inputService.PreUpdate(gameTime);
            base.Update(gameTime);

            if (kbState.KeyPress(Keys.F1))
            {
                Process.Start("explorer","https://localhost:5001/Home/Index");
            }

            if (kbState.KeyPress(Keys.F2))
            {
                Process.Start("explorer", "https://localhost:5001/swagger");
            }

            if (mState.LeftClicked)
            {
                if (BlackSquares.Any(s => new Rectangle(s.X, s.Y, 32, 32).Contains(mState.PositionRect)))
                {
                    BlackSquares.Remove(BlackSquares.First(s => new Rectangle(s.X, s.Y, 32, 32).Contains(mState.PositionRect)));
                }
                else if (RedSquares.Any(s => new Rectangle(s.X, s.Y, 32, 32).Contains(mState.PositionRect)))
                {
                    RedSquares.Remove(RedSquares.First(s => new Rectangle(s.X, s.Y, 32, 32).Contains(mState.PositionRect)));
                }                
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            // Draw our squares.

            foreach (Point pos in BlackSquares)
            {
                _spriteBatch.Draw(_texture, new Rectangle(pos.X, pos.Y, 32, 32), Color.Black);
            }

            foreach (Point pos in RedSquares)
            {
                _spriteBatch.Draw(_texture, new Rectangle(pos.X, pos.Y, 32, 32), Color.Red);
            }

            float line = 8;

            _spriteBatch.DrawString(_font, "Esc - Exit", new Vector2(8, line), Color.Gold);
            
            line += _font.LineSpacing;
            _spriteBatch.DrawString(_font, "F1 - Visit Home/Index", new Vector2(8, line), Color.Gold);
            
            line += _font.LineSpacing;
            _spriteBatch.DrawString(_font, "F2 - Visit Swagger API", new Vector2(8, line), Color.Gold);

            line += _font.LineSpacing;
            _spriteBatch.DrawString(_font, "Click mouse to delete a red or black square", new Vector2(8, line), Color.Gold);

            line += _font.LineSpacing * 2;
            _spriteBatch.DrawString(_font, $"There are currently [{RedSquares.Count}] Red squares, [{BlackSquares.Count}] black squares, totaling [{RedSquares.Count + BlackSquares.Count}] squares", new Vector2(8, line), Color.Gold);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}