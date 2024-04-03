
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Randomchaos.Services.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Services.Input.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Manager for touch collections. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class TouchCollectionManager : GameComponent, ITouchCollectionManager
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the handled. </summary>
        ///
        /// <value> True if handled, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool Handled { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state. </summary>
        ///
        /// <value> The state. </value>
        ///-------------------------------------------------------------------------------------------------

        public TouchCollection State { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the state of the last. </summary>
        ///
        /// <value> The last state. </value>
        ///-------------------------------------------------------------------------------------------------

        public TouchCollection LastState { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets this gesture. </summary>
        ///
        /// <value> this gesture. </value>
        ///-------------------------------------------------------------------------------------------------

        protected GestureSample thisGesture { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the gesture. </summary>
        ///
        /// <value> The gesture. </value>
        ///-------------------------------------------------------------------------------------------------

        public GestureSample Gesture { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the required gestures. </summary>
        ///
        /// <value> The required gestures. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<GestureType> RequiredGestures { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game"> The game. </param>
        ///-------------------------------------------------------------------------------------------------

        public TouchCollectionManager(Game game) : base(game)
        {
            RequiredGestures = new List<GestureType>();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="game">     The game. </param>
        /// <param name="gestures"> The gestures. </param>
        ///-------------------------------------------------------------------------------------------------

        public TouchCollectionManager(Game game, params GestureType[] gestures) : this(game)
        {
            UpdateEnabledGestures(gestures);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the enabled gestures described by gestures. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gestures"> The gestures. </param>
        ///-------------------------------------------------------------------------------------------------

        public void UpdateEnabledGestures(GestureType[] gestures)
        {
            RequiredGestures.AddRange(gestures);
            for (int g = 0; g < gestures.Length; g++)
                TouchPanel.EnabledGestures |= gestures[g];
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
            State = TouchPanel.GetState();

            if (TouchPanel.IsGestureAvailable && !Handled)
                thisGesture = TouchPanel.ReadGesture();

            base.Update(gameTime);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pre update. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public void PreUpdate(GameTime gameTime)
        {
            Handled = false;
            LastState = State;
            Gesture = thisGesture;
            thisGesture = new GestureSample();
        }
    }
}
