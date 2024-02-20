using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Animation.Animation2D.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Randomchaos.Animation.Animation2D
{
    public class SpriteAnimator : GameComponent, ISpriteAnimator
    {
        public string Name { get; set; }
        public string CurrentAnimation { get { return _animationPlayer.CurrentAnimation; } }
        public string SprteSheetAsset { get; set; }
        public Texture2D SpriteSheetTexture { get; set; }

        protected ISpriteSheetAnimationPlayer _animationPlayer;

        protected string _animatorDataAsset;
        protected int _XCells { get; set; }
        protected int _YCells { get; set; }

        protected Point _cellSize { get; set; }

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

        public Dictionary<string, ISpriteSheetAnimationClip> Clips { get; set; }

        protected string _waitingAnimation { get; set; } = null;

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

        public SpriteAnimator(Game game, string name, string animatorDataAsset) : base(game) 
        {
            _animatorDataAsset = animatorDataAsset;
        }

        public override void Initialize()
        {
            if (_animatorDataAsset != null)
            {
                SpriteAnimatorData data = Game.Content.Load<SpriteAnimatorData>(_animatorDataAsset);
                LoadAnimationData(data);
            }


            base.Initialize();
        }

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

        public virtual void StopAnimation()
        {
            if (animationPlayer != null)
            {
                animationPlayer.StopClip();
            }
        }

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

        protected virtual void OnAnimationStopped(ISpriteSheetAnimationClip clip)
        {

        }
    }
}
