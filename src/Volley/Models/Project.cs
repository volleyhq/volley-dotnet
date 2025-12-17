using Newtonsoft.Json;

namespace Volley.Models
{
    /// <summary>
    /// Project model.
    /// </summary>
    public class Project
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("organization_id")]
        public long OrganizationId { get; set; }

        [JsonProperty("user_id")]
        public long? UserId { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; } = string.Empty;
    }
}

