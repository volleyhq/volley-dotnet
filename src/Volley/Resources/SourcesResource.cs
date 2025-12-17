using System.Collections.Generic;
using System.Threading.Tasks;
using Volley.Models;

namespace Volley.Resources
{
    /// <summary>
    /// Sources API resource.
    /// </summary>
    public class SourcesResource
    {
        private readonly VolleyClient _client;

        internal SourcesResource(VolleyClient client)
        {
            _client = client;
        }

        /// <summary>
        /// List all sources for a project.
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>List of sources</returns>
        public async Task<List<Source>> ListAsync(long projectId)
        {
            var response = await _client.RequestAsync<Dictionary<string, object>>("GET", $"/api/projects/{projectId}/sources");
            var sources = new List<Source>();
            
            if (response.ContainsKey("sources") && response["sources"] != null)
            {
                var sourcesData = response["sources"];
                var sourcesJson = Newtonsoft.Json.JsonConvert.SerializeObject(sourcesData);
                var sourcesList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Source>>(sourcesJson);
                if (sourcesList != null)
                {
                    sources.AddRange(sourcesList);
                }
            }
            
            return sources;
        }

        /// <summary>
        /// Create a new source.
        /// </summary>
        public async Task<Source> CreateAsync(long projectId, string slug, string type, int? eps = null, 
            string? authType = null, string? authUsername = null, string? authKey = null, 
            bool? verifySignature = null, string? webhookSecret = null)
        {
            var data = new Dictionary<string, object>
            {
                ["slug"] = slug,
                ["type"] = type
            };
            if (eps.HasValue) data["eps"] = eps.Value;
            if (authType != null) data["auth_type"] = authType;
            if (authUsername != null) data["auth_username"] = authUsername;
            if (authKey != null) data["auth_key"] = authKey;
            if (verifySignature.HasValue) data["verify_signature"] = verifySignature.Value;
            if (webhookSecret != null) data["webhook_secret"] = webhookSecret;

            var response = await _client.RequestAsync<Source>("POST", $"/api/projects/{projectId}/sources", data);
            return response;
        }

        /// <summary>
        /// Get a source by ID.
        /// </summary>
        public async Task<Source> GetAsync(long projectId, long sourceId)
        {
            return await _client.RequestAsync<Source>("GET", $"/api/projects/{projectId}/sources/{sourceId}");
        }

        /// <summary>
        /// Update a source.
        /// </summary>
        public async Task<Source> UpdateAsync(long projectId, long sourceId, string? slug = null, 
            int? eps = null, string? authType = null, string? authUsername = null, 
            string? authKey = null, bool? verifySignature = null, string? webhookSecret = null)
        {
            var data = new Dictionary<string, object>();
            if (slug != null) data["slug"] = slug;
            if (eps.HasValue) data["eps"] = eps.Value;
            if (authType != null) data["auth_type"] = authType;
            if (authUsername != null) data["auth_username"] = authUsername;
            if (authKey != null) data["auth_key"] = authKey;
            if (verifySignature.HasValue) data["verify_signature"] = verifySignature.Value;
            if (webhookSecret != null) data["webhook_secret"] = webhookSecret;

            return await _client.RequestAsync<Source>("PUT", $"/api/projects/{projectId}/sources/{sourceId}", data);
        }

        /// <summary>
        /// Delete a source.
        /// </summary>
        public async Task DeleteAsync(long projectId, long sourceId)
        {
            await _client.RequestAsync("DELETE", $"/api/projects/{projectId}/sources/{sourceId}");
        }
    }
}

