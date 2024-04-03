using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Randomchaos.Extensions;
using MonoGame.Randomchaos.Interfaces;
using MonoGame.Randomchaos.Interfaces.Interfaces;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.UI;
using MonoGame.Randomchaos.UI.BaseClasses;
using MonoGame.Randomchaos.UI.Enums;
using Samples.MonoGame.Randomchaos.UI.Models;
using System.Collections.Generic;
using System.Linq;

namespace Samples.MonoGame.Randomchaos.UI
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        protected SpriteFont buttonFont;
        protected SpriteFont inputFont;

        protected UIButton btn1;
        protected UIButton btn2;
        protected UIButton btn3;

        protected UIImage img1;
        protected UIImage img2;
        protected UIImage img3;

        protected UIInputText input;
        protected UILabel lbl1;

        protected UIList lst1;

        protected UIMessageBox msgBox;

        protected UISlider slider;
        protected UISwitch uiSwitch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            new InputHandlerService(this, new KeyboardStateManager(this), new MouseStateManager(this));
        }

        protected override void Initialize()
        {
            buttonFont = Content.Load<SpriteFont>("Fonts/ButtonFont");
            inputFont = Content.Load<SpriteFont>("Fonts/InputFont");

            Point btnSize = new Point(256, 64);
            Point pos = new Point((GraphicsDevice.Viewport.Width / 2) - btnSize.X / 2, buttonFont.LineSpacing + 32);

            btn1 = CreateButton("Button One", Content.Load<Texture2D>("Textures/UI/Button"), pos, btnSize);
            Components.Add(btn1);

            img1 = new UIImage(this, pos + new Point(320,0), new Point(64, 64))
            {
                Texture = Content.Load<Texture2D>("Textures/UI/Bulb"),
                Tint = Color.Red,
            };

            Components.Add(img1);
            

            btn2 = CreateButton("Button Two", Content.Load<Texture2D>("Textures/UI/Button"), pos + new Point(0,66), btnSize);
            Components.Add(btn2);

            img2 = new UIImage(this, pos + new Point(320, 66), new Point(64, 64))
            {
                Texture = Content.Load<Texture2D>("Textures/UI/Bulb"),
                Tint = Color.Red,
            };

            Components.Add(img2);

            btn3 = CreateButton("Button Three", Content.Load<Texture2D>("Textures/UI/Button"), pos + new Point(0,132), btnSize);
            Components.Add(btn3);

            img3 = new UIImage(this, pos + new Point(320, 132), new Point(64, 64))
            {
                Texture = Content.Load<Texture2D>("Textures/UI/Bulb"),
                Tint = Color.Red,
            };

            Components.Add(img3);

            Texture2D txtBg = Content.Load<Texture2D>("Textures/UI/inputBg");
            Texture2D txtBdr = Content.Load<Texture2D>("Textures/UI/inputBorder");

            Point inputPos = new Point((GraphicsDevice.Viewport.Width / 2) - 256, 300);

            input = new UIInputText(this, inputPos, txtBg, txtBdr)
            {
                Font = inputFont,
                Tint = Color.White,
                Size = new Point(512, buttonFont.LineSpacing + 16),
                TextAlingment = TextAlingmentEnum.LeftMiddle,
                TextPositionOffset = new Vector2(8, 0),
                TextColor = Color.Black,
                ShadowColor = Color.DarkGray,
                ShadowOffset = new Vector2(1, 1),
                BorderSegment = new Rectangle(11,11,11,11),
                Prompt = "Enter some text..."
            };

            Components.Add(input);

            lbl1 = new UILabel(this)
            {
                Font = inputFont,
                Position = inputPos - new Point(168, -4),
                Text = "A Label:",
                Tint = Color.White,
                ShadowColor = Color.Black,
                ShadowOffset = new Vector2(-2, 2),
                Size = new Point(256,32)
            };

            Components.Add(lbl1);

            // TODO: Still work to do on lists.
            Texture2D lstBackground = new Texture2D(GraphicsDevice, 128, 100);
            lstBackground.FillWithBorder(Color.White, Color.Black, new Rectangle(1, 1, 1, 1));

            lst1 = new UIList(this, new Point(8, 8), new Point(128, 100))
            {
                Title = "A ListBox <WIP>",
                TitleFont = inputFont,
                ListBackgroundTexture = lstBackground,
                ListFont = inputFont,
                Items = new List<IListItem>()
                {
                    new ListItem("Item One", Color.Black),
                    new ListItem("Item Two", Color.Black),
                    new ListItem("Item Three", Color.Black),
                    new ListItem("Item Four", Color.Black),
                    new ListItem("Item Five", Color.Black),
                    new ListItem("Item Six", Color.Black),
                    new ListItem("Item Seven", Color.Black),
                },
            };

            Components.Add(lst1);

            Texture2D barBg = Content.Load<Texture2D>("Textures/UI/sliderBar");
            Texture2D sliderBg = Content.Load<Texture2D>("Textures/UI/slider");

            slider = new UISlider(this, new Point(8, GraphicsDevice.Viewport.Height - 64), new Point(256, 32), 16)
            {
                BarTexture = barBg,
                Font = inputFont,
                Label = "A Slider 50%",
                LabelTint = Color.White,
                SliderColor = Color.White,
                SliderHoverColor = Color.Red,
                SliderTexture = sliderBg,
                Value = .5f,
            };

            Components.Add(slider);


            uiSwitch = new UISwitch(this, new Point(GraphicsDevice.Viewport.Width - 200, GraphicsDevice.Viewport.Height - 72), new Point(64, 32))
            {
                BackgroundTexture = Content.Load<Texture2D>("Textures/UI/SwitchBg"),
                BorderColor = Color.LimeGreen,
                ButtonColor = Color.White,
                Font = inputFont,
                HighlightColor = Color.Lime,
                OffText = "Off",
                OnText = "On",
                Text = "Switch",
                TextColor = Color.White,
                TextPositionOffset = new Vector2(4  ,0),
                SwitchBorder = Content.Load<Texture2D>("Textures/UI/SwitchBorder"),
                SwitchOff = Content.Load<Texture2D>("Textures/UI/SwitchOff"),
                SwitchOn = Content.Load<Texture2D>("Textures/UI/SwitchOn"),
                Tint = Color.Green,
                OffColor = Color.Red,
                OnColor = Color.Lime
            };

            uiSwitch.OnMouseClickOff += UiSwitch_OnMouseClick;
            uiSwitch.OnMouseClickOn += UiSwitch_OnMouseClick;

            Components.Add(uiSwitch);

            Point msgBoxSize = new Point(512, 200);
            Point msgBoxPos = new Point((GraphicsDevice.Viewport.Width / 2) - (msgBoxSize.X / 2), (GraphicsDevice.Viewport.Height / 2) - (msgBoxSize.Y / 2));

            Texture2D msgBoxBg = new Texture2D(GraphicsDevice, msgBoxSize.X, msgBoxSize.Y);
            Texture2D titleBg = new Texture2D(GraphicsDevice, msgBoxSize.X, 42);

            msgBoxBg.FillWithBorder(Color.Orange, Color.Red, new Rectangle(2, 2, 2, 2), 2, 1);
            titleBg.FillWithColor(Color.Red);

            msgBox = new UIMessageBox(this, msgBoxPos, msgBoxSize)
            {
                Background = msgBoxBg,
                Message = "This is a message box!\nClick 'OK' or 'Cancel' to close it...\n\nPress F1 to show it again.",
                MessageFont = inputFont,
                Title = "Message Box!",
                TitleFont = inputFont,
                ButtonFont = inputFont,
                ButtonBackground = titleBg,
                NegativeText = "Cancel",
                PositiveText = "OK",
                ShowNegativeButton = true,
                ShowPositiveButton = true,
                ShowRetryBtn = false,
                Tint = Color.White,
                TitleBackground = titleBg,
            };

            msgBox.OnNegativeClicked += MsgBox_OnClicked;
            msgBox.OnPositiveClicked += MsgBox_OnClicked;

            Components.Add(msgBox);

            

            // Update order is important if you have overlapping controls, you want the update to occur in the opposite order to the draw or
            // you will have mouse events pass through to the controls below. This was implemented after UI version 2.0.0.6.
            var uiComponents = Components.Where(w => w is UIBase);
            
            int cnt = uiComponents.Count();
            int start = Components.Count - cnt;

            foreach (var uiComponent in uiComponents)
            {
                ((DrawableGameComponent)uiComponent).UpdateOrder = cnt - start++;
            }

            base.Initialize();
        }

        private void UiSwitch_OnMouseClick(IUIBase sender, IMouseStateManager mouseState)
        {
            //throw new System.NotImplementedException();
        }

        private void MsgBox_OnClicked(IUIBase sender, IMouseStateManager mouseState)
        {
            msgBox.Close();
        }

        protected UIButton CreateButton(string text, Texture2D bgTeture, Point pos, Point size)
        {
            var btn = new UIButton(this, pos, size)
            {
                BackgroundTexture = bgTeture,
                Tint = Color.Green,
                Font = buttonFont,
                Text = text,
                TextColor = Color.White,
                HighlightColor = Color.LimeGreen,
                Segments = new Rectangle(4, 4, 4, 4),
                TextHighlightColor = Color.Lime,
                //TextShadow = new Vector2(-1, 1),
                //ButtonShadow = new Vector2(-2, 2),
                ButtonShadowColor = Color.Lime,
            };

            btn.OnMouseClick += Btn_OnMouseClick;

            return btn;
        }

        protected void Btn_OnMouseClick(IUIBase sender, IMouseStateManager mouseState)
        {
            if (sender == btn1)
            {
                img1.Tint = img1.Tint == Color.Red ? Color.LimeGreen : Color.Red;
            }

            if (sender == btn2)
            {
                img2.Tint = img2.Tint == Color.Red ? Color.LimeGreen : Color.Red;
            }

            if (sender == btn3)
            {
                img3.Tint = img3.Tint == Color.Red ? Color.LimeGreen : Color.Red;
            }
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Services.GetService<IInputStateService>().PreUpdate(gameTime);
            base.Update(gameTime);

            btn1.Text = img1.Tint == Color.Red ? "Button One Off" : "Button One On";
            btn2.Text = img2.Tint == Color.Red ? "Button Two Off" : "Button Two On";
            btn3.Text = img3.Tint == Color.Red ? "Button Three Off" : "Button Three On";

            slider.Label = $"A Slider {(slider.Value * 100): 0}%";

            if (Services.GetService<IInputStateService>().KeyboardManager.KeyPress(Keys.F1))
            {
                msgBox.Show();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
