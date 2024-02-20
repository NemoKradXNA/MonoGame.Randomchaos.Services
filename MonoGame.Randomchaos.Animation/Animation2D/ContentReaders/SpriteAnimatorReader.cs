using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;

namespace MonoGame.Randomchaos.Animation.Animation2D.ContentReaders
{
    public class SpriteAnimatorReader : ContentTypeReader<SpriteAnimatorData>
    {
        protected override SpriteAnimatorData Read(ContentReader input, SpriteAnimatorData existingInstance)
        {
            string json = input.ReadString();
            return JsonConvert.DeserializeObject<SpriteAnimatorData>(json);
        }
    }
}
