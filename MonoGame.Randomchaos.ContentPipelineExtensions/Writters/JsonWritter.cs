using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using TImput = MonoGame.Randomchaos.ContentPipelineExtensions.Models.JsonStringData;

namespace MonoGame.Randomchaos.ContentPipelineExtensions.Writters
{
    [ContentTypeWriter] 
    public class JsonWritter : ContentTypeWriter<TImput>
    {
        protected string GameAsseblyName;
        protected string TypeReader;
        protected override void Write(ContentWriter output, TImput value)
        {
            GameAsseblyName = value.GameAssemblyName;
            TypeReader = value.TypeReader;

            output.Write(value.JsonData);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return $"{TypeReader}, {GameAsseblyName}";
        }
    }
}
