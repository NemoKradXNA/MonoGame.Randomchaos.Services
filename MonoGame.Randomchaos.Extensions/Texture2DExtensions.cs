

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Randomchaos.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A texture 2D extensions. </summary>
    ///
    /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class Texture2DExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Texture2D extension method that gets size rectangle. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="texture">  The texture to act on. </param>
        ///
        /// <returns>   The size rectangle. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Rectangle GetSizeRect(this Texture2D texture)
        {
            return new Rectangle(0, 0, texture.Width, texture.Height);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Texture2D extension method that fill with color. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="texture">  The texture to act on. </param>
        /// <param name="color">    The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void FillWithColor(this Texture2D texture, Color color)
        {
            Color[] colorData = new Color[texture.Width * texture.Height];
            texture.GetData(colorData);

            Array.Fill(colorData, color);

            texture.SetData(colorData);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Texture2D extension method that fill with border. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="texture">          The texture to act on. </param>
        /// <param name="color">            The color. </param>
        /// <param name="borderColor">      The border color. </param>
        /// <param name="borderThickenss">  The border thickenss. </param>
        /// <param name="horizontalFade">   (Optional) The horizontal fade. </param>
        /// <param name="verticalFade">     (Optional) The vertical fade. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void FillWithBorder(this Texture2D texture, Color color, Color borderColor, Rectangle borderThickenss, float horizontalFade = 2, float verticalFade = 2) 
        {
            Color[] c = new Color[texture.Width * texture.Height];

            Color defaultColor = new Color(0, 0, 0, 0);

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    Vector4 col = color.ToVector4();

                    if (x < borderThickenss.X || x >= texture.Width - borderThickenss.Width || y < borderThickenss.Height || y >= texture.Height - borderThickenss.Y)
                        col = borderColor.ToVector4();
                    else
                    {

                        if (horizontalFade > 0)
                            col *= MathF.Min(1, horizontalFade - ((float)x / (texture.Width - (borderThickenss.X + borderThickenss.Width))));
                        else
                            col *= MathF.Min(1, Math.Abs(horizontalFade) - (1 - ((float)x / (texture.Width - (borderThickenss.X + borderThickenss.Width)))));

                        if (verticalFade > 0)
                            col *= MathF.Min(1, verticalFade - ((float)y / texture.Height));
                        else
                            col *= MathF.Min(1, Math.Abs(verticalFade) - (1 - ((float)y / texture.Height)));
                    }

                    c[x + y * texture.Width] = new Color(col);
                }
            }

            texture.SetData(c);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Texture2D extension method that fill with normal. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="texture">  The texture to act on. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void FillWithNormal(this Texture2D texture) 
        {
            FillWithColor(texture, new Color(128, 128, 255));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Texture2D extension method that fill with bw noise. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="texture">  The texture to act on. </param>
        /// <param name="seed">     (Optional) The seed. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void FillWithBWNoise(this Texture2D texture, int? seed = null) 
        {
            Color[] colorData = new Color[texture.Width * texture.Height];
            texture.GetData(colorData);


            if (seed == null) 
            {
                seed = DateTime.Now.Millisecond;
            }

            Random rnd = new Random(seed.Value);

            for (int i = 0; i < colorData.Length; i++) 
            {
                Color c = Color.Lerp(Color.Black, Color.White, rnd.NextFloat());
                colorData[i] = c;
            }

            texture.SetData(colorData);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A Texture2D extension method that fill with noise. </summary>
        ///
        /// <remarks>   Charles Humphrey, 08/10/2023. </remarks>
        ///
        /// <param name="texture">  The texture to act on. </param>
        /// <param name="seed">     (Optional) The seed. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void FillWithNoise(this Texture2D texture, int? seed = null)
        {
            Color[] colorData = new Color[texture.Width * texture.Height];
            texture.GetData(colorData);


            if (seed == null)
            {
                seed = DateTime.Now.Millisecond;
            }

            Random rnd = new Random(seed.Value);

            for (int i = 0; i < colorData.Length; i++)
            {
                float r = rnd.NextFloat();
                float g = rnd.NextFloat();
                float b = rnd.NextFloat();
                float a = rnd.NextFloat();

                Color c = new Color(r,g,b,a);
                colorData[i] = c;
            }

            texture.SetData(colorData);
        }
    }
}
