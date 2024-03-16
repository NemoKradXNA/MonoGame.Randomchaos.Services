
using System;
using System.Collections.Generic;
using MonoGame.Randomchaos.Animation.Animation2D.Interfaces;
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.Animation.Animation2D
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A sprite animation clip generator. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SpriteAnimationClipGenerator
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the sprite sheet dimensions. </summary>
        ///
        /// <value> The sprite sheet dimensions. </value>
        ///-------------------------------------------------------------------------------------------------

        protected Vector2 SpriteSheetDimensions { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the slices. </summary>
        ///
        /// <value> The slices. </value>
        ///-------------------------------------------------------------------------------------------------

        protected Vector2 Slices { get; set; }

        /// <summary>   The master frames. </summary>
        SpriteSheetKeyFrame[,] masterFrames;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="spriteSheetDimensions">    The sprite sheet dimensions. </param>
        /// <param name="slices">                   The slices. </param>
        ///-------------------------------------------------------------------------------------------------

        public SpriteAnimationClipGenerator(Vector2 spriteSheetDimensions, Vector2 slices)
        {
            SpriteSheetDimensions = spriteSheetDimensions;
            Slices = slices;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Generates a master frames. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected void GenerateMasterFrames()
        {
            masterFrames = new SpriteSheetKeyFrame[(int)Slices.X, (int)Slices.Y];

            for (int y = 0; y < Slices.Y; y++)
            {
                for (int x = 0; x < Slices.X; x++)
                {
                    masterFrames[x, y] = new SpriteSheetKeyFrame();
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Generates. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="name">     The name. </param>
        /// <param name="start">    The start. </param>
        /// <param name="end">      The end. </param>
        /// <param name="duration"> The duration. </param>
        /// <param name="looped">   True if looped. </param>
        ///
        /// <returns>   An ISpriteSheetAnimationClip. </returns>
        ///-------------------------------------------------------------------------------------------------

        public ISpriteSheetAnimationClip Generate(string name, Vector2 start, Vector2 end, TimeSpan duration, bool looped)
        {
            List<ISpriteSheetKeyFrame> frames = new List<ISpriteSheetKeyFrame>();
            ISpriteSheetAnimationClip retVal = new SpriteSheetAnimationClip(name, duration, null, looped);

            float cnt = 0;
            int xIncDec = 1;
            int yIncDec = 1;

            float xCnt = 0;
            float yCnt = 0;

            // Are we going to be moving forward or backwards along the 
            // X axis of the sprite sheet to get the animation frames?
            if (start.X > end.X)
            {
                xIncDec = -1;
                xCnt = (start.X - end.X) + 1;
            }
            else
                xCnt = (end.X - start.X) + 1;

            // Are we going to be moving up or down along the 
            // Y axis of the sprite sheet to get the animation frames?
            if (start.Y > end.Y)
            {
                yIncDec = -1;
                yCnt = (start.Y - end.Y) + 1;
            }
            else
                yCnt = (end.Y - start.Y) + 1;

            // This is the base time each frame is made up of.
            TimeSpan time = new TimeSpan(duration.Ticks / (long)(xCnt * yCnt));

            // This is the size of a cell on the sprite sheet.
            Vector2 cellSize = SpriteSheetDimensions / Slices;

            // This is used to help calculate the time for each frame.
            int frameCount = 0;

            // Is this just one line off the sheet?
            // if both start and end Y are the same, then it is a horizontal
            // line of key frames
            if (start.Y == end.Y)
            {
                int y = (int)start.Y;
                for (int x = (int)start.X; xCnt > 0; x += xIncDec, xCnt--)
                {
                    SpriteSheetKeyFrame frame = new SpriteSheetKeyFrame(new Vector2(x * cellSize.X, y * cellSize.Y), new TimeSpan(time.Ticks * frameCount++));
                    frames.Add(frame);
                }
            }
            else if (start.X == end.X) // if both start and end X are the same, it's a vertical slice.
            {
                int x = (int)start.X;
                for (int y = (int)start.Y; yCnt > 0; y += yIncDec, yCnt--)
                {
                    SpriteSheetKeyFrame frame = new SpriteSheetKeyFrame(new Vector2(x * cellSize.X, y * cellSize.Y), new TimeSpan(time.Ticks * frameCount++));
                    frames.Add(frame);
                }

            }
            else // If neither start or end X or Y are the same, then it's a block of frames
            {
                for (int y = (int)start.Y; yCnt > 0; y += yIncDec, yCnt--)
                {
                    float xcnt = xCnt;
                    for (int x = (int)start.X; xcnt > 0; x += xIncDec, xcnt--)
                    {
                        SpriteSheetKeyFrame frame = new SpriteSheetKeyFrame(new Vector2(x * cellSize.X, y * cellSize.Y), new TimeSpan(time.Ticks * frameCount++));
                        frames.Add(frame);
                    }
                }
            }

            retVal.Keyframes = frames;

            return retVal;
        }
    }
}
