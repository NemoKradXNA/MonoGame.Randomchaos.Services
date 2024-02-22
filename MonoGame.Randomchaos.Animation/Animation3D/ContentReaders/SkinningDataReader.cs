
using Microsoft.Xna.Framework.Content;
using MonoGame.Randomchaos.Animation.Interfaces;

namespace MonoGame.Randomchaos.Animation.Animation3D.ContentReaders
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A skinning data reader. </summary>
    ///
    /// <remarks>   Charles Humphrey, 22/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SkinningDataReader : ContentTypeReader<ISkinnedData>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Reads. </summary>
        ///
        /// <remarks>   Charles Humphrey, 22/02/2024. </remarks>
        ///
        /// <param name="input">            The input. </param>
        /// <param name="existingInstance"> The existing instance. </param>
        ///
        /// <returns>   An ISkinnedData. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected override ISkinnedData Read(ContentReader input, ISkinnedData existingInstance)
        {
            return input.ReadExternalReference<ISkinnedData>();
        }
    }
}
