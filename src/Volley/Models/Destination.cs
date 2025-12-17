using Newtonsoft.Json;

namespace Volley.Models
{
    /// <summary>
    /// Destination model.
    /// </summary>
    public class Destination
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        [JsonProperty("eps")]
        public int Eps { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; } = string.Empty;
    }
}

