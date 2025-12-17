using System;
using Newtonsoft.Json;

namespace Volley.Models
{
    /// <summary>
    /// Organization model.
    /// </summary>
    public class Organization
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonProperty("role")]
        public string Role { get; set; } = string.Empty;

        [JsonProperty("account_id")]
        public long? AccountId { get; set; }

        [JsonProperty("created_at")]
        public string? CreatedAt { get; set; }
    }
}

