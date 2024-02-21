
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Animation.Animation2D.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A sprite animator. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SpriteAnimator : GameComponent, ISpriteAnimator
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current animation. </summary>
        ///
        /// <value> The current animation. </value>
        ///-------------------------------------------------------------------------------------------------

        public string CurrentAnimation { get { return _animationPlayer.CurrentAnimation; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sprte sheet asset. </summary>
        ///
        /// <value> The sprte sheet asset. </value>
        ///-------------------------------------------------------------------------------------------------

        public string SprteSheetAsset { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sprite sheet texture. </summary>
        ///
        /// <value> The sprite sheet texture. </value>
        ///-------------------------------------------------------------------------------------------------

        public Texture2D SpriteSheetTexture { get; set; }

        /// <summary>   The animation player. </summary>
        protected ISpriteSheetAnimationPlayer _animationPlayer;

        /// <summary>   The animator data asset. </summary>
        protected string _animatorDataAsset;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the cells. </summary>
        ///
        /// <value> The x coordinate cells. </value>
        ///-------------------------------------------------------------------------------------------------

        protected int _XCells { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the cells. </summary>
        ///
        /// <value> The y coordinate cells. </value>
        ///-------------------------------------------------------------------------------------------------

        protected int _YCells { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the cell size. </summary>
        ///
        /// <value> The size of the cell. </value>
        ///-------------------------------------------------------------------------------------------------

        protected Point _cellSize { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current cell rectangle. </summary>
        ///
        /// <value> The current cell rectangle. </value>
        ///-------------------------------------------------------------------------------------------------

        public Rectangle CurrentCellRect
        {
            get
            {
                if (animationPlayer != null)
                    return new Rectangle((int)animationPlayer.CurrentCell.X, (int)animationPlayer.CurrentCell.Y, _cellSize.X, _cellSize.Y);
                else
                {
                    if (_cellSize == Point.Zero)
                        _cellSize = new Point(SpriteSheetTexture.Width, SpriteSheetTexture.Height);

                    return new Rectangle(0, 0, _cellSize.X, _cellSize.Y);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the clips. </summary>
        ///
        /// <value> The clips. </value>
        ///-------------------------------------------------------------------------------------------------

        public Dictionary<string, ISpriteSheetAnimationClip> Clips { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the waiting animation. </summary>
        ///
        /// <value> The waiting animation. </value>
        ///-------------------------------------------------------------------------------------------------

        protected string _waitingAnimation { get; set; } = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the animation player. </summary>
        ///
        /// <value> The animation player. </value>
        ///-------------------------------------------------------------------------------------------------

        public ISpriteSheetAnimationPlayer animationPlayer
        {
            get { return _animationPlayer; }
            set
            {
                if (_animationPlayer != value && _animationPlayer != null)
                    _animationPlayer.OnAnimationStopped -= OnAnimationStopped;

                _animationPlayer = value;
                _animationPlayer.OnAnimationStopped += OnAnimationStopped;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="game">                 The game. </param>
        /// <param name="name">                 The name. </param>
        /// <param name="animatorDataAsset">    The animator data asset. </param>
        ///-------------------------------------------------------------------------------------------------

        public SpriteAnimator(Game game, string name, string animatorDataAsset) : base(game) 
        {
            _animatorDataAsset = animatorDataAsset;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            if (_animatorDataAsset != null)
            {
                SpriteAnimatorData data = Game.Content.Load<SpriteAnimatorData>(_animatorDataAsset);
                LoadAnimationData(data);
            }


            base.Initialize();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the given gameTime. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="gameTime"> The game time. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
            if (animationPlayer != null)
            {
                if (!string.IsNullOrEmpty(_waitingAnimation))
                {
                    StartAnimation(_waitingAnimation);
                    _waitingAnimation = null;
                }

                animationPlayer.Update(gameTime.ElapsedGameTime);
            }

            base.Update(gameTime);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts an animation. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="animation">    The animation. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void StartAnimation(string animation)
        {
            if (animationPlayer != null)
            {
                animationPlayer.StartClip(animation);
            }
            else
            {
                _waitingAnimation = animation;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Stops an animation. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public virtual void StopAnimation()
        {
            if (animationPlayer != null)
            {
                animationPlayer.StopClip();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads animation data. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="data"> The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public void LoadAnimationData(SpriteAnimatorData data)
        {
            SpriteSheetTexture = Game.Content.Load<Texture2D>(data.SpriteSheetAsset);
            
            SpriteAnimationClipGenerator sacg = new SpriteAnimationClipGenerator(new Vector2(SpriteSheetTexture.Width, SpriteSheetTexture.Height), data.CellsXY);

            _cellSize = data.CellSize;

            Dictionary<string, ISpriteSheetAnimationClip> clips = new Dictionary<string, ISpriteSheetAnimationClip>();

            foreach (var clip in data.Clips)
            {
                clips.Add(clip.Key, sacg.Generate(clip.Key, clip.Value.Start, clip.Value.End, clip.Value.Duration, clip.Value.Looped));
            }

            animationPlayer = new SpriteSheetAnimationPlayer(clips);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the 'animation stopped' action. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="clip"> The clip. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void OnAnimationStopped(ISpriteSheetAnimationClip clip)
        {

        }
    }
}
