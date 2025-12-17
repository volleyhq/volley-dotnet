using System.Collections.Generic;
using Newtonsoft.Json;

namespace Volley.Models
{
    /// <summary>
    /// Event model.
    /// </summary>
    public class Event
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; } = string.Empty;

        [JsonProperty("source_id")]
        public long SourceId { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }

        [JsonProperty("raw_body")]
        public string RawBody { get; set; } = string.Empty;

        [JsonProperty("headers")]
        public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>();

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("delivery_attempts")]
        public List<DeliveryAttempt>? DeliveryAttempts { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; } = string.Empty;
    }
}

