
using Microsoft.Xna.Framework.Content;
using MonoGame.Randomchaos.Animation.Interfaces;

namespace MonoGame.Randomchaos.Animation.Animation3D.ContentReaders
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   The randomchaos model data reader. </summary>
    ///
    /// <remarks>   Charles Humphrey, 21/02/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    internal class RandomchaosModelDataReader : ContentTypeReader<IRandomchaosModelData>
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

        protected override IRandomchaosModelData Read(ContentReader input, IRandomchaosModelData existingInstance)
        {
            return input.ReadExternalReference<IRandomchaosModelData>();
        }

    }
}
