
using Microsoft.Xna.Framework.Content;
using MonoGame.Randomchaos.Animation.Interfaces;

namespace MonoGame.Randomchaos.Animation.Animation3D.ContentReaders
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   The randomchaos model data reader. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    internal class SkinnedMeshReader : ContentTypeReader<ISkinnedMesh>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Reads. </summary>
        ///
        /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
        ///
        /// <param name="input">            The input. </param>
        /// <param name="existingInstance"> The existing instance. </param>
        ///
        /// <returns>   An IRandomchaosModelData. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected override ISkinnedMesh Read(ContentReader input, ISkinnedMesh existingInstance)
        {
            return input.ReadExternalReference<ISkinnedMesh>();
        }

    }
}
