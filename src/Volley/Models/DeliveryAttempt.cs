using Newtonsoft.Json;

namespace Volley.Models
{
    /// <summary>
    /// Delivery Attempt model.
    /// </summary>
    public class DeliveryAttempt
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; } = string.Empty;

        [JsonProperty("connection_id")]
        public long ConnectionId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("error_reason")]
        public string? ErrorReason { get; set; }

        [JsonProperty("duration_ms")]
        public int DurationMs { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; } = string.Empty;
    }
}

