using System.Text.Json.Serialization;

namespace DadJokesConsumer.Models
{
    public class DadJoke
    {        
        public bool success { get; set; }
        public List<DadJokeBody> body { get; set; }        
    }
    public class DadJokeBody
    {        
        public string setup { get; set; }
        public string punchline { get; set; }
        public string type { get; set; }     
    }    
}
