

using Microsoft.Xna.Framework.Content.Pipeline;
using TImport = System.String;
using TOutput = MonoGame.Randomchaos.ContentPipelineExtensions.Models.JsonStringData;

namespace MonoGame.Randomchaos.ContentPipelineExtensions.Processors
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A JSON processor. </summary>
    ///
    /// <remarks>   Charles Humphrey, 05/04/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [ContentProcessor(DisplayName = "MonoGame.Randomchaos - JSON Processor")]
    public class JsonProcessor : ContentProcessor<TImport, TOutput>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name of the game assembly. </summary>
        ///
        /// <value> The name of the game assembly. </value>
        ///-------------------------------------------------------------------------------------------------

        public string GameAssemblyName { get; set; } = "JSON Data";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the type reader. </summary>
        ///
        /// <value> The type reader. </value>
        ///-------------------------------------------------------------------------------------------------

        public string TypeReader { get; set; } = "MonoGame.Randomchaos.ContentReaders.Json";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Process this object. </summary>
        ///
        /// <remarks>   Charles Humphrey, 05/04/2024. </remarks>
        ///
        /// <param name="input">    The input. </param>
        /// <param name="context">  The context. </param>
        ///
        /// <returns>   A TOutput. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override TOutput Process(TImport input, ContentProcessorContext context)
        {
            return new TOutput(){ JsonData = input, GameAssemblyName = GameAssemblyName, TypeReader = TypeReader };
        }
    }
}
