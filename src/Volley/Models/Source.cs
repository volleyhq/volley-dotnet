using Newtonsoft.Json;

namespace Volley.Models
{
    /// <summary>
    /// Source model.
    /// </summary>
    public class Source
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonProperty("ingestion_id")]
        public string IngestionId { get; set; } = string.Empty;

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("eps")]
        public int Eps { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("connection_count")]
        public int ConnectionCount { get; set; }

        [JsonProperty("auth_type")]
        public string AuthType { get; set; } = string.Empty;

        [JsonProperty("verify_signature")]
        public bool VerifySignature { get; set; }

        [JsonProperty("webhook_secret_set")]
        public bool WebhookSecretSet { get; set; }

        [JsonProperty("auth_username")]
        public string? AuthUsername { get; set; }

        [JsonProperty("auth_key_name")]
        public string? AuthKeyName { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; } = string.Empty;
    }
}

