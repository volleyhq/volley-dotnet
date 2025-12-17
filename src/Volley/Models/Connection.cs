using Newtonsoft.Json;

namespace Volley.Models
{
    /// <summary>
    /// Connection model.
    /// </summary>
    public class Connection
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("source_id")]
        public long SourceId { get; set; }

        [JsonProperty("destination_id")]
        public long DestinationId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("eps")]
        public int Eps { get; set; }

        [JsonProperty("max_retries")]
        public int MaxRetries { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; } = string.Empty;
    }
}

