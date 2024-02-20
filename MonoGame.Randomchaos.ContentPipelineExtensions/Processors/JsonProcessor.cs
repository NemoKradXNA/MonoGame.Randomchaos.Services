
using Microsoft.Xna.Framework.Content.Pipeline;
using TImport = System.String;
using TOutput = MonoGame.Randomchaos.ContentPipelineExtensions.Models.JsonStringData;

namespace MonoGame.Randomchaos.ContentPipelineExtensions.Processors
{
    [ContentProcessor(DisplayName = "JSON Processor")]
    public class JsonProcessor : ContentProcessor<TImport, TOutput>
    {
        public string GameAssemblyName { get; set; } = "JSON Data";
        public string TypeReader { get; set; } = "MonoGame.Randomchaos.ContentReaders.Json";
        public override TOutput Process(TImport input, ContentProcessorContext context)
        {
            return new TOutput(){ JsonData = input, GameAssemblyName = GameAssemblyName, TypeReader = TypeReader };
        }
    }
}
