using System.Collections.Generic;
using System.Threading.Tasks;
using Volley.Models;

namespace Volley.Resources
{
    /// <summary>
    /// Destinations API resource.
    /// </summary>
    public class DestinationsResource
    {
        private readonly VolleyClient _client;

        internal DestinationsResource(VolleyClient client)
        {
            _client = client;
        }

        /// <summary>
        /// List all destinations for a project.
        /// </summary>
        public async Task<List<Destination>> ListAsync(long projectId)
        {
            var response = await _client.RequestAsync<Dictionary<string, object>>("GET", $"/api/projects/{projectId}/destinations");
            var destinations = new List<Destination>();
            
            if (response.ContainsKey("destinations") && response["destinations"] != null)
            {
                var destinationsData = response["destinations"];
                var destinationsJson = Newtonsoft.Json.JsonConvert.SerializeObject(destinationsData);
                var destinationsList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Destination>>(destinationsJson);
                if (destinationsList != null)
                {
                    destinations.AddRange(destinationsList);
                }
            }
            
            return destinations;
        }

        /// <summary>
        /// Create a new destination.
        /// </summary>
        public async Task<Destination> CreateAsync(long projectId, string name, string url, int? eps = null)
        {
            var data = new Dictionary<string, object>
            {
                ["name"] = name,
                ["url"] = url
            };
            if (eps.HasValue) data["eps"] = eps.Value;

            return await _client.RequestAsync<Destination>("POST", $"/api/projects/{projectId}/destinations", data);
        }

        /// <summary>
        /// Get a destination by ID.
        /// </summary>
        public async Task<Destination> GetAsync(long projectId, long destinationId)
        {
            return await _client.RequestAsync<Destination>("GET", $"/api/projects/{projectId}/destinations/{destinationId}");
        }

        /// <summary>
        /// Update a destination.
        /// </summary>
        public async Task<Destination> UpdateAsync(long projectId, long destinationId, string? name = null, 
            string? url = null, int? eps = null)
        {
            var data = new Dictionary<string, object>();
            if (name != null) data["name"] = name;
            if (url != null) data["url"] = url;
            if (eps.HasValue) data["eps"] = eps.Value;

            return await _client.RequestAsync<Destination>("PUT", $"/api/projects/{projectId}/destinations/{destinationId}", data);
        }

        /// <summary>
        /// Delete a destination.
        /// </summary>
        public async Task DeleteAsync(long projectId, long destinationId)
        {
            await _client.RequestAsync("DELETE", $"/api/projects/{projectId}/destinations/{destinationId}");
        }
    }
}

