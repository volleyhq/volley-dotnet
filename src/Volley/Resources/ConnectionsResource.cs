using System.Threading.Tasks;
using Volley.Models;

namespace Volley.Resources
{
    /// <summary>
    /// Connections API resource.
    /// </summary>
    public class ConnectionsResource
    {
        private readonly VolleyClient _client;

        internal ConnectionsResource(VolleyClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Create a new connection.
        /// </summary>
        public async Task<Connection> CreateAsync(long projectId, long sourceId, long destinationId, 
            string? status = null, int? eps = null, int? maxRetries = null)
        {
            var data = new System.Collections.Generic.Dictionary<string, object>
            {
                ["source_id"] = sourceId,
                ["destination_id"] = destinationId
            };
            if (status != null) data["status"] = status;
            if (eps.HasValue) data["eps"] = eps.Value;
            if (maxRetries.HasValue) data["max_retries"] = maxRetries.Value;

            return await _client.RequestAsync<Connection>("POST", $"/api/projects/{projectId}/connections", data);
        }

        /// <summary>
        /// Get a connection by ID.
        /// </summary>
        public async Task<Connection> GetAsync(long connectionId)
        {
            return await _client.RequestAsync<Connection>("GET", $"/api/connections/{connectionId}");
        }

        /// <summary>
        /// Update a connection.
        /// </summary>
        public async Task<Connection> UpdateAsync(long connectionId, string? status = null, 
            int? eps = null, int? maxRetries = null)
        {
            var data = new System.Collections.Generic.Dictionary<string, object>();
            if (status != null) data["status"] = status;
            if (eps.HasValue) data["eps"] = eps.Value;
            if (maxRetries.HasValue) data["max_retries"] = maxRetries.Value;

            return await _client.RequestAsync<Connection>("PUT", $"/api/connections/{connectionId}", data);
        }

        /// <summary>
        /// Delete a connection.
        /// </summary>
        public async Task DeleteAsync(long connectionId)
        {
            await _client.RequestAsync("DELETE", $"/api/connections/{connectionId}");
        }
    }
}

