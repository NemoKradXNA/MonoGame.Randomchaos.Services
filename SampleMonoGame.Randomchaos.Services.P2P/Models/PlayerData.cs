using System.Collections.Generic;

namespace SampleMonoGame.Randomchaos.Services.P2P.Models
{
    public class SessionData
    {
        public string Name { get; set; }
        public string Token { get; set; }
    }

    public class PlayerData
    {
        public SessionData Session { get; set; } = new SessionData();
        public string Name { get; set; }
        
        public Dictionary<string, object?> Properties { get; set; } = new Dictionary<string, object?>();
    }
}
