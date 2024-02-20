
using MonoGame.Randomchaos.ContentPipelineExtensions.Processors;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;

using TImport = System.String;

namespace MonoGame.Randomchaos.ContentPipelineExtensions
{
    [ContentImporter(".json",  DefaultProcessor = nameof(JsonProcessor), DisplayName = "Randomchaos JSON Importer")]
    public class JsonFileImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            return File.ReadAllText(filename);
        }
    }
}
