
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;

namespace MonoGame.Randomchaos.Animation.Animation2D.ContentReaders
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A sprite animator reader. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SpriteAnimatorReader : ContentTypeReader<SpriteAnimatorData>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Reads. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="input">            The input. </param>
        /// <param name="existingInstance"> The existing instance. </param>
        ///
        /// <returns>   A SpriteAnimatorData. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected override SpriteAnimatorData Read(ContentReader input, SpriteAnimatorData existingInstance)
        {
            string json = input.ReadString();
            return JsonConvert.DeserializeObject<SpriteAnimatorData>(json);
        }
    }
}
