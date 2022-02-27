using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Randomchaos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Randomchaos.Services.Input.Models
{
    public class TouchCollectionManager : GameComponent, ITouchCollectionManager
    {
        public TouchCollection State { get; set; }
        public TouchCollection LastState { get; set; }

        protected GestureSample thisGesture { get; set; }
        public GestureSample Gesture { get; set; }
        public List<GestureType> RequiredGestures { get; set; }

        public TouchCollectionManager(Game game) : base(game)
        {
            RequiredGestures = new List<GestureType>();
        }

        public TouchCollectionManager(Game game, params GestureType[] gestures) : this(game)
        {
            UpdateEnabledGestures(gestures);
        }

        public void UpdateEnabledGestures(GestureType[] gestures)
        {
            RequiredGestures.AddRange(gestures);
            for (int g = 0; g < gestures.Length; g++)
                TouchPanel.EnabledGestures |= gestures[g];
        }
        public override void Update(GameTime gameTime)
        {
            State = TouchPanel.GetState();

            if (TouchPanel.IsGestureAvailable)
                thisGesture = TouchPanel.ReadGesture();

            base.Update(gameTime);
        }

        public void PreUpdate(GameTime gameTime)
        {
            LastState = State;
            Gesture = thisGesture;
            thisGesture = new GestureSample();
        }
    }
}
