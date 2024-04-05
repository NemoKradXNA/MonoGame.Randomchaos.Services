
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using TImput = MonoGame.Randomchaos.ContentPipelineExtensions.Models.JsonStringData;

namespace MonoGame.Randomchaos.ContentPipelineExtensions.Writters
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A JSON writter. </summary>
    ///
    /// <remarks>   Charles Humphrey, 05/04/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [ContentTypeWriter] 
    public class JsonWritter : ContentTypeWriter<TImput>
    {
        /// <summary>   Name of the game assebly. </summary>
        protected string GameAsseblyName;
        /// <summary>   The type reader. </summary>
        protected string TypeReader;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Writes. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/04/2024. </remarks>
        ///
        /// <param name="output">   The output. </param>
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void Write(ContentWriter output, TImput value)
        {
            GameAsseblyName = value.GameAssemblyName;
            TypeReader = value.TypeReader;

            output.Write(value.JsonData);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets runtime reader. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/04/2024. </remarks>
        ///
        /// <param name="targetPlatform">   Target platform. </param>
        ///
        /// <returns>   The runtime reader. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return $"{TypeReader}, {GameAsseblyName}";
        }
    }
}
