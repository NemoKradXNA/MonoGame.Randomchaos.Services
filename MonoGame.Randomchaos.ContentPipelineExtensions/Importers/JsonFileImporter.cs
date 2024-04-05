

using MonoGame.Randomchaos.ContentPipelineExtensions.Processors;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;

using TImport = System.String;

namespace MonoGame.Randomchaos.ContentPipelineExtensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A JSON file importer. </summary>
    ///
    /// <remarks>   Charles Humphrey, 04/04/2024. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [ContentImporter(".json",  DefaultProcessor = nameof(JsonProcessor), DisplayName = "Randomchaos JSON Importer")]
    public class JsonFileImporter : ContentImporter<TImport>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Imports the given file. </summary>
        ///
        /// <remarks>   Charles Humphrey, 04/04/2024. </remarks>
        ///
        /// <param name="filename"> Filename of the file. </param>
        /// <param name="context">  The context. </param>
        ///
        /// <returns>   A TImport. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override TImport Import(string filename, ContentImporterContext context)
        {
            return File.ReadAllText(filename);
        }
    }
}
